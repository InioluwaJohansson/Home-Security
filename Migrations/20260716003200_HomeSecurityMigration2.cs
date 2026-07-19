using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Home_Security.Migrations
{
    /// <inheritdoc />
    public partial class HomeSecurityMigration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Camera",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Port",
                table: "Camera",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "StreamPath",
                table: "Camera",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Camera",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Camera");

            migrationBuilder.DropColumn(
                name: "Port",
                table: "Camera");

            migrationBuilder.DropColumn(
                name: "StreamPath",
                table: "Camera");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Camera");
        }
    }
}
