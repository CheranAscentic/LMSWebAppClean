using LMSWebAppClean.Application.DTO;
using LMSWebAppClean.API.Interface;
using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Application.Service;
using LMSWebAppClean.Domain.Base;

namespace LMSWebAppClean.API.Endpoint
{
    public class LoginEndpoint : IEndpointGroup
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            var login = app.MapGroup("/api/login")
                .WithTags("Login")
                .WithOpenApi();

            //
            login.MapPost("/", (LoginDTO loginDTO, ILoginService loginService) =>
            {
                try
                {
                    var user = loginService.Login(loginDTO.UserId);
                    return Results.Ok(user);
                }
                catch (Exception e)
                {
                    return Results.NotFound("User with ID could not be found to login.");
                }
            })
                .WithName("Login")
                .WithSummary("Authenticate use with Id")
                .WithDescription("returns a User entity based Id provided, for now same as find user(Temporary implementation).")
                .Produces<BaseUser>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound);



            login.MapPost("/register", (CreateUserDTO createUserDTO, ILoginService loginService) =>
            {
                try
                {
                    var user = loginService.Regsiter(createUserDTO.Name, createUserDTO.Type);
                    return Results.Created($"/api/users/{user.Id}", user);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest("User could not be Registered.");
                }
            })
                .WithName("Register")
                .WithSummary("Register a new user")
                .WithDescription("Registers a new User based on use UserDTO")
                .Produces<BaseUser>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest);            
        }
    }
}
