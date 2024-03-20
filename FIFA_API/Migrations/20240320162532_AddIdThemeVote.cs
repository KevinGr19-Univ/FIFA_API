using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FIFA_API.Migrations
{
    public partial class AddIdThemeVote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_voteutilisateur_thv_nomtheme",
                table: "t_e_voteutilisateur_vtl");

            migrationBuilder.DropPrimaryKey(
                name: "PK_t_e_voteutilisateur_vtl",
                table: "t_e_voteutilisateur_vtl");

            migrationBuilder.DropIndex(
                name: "IX_voteutilisateur_thv_nomtheme",
                table: "t_e_voteutilisateur_vtl");

            migrationBuilder.DropPrimaryKey(
                name: "PK_t_e_themevote_thv",
                table: "t_e_themevote_thv");

            migrationBuilder.DropColumn(
                name: "thv_nomtheme",
                table: "t_e_voteutilisateur_vtl");

            migrationBuilder.AddColumn<int>(
                name: "thv_id",
                table: "t_e_voteutilisateur_vtl",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AddColumn<int>(
                name: "thv_id",
                table: "t_e_themevote_thv",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_t_e_voteutilisateur_vtl",
                table: "t_e_voteutilisateur_vtl",
                columns: new[] { "utl_id", "thv_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_t_e_themevote_thv",
                table: "t_e_themevote_thv",
                column: "thv_id");

            migrationBuilder.CreateIndex(
                name: "IX_voteutilisateur_thv_id",
                table: "t_e_voteutilisateur_vtl",
                column: "thv_id");

            migrationBuilder.CreateIndex(
                name: "IX_themevote_thv_nomtheme",
                table: "t_e_themevote_thv",
                column: "thv_nomtheme",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_voteutilisateur_thv_id",
                table: "t_e_voteutilisateur_vtl",
                column: "thv_id",
                principalTable: "t_e_themevote_thv",
                principalColumn: "thv_id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_voteutilisateur_thv_id",
                table: "t_e_voteutilisateur_vtl");

            migrationBuilder.DropPrimaryKey(
                name: "PK_t_e_voteutilisateur_vtl",
                table: "t_e_voteutilisateur_vtl");

            migrationBuilder.DropIndex(
                name: "IX_voteutilisateur_thv_id",
                table: "t_e_voteutilisateur_vtl");

            migrationBuilder.DropPrimaryKey(
                name: "PK_t_e_themevote_thv",
                table: "t_e_themevote_thv");

            migrationBuilder.DropIndex(
                name: "IX_themevote_thv_nomtheme",
                table: "t_e_themevote_thv");

            migrationBuilder.DropColumn(
                name: "thv_id",
                table: "t_e_voteutilisateur_vtl");

            migrationBuilder.DropColumn(
                name: "thv_id",
                table: "t_e_themevote_thv");

            migrationBuilder.AddColumn<string>(
                name: "thv_nomtheme",
                table: "t_e_voteutilisateur_vtl",
                type: "character varying(60)",
                nullable: false,
                defaultValue: "")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AddPrimaryKey(
                name: "PK_t_e_voteutilisateur_vtl",
                table: "t_e_voteutilisateur_vtl",
                columns: new[] { "utl_id", "thv_nomtheme" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_t_e_themevote_thv",
                table: "t_e_themevote_thv",
                column: "thv_nomtheme");

            migrationBuilder.CreateIndex(
                name: "IX_voteutilisateur_thv_nomtheme",
                table: "t_e_voteutilisateur_vtl",
                column: "thv_nomtheme");

            migrationBuilder.AddForeignKey(
                name: "FK_voteutilisateur_thv_nomtheme",
                table: "t_e_voteutilisateur_vtl",
                column: "thv_nomtheme",
                principalTable: "t_e_themevote_thv",
                principalColumn: "thv_nomtheme",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
