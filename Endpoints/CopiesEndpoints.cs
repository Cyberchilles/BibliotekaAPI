using BibliotekaAPI.Data;
using BibliotekaAPI.Dto;
using BibliotekaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotekaAPI.Endpoints;

public static class CopiesEndpoints
{
    public static void MapCopies(this WebApplication app)
    {
        var copies = app.MapGroup("/copies");

        copies.MapGet("", async (int? bookId, LibraryDbContext db) =>
        {
            var query = db.Copies.AsNoTracking().AsQueryable();
            if (bookId.HasValue) query = query.Where(c => c.BookId == bookId.Value);
            return (await query.ToListAsync()).Select(CopyDto.From);
        });

        copies.MapGet("/{id:int}", async (int id, LibraryDbContext db) =>
        {
            var copy = await db.Copies.FindAsync(id);
            return copy is null ? Results.NotFound() : Results.Ok(CopyDto.From(copy));
        });

        copies.MapPost("", async (CopyInput input, LibraryDbContext db) =>
        {
            if (!await db.Books.AnyAsync(b => b.Id == input.BookId)) return Results.BadRequest();

            var copy = new Copy { BookId = input.BookId,
                IsAvailable = input.IsAvailable ?? true };
            db.Copies.Add(copy);
            await db.SaveChangesAsync();
            return Results.Created($"/copies/{copy.Id}", CopyDto.From(copy));
        });

        copies.MapPut("/{id:int}", async (int id, CopyInput input, LibraryDbContext db) =>
        {
            var copy = await db.Copies.FindAsync(id);
            if (copy is null) return Results.NotFound();
            if (!await db.Books.AnyAsync(b => b.Id == input.BookId)) return Results.BadRequest();

            copy.BookId = input.BookId;
            copy.IsAvailable = input.IsAvailable ?? true;
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        copies.MapDelete("/{id:int}", async (int id, LibraryDbContext db) =>
        {
            var copy = await db.Copies.FindAsync(id);
            if (copy is null) return Results.NotFound();

            db.Copies.Remove(copy);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });
    }
}
