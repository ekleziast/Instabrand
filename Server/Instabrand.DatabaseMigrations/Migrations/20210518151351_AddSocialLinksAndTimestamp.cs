using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Instabrand.DatabaseMigrations.Migrations
{
    public partial class AddSocialLinksAndTimestamp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Timestamp",
                table: "Instaposts",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Telegram",
                table: "Instapages",
                type: "character varying(1024)",
                maxLength: 1024,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Vkontakte",
                table: "Instapages",
                type: "character varying(1024)",
                maxLength: 1024,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "Instaposts");

            migrationBuilder.DropColumn(
                name: "Telegram",
                table: "Instapages");

            migrationBuilder.DropColumn(
                name: "Vkontakte",
                table: "Instapages");
        }
    }
}
