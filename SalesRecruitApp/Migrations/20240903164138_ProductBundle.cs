using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesRecruitApp.Migrations
{
    /// <inheritdoc />
    public partial class ProductBundle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "StockQuantity",
                table: "ProductBuddles",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Highlights",
                table: "ProductBuddles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubTitle",
                table: "ProductBuddles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "ProductBuddles",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Highlights",
                table: "ProductBuddles");

            migrationBuilder.DropColumn(
                name: "SubTitle",
                table: "ProductBuddles");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "ProductBuddles");

            migrationBuilder.AlterColumn<int>(
                name: "StockQuantity",
                table: "ProductBuddles",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
