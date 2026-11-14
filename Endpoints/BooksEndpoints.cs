using BibliotekaAPI.Data;
using BibliotekaAPI.Dto;
using BibliotekaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotekaAPI.Endpoints;

public static class BooksEndpoints
{
    public static void MapBooks(this WebApplication app)
    {
        var books = app.MapGroup("/books");

        books.MapGet("", async (int? authorId, LibraryDbContext db) =>
        {
            var query = db.Books.AsNoTracking().Include(b => b.Author).AsQueryable();
            if (authorId.HasValue) query = query.Where(b => b.AuthorId == authorId.Value);
            return (await query.ToListAsync()).Select(BookDto.From);
        });

        books.MapGet("/{id:int}", async (int id, LibraryDbContext db) =>
        {
            var book = await db.Books.AsNoTracking().Include(b => b.Author)
                .FirstOrDefaultAsync(b => b.Id == id);
            return book is null ? Results.NotFound() : Results.Ok(BookDto.From(book));
        });

        books.MapPost("", async (BookInput input, LibraryDbContext db) =>
        {
            if (Invalid(input)) return Results.BadRequest();
            var author = await db.Authors.FindAsync(input.AuthorId);
            if (author is null) return Results.BadRequest();

            var book = new Book { Title = input.Title!, Year = input.Year,
                AuthorId = author.Id, Author = author };
            db.Books.Add(book);
            await db.SaveChangesAsync();
            return Results.Created($"/books/{book.Id}", BookDto.From(book));
        });

        books.MapPut("/{id:int}", async (int id, BookInput input, LibraryDbContext db) =>
        {
            var book = await db.Books.FindAsync(id);
            if (book is null) return Results.NotFound();
            if (Invalid(input) || !await db.Authors.AnyAsync(a => a.Id == input.AuthorId))
                return Results.BadRequest();

            book.Title = input.Title!;
            book.Year = input.Year;
            book.AuthorId = input.AuthorId;
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        books.MapDelete("/{id:int}", async (int id, LibraryDbContext db) =>
        {
            var book = await db.Books.FindAsync(id);
            if (book is null) return Results.NotFound();

            db.Books.Remove(book);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });
    }

    private static bool Invalid(BookInput b) =>
        string.IsNullOrWhiteSpace(b.Title) || b.Year < 0 || b.AuthorId <= 0;
}
