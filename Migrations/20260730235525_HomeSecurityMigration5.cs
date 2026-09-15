using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Home_Security.Migrations
{
    /// <inheritdoc />
    public partial class HomeSecurityMigration5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPrivate",
                table: "Action",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRecurring",
                table: "Action",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastRunDate",
                table: "Action",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Time",
                table: "Action",
                type: "time(6)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatParticipant_PersonId",
                table: "ChatParticipant",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatParticipant_Person_PersonId",
                table: "ChatParticipant",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatParticipant_Person_PersonId",
                table: "ChatParticipant");

            migrationBuilder.DropIndex(
                name: "IX_ChatParticipant_PersonId",
                table: "ChatParticipant");

            migrationBuilder.DropColumn(
                name: "IsPrivate",
                table: "Action");

            migrationBuilder.DropColumn(
                name: "IsRecurring",
                table: "Action");

            migrationBuilder.DropColumn(
                name: "LastRunDate",
                table: "Action");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "Action");
        }
    }
}
