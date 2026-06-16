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
                name: "FK_External_Action_ActionId",
                table: "External");

            migrationBuilder.DropIndex(
                name: "IX_External_ActionId",
                table: "External");

            migrationBuilder.DropColumn(
                name: "ActionId",
                table: "External");

            migrationBuilder.AlterColumn<string>(
                name: "ActionIds",
                table: "External",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "External",
                keyColumn: "ActionIds",
                keyValue: null,
                column: "ActionIds",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ActionIds",
                table: "External",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "ActionId",
                table: "External",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_External_ActionId",
                table: "External",
                column: "ActionId");

            migrationBuilder.AddForeignKey(
                name: "FK_External_Action_ActionId",
                table: "External",
                column: "ActionId",
                principalTable: "Action",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
