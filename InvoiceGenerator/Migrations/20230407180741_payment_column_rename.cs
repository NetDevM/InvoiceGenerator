using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvoiceGenerator.Migrations
{
    /// <inheritdoc />
    public partial class paymentcolumnrename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountReceived",
                schema: "Identity",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "AmountToReceive",
                schema: "Identity",
                table: "Payments");

            migrationBuilder.AddColumn<float>(
                name: "DueAmount",
                schema: "Identity",
                table: "Payments",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "GrandTotal",
                schema: "Identity",
                table: "Payments",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "ReceivedAmount",
                schema: "Identity",
                table: "Payments",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DueAmount",
                schema: "Identity",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "GrandTotal",
                schema: "Identity",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ReceivedAmount",
                schema: "Identity",
                table: "Payments");

            migrationBuilder.AddColumn<string>(
                name: "AmountReceived",
                schema: "Identity",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AmountToReceive",
                schema: "Identity",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
