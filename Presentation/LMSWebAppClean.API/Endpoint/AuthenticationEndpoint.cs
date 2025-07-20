using LMSWebAppClean.Application.DTO;
using LMSWebAppClean.Application.DTO.Auth;
using LMSWebAppClean.Application.Usecase.Identity.LoginUser;
using LMSWebAppClean.Application.Usecase.Identity.RegisterUser;
using LMSWebAppClean.API.Interface;
using LMSWebAppClean.Application.Interface;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using LMSWebAppClean.Domain.Enum;

namespace LMSWebAppClean.API.Endpoint
{
    public class AuthenticationEndpoint : IEndpointGroup
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            var auth = app.MapGroup("/api/auth")
                .WithTags("Authentication")
                .WithOpenApi();

            // Register endpoint - Public access, no authentication required
            auth.MapPost("/register", HandleRegister)
                .WithName("Register")
                .WithSummary("Register a new user")
                .AllowAnonymous()
                .WithDescription("Registers a new user with email/password authentication and creates associated domain user. No authentication required - public access.")
                .Produces<StandardResponseObject<string>>(StatusCodes.Status201Created)
                .Produces<StandardResponseObject<string>>(StatusCodes.Status400BadRequest)
                .Produces<StandardResponseObject<string>>(StatusCodes.Status409Conflict)
                .Produces<StandardResponseObject<string>>(StatusCodes.Status500InternalServerError);

            // Login endpoint - Public access, no authentication required
            auth.MapPost("/login", HandleLogin)
                .WithName("Login")
                .WithSummary("Authenticate user")
                .WithDescription("Authenticates user with email and password. No authentication required - public access.")
                .Produces<StandardResponseObject<LoginDTO>>(StatusCodes.Status200OK)
                .Produces<StandardResponseObject<string>>(StatusCodes.Status400BadRequest)
                .Produces<StandardResponseObject<string>>(StatusCodes.Status401Unauthorized)
                .Produces<StandardResponseObject<string>>(StatusCodes.Status500InternalServerError);

            // Logout endpoint - Requires authentication
            auth.MapPost("/logout", HandleLogout)
                .WithName("Logout")
                .RequireAuthorization()
                .WithSummary("Logout user")
                .WithDescription("Signs out the currently authenticated user. Requires authentication.")
                .Produces<StandardResponseObject<string>>(StatusCodes.Status200OK)
                .Produces<StandardResponseObject<string>>(StatusCodes.Status401Unauthorized)
                .Produces<StandardResponseObject<string>>(StatusCodes.Status500InternalServerError);

            // Get all user types - Public access, no authentication required
            auth.MapGet("/usertypes", HandleGetUserTypes)
                .WithName("GetUserTypes")
                .WithSummary("Get all user types")
                .WithDescription("Returns a list of all user types available in the system. No authentication required.")
                .Produces<StandardResponseObject<List<string>>>(StatusCodes.Status200OK)
                .Produces<StandardResponseObject<List<string>>>(StatusCodes.Status500InternalServerError);

            // Test endpoint - Requires authentication
            auth.MapPost("/test", HandleTest)
                .WithName("TestAuth")
                .RequireAuthorization()
                .WithSummary("Test authentication")
                .WithDescription("Test endpoint to verify JWT authentication is working.")
                .Produces<StandardResponseObject<object>>(StatusCodes.Status200OK)
                .Produces<StandardResponseObject<object>>(StatusCodes.Status401Unauthorized);
        }

        private async Task<IResult> HandleRegister(
            [FromBody] StandardRequestObject<RegisterUserCommand> request, 
            [FromServices] IMediator mediator)
        {
            // Simple validation - let global handler catch exceptions
            if (request?.Data == null)
            {
                var validationResponse = StandardResponseObject<string>.BadRequest(
                    "Request data is required",
                    "Registration validation failed");
                return Results.BadRequest(validationResponse);
            }

            // Let global exception handler catch and handle any exceptions
            var userId = await mediator.Send(request.Data);
            var response = StandardResponseObject<string>.Created(userId, "User registered successfully");
            return Results.Created($"/api/auth/user/{userId}", response);
        }

        private async Task<IResult> HandleLogin(
            [FromBody] StandardRequestObject<LoginUserCommand> request, 
            [FromServices] IMediator mediator)
        {
            // Simple validation - let global handler catch exceptions
            if (request?.Data == null)
            {
                var validationResponse = StandardResponseObject<string>.BadRequest(
                    "Request data is required",
                    "Login validation failed");
                return Results.BadRequest(validationResponse);
            }

            // Let global exception handler catch and handle any exceptions
            var loginDTO = await mediator.Send(request.Data);
            var response = StandardResponseObject<LoginDTO>.Ok(loginDTO, "Login successful");
            return Results.Ok(response);
        }

        private IResult HandleLogout(
            [FromServices] ICurrentUserService currentUserService)
        {
            // Simple check - let global handler catch exceptions
            var currentUserId = currentUserService.DomainUserId;
            if (!currentUserId.HasValue)
            {
                return Results.Unauthorized();
            }

            var response = StandardResponseObject<string>.Ok("Logged out", "Logout successful");
            return Results.Ok(response);
        }

        private IResult HandleGetUserTypes()
        {
            // Let global exception handler catch any exceptions
            var userTypes = new List<string>
            {
                UserType.Member,
                UserType.StaffMinor,
                UserType.StaffManagement,
            };

            var response = StandardResponseObject<List<string>>.Ok(userTypes, "User types retrieved successfully");
            return Results.Ok(response);
        }

        private IResult HandleTest(
            [FromServices] ICurrentUserService currentUserService)
        {
            // Let global exception handler catch any exceptions
            var userInfo = new
            {
                IsAuthenticated = currentUserService.IsAuthenticated,
                UserId = currentUserService.UserId,
                DomainUserId = currentUserService.DomainUserId,
                Email = currentUserService.Email,
                UserType = currentUserService.UserType
            };

            var response = StandardResponseObject<object>.Ok(userInfo, "Authentication test successful");
            return Results.Ok(response);
        }
    }
}