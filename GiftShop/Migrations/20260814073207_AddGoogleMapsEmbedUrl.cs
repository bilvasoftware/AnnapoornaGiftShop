using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GiftShop.Migrations
{
    /// <inheritdoc />
    public partial class AddGoogleMapsEmbedUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GoogleMapsEmbedUrl",
                table: "ShopSettings",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoogleMapsEmbedUrl",
                table: "ShopSettings");
        }
    }
}
