using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FIFA_API.Migrations
{
    public partial class AuthEmailVerif : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "t_e_authemailverif_aev",
                columns: table => new
                {
                    utl_id = table.Column<int>(type: "integer", nullable: false),
                    utl_mail = table.Column<string>(type: "text", nullable: false),
                    aev_code = table.Column<string>(type: "text", nullable: false),
                    aev_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_authemailverif_aev", x => x.utl_id);
                    table.ForeignKey(
                        name: "FK_authemailverif_utl_id",
                        column: x => x.utl_id,
                        principalTable: "t_e_utilisateur_utl",
                        principalColumn: "utl_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_authemailverif_aev_code",
                table: "t_e_authemailverif_aev",
                column: "aev_code",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "t_e_authemailverif_aev");
        }
    }
}
