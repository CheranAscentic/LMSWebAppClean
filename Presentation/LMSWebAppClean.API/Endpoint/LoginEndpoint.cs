using LMSWebAppClean.Application.DTO;
using LMSWebAppClean.API.Interface;
using LMSWebAppClean.Domain.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using LMSWebAppClean.Application.Usecase.Auth.Login;
using LMSWebAppClean.Application.Usecase.Auth.Register;

namespace LMSWebAppClean.API.Endpoint
{
    public class LoginEndpoint : IEndpointGroup
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            var login = app.MapGroup("/api/login")
                .WithTags("Login")
                .WithOpenApi();

            // Login endpoint
            login.MapPost("/", HandleLogin)
                .WithName("Login")
                .WithSummary("Authenticate user with Id")
                .WithDescription("Returns a User entity based on Id provided, for now same as find user (Temporary implementation).")
                .Produces<StandardResponseObject<BaseUser>>(StatusCodes.Status200OK)
                .Produces<StandardResponseObject<BaseUser>>(StatusCodes.Status404NotFound)
                .Produces<StandardResponseObject<BaseUser>>(StatusCodes.Status500InternalServerError);

            // Register endpoint
            login.MapPost("/register", HandleRegister)
                .WithName("Register")
                .WithSummary("Register a new user")
                .WithDescription("Registers a new User based on UserDTO")
                .Produces<StandardResponseObject<BaseUser>>(StatusCodes.Status201Created)
                .Produces<StandardResponseObject<BaseUser>>(StatusCodes.Status400BadRequest)
                .Produces<StandardResponseObject<BaseUser>>(StatusCodes.Status500InternalServerError);
        }

        private async Task<IResult> HandleLogin([FromBody] StandardRequestObject<LoginDTO> request, [FromServices] IMediator mediator)
        {
            try
            {
                // Validate request
                if (request.Data == null)
                {
                    var badRequestResponse = StandardResponseObject<BaseUser>.BadRequest(
                        "Login data is required", 
                        "Invalid request format");
                    return Results.BadRequest(badRequestResponse);
                }

                // Create query from request data
                var query = new LoginQuery(request.Data.UserId);
                var user = await mediator.Send(query);
                
                var response = StandardResponseObject<BaseUser>.Ok(user, "Login successful");
                return Results.Ok(response);
            }
            catch (KeyNotFoundException)
            {
                var notFoundResponse = StandardResponseObject<BaseUser>.NotFound(
                    $"User with ID {request.Data?.UserId} not found.",
                    "User not found");
                return Results.NotFound(notFoundResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = StandardResponseObject<BaseUser>.InternalError(
                    ex.Message,
                    "An error occurred during login");
                return Results.Problem(detail: errorResponse.Error, statusCode: 500);
            }
        }

        private async Task<IResult> HandleRegister([FromBody] StandardRequestObject<CreateUserDTO> request, [FromServices] IMediator mediator)
        {
            try
            {
                // Validate request
                if (request.Data == null)
                {
                    var badRequestResponse = StandardResponseObject<BaseUser>.BadRequest(
                        "Registration data is required", 
                        "Invalid request format");
                    return Results.BadRequest(badRequestResponse);
                }

                // Create command from request data
                var command = new RegisterCommand(
                    request.Data.Name,
                    request.Data.Type
                );
                
                var user = await mediator.Send(command);
                var response = StandardResponseObject<BaseUser>.Created(user, "User registered successfully");
                return Results.Created($"/api/users/{user.Id}", response);
            }
            catch (ArgumentException ex)
            {
                var badRequestResponse = StandardResponseObject<BaseUser>.BadRequest(
                    ex.Message,
                    "Registration failed");
                return Results.BadRequest(badRequestResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = StandardResponseObject<BaseUser>.InternalError(
                    ex.Message,
                    "An error occurred during registration");
                return Results.Problem(detail: errorResponse.Error, statusCode: 500);
            }
        }
    }
}
