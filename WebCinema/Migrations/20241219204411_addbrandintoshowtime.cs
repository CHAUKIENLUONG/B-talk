using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebCinema.Migrations
{
    /// <inheritdoc />
    public partial class addbrandintoshowtime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Showtimes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Showtimes_BranchId",
                table: "Showtimes",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Showtimes_Branches_BranchId",
                table: "Showtimes",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "BranchId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Showtimes_Branches_BranchId",
                table: "Showtimes");

            migrationBuilder.DropIndex(
                name: "IX_Showtimes_BranchId",
                table: "Showtimes");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Showtimes");
        }
    }
}
