using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvoiceGenerator.Migrations
{
    /// <inheritdoc />
    public partial class modifieddatatypeandnameinsalesinvoiceandsalesproductlineitems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductName",
                schema: "Identity",
                table: "SalesProductLineItems");

            migrationBuilder.DropColumn(
                name: "CustomerName",
                schema: "Identity",
                table: "SalesInvoices");

            migrationBuilder.DropColumn(
                name: "SubTtotal",
                schema: "Identity",
                table: "SalesInvoices");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                schema: "Identity",
                table: "SalesProductLineItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                schema: "Identity",
                table: "SalesInvoices",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductId",
                schema: "Identity",
                table: "SalesProductLineItems");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                schema: "Identity",
                table: "SalesInvoices");

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                schema: "Identity",
                table: "SalesProductLineItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                schema: "Identity",
                table: "SalesInvoices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "SubTtotal",
                schema: "Identity",
                table: "SalesInvoices",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
