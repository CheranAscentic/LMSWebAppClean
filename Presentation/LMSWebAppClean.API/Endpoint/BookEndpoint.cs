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
            var books = app.MapGroup("/api/books")
                .WithTags("Books")
                .WithOpenApi();

            // Get All Books
            books.MapPost("/list", HandleGetAllBooks)
                .WithName("GetAllBooks")
                .WithSummary("Get all books")
                .WithDescription("Returns a list of all books in the library")
                .Produces<StandardResponseObject<List<Book>>>(StatusCodes.Status200OK)
                .Produces<StandardResponseObject<List<Book>>>(StatusCodes.Status400BadRequest)
                .Produces<StandardResponseObject<List<Book>>>(StatusCodes.Status500InternalServerError);

            // Get Book with Id
            books.MapPost("/get", HandleGetBookById)
                .WithName("GetBookById")
                .WithSummary("Get a book by ID")
                .WithDescription("Retrieves a specific book by its ID")
                .Produces<StandardResponseObject<Book>>(StatusCodes.Status200OK)
                .Produces<StandardResponseObject<Book>>(StatusCodes.Status404NotFound)
                .Produces<StandardResponseObject<Book>>(StatusCodes.Status400BadRequest)
                .Produces<StandardResponseObject<Book>>(StatusCodes.Status500InternalServerError);

            // Create Book with BookDTO
            books.MapPost("/", HandleCreateBook)
                .WithName("CreateBook")
                .WithSummary("Create a new book")
                .WithDescription("Adds a new book to the library collection")
                .Produces<StandardResponseObject<Book>>(StatusCodes.Status201Created)
                .Produces<StandardResponseObject<Book>>(StatusCodes.Status400BadRequest)
                .Produces<StandardResponseObject<Book>>(StatusCodes.Status500InternalServerError);

            // Update Book with id to BookDTO
            books.MapPut("/", HandleUpdateBookById)
                .WithName("UpdateBook")
                .WithSummary("Update a book")
                .WithDescription("Updates a book's information by its ID")
                .Produces<StandardResponseObject<Book>>(StatusCodes.Status200OK)
                .Produces<StandardResponseObject<Book>>(StatusCodes.Status404NotFound)
                .Produces<StandardResponseObject<Book>>(StatusCodes.Status400BadRequest)
                .Produces<StandardResponseObject<Book>>(StatusCodes.Status500InternalServerError);

            // Delete Book with Id
            books.MapDelete("/", HandleDeleteBookById)
                .WithName("DeleteBook")
                .WithSummary("Delete a book")
                .WithDescription("Removes a book from the library by its ID")
                .Produces<StandardResponseObject<string>>(StatusCodes.Status200OK)
                .Produces<StandardResponseObject<string>>(StatusCodes.Status404NotFound)
                .Produces<StandardResponseObject<string>>(StatusCodes.Status400BadRequest)
                .Produces<StandardResponseObject<string>>(StatusCodes.Status500InternalServerError);
        }

        private async Task<IResult> HandleGetAllBooks(StandardRequestObject<EmptyRequestDTO> request, IMediator mediator)
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

                var query = new GetAllBooksQuery(request.Bearer);
                var books = await mediator.Send(query);

                var response = StandardResponseObject<List<Book>>.Ok(books, "Books retrieved successfully");
                return Results.Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                var unauthorizedResponse = StandardResponseObject<List<Book>>.BadRequest(
                    ex.Message,
                    "Unauthorized access");
                return Results.BadRequest(unauthorizedResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = StandardResponseObject<List<Book>>.InternalError(
                    ex.Message,
                    "An error occurred while retrieving books");
                return Results.Problem(detail: errorResponse.Error, statusCode: 500);
            }
        }

        private async Task<IResult> HandleGetBookById([FromBody] StandardRequestObject<GetByIdRequestDTO> request, [FromServices] IMediator mediator)
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
                        "Book ID is required",
                        "Invalid request format");
                    return Results.BadRequest(badRequestResponse);
                }

                var query = new GetBookByIdQuery(request.Bearer, request.Data.Id);
                var book = await mediator.Send(query);

                var response = StandardResponseObject<Book>.Ok(book, "Book retrieved successfully");
                return Results.Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                var unauthorizedResponse = StandardResponseObject<Book>.BadRequest(
                    ex.Message,
                    "Unauthorized access");
                return Results.BadRequest(unauthorizedResponse);
            }
            catch (KeyNotFoundException)
            {
                var notFoundResponse = StandardResponseObject<Book>.NotFound(
                    $"Book with ID {request.Data?.Id} not found.",
                    "Book not found");
                return Results.NotFound(notFoundResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = StandardResponseObject<Book>.InternalError(
                    ex.Message,
                    "An error occurred while retrieving the book");
                return Results.Problem(detail: errorResponse.Error, statusCode: 500);
            }
        }

        private async Task<IResult> HandleCreateBook([FromBody] StandardRequestObject<BookDTO> request, [FromServices] IMediator mediator)
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
                        "Book data is required",
                        "Invalid request format");
                    return Results.BadRequest(badRequestResponse);
                }

                var command = new CreateBookCommand(
                    request.Bearer,
                    request.Data.Title,
                    request.Data.Author,
                    request.Data.Year,
                    request.Data.Category
                );

                var book = await mediator.Send(command);
                var response = StandardResponseObject<Book>.Created(book, "Book created successfully");
                return Results.Created($"/api/books/{book.Id}", response);
            }
            catch (UnauthorizedAccessException ex)
            {
                var unauthorizedResponse = StandardResponseObject<Book>.BadRequest(
                    ex.Message,
                    "Unauthorized access");
                return Results.BadRequest(unauthorizedResponse);
            }
            catch (ArgumentException ex)
            {
                var badRequestResponse = StandardResponseObject<Book>.BadRequest(
                    ex.Message,
                    "Book creation failed");
                return Results.BadRequest(badRequestResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = StandardResponseObject<Book>.InternalError(
                    ex.Message,
                    "An error occurred while creating the book");
                return Results.Problem(detail: errorResponse.Error, statusCode: 500);
            }
        }

        private async Task<IResult> HandleUpdateBookById([FromBody] StandardRequestObject<BookUpdateDTO> request, [FromServices] IMediator mediator)
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
                        "Book update data is required",
                        "Invalid request format");
                    return Results.BadRequest(badRequestResponse);
                }

                var command = new UpdateBookCommand(
                    request.Bearer,
                    request.Data.Id,
                    request.Data.Title,
                    request.Data.Author,
                    request.Data.Year,
                    request.Data.Category
                );

                var book = await mediator.Send(command);
                var response = StandardResponseObject<Book>.Ok(book, "Book updated successfully");
                return Results.Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                var unauthorizedResponse = StandardResponseObject<Book>.BadRequest(
                    ex.Message,
                    "Unauthorized access");
                return Results.BadRequest(unauthorizedResponse);
            }
            catch (KeyNotFoundException)
            {
                var notFoundResponse = StandardResponseObject<Book>.NotFound(
                    $"Book with ID {request.Data?.Id} not found.",
                    "Book not found");
                return Results.NotFound(notFoundResponse);
            }
            catch (ArgumentException ex)
            {
                var badRequestResponse = StandardResponseObject<Book>.BadRequest(
                    ex.Message,
                    "Book update failed");
                return Results.BadRequest(badRequestResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = StandardResponseObject<Book>.InternalError(
                    ex.Message,
                    "An error occurred while updating the book");
                return Results.Problem(detail: errorResponse.Error, statusCode: 500);
            }
        }

        private async Task<IResult> HandleDeleteBookById([FromBody] StandardRequestObject<GetByIdRequestDTO> request, [FromServices] IMediator mediator)
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
                        "Book ID is required",
                        "Invalid request format");
                    return Results.BadRequest(badRequestResponse);
                }

                var command = new DeleteBookCommand(request.Bearer, request.Data.Id);
                await mediator.Send(command);
                
                var response = StandardResponseObject<string>.Ok("Book deleted successfully", "Book deleted successfully");
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
                    $"Book with ID {request.Data?.Id} not found.",
                    "Book not found");
                return Results.NotFound(notFoundResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = StandardResponseObject<string>.InternalError(
                    ex.Message,
                    "An error occurred while deleting the book");
                return Results.Problem(detail: errorResponse.Error, statusCode: 500);
            }
        }
    }
}
