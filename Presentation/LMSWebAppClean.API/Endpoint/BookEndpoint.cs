using LMSWebAppClean.Application.Usecase.Books.GetAllBooks;
using LMSWebAppClean.Application.Usecase.Books.GetBookById;
using LMSWebAppClean.Application.Usecase.Books.CreateBook;
using LMSWebAppClean.Application.Usecase.Books.UpdateBook;
using LMSWebAppClean.Application.Usecase.Books.DeleteBook;
using LMSWebAppClean.API.Interface;
using LMSWebAppClean.Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using LMSWebAppClean.Application.DTO;

namespace LMSWebAppClean.API.Endpoint
{
    public class BookEndpoint : IEndpointGroup
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            // Get All Books
            var books = app.MapGroup("/api/books")
                .WithTags("Books")
                .WithOpenApi();

            books.MapGet("/", HandleGetAllBooks)
            .WithName("GetAllBooks")
            .WithSummary("Get all books")
            .WithDescription("Returns a list of all books in the library")
            .Produces<List<Book>>(StatusCodes.Status200OK);

            // Get Book with Id
            books.MapGet("/{bookId}", HandleGetBookById)
            .WithName("GetBookById")
            .WithSummary("Get a book by ID")
            .WithDescription("Retrieves a specific book by its ID")
            .Produces<Book>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound);

            // Create Book with BookDTO
            books.MapPost("/", HandleCreateBook)
            .WithName("CreateBook")
            .WithSummary("Create a new book")
            .WithDescription("Adds a new book to the library collection")
            .Produces<Book>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest);

            // Update Book with id to BookDTO
            books.MapPut("/{bookId}", HandleUpdateBookById)
            .WithName("UpdateBook")
            .WithSummary("Update a book")
            .WithDescription("Updates a book's information by its ID")
            .Produces<Book>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound);

            // Delete Book with Id
            books.MapDelete("/{bookId}", HandleDeleteBookById)
            .WithName("DeleteBook")
            .WithSummary("Delete a book")
            .WithDescription("Removes a book from the library by its ID")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound);
        }

        private async Task<IResult> HandleGetAllBooks([FromHeader(Name = "Bearer")] int authId, IMediator mediator)
        {
            try
            {
                if (authId <= 0)
                {
                    return Results.BadRequest("Bearer header must be a valid positive integer");
                }

                var query = new GetAllBooksQuery(authId);
                var books = await mediator.Send(query);

                return Results.Ok(books);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Results.Forbid();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        private async Task<IResult> HandleGetBookById(int bookId, [FromHeader(Name = "Bearer")] int authId, IMediator mediator)
        {
            try
            {
                if (authId <= 0)
                {
                    return Results.BadRequest("Bearer header must be a valid positive integer");
                }

                var query = new GetBookByIdQuery(authId, bookId);
                var book = await mediator.Send(query);

                return Results.Ok(book);
            }
            catch (UnauthorizedAccessException)
            {
                return Results.Forbid();
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound($"Book with ID {bookId} not found.");
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        private async Task<IResult> HandleCreateBook(BookDTO bookDTO, [FromHeader(Name = "Bearer")] int authId, IMediator mediator)
        {
            try
            {
                if (authId <= 0)
                {
                    return Results.BadRequest("Bearer header must be a valid positive integer");
                }
                
                var command = new CreateBookCommand(
                    authId,
                    bookDTO.Title,
                    bookDTO.Author,
                    bookDTO.Year,
                    bookDTO.Category
                );

                var book = await mediator.Send(command);
                return Results.Created($"/api/books/{book.Id}", book);
            }
            catch (UnauthorizedAccessException)
            {
                return Results.Forbid();
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

        private async Task<IResult> HandleUpdateBookById(int bookId, BookDTO bookDTO, [FromHeader(Name = "Bearer")] int authId, IMediator mediator)
        {
            try
            {
                if (authId <= 0)
                {
                    return Results.BadRequest("Bearer header must be a valid positive integer");
                }

                var command = new UpdateBookCommand(
                    authId,
                    bookId,
                    bookDTO.Title,
                    bookDTO.Author,
                    bookDTO.Year,
                    bookDTO.Category
                );

                var book = await mediator.Send(command);
                return Results.Ok(book);
            }
            catch (UnauthorizedAccessException)
            {
                return Results.Forbid();
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound($"Book with ID {bookId} not found.");
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

        private async Task<IResult> HandleDeleteBookById(int bookId, [FromHeader(Name = "Bearer")] int authId, IMediator mediator)
        {
            try
            {
                if (authId <= 0)
                {
                    return Results.BadRequest("Bearer header must be a valid positive integer");
                }

                var command = new DeleteBookCommand(authId, bookId);
                await mediator.Send(command);
                
                return Results.NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                return Results.Forbid();
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound($"Book with ID {bookId} not found.");
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }
    }
}
