using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Home_Security.Migrations
{
    /// <inheritdoc />
    public partial class HomeSecurityMigration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contact_ContactCategory_CategoryId",
                table: "Contact");

            migrationBuilder.DropIndex(
                name: "IX_Contact_CategoryId",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Contact");

            migrationBuilder.RenameColumn(
                name: "ContactCategory",
                table: "Contact",
                newName: "ContactCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_ContactCategoryId",
                table: "Contact",
                column: "ContactCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contact_ContactCategory_ContactCategoryId",
                table: "Contact",
                column: "ContactCategoryId",
                principalTable: "ContactCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contact_ContactCategory_ContactCategoryId",
                table: "Contact");

            migrationBuilder.DropIndex(
                name: "IX_Contact_ContactCategoryId",
                table: "Contact");

            migrationBuilder.RenameColumn(
                name: "ContactCategoryId",
                table: "Contact",
                newName: "ContactCategory");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Contact",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Contact_CategoryId",
                table: "Contact",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contact_ContactCategory_CategoryId",
                table: "Contact",
                column: "CategoryId",
                principalTable: "ContactCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
