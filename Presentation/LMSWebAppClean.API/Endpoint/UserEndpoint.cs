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
            users.MapPost("/list", HandleGetAllUsers)
                .WithName("GetAllUsers")
                .WithSummary("Get all users")
                .WithDescription("Returns a list of all users in the system")
                .Produces<StandardResponseObject<List<BaseUser>>>(StatusCodes.Status200OK)
                .Produces<StandardResponseObject<List<BaseUser>>>(StatusCodes.Status400BadRequest)
                .Produces<StandardResponseObject<List<BaseUser>>>(StatusCodes.Status500InternalServerError);

            // Get User with Id
            users.MapPost("/get", HandleGetUserById)
                .WithName("GetUserById")
                .WithSummary("Get a user by ID")
                .WithDescription("Retrieves a specific user by their ID")
                .Produces<StandardResponseObject<BaseUser>>(StatusCodes.Status200OK)
                .Produces<StandardResponseObject<BaseUser>>(StatusCodes.Status404NotFound)
                .Produces<StandardResponseObject<BaseUser>>(StatusCodes.Status400BadRequest)
                .Produces<StandardResponseObject<BaseUser>>(StatusCodes.Status500InternalServerError);

            // Add user with CreateUserDTO
            users.MapPost("/", HandleCreateUser)
                .WithName("CreateUser")
                .WithSummary("Create a new user")
                .WithDescription("Creates a new user with the specified name and type")
                .Produces<StandardResponseObject<BaseUser>>(StatusCodes.Status201Created)
                .Produces<StandardResponseObject<BaseUser>>(StatusCodes.Status400BadRequest)
                .Produces<StandardResponseObject<BaseUser>>(StatusCodes.Status500InternalServerError);

            // Update user with Id to User
            users.MapPut("/", HandleUpdateUserById)
                .WithName("UpdateUser")
                .WithSummary("Update a user")
                .WithDescription("Updates a user's information by their ID")
                .Produces<StandardResponseObject<BaseUser>>(StatusCodes.Status200OK)
                .Produces<StandardResponseObject<BaseUser>>(StatusCodes.Status404NotFound)
                .Produces<StandardResponseObject<BaseUser>>(StatusCodes.Status400BadRequest)
                .Produces<StandardResponseObject<BaseUser>>(StatusCodes.Status500InternalServerError);

            // Delete user with Id
            users.MapDelete("/", HandleDeleteUserById)
                .WithName("DeleteUser")
                .WithSummary("Delete a user")
                .WithDescription("Deletes a user by their ID")
                .Produces<StandardResponseObject<string>>(StatusCodes.Status200OK)
                .Produces<StandardResponseObject<string>>(StatusCodes.Status404NotFound)
                .Produces<StandardResponseObject<string>>(StatusCodes.Status400BadRequest)
                .Produces<StandardResponseObject<string>>(StatusCodes.Status500InternalServerError);
        }

        private async Task<IResult> HandleGetAllUsers([FromBody] StandardRequestObject<EmptyRequestDTO> request, [FromServices] IMediator mediator)
        {
            try
            {
                if (request.Bearer <= 0)
                {
                    var badRequestResponse = StandardResponseObject<List<BaseUser>>.BadRequest(
                        "Bearer token must be a valid positive integer",
                        "Invalid bearer token");
                    return Results.BadRequest(badRequestResponse);
                }

                var query = new GetAllUsersQuery(request.Bearer);
                var users = await mediator.Send(query);
                
                var response = StandardResponseObject<List<BaseUser>>.Ok(users, "Users retrieved successfully");
                return Results.Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                var unauthorizedResponse = StandardResponseObject<List<BaseUser>>.BadRequest(
                    ex.Message,
                    "Unauthorized access");
                return Results.BadRequest(unauthorizedResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = StandardResponseObject<List<BaseUser>>.InternalError(
                    ex.Message,
                    "An error occurred while retrieving users");
                return Results.Problem(detail: errorResponse.Error, statusCode: 500);
            }
        }

        private async Task<IResult> HandleGetUserById([FromBody] StandardRequestObject<GetByIdRequestDTO> request, [FromServices] IMediator mediator)
        {
            try
            {
                if (request.Bearer <= 0)
                {
                    var badRequestResponse = StandardResponseObject<BaseUser>.BadRequest(
                        "Bearer token must be a valid positive integer",
                        "Invalid bearer token");
                    return Results.BadRequest(badRequestResponse);
                }

                if (request.Data == null)
                {
                    var badRequestResponse = StandardResponseObject<BaseUser>.BadRequest(
                        "User ID is required",
                        "Invalid request format");
                    return Results.BadRequest(badRequestResponse);
                }

                var query = new GetUserByIdQuery(request.Bearer, request.Data.Id);
                var user = await mediator.Send(query);
                
                var response = StandardResponseObject<BaseUser>.Ok(user, "User retrieved successfully");
                return Results.Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                var unauthorizedResponse = StandardResponseObject<BaseUser>.BadRequest(
                    ex.Message,
                    "Unauthorized access");
                return Results.BadRequest(unauthorizedResponse);
            }
            catch (KeyNotFoundException)
            {
                var notFoundResponse = StandardResponseObject<BaseUser>.NotFound(
                    $"User with ID {request.Data?.Id} not found.",
                    "User not found");
                return Results.NotFound(notFoundResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = StandardResponseObject<BaseUser>.InternalError(
                    ex.Message,
                    "An error occurred while retrieving the user");
                return Results.Problem(detail: errorResponse.Error, statusCode: 500);
            }
        }

        private async Task<IResult> HandleCreateUser([FromBody] StandardRequestObject<CreateUserDTO> request, [FromServices] IMediator mediator)
        {
            try
            {
                if (request.Bearer <= 0)
                {
                    var badRequestResponse = StandardResponseObject<BaseUser>.BadRequest(
                        "Bearer token must be a valid positive integer",
                        "Invalid bearer token");
                    return Results.BadRequest(badRequestResponse);
                }

                if (request.Data == null)
                {
                    var badRequestResponse = StandardResponseObject<BaseUser>.BadRequest(
                        "User data is required",
                        "Invalid request format");
                    return Results.BadRequest(badRequestResponse);
                }

                var command = new CreateUserCommand(request.Bearer, request.Data.Name, request.Data.Type);
                var user = await mediator.Send(command);
                
                var response = StandardResponseObject<BaseUser>.Created(user, "User created successfully");
                return Results.Created($"/api/users/{user.Id}", response);
            }
            catch (UnauthorizedAccessException ex)
            {
                var unauthorizedResponse = StandardResponseObject<BaseUser>.BadRequest(
                    ex.Message,
                    "Unauthorized access");
                return Results.BadRequest(unauthorizedResponse);
            }
            catch (ArgumentException ex)
            {
                var badRequestResponse = StandardResponseObject<BaseUser>.BadRequest(
                    ex.Message,
                    "User creation failed");
                return Results.BadRequest(badRequestResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = StandardResponseObject<BaseUser>.InternalError(
                    ex.Message,
                    "An error occurred while creating the user");
                return Results.Problem(detail: errorResponse.Error, statusCode: 500);
            }
        }

        private async Task<IResult> HandleUpdateUserById([FromBody] StandardRequestObject<UserUpdateDTO> request, [FromServices] IMediator mediator)
        {
            try
            {
                if (request.Bearer <= 0)
                {
                    var badRequestResponse = StandardResponseObject<BaseUser>.BadRequest(
                        "Bearer token must be a valid positive integer",
                        "Invalid bearer token");
                    return Results.BadRequest(badRequestResponse);
                }

                if (request.Data == null)
                {
                    var badRequestResponse = StandardResponseObject<BaseUser>.BadRequest(
                        "User update data is required",
                        "Invalid request format");
                    return Results.BadRequest(badRequestResponse);
                }

                var command = new UpdateUserCommand(request.Bearer, request.Data.Id, request.Data.Name);
                var user = await mediator.Send(command);
                
                var response = StandardResponseObject<BaseUser>.Ok(user, "User updated successfully");
                return Results.Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                var unauthorizedResponse = StandardResponseObject<BaseUser>.BadRequest(
                    ex.Message,
                    "Unauthorized access");
                return Results.BadRequest(unauthorizedResponse);
            }
            catch (KeyNotFoundException)
            {
                var notFoundResponse = StandardResponseObject<BaseUser>.NotFound(
                    $"User with ID {request.Data?.Id} not found.",
                    "User not found");
                return Results.NotFound(notFoundResponse);
            }
            catch (ArgumentException ex)
            {
                var badRequestResponse = StandardResponseObject<BaseUser>.BadRequest(
                    ex.Message,
                    "User update failed");
                return Results.BadRequest(badRequestResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = StandardResponseObject<BaseUser>.InternalError(
                    ex.Message,
                    "An error occurred while updating the user");
                return Results.Problem(detail: errorResponse.Error, statusCode: 500);
            }
        }

        private async Task<IResult> HandleDeleteUserById([FromBody] StandardRequestObject<GetByIdRequestDTO> request, [FromServices] IMediator mediator)
        {
            try
            {
                if (request.Bearer <= 0)
                {
                    var badRequestResponse = StandardResponseObject<string>.BadRequest(
                        "Bearer token must be a valid positive integer",
                        "Invalid bearer token");
                    return Results.BadRequest(badRequestResponse);
                }

                if (request.Data == null)
                {
                    var badRequestResponse = StandardResponseObject<string>.BadRequest(
                        "User ID is required",
                        "Invalid request format");
                    return Results.BadRequest(badRequestResponse);
                }

                var command = new DeleteUserCommand(request.Bearer, request.Data.Id);
                await mediator.Send(command);
                
                var response = StandardResponseObject<string>.Ok("User deleted successfully", "User deleted successfully");
                return Results.Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                var unauthorizedResponse = StandardResponseObject<string>.BadRequest(
                    ex.Message,
                    "Unauthorized access");
                return Results.BadRequest(unauthorizedResponse);
            }
            catch (KeyNotFoundException)
            {
                var notFoundResponse = StandardResponseObject<string>.NotFound(
                    $"User with ID {request.Data?.Id} not found.",
                    "User not found");
                return Results.NotFound(notFoundResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = StandardResponseObject<string>.InternalError(
                    ex.Message,
                    "An error occurred while deleting the user");
                return Results.Problem(detail: errorResponse.Error, statusCode: 500);
            }
        }
    }
}
