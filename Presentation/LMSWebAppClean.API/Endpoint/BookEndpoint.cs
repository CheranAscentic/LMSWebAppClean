using LMSWebAppClean.API.Interface;
using LMSWebAppClean.Application.DTO;
using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Application.Usecase.Books.CreateBook;
using LMSWebAppClean.Application.Usecase.Books.GetAllBooks;
using LMSWebAppClean.Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

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

        private async Task<IResult> HandleGetAllBooks([FromHeader(Name = "Bearer")] int authId, IBookService bookService, IMediator mediator)
        {
            if (authId <= 0)
            {
                return Results.BadRequest("Bearer header must be a valid positive integer");
            }

            var query = new GetAllBooksQuery(authId);
            var books = await mediator.Send(query);

            return Results.Ok(books);
        }

        private IResult HandleGetBookById(int bookId, [FromHeader(Name = "Bearer")] int authId, IBookService bookService)
        {
            try
            {
                if (authId <= 0)
                {
                    return Results.BadRequest("Bearer header must be a valid positive integer");
                }

                var book = bookService.GetBook(authId, bookId);
                return Results.Ok(book);
            }
            catch (Exception e)
            {
                return Results.NotFound("Book with Id could not be found.");
            }
        }

        private async Task<IResult> HandleCreateBook(BookDTO bookDTO, [FromHeader(Name = "Bearer")] int authId, IBookService bookService, IMediator mediator)
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
            catch (Exception e)
            {
                return Results.NotFound("Book could not be created");
            }
        }

        private IResult HandleUpdateBookById(int bookId, BookDTO bookDTO, [FromHeader(Name = "Bearer")] int authId, IBookService bookService)
        {
            try
            {
                if (authId <= 0)
                {
                    return Results.BadRequest("Bearer header must be a valid positive integer");
                }

                var book = bookService.UpdateBook(authId, bookId, bookDTO.Title, bookDTO.Author, bookDTO.Year, bookDTO.Category);
                return Results.Ok(book);
            }
            catch (Exception e)
            {
                return Results.NotFound("Book with Id could not be updated");
            }
        }

        private IResult HandleDeleteBookById(int bookId, [FromHeader(Name = "Bearer")] int authId, IBookService bookService)
        {
            try
            {
                if (authId <= 0)
                {
                    return Results.BadRequest("Bearer header must be a valid positive integer");
                }

                var book = bookService.RemoveBook(authId, bookId);
                return Results.NoContent();
            }
            catch (Exception ex)
            {
                return Results.NotFound("Book with Id could not be deleted.");
            }
        }
    }
}
