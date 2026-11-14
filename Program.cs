using BibliotekaAPI.Data;
using BibliotekaAPI.Endpoints;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Baza SQLite zapisuje dane w pliku projektu
var dbFile = Path.Combine(builder.Environment.ContentRootPath, "biblioteka.db");
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlite($"Data Source={dbFile}"));

var app = builder.Build();

// Tworzymy baze z migracji przy pierwszym uruchomieniu
using (var scope = app.Services.CreateScope())
    scope.ServiceProvider.GetRequiredService<LibraryDbContext>().Database.Migrate();

// Tutaj jest tez strona z testami z zadania
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapAuthors();
app.MapBooks();
app.MapCopies();

app.Run();
