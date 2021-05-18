using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Instabrand.DatabaseMigrations.Migrations
{
    public partial class AddInstaposts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ConcurrencyToken",
                table: "Instapages",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Instapages",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Instapages",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Instaposts",
                columns: table => new
                {
                    InstapostId = table.Column<string>(type: "text", nullable: false),
                    InstapageId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Currency = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instaposts", x => x.InstapostId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Instaposts");

            migrationBuilder.DropColumn(
                name: "ConcurrencyToken",
                table: "Instapages");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Instapages");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Instapages");
        }
    }
}
