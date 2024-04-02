using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FIFA_API.Migrations
{
    public partial class ThemeVoteJoueur : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "t_j_themevotejoueur_thj",
                columns: table => new
                {
                    thv_id = table.Column<int>(type: "integer", nullable: false),
                    jou_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_j_themevotejoueur_thj", x => new { x.thv_id, x.jou_id });
                    table.ForeignKey(
                        name: "FK_themevotejoueur_jou_id",
                        column: x => x.jou_id,
                        principalTable: "t_e_joueur_jou",
                        principalColumn: "jou_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_themevotejoueur_thv_id",
                        column: x => x.thv_id,
                        principalTable: "t_e_themevote_thv",
                        principalColumn: "thv_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_themevotejoueur_jou_id",
                table: "t_j_themevotejoueur_thj",
                column: "jou_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "t_j_themevotejoueur_thj");
        }
    }
}
