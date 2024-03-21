using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FIFA_API.Migrations
{
    public partial class VCProduitFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "t_j_produitcouleur_prc");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "t_j_produitcouleur_prc",
                columns: table => new
                {
                    prd_id = table.Column<int>(type: "integer", nullable: false),
                    col_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_j_produitcouleur_prc", x => new { x.prd_id, x.col_id });
                    table.ForeignKey(
                        name: "FK_produitcouleur_col_id",
                        column: x => x.col_id,
                        principalTable: "t_e_couleur_col",
                        principalColumn: "col_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_produitcouleur_prd_id",
                        column: x => x.prd_id,
                        principalTable: "t_e_produit_prd",
                        principalColumn: "prd_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_produitcouleur_col_id",
                table: "t_j_produitcouleur_prc",
                column: "col_id");
        }
    }
}
