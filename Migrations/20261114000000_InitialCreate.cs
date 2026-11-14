using BibliotekaAPI.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibliotekaAPI.Migrations;

[DbContext(typeof(LibraryDbContext))]
[Migration("20261114000000_InitialCreate")]
public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Authors",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                FirstName = table.Column<string>(type: "TEXT", nullable: false),
                LastName = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_Authors", x => x.Id));

        migrationBuilder.CreateTable(
            name: "Books",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Title = table.Column<string>(type: "TEXT", nullable: false),
                Year = table.Column<int>(type: "INTEGER", nullable: false),
                AuthorId = table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Books", x => x.Id);
                table.ForeignKey("FK_Books_Authors_AuthorId", x => x.AuthorId,
                    "Authors", "Id", onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Copies",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                BookId = table.Column<int>(type: "INTEGER", nullable: false),
                IsAvailable = table.Column<bool>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Copies", x => x.Id);
                table.ForeignKey("FK_Copies_Books_BookId", x => x.BookId,
                    "Books", "Id", onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex("IX_Books_AuthorId", "Books", "AuthorId");
        migrationBuilder.CreateIndex("IX_Copies_BookId", "Copies", "BookId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable("Copies");
        migrationBuilder.DropTable("Books");
        migrationBuilder.DropTable("Authors");
    }
}
