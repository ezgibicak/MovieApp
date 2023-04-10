using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoviesAPI.Migrations
{
    /// <inheritdoc />
    public partial class Theater1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieAndMovieTheater_MovieTheathers_MovieTheatherId",
                table: "MovieAndMovieTheater");

            migrationBuilder.DropIndex(
                name: "IX_MovieAndMovieTheater_MovieTheatherId",
                table: "MovieAndMovieTheater");

            migrationBuilder.DropColumn(
                name: "MovieTheatherId",
                table: "MovieAndMovieTheater");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MovieTheatherId",
                table: "MovieAndMovieTheater",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MovieAndMovieTheater_MovieTheatherId",
                table: "MovieAndMovieTheater",
                column: "MovieTheatherId");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieAndMovieTheater_MovieTheathers_MovieTheatherId",
                table: "MovieAndMovieTheater",
                column: "MovieTheatherId",
                principalTable: "MovieTheathers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
