using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesRecruitApp.Migrations
{
    /// <inheritdoc />
    public partial class ProductBundle1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShortDescription",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UsageInstructions",
                table: "ProductBuddles",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShortDescription",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UsageInstructions",
                table: "ProductBuddles");
        }
    }
}
