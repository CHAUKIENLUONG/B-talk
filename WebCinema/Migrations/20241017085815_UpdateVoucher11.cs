using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebCinema.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVoucher11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DiscountAmount",
                table: "Vouchers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "FinalTotal",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VoucherId",
                table: "Tickets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_VoucherId",
                table: "Tickets",
                column: "VoucherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Vouchers_VoucherId",
                table: "Tickets",
                column: "VoucherId",
                principalTable: "Vouchers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Vouchers_VoucherId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_VoucherId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "DiscountAmount",
                table: "Vouchers");

            migrationBuilder.DropColumn(
                name: "FinalTotal",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "VoucherId",
                table: "Tickets");
        }
    }
}
