using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FIFA_API.Migrations
{
    public partial class DeleteBehaviorsCommande : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_lignecommande_cmd_id",
                table: "t_e_lignecommande_lco");

            migrationBuilder.DropForeignKey(
                name: "FK_lignecommande_prd_id",
                table: "t_e_lignecommande_lco");

            migrationBuilder.DropForeignKey(
                name: "FK_lignecommande_tpr_id",
                table: "t_e_lignecommande_lco");

            migrationBuilder.AddForeignKey(
                name: "FK_lignecommande_cmd_id",
                table: "t_e_lignecommande_lco",
                column: "cmd_id",
                principalTable: "t_e_commande_cmd",
                principalColumn: "cmd_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_lignecommande_prd_id",
                table: "t_e_lignecommande_lco",
                column: "prd_id",
                principalTable: "t_j_variantecouleurproduit_vcp",
                principalColumn: "vcp_id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_lignecommande_tpr_id",
                table: "t_e_lignecommande_lco",
                column: "tpr_id",
                principalTable: "t_e_tailleproduit_tpr",
                principalColumn: "tpr_id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_lignecommande_cmd_id",
                table: "t_e_lignecommande_lco");

            migrationBuilder.DropForeignKey(
                name: "FK_lignecommande_prd_id",
                table: "t_e_lignecommande_lco");

            migrationBuilder.DropForeignKey(
                name: "FK_lignecommande_tpr_id",
                table: "t_e_lignecommande_lco");

            migrationBuilder.AddForeignKey(
                name: "FK_lignecommande_cmd_id",
                table: "t_e_lignecommande_lco",
                column: "cmd_id",
                principalTable: "t_e_commande_cmd",
                principalColumn: "cmd_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_lignecommande_prd_id",
                table: "t_e_lignecommande_lco",
                column: "prd_id",
                principalTable: "t_j_variantecouleurproduit_vcp",
                principalColumn: "vcp_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_lignecommande_tpr_id",
                table: "t_e_lignecommande_lco",
                column: "tpr_id",
                principalTable: "t_e_tailleproduit_tpr",
                principalColumn: "tpr_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
