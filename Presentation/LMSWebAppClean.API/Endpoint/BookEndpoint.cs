using LMSWebAppClean.Application.Usecase.Books.GetAllBooks;
using LMSWebAppClean.Application.Usecase.Books.GetBookById;
using LMSWebAppClean.Application.Usecase.Books.CreateBook;
using LMSWebAppClean.Application.Usecase.Books.UpdateBook;
using LMSWebAppClean.Application.Usecase.Books.DeleteBook;
using LMSWebAppClean.API.Interface;
using LMSWebAppClean.Domain.Model;
using LMSWebAppClean.Application.Interface;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using LMSWebAppClean.Application.DTO;
using LMSWebAppClean.Domain.Enum;

namespace LMSWebAppClean.API.Endpoint
{
    public class BookEndpoint : IEndpointGroup
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            var books = app.MapGroup("/api/books")
                .WithTags("Books")
                .WithOpenApi();

            // Get All Books - Public endpoint (no permission check needed)
            books.MapPost("/list", HandleGetAllBooks)
                .WithName("GetAllBooks")
                .WithSummary("Get all books")
                .WithDescription("Returns a list of all books in the library")
                .Produces<StandardResponseObject<List<Book>>>(StatusCodes.Status200OK)
                .Produces<StandardResponseObject<List<Book>>>(StatusCodes.Status400BadRequest)
                .Produces<StandardResponseObject<List<Book>>>(StatusCodes.Status500InternalServerError);

            // Get Book with Id - Public endpoint (no permission check needed)
            books.MapPost("/get", HandleGetBookById)
                .WithName("GetBookById")
                .WithSummary("Get a book by ID")
                .WithDescription("Retrieves a specific book by its ID")
                .Produces<StandardResponseObject<Book>>(StatusCodes.Status200OK)
                .Produces<StandardResponseObject<Book>>(StatusCodes.Status404NotFound)
                .Produces<StandardResponseObject<Book>>(StatusCodes.Status400BadRequest)
                .Produces<StandardResponseObject<Book>>(StatusCodes.Status500InternalServerError);

                // Create Book - Requires Staff Permission
            books.MapPost("/", HandleCreateBook)
                .WithName("CreateBook")
                .RequireAuthorization()
                .WithSummary("Create a new book")
                .WithDescription("Adds a new book to the library collection. Requires staff permissions.")
                .Produces<StandardResponseObject<Book>>(StatusCodes.Status201Created)
                .Produces<StandardResponseObject<Book>>(StatusCodes.Status400BadRequest)
                .Produces<StandardResponseObject<Book>>(StatusCodes.Status401Unauthorized)
                .Produces<StandardResponseObject<Book>>(StatusCodes.Status403Forbidden)
                .Produces<StandardResponseObject<Book>>(StatusCodes.Status500InternalServerError);

            // Update Book - Requires Staff Permission
            books.MapPut("/", HandleUpdateBookById)
                .WithName("UpdateBook")
                .RequireAuthorization()
                .WithSummary("Update a book")
                .WithDescription("Updates a book's information by its ID. Requires staff permissions.")
                .Produces<StandardResponseObject<Book>>(StatusCodes.Status200OK)
                .Produces<StandardResponseObject<Book>>(StatusCodes.Status404NotFound)
                .Produces<StandardResponseObject<Book>>(StatusCodes.Status400BadRequest)
                .Produces<StandardResponseObject<Book>>(StatusCodes.Status401Unauthorized)
                .Produces<StandardResponseObject<Book>>(StatusCodes.Status403Forbidden)
                .Produces<StandardResponseObject<Book>>(StatusCodes.Status500InternalServerError);

