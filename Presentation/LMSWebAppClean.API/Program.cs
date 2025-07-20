using LMSWebAppClean.API.Extension;
using LMSWebAppClean.API.Middleware;
using LMSWebAppClean.Application.Extension;
using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Application.PermissionChecker;
using LMSWebAppClean.Application.Service;
using LMSWebAppClean.Application.Usecase.Books.GetAllBooks;
using LMSWebAppClean.Domain.Base;
using LMSWebAppClean.Domain.Enum;
using LMSWebAppClean.Domain.Model;
using LMSWebAppClean.Identity.Context;
using LMSWebAppClean.Identity.Models;
using LMSWebAppClean.Identity.Services;
using LMSWebAppClean.Identity.UnitOfWork;
using LMSWebAppClean.Persistence.Context;
using LMSWebAppClean.Persistence.Repository;
using LMSWebAppClean.Persistence.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;

// Configure Serilog early to capture startup logs
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting LMS Web Application");

    var builder = WebApplication.CreateBuilder(args);

    // Add Serilog
    builder.Host.UseSerilog((context, configuration) =>
        configuration.ReadFrom.Configuration(context.Configuration));

    // Add OpenAPI and Swagger services
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Library Management System API",
            Version = "v1",
            Description = "A .NET 9 Minimal API for Library Management System"
        });

        // Add JWT Authentication to Swagger
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT"
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    });

    builder.Services.AddOpenApi();

    // Register the DbContext
    builder.Services.AddDbContext<DataDBContext>(options =>
    {
        options.UseSqlServer(builder
            .Configuration.GetConnectionString("DefaultConnection"));
        // Enable sensitive data logging in development
        if (builder.Environment.IsDevelopment())
        {
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
        }
    });

    builder.Services.AddDbContext<IdentityDBContext>(options =>
    {
        options.UseSqlServer(builder
            .Configuration.GetConnectionString("DefaultConnection"));
        if (builder.Environment.IsDevelopment())
        {
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
        }
    });

    // Register IdentityDBContext
    builder.Services.AddIdentity<AppUser, IdentityType>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequiredLength = 12;
    })
        .AddEntityFrameworkStores<IdentityDBContext>();

    builder.Services.AddScoped<IUserClaimsPrincipalFactory<AppUser>, CustomUserClaimsPrincipalFactory>();

    builder.Services.AddAuthentication(options => {
        options.DefaultAuthenticateScheme =
        options.DefaultChallengeScheme =
        options.DefaultForbidScheme =
        options.DefaultScheme =
        options.DefaultSignInScheme =
        options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
    })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = builder.Configuration["JWT:Issuer"],
                ValidateAudience = true,
                ValidAudience = builder.Configuration["JWT:Audience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])
                ),
                ClockSkew = TimeSpan.Zero
            };

            // Enhanced JWT events with logging
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                    if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
                    {
                        context.Token = authHeader.Substring("Bearer ".Length).Trim();
                    }
                    return Task.CompletedTask;
                },
                OnAuthenticationFailed = context =>
                {
                    Log.Warning("JWT Authentication failed: {Error}", context.Exception.Message);
                    return Task.CompletedTask;
                },
                OnTokenValidated = context =>
                {
                    Log.Debug("JWT Token validated for user: {UserId}", 
                        context.Principal?.Identity?.Name ?? "Unknown");
                    return Task.CompletedTask;
                }
            };
        });

    builder.Services.AddAuthorization();

    // Register UnitOfWork
    builder.Services.AddScoped<IUnitOfWork, DataUnitOfWork>();

    // Register repositories required by services
    builder.Services.AddScoped<IRepository<BaseUser>, DatabaseRepository<BaseUser>>();
    builder.Services.AddScoped<IRepository<Book>, DatabaseRepository<Book>>();

    // Register application services
    builder.Services.AddScoped<IPermissionChecker, PermissionChecker>();
    builder.Services.AddScoped<IUserRegistrationService, UserRegistrationService>();
    builder.Services.AddScoped<IUserLoginService, UserLoginService>();
    builder.Services.AddScoped<ITokenService, TokenService>();

    // Register the missing CurrentUserService
    builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

    // Register MediatR
    builder.Services.AddMediatR(cfg =>
        cfg.RegisterServicesFromAssembly(typeof(GetAllBooksQuery).Assembly));

    // Auto-register ALL validators and validation behavior
    builder.Services.AddApplicationValidationFromAssemblyContaining<GetAllBooksQuery>();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddScoped<IRequestUserValidationService, RequestUserValidationService>();

    var app = builder.Build();

    // Add Serilog request logging
    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000}ms";
        options.GetLevel = (httpContext, elapsed, ex) => LogEventLevel.Information;
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
            diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
            diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].FirstOrDefault());
            
            if (httpContext.User.Identity?.IsAuthenticated == true)
            {
                diagnosticContext.Set("UserId", httpContext.User.Identity.Name);
            }
        };
    });

    // Configure the HTTP request pipeline
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "LMS API v1");
            options.RoutePrefix = string.Empty; // Serves Swagger UI at the app's root
        });
        app.MapOpenApi();
    }

    // Add global exception handler BEFORE other middleware
    app.UseMiddleware<GlobalExceptionHandler>();

    // Only redirect to HTTPS in production to prevent Authorization header loss
    if (!app.Environment.IsDevelopment())
    {
        app.UseHttpsRedirection();
    }

    app.UseCors("AllowAll");
    app.UseAuthentication();
    app.UseAuthorization();

    // Map all endpoints
    app.RegisterAllEndpointGroups();

    Log.Information("LMS Application started successfully");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "LMS Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

