using LMSWebAppClean.Application.DTO;
using LMSWebAppClean.API.Interface;
using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Domain.Model;
using Microsoft.AspNetCore.Mvc;

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

            books.MapGet("/", ([FromHeader(Name = "X-User-Id")] int authId, IBookService bookService) =>
            {
                if (authId <= 0)
                {
                    return Results.BadRequest("X-User-Id header must be a valid positive integer");
                }

                return Results.Ok(bookService.GetBooks(authId));
            })
            .WithName("GetAllBooks")
            .WithSummary("Get all books")
            .WithDescription("Returns a list of all books in the library")
            .Produces<List<Book>>(StatusCodes.Status200OK);

            // Get Book with Id
            books.MapGet("/{bookId}", (int bookId, [FromHeader(Name = "X-User-Id")] int authId, IBookService bookService) =>
            {
                try
                {
                    if (authId <= 0)
                    {
                        return Results.BadRequest("X-User-Id header must be a valid positive integer");
                    }

                    var book = bookService.GetBook(authId, bookId);
                    return Results.Ok(book);
                }
                catch (Exception e)
                {
                    return Results.NotFound("Book with Id could not be found.");
                }
            })
            .WithName("GetBookById")
            .WithSummary("Get a book by ID")
            .WithDescription("Retrieves a specific book by its ID")
            .Produces<Book>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound);

            // Create Book with BookDTO
            books.MapPost("/", (BookDTO bookDTO, [FromHeader(Name = "X-User-Id")] int authId, IBookService bookService) =>
            {
                try
                {
                    if (authId <= 0)
                    {
                        return Results.BadRequest("X-User-Id header must be a valid positive integer");
                    }

                    var book = bookService.AddBook(authId, bookDTO.Title, bookDTO.Author, bookDTO.Year, bookDTO.Category);
                    return Results.Created($"/api/books/{book.Id}", book);
                }
                catch (Exception e)
                {
                    return Results.NotFound("Book could not be created");
                }
            })
            .WithName("CreateBook")
            .WithSummary("Create a new book")
            .WithDescription("Adds a new book to the library collection")
            .Produces<Book>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest);

            // Update Book with id to BookDTO
            books.MapPut("/{bookId}", (int bookId, BookDTO bookDTO, [FromHeader(Name = "X-User-Id")] int authId, IBookService bookService) =>
            {
                try
                {
                    if (authId <= 0)
                    {
                        return Results.BadRequest("X-User-Id header must be a valid positive integer");
                    }

                    var book = bookService.UpdateBook(authId, bookId, bookDTO.Title, bookDTO.Author, bookDTO.Year, bookDTO.Category);
                    return Results.Ok(book);
                }
                catch (Exception e)
                {
                    return Results.NotFound("Book with Id could not be updated");
                }
            })
            .WithName("UpdateBook")
            .WithSummary("Update a book")
            .WithDescription("Updates a book's information by its ID")
            .Produces<Book>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound);

            // Delete Book with Id
            books.MapDelete("/{bookId}", (int bookId, [FromHeader(Name = "X-User-Id")] int authId, IBookService bookService) =>
            {
                try
                {
                    if (authId <= 0)
                    {
                        return Results.BadRequest("X-User-Id header must be a valid positive integer");
                    }

                    var book = bookService.RemoveBook(authId, bookId);
                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    return Results.NotFound("Book with Id could not be deleted.");
                }
            })
            .WithName("DeleteBook")
            .WithSummary("Delete a book")
            .WithDescription("Removes a book from the library by its ID")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound);
        }
    }
}
