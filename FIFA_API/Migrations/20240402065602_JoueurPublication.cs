using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FIFA_API.Migrations
{
    public partial class JoueurPublication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "t_j_joueurpublication_jop",
                columns: table => new
                {
                    jou_id = table.Column<int>(type: "integer", nullable: false),
                    pub_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_j_joueurpublication_jop", x => new { x.jou_id, x.pub_id });
                    table.ForeignKey(
                        name: "FK_joueurpublication_jou_id",
                        column: x => x.jou_id,
                        principalTable: "t_e_joueur_jou",
                        principalColumn: "jou_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_joueurpublication_pub_id",
                        column: x => x.pub_id,
                        principalTable: "t_g_publication_pub",
                        principalColumn: "pub_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_joueurpublication_pub_id",
                table: "t_j_joueurpublication_jop",
                column: "pub_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "t_j_joueurpublication_jop");
        }
    }
}
