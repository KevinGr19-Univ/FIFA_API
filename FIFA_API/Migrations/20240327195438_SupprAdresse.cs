using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FIFA_API.Migrations
{
    public partial class SupprAdresse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_commande_adr_facuration_id",
                table: "t_e_commande_cmd");

            migrationBuilder.DropForeignKey(
                name: "FK_commande_adr_livraison_id",
                table: "t_e_commande_cmd");

            migrationBuilder.DropTable(
                name: "t_e_adresse_adr");

            migrationBuilder.DropIndex(
                name: "IX_commande_adr_facuration_id",
                table: "t_e_commande_cmd");

            migrationBuilder.DropIndex(
                name: "IX_commande_adr_livraison_id",
                table: "t_e_commande_cmd");

            migrationBuilder.DropColumn(
                name: "adr_facuration_id",
                table: "t_e_commande_cmd");

            migrationBuilder.DropColumn(
                name: "adr_livraison_id",
                table: "t_e_commande_cmd");

            migrationBuilder.AddColumn<string>(
                name: "cmd_codepostalfacturation",
                table: "t_e_commande_cmd",
                type: "character(5)",
                fixedLength: true,
                maxLength: 5,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "cmd_codepostallivraison",
                table: "t_e_commande_cmd",
                type: "character(5)",
                fixedLength: true,
                maxLength: 5,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "cmd_ruefacturation",
                table: "t_e_commande_cmd",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "cmd_ruelivraison",
                table: "t_e_commande_cmd",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "cmd_villefacturation",
                table: "t_e_commande_cmd",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "cmd_villelivraison",
                table: "t_e_commande_cmd",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddCheckConstraint(
                name: "ck_cmd_codepostalfacturation",
                table: "t_e_commande_cmd",
                sql: "cmd_codepostalfacturation ~ '^([0-9]{2}|2[AB])[0-9]{3}$'");

            migrationBuilder.AddCheckConstraint(
                name: "ck_cmd_codepostallivraison",
                table: "t_e_commande_cmd",
                sql: "cmd_codepostallivraison ~ '^([0-9]{2}|2[AB])[0-9]{3}$'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "ck_cmd_codepostalfacturation",
                table: "t_e_commande_cmd");

            migrationBuilder.DropCheckConstraint(
                name: "ck_cmd_codepostallivraison",
                table: "t_e_commande_cmd");

            migrationBuilder.DropColumn(
                name: "cmd_codepostalfacturation",
                table: "t_e_commande_cmd");

            migrationBuilder.DropColumn(
                name: "cmd_codepostallivraison",
                table: "t_e_commande_cmd");

            migrationBuilder.DropColumn(
                name: "cmd_ruefacturation",
                table: "t_e_commande_cmd");

            migrationBuilder.DropColumn(
                name: "cmd_ruelivraison",
                table: "t_e_commande_cmd");

            migrationBuilder.DropColumn(
                name: "cmd_villefacturation",
                table: "t_e_commande_cmd");

            migrationBuilder.DropColumn(
                name: "cmd_villelivraison",
                table: "t_e_commande_cmd");

            migrationBuilder.AddColumn<int>(
                name: "adr_facuration_id",
                table: "t_e_commande_cmd",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "adr_livraison_id",
                table: "t_e_commande_cmd",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "t_e_adresse_adr",
                columns: table => new
                {
                    adr_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    adr_codepostal = table.Column<string>(type: "character(5)", fixedLength: true, maxLength: 5, nullable: false),
                    adr_rue = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    adr_ville = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_adresse_adr", x => x.adr_id);
                    table.CheckConstraint("ck_adr_codepostal", "adr_codepostal ~ '^([0-9]{2}|2[AB])[0-9]{3}$'");
                });

            migrationBuilder.CreateIndex(
                name: "IX_commande_adr_facuration_id",
                table: "t_e_commande_cmd",
                column: "adr_facuration_id");

            migrationBuilder.CreateIndex(
                name: "IX_commande_adr_livraison_id",
                table: "t_e_commande_cmd",
                column: "adr_livraison_id");

            migrationBuilder.AddForeignKey(
                name: "FK_commande_adr_facuration_id",
                table: "t_e_commande_cmd",
                column: "adr_facuration_id",
                principalTable: "t_e_adresse_adr",
                principalColumn: "adr_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_commande_adr_livraison_id",
                table: "t_e_commande_cmd",
                column: "adr_livraison_id",
                principalTable: "t_e_adresse_adr",
                principalColumn: "adr_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
