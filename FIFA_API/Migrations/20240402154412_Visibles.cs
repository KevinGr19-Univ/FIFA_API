using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FIFA_API.Migrations
{
    public partial class Visibles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "vcp_visible",
                table: "t_j_variantecouleurproduit_vcp",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "pub_visible",
                table: "t_g_publication_pub",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "thv_visible",
                table: "t_e_themevote_thv",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "tpr_visible",
                table: "t_e_tailleproduit_tpr",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "prd_visible",
                table: "t_e_produit_prd",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "nat_visible",
                table: "t_e_nation_nat",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "gen_visible",
                table: "t_e_genre_gen",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "col_visible",
                table: "t_e_couleur_col",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "cmp_visible",
                table: "t_e_competition_cmp",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "cpr_visible",
                table: "t_e_categorieproduit_cpr",
                type: "boolean",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "vcp_visible",
                table: "t_j_variantecouleurproduit_vcp");

            migrationBuilder.DropColumn(
                name: "pub_visible",
                table: "t_g_publication_pub");

            migrationBuilder.DropColumn(
                name: "thv_visible",
                table: "t_e_themevote_thv");

            migrationBuilder.DropColumn(
                name: "tpr_visible",
                table: "t_e_tailleproduit_tpr");

            migrationBuilder.DropColumn(
                name: "prd_visible",
                table: "t_e_produit_prd");

            migrationBuilder.DropColumn(
                name: "nat_visible",
                table: "t_e_nation_nat");

            migrationBuilder.DropColumn(
                name: "gen_visible",
                table: "t_e_genre_gen");

            migrationBuilder.DropColumn(
                name: "col_visible",
                table: "t_e_couleur_col");

            migrationBuilder.DropColumn(
                name: "cmp_visible",
                table: "t_e_competition_cmp");

            migrationBuilder.DropColumn(
                name: "cpr_visible",
                table: "t_e_categorieproduit_cpr");
        }
    }
}
