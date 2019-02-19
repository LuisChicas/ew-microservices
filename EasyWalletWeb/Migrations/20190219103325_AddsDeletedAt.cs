using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EasyWalletWeb.Migrations
{
    public partial class AddsDeletedAt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Tags",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Entries",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Categories",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Entries");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Categories");
        }
    }
}
