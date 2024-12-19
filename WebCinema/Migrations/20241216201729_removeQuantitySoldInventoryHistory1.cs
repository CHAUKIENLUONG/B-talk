using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebCinema.Migrations
{
    /// <inheritdoc />
    public partial class removeQuantitySoldInventoryHistory1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuantitySold",
                table: "InventoryHistories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuantitySold",
                table: "InventoryHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
