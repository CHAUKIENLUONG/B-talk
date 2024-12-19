using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebCinema.Migrations
{
    /// <inheritdoc />
    public partial class removequantityTiketCombo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "TicketCombos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "TicketCombos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
