using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebCinema.Migrations
{
    /// <inheritdoc />
    public partial class addIventory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Inventories",
                columns: table => new
                {
                    InventoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComboId = table.Column<int>(type: "int", nullable: false),
                    QuantityInStock = table.Column<int>(type: "int", nullable: false),
                    QuantitySold = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventories", x => x.InventoryId);
                    table.ForeignKey(
                        name: "FK_Inventories_Combos_ComboId",
                        column: x => x.ComboId,
                        principalTable: "Combos",
                        principalColumn: "ComboId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InventoryEntries",
                columns: table => new
                {
                    InventoryEntryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComboId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryEntries", x => x.InventoryEntryId);
                    table.ForeignKey(
                        name: "FK_InventoryEntries_Combos_ComboId",
                        column: x => x.ComboId,
                        principalTable: "Combos",
                        principalColumn: "ComboId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_ComboId",
                table: "Inventories",
                column: "ComboId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryEntries_ComboId",
                table: "InventoryEntries",
                column: "ComboId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inventories");

            migrationBuilder.DropTable(
                name: "InventoryEntries");
        }
    }
}
