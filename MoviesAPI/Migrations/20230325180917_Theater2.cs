using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoviesAPI.Migrations
{
    /// <inheritdoc />
    public partial class Theater2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TheaterId",
                table: "MovieAndMovieTheater",
                newName: "MovieTheatherId");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieAndMovieTheater_MovieTheathers_MovieTheatherId",
                table: "MovieAndMovieTheater",
                column: "MovieTheatherId",
                principalTable: "MovieTheathers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieAndMovieTheater_MovieTheathers_MovieTheatherId",
                table: "MovieAndMovieTheater");

            migrationBuilder.RenameColumn(
                name: "MovieTheatherId",
                table: "MovieAndMovieTheater",
                newName: "TheaterId");
        }
    }
}
