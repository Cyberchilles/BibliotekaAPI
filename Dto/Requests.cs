using System.Text.Json.Serialization;

namespace BibliotekaAPI.Dto;

public record AuthorInput(
    [property: JsonPropertyName("first_name")] string? FirstName,
    [property: JsonPropertyName("last_name")] string? LastName);

public record BookInput(string? Title, int Year, int AuthorId);

// Dodatkowe pole egzemplarza okresla, czy jest dostepny.
public record CopyInput(int BookId, bool? IsAvailable);
