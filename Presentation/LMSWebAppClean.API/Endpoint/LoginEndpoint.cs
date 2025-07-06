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
                .WithDescription("returns a User entity based Id provided, for now same as find user(Temporary implementation).")
                .Produces<BaseUser>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound);

            // Register endpoint
            login.MapPost("/register", HandleRegister)
                .WithName("Register")
                .WithSummary("Register a new user")
                .WithDescription("Registers a new User based on UserDTO")
                .Produces<BaseUser>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest);
        }

        private async Task<IResult> HandleLogin(LoginDTO loginDTO, IMediator mediator)
        {
            try
            {
                var query = new LoginQuery(loginDTO.UserId);
                var user = await mediator.Send(query);
                
                return Results.Ok(user);
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound($"User with ID {loginDTO.UserId} not found.");
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        private async Task<IResult> HandleRegister(CreateUserDTO createUserDTO, IMediator mediator)
        {
            try
            {
                var command = new RegisterCommand(
                    createUserDTO.Name,
                    createUserDTO.Type
                );
                
                var user = await mediator.Send(command);
                return Results.Created($"/api/users/{user.Id}", user);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }
    }
}
