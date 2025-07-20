using LMSWebAppClean.Application.DTO;
using LMSWebAppClean.API.Interface;
using LMSWebAppClean.Domain.Base;
using LMSWebAppClean.Application.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using LMSWebAppClean.Application.Usecase.Users.GetAllUsers;
using LMSWebAppClean.Application.Usecase.Users.GetUserById;
using LMSWebAppClean.Application.Usecase.Users.CreateUser;
using LMSWebAppClean.Application.Usecase.Users.UpdateUser;
using LMSWebAppClean.Application.Usecase.Users.DeleteUser;
using LMSWebAppClean.Domain.Enum;
using Microsoft.AspNetCore.Identity;
using LMSWebAppClean.Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace LMSWebAppClean.API.Endpoint
{
    public class UserEndpoint : IEndpointGroup
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            var users = app.MapGroup("/api/users")
                .WithTags("Users")
                .WithOpenApi();

            // Get all users - Requires staff permissions
            users.MapPost("/list", HandleGetAllUsers)
                .WithName("GetAllUsers")
                .RequireAuthorization()
                .WithSummary("Get all users")
                .WithDescription("Returns a list of all users in the system. Requires staff permissions.")
                .Produces<StandardResponseObject<List<BaseUser>>>(StatusCodes.Status200OK)
                .Produces<StandardResponseObject<List<BaseUser>>>(StatusCodes.Status400BadRequest)
                .Produces<StandardResponseObject<List<BaseUser>>>(StatusCodes.Status401Unauthorized)
                .Produces<StandardResponseObject<List<BaseUser>>>(StatusCodes.Status403Forbidden)
                .Produces<StandardResponseObject<List<BaseUser>>>(StatusCodes.Status500InternalServerError);

            // Get User with Id - Requires authentication (self or staff permissions)
            users.MapPost("/get", HandleGetUserById)
                .WithName("GetUserById")
                .RequireAuthorization()
                .WithSummary("Get a user by ID")
                .WithDescription("Retrieves a specific user by their ID. Users can view their own profile, staff can view any user. Requires authentication.")
                .Produces<StandardResponseObject<BaseUser>>(StatusCodes.Status200OK)
                .Produces<StandardResponseObject<BaseUser>>(StatusCodes.Status404NotFound)
                .Produces<StandardResponseObject<BaseUser>>(StatusCodes.Status400BadRequest)
                .Produces<StandardResponseObject<BaseUser>>(StatusCodes.Status401Unauthorized)
                .Produces<StandardResponseObject<BaseUser>>(StatusCodes.Status403Forbidden)
                .Produces<StandardResponseObject<BaseUser>>(StatusCodes.Status500InternalServerError);

            // Add user - Requires staff permissions
            users.MapPost("/", HandleCreateUser)
                .WithName("CreateUser")
                .RequireAuthorization()
                .WithSummary("Create a new user")
                .WithDescription("Creates a new user with the specified name and type. Requires staff permissions.")
                .Produces<StandardResponseObject<BaseUser>>(StatusCodes.Status201Created)
                .Produces<StandardResponseObject<BaseUser>>(StatusCodes.Status400BadRequest)
                .Produces<StandardResponseObject<BaseUser>>(StatusCodes.Status401Unauthorized)
                .Produces<StandardResponseObject<BaseUser>>(StatusCodes.Status403Forbidden)
                .Produces<StandardResponseObject<BaseUser>>(StatusCodes.Status500InternalServerError);

            // Update user - Requires authentication (self or staff permissions)
            users.MapPut("/", HandleUpdateUserById)
                .WithName("UpdateUser")
                .RequireAuthorization()
                .WithSummary("Update a user")
                .WithDescription("Updates a user's information by their ID. Users can update their own profile, staff can update any user. Requires authentication.")
                .Produces<StandardResponseObject<BaseUser>>(StatusCodes.Status200OK)
                .Produces<StandardResponseObject<BaseUser>>(StatusCodes.Status404NotFound)
                .Produces<StandardResponseObject<BaseUser>>(StatusCodes.Status400BadRequest)
                .Produces<StandardResponseObject<BaseUser>>(StatusCodes.Status401Unauthorized)
                .Produces<StandardResponseObject<BaseUser>>(StatusCodes.Status403Forbidden)
                .Produces<StandardResponseObject<BaseUser>>(StatusCodes.Status500InternalServerError);

            // Delete user - Requires staff permissions
            users.MapDelete("/", HandleDeleteUserById)
                .WithName("DeleteUser")
                .RequireAuthorization()
                .WithSummary("Delete a user")
                .WithDescription("Deletes a user by their ID. Requires staff permissions.")
                .Produces<StandardResponseObject<string>>(StatusCodes.Status200OK)
                .Produces<StandardResponseObject<string>>(StatusCodes.Status404NotFound)
                .Produces<StandardResponseObject<string>>(StatusCodes.Status400BadRequest)
                .Produces<StandardResponseObject<string>>(StatusCodes.Status401Unauthorized)
                .Produces<StandardResponseObject<string>>(StatusCodes.Status403Forbidden)
                .Produces<StandardResponseObject<string>>(StatusCodes.Status500InternalServerError);
        }

        private async Task<IResult> HandleGetAllUsers(
            [FromBody] StandardRequestObject<GetAllUsersQuery> request, 
            [FromServices] IMediator mediator,
            [FromServices] ICurrentUserService currentUserService,
            [FromServices] IPermissionChecker permissionChecker)
        {
            var currentUserId = currentUserService.DomainUserId;
            if (!currentUserId.HasValue)
            {
                return Results.Unauthorized();
            }
            permissionChecker.Check(
                currentUserId.Value, 
                Permission.Process.GetAllUsers, 
                "You don't have permission to view all users.");
            var users = await mediator.Send(request.Data);
            var response = StandardResponseObject<List<BaseUser>>.Ok(users, "Users retrieved successfully");
            return Results.Ok(response);
        }

        private async Task<IResult> HandleGetUserById(
            [FromBody] StandardRequestObject<GetUserByIdQuery> request, 
            [FromServices] IMediator mediator,
            [FromServices] ICurrentUserService currentUserService,
            [FromServices] IPermissionChecker permissionChecker)
        {
            var currentUserId = currentUserService.DomainUserId;
            if (!currentUserId.HasValue)
            {
                return Results.Unauthorized();
            }
            permissionChecker.Check(
                currentUserId.Value,
                request.Data.UserId,
                Permission.Self.GetUserById,    // Self permission
                Permission.Process.GetUserById); // Process permission
            var user = await mediator.Send(request.Data);
            var response = StandardResponseObject<BaseUser>.Ok(user, "User retrieved successfully");
            return Results.Ok(response);
        }

        private async Task<IResult> HandleCreateUser(
            [FromBody] StandardRequestObject<CreateUserCommand> request, 
            [FromServices] IMediator mediator,
            [FromServices] ICurrentUserService currentUserService,
            [FromServices] IPermissionChecker permissionChecker)
        {
            var currentUserId = currentUserService.DomainUserId;
            if (!currentUserId.HasValue)
            {
                return Results.Unauthorized();
            }
            permissionChecker.Check(
                currentUserId.Value, 
                Permission.Process.CreateUser, 
                "You don't have permission to create users.");
            var user = await mediator.Send(request.Data);
            var response = StandardResponseObject<BaseUser>.Created(user, "User created successfully");
            return Results.Created($"/api/users/{user.Id}", response);
        }

        private async Task<IResult> HandleUpdateUserById(
            [FromBody] StandardRequestObject<UpdateUserCommand> request, 
            [FromServices] IMediator mediator,
            [FromServices] ICurrentUserService currentUserService,
            [FromServices] IPermissionChecker permissionChecker)
        {
            var currentUserId = currentUserService.DomainUserId;
            if (!currentUserId.HasValue)
            {
                return Results.Unauthorized();
            }
            permissionChecker.Check(
                currentUserId.Value,
                request.Data.UserId,
                Permission.Self.UpdateUser,     // Self permission
                Permission.Process.UpdateUser); // Process permission
            var user = await mediator.Send(request.Data);
            var response = StandardResponseObject<BaseUser>.Ok(user, "User updated successfully");
            return Results.Ok(response);
        }

        private async Task<IResult> HandleDeleteUserById(
            [FromBody] StandardRequestObject<DeleteUserCommand> request, 
            [FromServices] IMediator mediator,
            [FromServices] ICurrentUserService currentUserService,
            [FromServices] IPermissionChecker permissionChecker,
            [FromServices] UserManager<AppUser> userManager)
        {
            var currentUserId = currentUserService.DomainUserId;
            if (!currentUserId.HasValue)
            {
                return Results.Unauthorized();
            }
            permissionChecker.Check(
                currentUserId.Value, 
                Permission.Process.DeleteUser, 
                "You don't have permission to delete users.");
            var appUser = await userManager.Users
                .FirstOrDefaultAsync(u => u.DomainUserId == request.Data.UserId);
            await mediator.Send(request.Data);
            if (appUser != null)
            {
                var identityResult = await userManager.DeleteAsync(appUser);
                // Optionally log identityResult errors
            }
            var response = StandardResponseObject<AppUser>.Ok(appUser, "User deleted successfully");
            return Results.Ok(response);
        }
    }
}
