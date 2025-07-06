using LMSWebAppClean.Application.DTO;
using LMSWebAppClean.API.Interface;
using LMSWebAppClean.Domain.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using LMSWebAppClean.Application.Usecase.Borrowing.Borrow;
using LMSWebAppClean.Application.Usecase.Borrowing.Return;
using LMSWebAppClean.Application.Usecase.Borrowing.GetBorrowedBooks;

namespace LMSWebAppClean.API.Endpoint
{
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

        private async Task<IResult> HandleBorrowBook([FromHeader(Name = "Bearer")] int authId, BookBorrowDTO borrowDTO, IMediator mediator)
        {
            try
            {
                if (authId <= 0)
                {
                    return Results.BadRequest("Bearer header must be a valid positive integer");
                }

                var command = new BorrowCommand(authId, borrowDTO.BookId, borrowDTO.MemberId);
                var book = await mediator.Send(command);
                
                return Results.Ok(book);
            }
            catch (UnauthorizedAccessException)
            {
                return Results.Forbid();
            }
            catch (KeyNotFoundException ex)
            {
                return Results.NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        private async Task<IResult> HandleReturnBook([FromHeader(Name = "Bearer")] int authId, BookBorrowDTO borrowDTO, IMediator mediator)
        {
            try
            {
                if (authId <= 0)
                {
                    return Results.BadRequest("Bearer header must be a valid positive integer");
                }

                var command = new ReturnCommand(authId, borrowDTO.BookId, borrowDTO.MemberId);
                var book = await mediator.Send(command);
                
                return Results.Ok(book);
            }
            catch (UnauthorizedAccessException)
            {
                return Results.Forbid();
            }
            catch (KeyNotFoundException ex)
            {
                return Results.NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        private async Task<IResult> HandleGetBorrowedBooksByMemberId([FromHeader(Name = "Bearer")] int authId, int memberId, IMediator mediator)
        {
            try
            {
                if (authId <= 0)
                {
                    return Results.BadRequest("Bearer header must be a valid positive integer");
                }

                var query = new GetBorrowedBooksQuery(authId, memberId);
                var books = await mediator.Send(query);
                
                return Results.Ok(books);
            }
            catch (UnauthorizedAccessException)
            {
                return Results.Forbid();
            }
            catch (KeyNotFoundException ex)
            {
                return Results.NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }
    }
}