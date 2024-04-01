using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FIFA_API.Migrations
{
    public partial class Auth2FALogin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "utl_token2fa",
                table: "t_e_utilisateur_utl",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "t_e_auth2falogin_a2f",
                columns: table => new
                {
                    utl_id = table.Column<int>(type: "integer", nullable: false),
                    a2f_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    a2f_code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_auth2falogin_a2f", x => x.utl_id);
                    table.ForeignKey(
                        name: "FK_t_e_auth2falogin_a2f_t_e_utilisateur_utl_utl_id",
                        column: x => x.utl_id,
                        principalTable: "t_e_utilisateur_utl",
                        principalColumn: "utl_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_utilisateur_utl_token2fa",
                table: "t_e_utilisateur_utl",
                column: "utl_token2fa",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_authpasswordreset_apr_code",
                table: "t_e_authpasswordreset_apr",
                column: "apr_code",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "t_e_auth2falogin_a2f");

            migrationBuilder.DropIndex(
                name: "IX_utilisateur_utl_token2fa",
                table: "t_e_utilisateur_utl");

            migrationBuilder.DropIndex(
                name: "IX_authpasswordreset_apr_code",
                table: "t_e_authpasswordreset_apr");

            migrationBuilder.DropColumn(
                name: "utl_token2fa",
                table: "t_e_utilisateur_utl");
        }
    }
}
