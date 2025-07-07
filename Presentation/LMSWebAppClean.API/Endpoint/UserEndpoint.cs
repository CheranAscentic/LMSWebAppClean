using LMSWebAppClean.Application.DTO;
using LMSWebAppClean.API.Interface;
using LMSWebAppClean.Domain.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using LMSWebAppClean.Application.Usecase.Users.GetAllUsers;
using LMSWebAppClean.Application.Usecase.Users.GetUserById;
using LMSWebAppClean.Application.Usecase.Users.CreateUser;
using LMSWebAppClean.Application.Usecase.Users.UpdateUser;
using LMSWebAppClean.Application.Usecase.Users.DeleteUser;

namespace LMSWebAppClean.API.Endpoint
{
    public class UserEndpoint : IEndpointGroup
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            var users = app.MapGroup("/api/users")
                .WithTags("Users")
                .WithOpenApi();

            // Get all users
            users.MapGet("/", HandleGetAllUsers)
            .WithName("GetAllUsers")
            .WithSummary("Get all users")
            .WithDescription("Returns a list of all users in the system")
            .Produces<List<BaseUser>>(StatusCodes.Status200OK);

            // Get User with Id
            users.MapGet("/{userId}", HandleGetUserById)
            .WithName("GetUserById")
            .WithSummary("Get a user by ID")
            .WithDescription("Retrieves a specific user by their ID")
            .Produces<BaseUser>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound);

            // Add user with CreateUserDTO
            users.MapPost("/", HandleCreateUser)
            .WithName("CreateUser")
            .WithSummary("Create a new user")
            .WithDescription("Creates a new user with the specified name and type")
            .Produces<BaseUser>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest);

            // Update user with Id to User
            users.MapPut("/{userId}", HandleUpdateUserById)
            .WithName("UpdateUser")
            .WithSummary("Update a user")
            .WithDescription("Updates a user's information by their ID")
            .Produces<BaseUser>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound);

            // Delete user with Id
            users.MapDelete("/{userId}", HandleDeleteUserById)
            .WithName("DeleteUser")
            .WithSummary("Delete a user")
            .WithDescription("Deletes a user by their ID")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound);
        }

        private async Task<IResult> HandleGetAllUsers([FromHeader(Name = "Bearer")] int authId, [FromServices] IMediator mediator)
        {
            try
            {
                if (authId <= 0)
                {
                    return Results.BadRequest("Bearer header must be a valid positive integer");
                }

                var query = new GetAllUsersQuery(authId);
                var users = await mediator.Send(query);
                
                return Results.Ok(users);
            }
            catch (UnauthorizedAccessException ex)
            {
                //return Results.Forbid();
                return Results.Problem(ex.Message);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        private async Task<IResult> HandleGetUserById([FromHeader(Name = "Bearer")] int authId, int userId, [FromServices] IMediator mediator)
        {
            try
            {
                if (authId <= 0)
                {
                    return Results.BadRequest("Bearer header must be a valid positive integer");
                }

                var query = new GetUserByIdQuery(authId, userId);
                var user = await mediator.Send(query);
                
                return Results.Ok(user);
            }
            catch (UnauthorizedAccessException ex)
            {
                //return Results.Forbid();
                return Results.Problem(ex.Message);
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound($"User with ID {userId} not found.");
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        private async Task<IResult> HandleCreateUser([FromHeader(Name = "Bearer")] int authId, CreateUserDTO createUserDTO, [FromServices] IMediator mediator)
        {
            try
            {
                if (authId <= 0)
                {
                    return Results.BadRequest("Bearer header must be a valid positive integer");
                }

                var command = new CreateUserCommand(authId, createUserDTO.Name, createUserDTO.Type);
                var user = await mediator.Send(command);
                
                return Results.Created($"/api/users/{user.Id}", user);
            }
            catch (UnauthorizedAccessException ex)
            {
                //return Results.Forbid();
                return Results.Problem(ex.Message);
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

        private async Task<IResult> HandleUpdateUserById([FromHeader(Name = "Bearer")] int authId, int userId, UserDTO userDTO, [FromServices] IMediator mediator)
        {
            try
            {
                if (authId <= 0)
                {
                    return Results.BadRequest("Bearer header must be a valid positive integer");
                }

                var command = new UpdateUserCommand(authId, userId, userDTO.Name);
                var user = await mediator.Send(command);
                
                return Results.Ok(user);
            }
            catch (UnauthorizedAccessException ex)
            {
                //return Results.Forbid();
                return Results.Problem(ex.Message);
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound($"User with ID {userId} not found.");
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

        private async Task<IResult> HandleDeleteUserById([FromHeader(Name = "Bearer")] int authId, int userId, [FromServices] IMediator mediator)
        {
            try
            {
                if (authId <= 0)
                {
                    return Results.BadRequest("Bearer header must be a valid positive integer");
                }

                var command = new DeleteUserCommand(authId, userId);
                await mediator.Send(command);
                
                return Results.NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                //return Results.Forbid();
                return Results.Problem(ex.Message);
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound($"User with ID {userId} not found.");
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }
    }
}
