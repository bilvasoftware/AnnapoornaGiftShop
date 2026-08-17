using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GiftShop.Migrations
{
    /// <inheritdoc />
    public partial class AddVisitorKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VisitorKey",
                table: "WebsiteVisitors",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VisitorKey",
                table: "WebsiteVisitors");
        }
    }
}
