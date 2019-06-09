using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EasyWalletWeb.Migrations
{
    public partial class CategoryTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Entries",
                type: "decimal(12, 2)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Entries",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CategoryTypeId",
                table: "Categories",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CategoryTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryTypes", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryTypes");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Entries");

            migrationBuilder.DropColumn(
                name: "CategoryTypeId",
                table: "Categories");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Entries",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12, 2)");
        }
    }
}
