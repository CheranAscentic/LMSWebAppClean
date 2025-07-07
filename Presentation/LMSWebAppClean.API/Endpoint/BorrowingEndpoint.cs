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
                .Produces<StandardResponseObject<Book>>(StatusCodes.Status200OK)
                .Produces<StandardResponseObject<Book>>(StatusCodes.Status400BadRequest)
                .Produces<StandardResponseObject<Book>>(StatusCodes.Status404NotFound)
                .Produces<StandardResponseObject<Book>>(StatusCodes.Status500InternalServerError);

            // POST return book
            borrowing.MapPost("/return", HandleReturnBook)
                .WithName("ReturnBook")
                .WithSummary("Return a borrowed book")
                .WithDescription("Allows a member to return a previously borrowed book")
                .Produces<StandardResponseObject<Book>>(StatusCodes.Status200OK)
                .Produces<StandardResponseObject<Book>>(StatusCodes.Status400BadRequest)
                .Produces<StandardResponseObject<Book>>(StatusCodes.Status404NotFound)
                .Produces<StandardResponseObject<Book>>(StatusCodes.Status500InternalServerError);

            // GET borrowed books by member ID
            borrowing.MapPost("/member/books", HandleGetBorrowedBooksByMemberId)
                .WithName("GetBorrowedBooks")
                .WithSummary("Get borrowed books by member")
                .WithDescription("Retrieves all books currently borrowed by a specific member")
                .Produces<StandardResponseObject<List<Book>>>(StatusCodes.Status200OK)
                .Produces<StandardResponseObject<List<Book>>>(StatusCodes.Status400BadRequest)
                .Produces<StandardResponseObject<List<Book>>>(StatusCodes.Status404NotFound)
                .Produces<StandardResponseObject<List<Book>>>(StatusCodes.Status500InternalServerError);
        }

        private async Task<IResult> HandleBorrowBook([FromBody] StandardRequestObject<BookBorrowDTO> request, [FromServices] IMediator mediator)
        {
            try
            {
                if (request.Bearer <= 0)
                {
                    var badRequestResponse = StandardResponseObject<Book>.BadRequest(
                        "Bearer token must be a valid positive integer",
                        "Invalid bearer token");
                    return Results.BadRequest(badRequestResponse);
                }

                if (request.Data == null)
                {
                    var badRequestResponse = StandardResponseObject<Book>.BadRequest(
                        "Borrow data is required",
                        "Invalid request format");
                    return Results.BadRequest(badRequestResponse);
                }

                var command = new BorrowCommand(request.Bearer, request.Data.BookId, request.Data.MemberId);
                var book = await mediator.Send(command);
                
                var response = StandardResponseObject<Book>.Ok(book, "Book borrowed successfully");
                return Results.Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                var unauthorizedResponse = StandardResponseObject<Book>.BadRequest(
                    ex.Message,
                    "Unauthorized access");
                return Results.BadRequest(unauthorizedResponse);
            }
            catch (KeyNotFoundException ex)
            {
                var notFoundResponse = StandardResponseObject<Book>.NotFound(
                    ex.Message,
                    "Resource not found");
                return Results.NotFound(notFoundResponse);
            }
            catch (InvalidOperationException ex)
            {
                var badRequestResponse = StandardResponseObject<Book>.BadRequest(
                    ex.Message,
                    "Borrow operation failed");
                return Results.BadRequest(badRequestResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = StandardResponseObject<Book>.InternalError(
                    ex.Message,
                    "An error occurred while borrowing the book");
                return Results.Problem(detail: errorResponse.Error, statusCode: 500);
            }
        }

        private async Task<IResult> HandleReturnBook([FromBody] StandardRequestObject<BookBorrowDTO> request, [FromServices] IMediator mediator)
        {
            try
            {
                if (request.Bearer <= 0)
                {
                    var badRequestResponse = StandardResponseObject<Book>.BadRequest(
                        "Bearer token must be a valid positive integer",
                        "Invalid bearer token");
                    return Results.BadRequest(badRequestResponse);
                }

                if (request.Data == null)
                {
                    var badRequestResponse = StandardResponseObject<Book>.BadRequest(
                        "Return data is required",
                        "Invalid request format");
                    return Results.BadRequest(badRequestResponse);
                }

                var command = new ReturnCommand(request.Bearer, request.Data.BookId, request.Data.MemberId);
                var book = await mediator.Send(command);
                
                var response = StandardResponseObject<Book>.Ok(book, "Book returned successfully");
                return Results.Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                var unauthorizedResponse = StandardResponseObject<Book>.BadRequest(
                    ex.Message,
                    "Unauthorized access");
                return Results.BadRequest(unauthorizedResponse);
            }
            catch (KeyNotFoundException ex)
            {
                var notFoundResponse = StandardResponseObject<Book>.NotFound(
                    ex.Message,
                    "Resource not found");
                return Results.NotFound(notFoundResponse);
            }
            catch (InvalidOperationException ex)
            {
                var badRequestResponse = StandardResponseObject<Book>.BadRequest(
                    ex.Message,
                    "Return operation failed");
                return Results.BadRequest(badRequestResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = StandardResponseObject<Book>.InternalError(
                    ex.Message,
                    "An error occurred while returning the book");
                return Results.Problem(detail: errorResponse.Error, statusCode: 500);
            }
        }

        private async Task<IResult> HandleGetBorrowedBooksByMemberId([FromBody] StandardRequestObject<GetByIdRequestDTO> request, [FromServices] IMediator mediator)
        {
            try
            {
                if (request.Bearer <= 0)
                {
                    var badRequestResponse = StandardResponseObject<List<Book>>.BadRequest(
                        "Bearer token must be a valid positive integer",
                        "Invalid bearer token");
                    return Results.BadRequest(badRequestResponse);
                }

                if (request.Data == null)
                {
                    var badRequestResponse = StandardResponseObject<List<Book>>.BadRequest(
                        "Member ID is required",
                        "Invalid request format");
                    return Results.BadRequest(badRequestResponse);
                }

                var query = new GetBorrowedBooksQuery(request.Bearer, request.Data.Id);
                var books = await mediator.Send(query);
                
                var response = StandardResponseObject<List<Book>>.Ok(books, "Borrowed books retrieved successfully");
                return Results.Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                var unauthorizedResponse = StandardResponseObject<List<Book>>.BadRequest(
                    ex.Message,
                    "Unauthorized access");
                return Results.BadRequest(unauthorizedResponse);
            }
            catch (KeyNotFoundException ex)
            {
                var notFoundResponse = StandardResponseObject<List<Book>>.NotFound(
                    ex.Message,
                    "Member not found");
                return Results.NotFound(notFoundResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = StandardResponseObject<List<Book>>.InternalError(
                    ex.Message,
                    "An error occurred while retrieving borrowed books");
                return Results.Problem(detail: errorResponse.Error, statusCode: 500);
            }
        }
    }
}