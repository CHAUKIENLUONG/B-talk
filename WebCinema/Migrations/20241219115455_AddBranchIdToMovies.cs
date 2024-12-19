using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebCinema.Migrations
{
    /// <inheritdoc />
    public partial class AddBranchIdToMovies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BranchId",
                table: "Movies",
                nullable: true); // Hoặc false nếu không cho phép null
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Movies");
        }
    }
}
