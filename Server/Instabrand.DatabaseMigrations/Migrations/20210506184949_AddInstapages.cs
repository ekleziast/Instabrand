using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Instabrand.DatabaseMigrations.Migrations
{
    public partial class AddInstapages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Instapages",
                columns: table => new
                {
                    InstapageId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    InstagramLogin = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    InstagramId = table.Column<string>(type: "text", nullable: false),
                    AccessToken = table.Column<string>(type: "text", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    State = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instapages", x => x.InstapageId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Instapages");
        }
    }
}
