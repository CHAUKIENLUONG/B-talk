using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebCinema.Migrations
{
    /// <inheritdoc />
    public partial class RemoveBranchColumnFromMovies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Branch",
                table: "Movies");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Branch",
                table: "Movies",
                nullable: true);
        }
    }
}