            // Delete Book - Requires Staff Permission
            books.MapDelete("/", HandleDeleteBookById)
                .WithName("DeleteBook")
                .RequireAuthorization()
                .WithSummary("Delete a book")
                .WithDescription("Removes a book from the library by its ID. Requires staff permissions.")
                .Produces<StandardResponseObject<string>>(StatusCodes.Status200OK)
                .Produces<StandardResponseObject<string>>(StatusCodes.Status404NotFound)
                .Produces<StandardResponseObject<string>>(StatusCodes.Status400BadRequest)
                .Produces<StandardResponseObject<string>>(StatusCodes.Status401Unauthorized)
                .Produces<StandardResponseObject<string>>(StatusCodes.Status403Forbidden)
                .Produces<StandardResponseObject<string>>(StatusCodes.Status500InternalServerError);
        }

        private async Task<IResult> HandleGetAllBooks(
            StandardRequestObject<GetAllBooksQuery> request, 
            IMediator mediator)
        {
            try
            {
                // No permission check needed - public endpoint
                var books = await mediator.Send(request.Data);
                var response = StandardResponseObject<List<Book>>.Ok(books, "Books retrieved successfully");
                return Results.Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = StandardResponseObject<List<Book>>.InternalError(
                    ex.Message,
                    "An error occurred while retrieving books");
                return Results.Problem(detail: errorResponse.Error, statusCode: 500);
            }
        }

        private async Task<IResult> HandleGetBookById(
            [FromBody] StandardRequestObject<GetBookByIdQuery> request, 
            [FromServices] IMediator mediator)
        {
            try
            {
                // No permission check needed - public endpoint
                var book = await mediator.Send(request.Data);
                var response = StandardResponseObject<Book>.Ok(book, "Book retrieved successfully");
                return Results.Ok(response);
            }
            catch (KeyNotFoundException)
            {
                var notFoundResponse = StandardResponseObject<Book>.NotFound(
                    $"Book not found.",
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

        private async Task<IResult> HandleCreateBook(
            [FromBody] StandardRequestObject<CreateBookCommand> request, 
            [FromServices] IMediator mediator,
            [FromServices] ICurrentUserService currentUserService,
            [FromServices] IPermissionChecker permissionChecker)
        {
            try
            {
                // Check permission at endpoint level
                var currentUserId = currentUserService.DomainUserId;
                if (!currentUserId.HasValue)
                {
                    var unauthorizedResponse = StandardResponseObject<Book>.BadRequest(
                        "User is not authenticated.",
                        "Authentication required");
                    return Results.Unauthorized();
                }

                // Check if user has permission to create books
                permissionChecker.Check(
                    currentUserId.Value, 
                    Permission.Process.CreateBook, 
                    "You don't have permission to create books.");

                var book = await mediator.Send(request.Data);
                var response = StandardResponseObject<Book>.Created(book, "Book created successfully");
                return Results.Created($"/api/books/{book.Id}", response);
            }
            catch (UnauthorizedAccessException ex)
            {
                var forbiddenResponse = StandardResponseObject<Book>.BadRequest(
                    ex.Message,
                    "Insufficient permissions");
                return Results.StatusCode(403); // Forbidden
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

        private async Task<IResult> HandleUpdateBookById(
            [FromBody] StandardRequestObject<UpdateBookCommand> request, 
            [FromServices] IMediator mediator,
            [FromServices] ICurrentUserService currentUserService,
            [FromServices] IPermissionChecker permissionChecker)
        {
            try
            {
                // Check permission at endpoint level
                var currentUserId = currentUserService.DomainUserId;
                if (!currentUserId.HasValue)
                {
                    return Results.Unauthorized();
                }

                // Check if user has permission to update books
                permissionChecker.Check(
                    currentUserId.Value, 
                    Permission.Process.UpdateBook, 
                    "You don't have permission to update books.");

                var book = await mediator.Send(request.Data);
                var response = StandardResponseObject<Book>.Ok(book, "Book updated successfully");
                return Results.Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                var forbiddenResponse = StandardResponseObject<Book>.BadRequest(
                    ex.Message,
                    "Insufficient permissions");
                return Results.StatusCode(403); // Forbidden
            }
            catch (KeyNotFoundException)
            {
                var notFoundResponse = StandardResponseObject<Book>.NotFound(
                    $"Book not found.",
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

        private async Task<IResult> HandleDeleteBookById(
            [FromBody] StandardRequestObject<DeleteBookCommand> request, 
            [FromServices] IMediator mediator,
            [FromServices] ICurrentUserService currentUserService,
            [FromServices] IPermissionChecker permissionChecker)
        {
            try
            {
                // Check permission at endpoint level
                var currentUserId = currentUserService.DomainUserId;
                if (!currentUserId.HasValue)
                {
                    return Results.Unauthorized();
                }

                // Check if user has permission to delete books
                permissionChecker.Check(
                    currentUserId.Value, 
                    Permission.Process.DeleteBook, 
                    "You don't have permission to delete books.");

                await mediator.Send(request.Data);
                var response = StandardResponseObject<string>.Ok("Book deleted successfully", "Book deleted successfully");
                return Results.Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                var forbiddenResponse = StandardResponseObject<string>.BadRequest(
                    ex.Message,
                    "Insufficient permissions");
                return Results.StatusCode(403); // Forbidden
            }
            catch (KeyNotFoundException)
            {
                var notFoundResponse = StandardResponseObject<string>.NotFound(
                    $"Book not found.",
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
