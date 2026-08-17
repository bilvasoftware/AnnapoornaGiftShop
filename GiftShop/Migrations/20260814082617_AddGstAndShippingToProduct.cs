using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GiftShop.Migrations
{
    /// <inheritdoc />
    public partial class AddGstAndShippingToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "GstPercentage",
                table: "Products",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ShippingCharge",
                table: "Products",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GstPercentage",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ShippingCharge",
                table: "Products");
        }
    }
}
