using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FIFA_API.Migrations
{
    public partial class Anonymisation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_commande_utl_id",
                table: "t_e_commande_cmd");

            migrationBuilder.DropForeignKey(
                name: "FK_voteutilisateur_utl_id",
                table: "t_e_voteutilisateur_vtl");

            migrationBuilder.AddColumn<bool>(
                name: "utl_anonyme",
                table: "t_e_utilisateur_utl",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "utl_id",
                table: "t_e_commande_cmd",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_commande_utl_id",
                table: "t_e_commande_cmd",
                column: "utl_id",
                principalTable: "t_e_utilisateur_utl",
                principalColumn: "utl_id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_voteutilisateur_utl_id",
                table: "t_e_voteutilisateur_vtl",
                column: "utl_id",
                principalTable: "t_e_utilisateur_utl",
                principalColumn: "utl_id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_commande_utl_id",
                table: "t_e_commande_cmd");

            migrationBuilder.DropForeignKey(
                name: "FK_voteutilisateur_utl_id",
                table: "t_e_voteutilisateur_vtl");

            migrationBuilder.DropColumn(
                name: "utl_anonyme",
                table: "t_e_utilisateur_utl");

            migrationBuilder.AlterColumn<int>(
                name: "utl_id",
                table: "t_e_commande_cmd",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_commande_utl_id",
                table: "t_e_commande_cmd",
                column: "utl_id",
                principalTable: "t_e_utilisateur_utl",
                principalColumn: "utl_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_voteutilisateur_utl_id",
                table: "t_e_voteutilisateur_vtl",
                column: "utl_id",
                principalTable: "t_e_utilisateur_utl",
                principalColumn: "utl_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
