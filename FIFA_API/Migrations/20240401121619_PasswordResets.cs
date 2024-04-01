using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FIFA_API.Migrations
{
    public partial class PasswordResets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_authemailverif_aev_code",
                table: "t_e_authemailverif_aev");

            migrationBuilder.CreateTable(
                name: "t_e_authpasswordreset_apr",
                columns: table => new
                {
                    utl_mail = table.Column<string>(type: "text", nullable: false),
                    apr_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    apr_code = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_authpasswordreset_apr", x => x.utl_mail);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "t_e_authpasswordreset_apr");

            migrationBuilder.CreateIndex(
                name: "IX_authemailverif_aev_code",
                table: "t_e_authemailverif_aev",
                column: "aev_code",
                unique: true);
        }
    }
}
