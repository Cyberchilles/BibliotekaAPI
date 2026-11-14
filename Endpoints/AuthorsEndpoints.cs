using BibliotekaAPI.Data;
using BibliotekaAPI.Dto;
using BibliotekaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotekaAPI.Endpoints;

public static class AuthorsEndpoints
{
    public static void MapAuthors(this WebApplication app)
    {
        var authors = app.MapGroup("/authors");

        authors.MapGet("", async (LibraryDbContext db) =>
            (await db.Authors.AsNoTracking().ToListAsync()).Select(AuthorDto.From));

        authors.MapGet("/{id:int}", async (int id, LibraryDbContext db) =>
        {
            var author = await db.Authors.FindAsync(id);
            return author is null ? Results.NotFound() : Results.Ok(AuthorDto.From(author));
        });

        authors.MapPost("", async (AuthorInput input, LibraryDbContext db) =>
        {
            if (Invalid(input)) return Results.BadRequest();

            var author = new Author { FirstName = input.FirstName!, LastName = input.LastName! };
            db.Authors.Add(author);
            await db.SaveChangesAsync();
            return Results.Created($"/authors/{author.Id}", AuthorDto.From(author));
        });

        authors.MapPut("/{id:int}", async (int id, AuthorInput input, LibraryDbContext db) =>
        {
            var author = await db.Authors.FindAsync(id);
            if (author is null) return Results.NotFound();
            if (Invalid(input)) return Results.BadRequest();

            author.FirstName = input.FirstName!;
            author.LastName = input.LastName!;
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        authors.MapDelete("/{id:int}", async (int id, LibraryDbContext db) =>
        {
            var author = await db.Authors.FindAsync(id);
            if (author is null) return Results.NotFound();

            db.Authors.Remove(author);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });
    }

    private static bool Invalid(AuthorInput a) =>
        string.IsNullOrWhiteSpace(a.FirstName) || string.IsNullOrWhiteSpace(a.LastName);
}
