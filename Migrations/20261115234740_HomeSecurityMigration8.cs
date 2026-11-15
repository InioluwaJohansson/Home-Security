using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Home_Security.Migrations
{
    /// <inheritdoc />
    public partial class HomeSecurityMigration8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UtilityIds",
                table: "Hardware",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UtilityIds",
                table: "Hardware");
        }
    }
}
