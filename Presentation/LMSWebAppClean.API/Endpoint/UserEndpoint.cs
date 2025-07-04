using LMSWebAppClean.Application.DTO;
using LMSWebAppClean.API.Interface;
using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Domain.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        private IResult HandleGetAllUsers([FromHeader(Name = "Bearer")] int authId, IUserService userService)
        {
            if (authId <= 0)
            {
                return Results.BadRequest("Bearer header must be a valid positive integer");
            }

            return Results.Ok(userService.GetAllUsers(authId));
        }

        private IResult HandleGetUserById([FromHeader(Name = "Bearer")] int authId, int userId, IUserService userService)
        {
            try
            {
                if (authId <= 0)
                {
                    return Results.BadRequest("Bearer header must be a valid positive integer");
                }

                var user = userService.GetUser(authId, userId);
                return Results.Ok(user);
            }
            catch (Exception ex)
            {
                return Results.NotFound("User with Id not found.");
            }
        }

        private IResult HandleCreateUser([FromHeader(Name = "Bearer")] int authId, CreateUserDTO createUserDTO, IUserService userService)
        {
            try
            {
                if (authId <= 0)
                {
                    return Results.BadRequest("Bearer header must be a valid positive integer");
                }

                var user = userService.AddUser(authId, createUserDTO.Name, createUserDTO.Type);
                return Results.Created($"/api/users/{user.Id}", user);
            }
            catch (Exception ex)
            {
                return Results.BadRequest("User could not be created.");
            }
        }

        private IResult HandleUpdateUserById([FromHeader(Name = "Bearer")] int authId, int userId, UserDTO userDTO, IUserService userService)
        {
            try
            {
                if (authId <= 0)
                {
                    return Results.BadRequest("Bearer header must be a valid positive integer");
                }

                var updatedUser = userService.UpdateUser(authId, userId, userDTO.Name, userDTO.Type);
                return Results.Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return Results.NotFound("User with Id could not be updated");
            }
        }

        private IResult HandleDeleteUserById([FromHeader(Name = "Bearer")] int authId, int userId, IUserService userService)
        {
            try
            {
                if (authId <= 0)
                {
                    return Results.BadRequest("Bearer header must be a valid positive integer");
                }

                var user = userService.RemoveUser(authId, userId);
                return Results.NoContent();
            }
            catch (Exception ex)
            {
                return Results.NotFound("User with Id could not be deleted.");
            }
        }
    }
}
