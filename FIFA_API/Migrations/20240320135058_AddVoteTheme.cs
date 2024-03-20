using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FIFA_API.Migrations
{
    public partial class AddVoteTheme : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_voteutilisateur_col_id",
                table: "t_e_voteutilisateur_vtl");

            migrationBuilder.DropForeignKey(
                name: "FK_voteutilisateur_tpr_id",
                table: "t_e_voteutilisateur_vtl");

            migrationBuilder.DropPrimaryKey(
                name: "PK_t_e_voteutilisateur_vtl",
                table: "t_e_voteutilisateur_vtl");

            migrationBuilder.RenameColumn(
                name: "vtl_rankvote",
                table: "t_e_voteutilisateur_vtl",
                newName: "jou_id3");

            migrationBuilder.RenameColumn(
                name: "tpr_id",
                table: "t_e_voteutilisateur_vtl",
                newName: "jou_id2");

            migrationBuilder.RenameColumn(
                name: "col_id",
                table: "t_e_voteutilisateur_vtl",
                newName: "jou_id1");

            migrationBuilder.RenameIndex(
                name: "IX_voteutilisateur_tpr_id",
                table: "t_e_voteutilisateur_vtl",
                newName: "IX_voteutilisateur_jou_id2");

            migrationBuilder.RenameIndex(
                name: "IX_voteutilisateur_col_id",
                table: "t_e_voteutilisateur_vtl",
                newName: "IX_voteutilisateur_jou_id1");

            migrationBuilder.AlterColumn<int>(
                name: "jou_id2",
                table: "t_e_voteutilisateur_vtl",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<int>(
                name: "jou_id1",
                table: "t_e_voteutilisateur_vtl",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Relational:ColumnOrder", 1);

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

            migrationBuilder.CreateTable(
                name: "t_e_themevote_thv",
                columns: table => new
                {
                    thv_nomtheme = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_themevote_thv", x => x.thv_nomtheme);
                });

            migrationBuilder.CreateIndex(
                name: "IX_voteutilisateur_jou_id3",
                table: "t_e_voteutilisateur_vtl",
                column: "jou_id3");

            migrationBuilder.CreateIndex(
                name: "IX_voteutilisateur_thv_nomtheme",
                table: "t_e_voteutilisateur_vtl",
                column: "thv_nomtheme");

            migrationBuilder.AddForeignKey(
                name: "FK_voteutilisateur_jou_id1",
                table: "t_e_voteutilisateur_vtl",
                column: "jou_id1",
                principalTable: "t_e_joueur_jou",
                principalColumn: "jou_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_voteutilisateur_jou_id2",
                table: "t_e_voteutilisateur_vtl",
                column: "jou_id2",
                principalTable: "t_e_joueur_jou",
                principalColumn: "jou_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_voteutilisateur_jou_id3",
                table: "t_e_voteutilisateur_vtl",
                column: "jou_id3",
                principalTable: "t_e_joueur_jou",
                principalColumn: "jou_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_voteutilisateur_thv_nomtheme",
                table: "t_e_voteutilisateur_vtl",
                column: "thv_nomtheme",
                principalTable: "t_e_themevote_thv",
                principalColumn: "thv_nomtheme",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_voteutilisateur_jou_id1",
                table: "t_e_voteutilisateur_vtl");

            migrationBuilder.DropForeignKey(
                name: "FK_voteutilisateur_jou_id2",
                table: "t_e_voteutilisateur_vtl");

            migrationBuilder.DropForeignKey(
                name: "FK_voteutilisateur_jou_id3",
                table: "t_e_voteutilisateur_vtl");

            migrationBuilder.DropForeignKey(
                name: "FK_voteutilisateur_thv_nomtheme",
                table: "t_e_voteutilisateur_vtl");

            migrationBuilder.DropTable(
                name: "t_e_themevote_thv");

            migrationBuilder.DropPrimaryKey(
                name: "PK_t_e_voteutilisateur_vtl",
                table: "t_e_voteutilisateur_vtl");

            migrationBuilder.DropIndex(
                name: "IX_voteutilisateur_jou_id3",
                table: "t_e_voteutilisateur_vtl");

            migrationBuilder.DropIndex(
                name: "IX_voteutilisateur_thv_nomtheme",
                table: "t_e_voteutilisateur_vtl");

            migrationBuilder.DropColumn(
                name: "thv_nomtheme",
                table: "t_e_voteutilisateur_vtl");

            migrationBuilder.RenameColumn(
                name: "jou_id3",
                table: "t_e_voteutilisateur_vtl",
                newName: "vtl_rankvote");

            migrationBuilder.RenameColumn(
                name: "jou_id2",
                table: "t_e_voteutilisateur_vtl",
                newName: "tpr_id");

            migrationBuilder.RenameColumn(
                name: "jou_id1",
                table: "t_e_voteutilisateur_vtl",
                newName: "col_id");

            migrationBuilder.RenameIndex(
                name: "IX_voteutilisateur_jou_id2",
                table: "t_e_voteutilisateur_vtl",
                newName: "IX_voteutilisateur_tpr_id");

            migrationBuilder.RenameIndex(
                name: "IX_voteutilisateur_jou_id1",
                table: "t_e_voteutilisateur_vtl",
                newName: "IX_voteutilisateur_col_id");

            migrationBuilder.AlterColumn<int>(
                name: "tpr_id",
                table: "t_e_voteutilisateur_vtl",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<int>(
                name: "col_id",
                table: "t_e_voteutilisateur_vtl",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AddPrimaryKey(
                name: "PK_t_e_voteutilisateur_vtl",
                table: "t_e_voteutilisateur_vtl",
                columns: new[] { "utl_id", "col_id", "tpr_id" });

            migrationBuilder.AddForeignKey(
                name: "FK_voteutilisateur_col_id",
                table: "t_e_voteutilisateur_vtl",
                column: "col_id",
                principalTable: "t_e_couleur_col",
                principalColumn: "col_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_voteutilisateur_tpr_id",
                table: "t_e_voteutilisateur_vtl",
                column: "tpr_id",
                principalTable: "t_e_tailleproduit_tpr",
                principalColumn: "tpr_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
