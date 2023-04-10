using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoviesAPI.Migrations
{
    /// <inheritdoc />
    public partial class MovieAndMovieTheater : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MovieAndMovieTheater",
                columns: table => new
                {
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    TheaterId = table.Column<int>(type: "int", nullable: false),
                    MovieTheatherId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieAndMovieTheater", x => new { x.TheaterId, x.MovieId });
                    table.ForeignKey(
                        name: "FK_MovieAndMovieTheater_MovieTheathers_MovieTheatherId",
                        column: x => x.MovieTheatherId,
                        principalTable: "MovieTheathers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieAndMovieTheater_Movie_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovieAndMovieTheater_MovieId",
                table: "MovieAndMovieTheater",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieAndMovieTheater_MovieTheatherId",
                table: "MovieAndMovieTheater",
                column: "MovieTheatherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovieAndMovieTheater");

            migrationBuilder.CreateTable(
                name: "MovieTheaterMovie",
                columns: table => new
                {
                    MovieTheaterId = table.Column<int>(type: "int", nullable: false),
                    MovieId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieTheaterMovie", x => new { x.MovieTheaterId, x.MovieId });
                    table.ForeignKey(
                        name: "FK_MovieTheaterMovie_MovieTheathers_MovieTheaterId",
                        column: x => x.MovieTheaterId,
                        principalTable: "MovieTheathers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieTheaterMovie_Movie_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovieTheaterMovie_MovieId",
                table: "MovieTheaterMovie",
                column: "MovieId");
        }
    }
}
