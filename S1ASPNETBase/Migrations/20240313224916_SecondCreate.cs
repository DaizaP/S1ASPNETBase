using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace S1ASPNETBase.Migrations
{
    /// <inheritdoc />
    public partial class SecondCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductName",
                table: "ProductCategory",
                newName: "CategoryName");

            migrationBuilder.RenameIndex(
                name: "IX_ProductCategory_ProductName",
                table: "ProductCategory",
                newName: "IX_ProductCategory_CategoryName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CategoryName",
                table: "ProductCategory",
                newName: "ProductName");

            migrationBuilder.RenameIndex(
                name: "IX_ProductCategory_CategoryName",
                table: "ProductCategory",
                newName: "IX_ProductCategory_ProductName");
        }
    }
}
