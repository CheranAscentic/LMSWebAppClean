using LMSWebAppClean.Application.DTO;
using LMSWebAppClean.API.Interface;
using LMSWebAppClean.Domain.Model;
using LMSWebAppClean.Application.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using LMSWebAppClean.Application.Usecase.Borrowing.Borrow;
using LMSWebAppClean.Application.Usecase.Borrowing.Return;
using LMSWebAppClean.Application.Usecase.Borrowing.GetBorrowedBooks;
using LMSWebAppClean.Domain.Enum;

namespace LMSWebAppClean.API.Endpoint
{
    public class BorrowingEndpoint : IEndpointGroup
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            var borrowing = app.MapGroup("/api/borrowing")
                .WithTags("Book Borrowing")
                .WithOpenApi();

            // POST borrow book - Requires authentication (self or staff permissions)
            borrowing.MapPost("/borrow", HandleBorrowBook)
                .WithName("BorrowBook")
                .RequireAuthorization()
                .WithSummary("Borrow a book")
                .WithDescription("Allows a member to borrow a specific book for themselves, or staff to borrow on behalf of a member. Requires authentication.")
                .Produces<StandardResponseObject<Book>>(StatusCodes.Status200OK)
                .Produces<StandardResponseObject<Book>>(StatusCodes.Status400BadRequest)
                .Produces<StandardResponseObject<Book>>(StatusCodes.Status404NotFound)
                .Produces<StandardResponseObject<Book>>(StatusCodes.Status401Unauthorized)
                .Produces<StandardResponseObject<Book>>(StatusCodes.Status403Forbidden)
                .Produces<StandardResponseObject<Book>>(StatusCodes.Status500InternalServerError);

            // POST return book - Requires authentication (self or staff permissions)
            borrowing.MapPost("/return", HandleReturnBook)
                .WithName("ReturnBook")
                .RequireAuthorization()
                .WithSummary("Return a borrowed book")
                .WithDescription("Allows a member to return their own borrowed book, or staff to process return on behalf of a member. Requires authentication.")
                .Produces<StandardResponseObject<Book>>(StatusCodes.Status200OK)
                .Produces<StandardResponseObject<Book>>(StatusCodes.Status400BadRequest)
                .Produces<StandardResponseObject<Book>>(StatusCodes.Status404NotFound)
                .Produces<StandardResponseObject<Book>>(StatusCodes.Status401Unauthorized)
                .Produces<StandardResponseObject<Book>>(StatusCodes.Status403Forbidden)
                .Produces<StandardResponseObject<Book>>(StatusCodes.Status500InternalServerError);

            // GET borrowed books by member ID - Requires authentication (self or staff permissions)
            borrowing.MapPost("/member/books", HandleGetBorrowedBooksByMemberId)
                .WithName("GetBorrowedBooks")
                .RequireAuthorization()
                .WithSummary("Get borrowed books by member")
                .WithDescription("Retrieves all books currently borrowed by a specific member. Members can view their own borrowed books, staff can view any member's borrowed books. Requires authentication.")
                .Produces<StandardResponseObject<List<Book>>>(StatusCodes.Status200OK)
                .Produces<StandardResponseObject<List<Book>>>(StatusCodes.Status400BadRequest)
                .Produces<StandardResponseObject<List<Book>>>(StatusCodes.Status404NotFound)
                .Produces<StandardResponseObject<List<Book>>>(StatusCodes.Status401Unauthorized)
                .Produces<StandardResponseObject<List<Book>>>(StatusCodes.Status403Forbidden)
                .Produces<StandardResponseObject<List<Book>>>(StatusCodes.Status500InternalServerError);
        }

        private async Task<IResult> HandleBorrowBook(
            [FromBody] StandardRequestObject<BorrowCommand> request, 
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
                request.Data.MemberId,
                Permission.Self.BorrowBook,     // Self permission
                Permission.Process.BorrowBook); // Process permission
            var book = await mediator.Send(request.Data);
            var response = StandardResponseObject<Book>.Ok(book, "Book borrowed successfully");
            return Results.Ok(response);
        }

        private async Task<IResult> HandleReturnBook(
            [FromBody] StandardRequestObject<ReturnCommand> request, 
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
                request.Data.MemberId,
                Permission.Self.ReturnBook,     // Self permission
                Permission.Process.ReturnBook); // Process permission
            var book = await mediator.Send(request.Data);
            var response = StandardResponseObject<Book>.Ok(book, "Book returned successfully");
            return Results.Ok(response);
        }

        private async Task<IResult> HandleGetBorrowedBooksByMemberId(
            [FromBody] StandardRequestObject<GetBorrowedBooksQuery> request, 
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
                request.Data.MemberId,
                Permission.Self.GetBorrowedBooks,     // Self permission
                Permission.Process.GetBorrowedBooks); // Process permission
            var books = await mediator.Send(request.Data);
            var response = StandardResponseObject<List<Book>>.Ok(books, "Borrowed books retrieved successfully");
            return Results.Ok(response);
        }
    }
}