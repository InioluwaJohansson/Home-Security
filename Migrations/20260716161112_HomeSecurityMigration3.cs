using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Home_Security.Migrations
{
    /// <inheritdoc />
    public partial class HomeSecurityMigration3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recording_Camera_CameraId",
                table: "Recording");

            migrationBuilder.AlterColumn<int>(
                name: "CameraId",
                table: "Recording",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Recording",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Recording_Camera_CameraId",
                table: "Recording",
                column: "CameraId",
                principalTable: "Camera",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recording_Camera_CameraId",
                table: "Recording");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Recording");

            migrationBuilder.AlterColumn<int>(
                name: "CameraId",
                table: "Recording",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Recording_Camera_CameraId",
                table: "Recording",
                column: "CameraId",
                principalTable: "Camera",
                principalColumn: "Id");
        }
    }
}
