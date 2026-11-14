using BibliotekaAPI.Models;
using System.Text.Json.Serialization;

namespace BibliotekaAPI.Dto;

public record AuthorDto(
    int Id,
    [property: JsonPropertyName("first_name")] string FirstName,
    [property: JsonPropertyName("last_name")] string LastName)
{
    public static AuthorDto From(Author a) => new(a.Id, a.FirstName, a.LastName);
}

public record BookDto(int Id, string Title, int Year, AuthorDto Author)
{
    public static BookDto From(Book b) =>
        new(b.Id, b.Title, b.Year, AuthorDto.From(b.Author));
}

public record CopyDto(int Id, int BookId, bool IsAvailable)
{
    public static CopyDto From(Copy c) => new(c.Id, c.BookId, c.IsAvailable);
}
