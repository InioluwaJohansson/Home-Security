using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Home_Security.Migrations
{
    /// <inheritdoc />
    public partial class HomeSecurityMigration7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Utilities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    UtilityName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UtilityId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UtilityType = table.Column<int>(type: "int", nullable: false),
                    UtilityComponent = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TimeOut = table.Column<double>(type: "double", nullable: true),
                    CurrentLevel = table.Column<double>(type: "double", nullable: true),
                    VoltageLevel = table.Column<double>(type: "double", nullable: true),
                    PowerConsumption = table.Column<double>(type: "double", nullable: true),
                    PowerConsumptionToday = table.Column<double>(type: "double", nullable: true),
                    ActionIds = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CurrentWaterLevel = table.Column<double>(type: "double", nullable: true),
                    MaxWaterLevel = table.Column<double>(type: "double", nullable: true),
                    MinWaterLevel = table.Column<double>(type: "double", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastModifiedBy = table.Column<int>(type: "int", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilities", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Utilities");
        }
    }
}
