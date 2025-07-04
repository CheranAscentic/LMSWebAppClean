using LMSWebAppClean.Application.DTO;
using LMSWebAppClean.API.Interface;
using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Domain.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace LMSWebAppClean.API.Endpoint;

public class BorrowingEndpoint : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var borrowing = app.MapGroup("/api/borrowing")
            .WithTags("Book Borrowing")
            .WithOpenApi();

        // POST borrow book
        borrowing.MapPost("/borrow", HandleBorrowBook)
        .WithName("BorrowBook")
        .WithSummary("Borrow a book")
        .WithDescription("Allows a member to borrow a specific book")
        .Produces<Book>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest);

        // POST return book
        borrowing.MapPost("/return", HandleReturnBook)
        .WithName("ReturnBook")
        .WithSummary("Return a borrowed book")
        .WithDescription("Allows a member to return a previously borrowed book")
        .Produces<Book>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest);

        // GET borrowed books by member ID
        borrowing.MapGet("/member/{memberId}", HandleGetBorrowedBooksByMemberId)
        .WithName("GetBorrowedBooks")
        .WithSummary("Get borrowed books by member")
        .WithDescription("Retrieves all books currently borrowed by a specific member")
        .Produces<List<Book>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }

    private IResult HandleBorrowBook([FromHeader(Name = "X-User-Id")] int authId, BookBorrowDTO borrowDTO, IBorrowingService borrowingService)
    {
        try
        {
            if (authId <= 0)
            {
                return Results.BadRequest("X-User-Id header must be a valid positive integer");
            }

            var book = borrowingService.BorrowBook(authId, borrowDTO.BookId, borrowDTO.MemberId);
            return Results.Ok(book);
        }
        catch (Exception)
        {
            return Results.BadRequest("Could not borrow Book for user");
        }
    }

    private IResult HandleReturnBook([FromHeader(Name = "X-User-Id")] int authId, BookBorrowDTO borrowDTO, IBorrowingService borrowingService)
    {
        try
        {
            if (authId <= 0)
            {
                return Results.BadRequest("X-User-Id header must be a valid positive integer");
            }

            var book = borrowingService.ReturnBook(authId, borrowDTO.BookId, borrowDTO.MemberId);
            return Results.Ok(book);
        }
        catch (Exception)
        {
            return Results.BadRequest("Could not return book for user.");
        }
    }

    private IResult HandleGetBorrowedBooksByMemberId([FromHeader(Name = "X-User-Id")] int authId, int memberId, IBorrowingService borrowingService)
    {
        try
        {
            if (authId <= 0)
            {
                return Results.BadRequest("X-User-Id header must be a valid positive integer");
            }

            var books = borrowingService.GetBorrowedBooks(authId, memberId);
            return Results.Ok(books);
        }
        catch (Exception)
        {
            return Results.BadRequest("Could not get borrowed books for user with Id.");
        }
    }
}