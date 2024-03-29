using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FIFA_API.Migrations
{
    public partial class FixUniqueCommande : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "ck_utl_telephone",
                table: "t_e_utilisateur_utl");

            migrationBuilder.DropIndex(
                name: "IX_commande_cmd_urlfacture",
                table: "t_e_commande_cmd");

            migrationBuilder.AddCheckConstraint(
                name: "ck_utl_telephone",
                table: "t_e_utilisateur_utl",
                sql: "utl_telephone ~ '^(0|\\+33)[1-9][0-9]{8}$'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "ck_utl_telephone",
                table: "t_e_utilisateur_utl");

            migrationBuilder.AddCheckConstraint(
                name: "ck_utl_telephone",
                table: "t_e_utilisateur_utl",
                sql: "utl_telephone ~ '^0[1-9][0-9]{8}$'");

            migrationBuilder.CreateIndex(
                name: "IX_commande_cmd_urlfacture",
                table: "t_e_commande_cmd",
                column: "cmd_urlfacture",
                unique: true);
        }
    }
}
