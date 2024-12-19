using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebCinema.Migrations
{
    /// <inheritdoc />
    public partial class AddBranchColumnToMovies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Branch",
                table: "Movies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Branch",
                table: "Movies");
        }
    }
}
