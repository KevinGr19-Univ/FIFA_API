using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FIFA_API.Migrations
{
    public partial class FixBDD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_joueur_pht_id",
                table: "t_e_joueur_jou");

            migrationBuilder.DropTable(
                name: "t_j_joueurphoto_jop");

            migrationBuilder.DropIndex(
                name: "IX_joueur_pht_id",
                table: "t_e_joueur_jou");

            migrationBuilder.DropCheckConstraint(
                name: "ck_col_codehexa",
                table: "t_e_couleur_col");

            migrationBuilder.DropColumn(
                name: "pht_id",
                table: "t_e_joueur_jou");

            migrationBuilder.AlterColumn<string>(
                name: "prd_description",
                table: "t_e_produit_prd",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<int>(
                name: "nat_id",
                table: "t_e_produit_prd",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "gen_id",
                table: "t_e_produit_prd",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "cmp_id",
                table: "t_e_produit_prd",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "jou_imageurl",
                table: "t_e_joueur_jou",
                type: "text",
                nullable: true);

            migrationBuilder.AddCheckConstraint(
                name: "ck_col_codehexa",
                table: "t_e_couleur_col",
                sql: "col_codehexa ~ '^[0-9A-Fa-f]{6}$'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "ck_col_codehexa",
                table: "t_e_couleur_col");

            migrationBuilder.DropColumn(
                name: "jou_imageurl",
                table: "t_e_joueur_jou");

            migrationBuilder.AlterColumn<string>(
                name: "prd_description",
                table: "t_e_produit_prd",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "nat_id",
                table: "t_e_produit_prd",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "gen_id",
                table: "t_e_produit_prd",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "cmp_id",
                table: "t_e_produit_prd",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "pht_id",
                table: "t_e_joueur_jou",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "t_j_joueurphoto_jop",
                columns: table => new
                {
                    jou_id = table.Column<int>(type: "integer", nullable: false),
                    pht_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_j_joueurphoto_jop", x => new { x.jou_id, x.pht_id });
                    table.ForeignKey(
                        name: "FK_joueurphoto_jou_id",
                        column: x => x.jou_id,
                        principalTable: "t_e_joueur_jou",
                        principalColumn: "jou_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_joueurphoto_pht_id",
                        column: x => x.pht_id,
                        principalTable: "t_e_photo_pht",
                        principalColumn: "pht_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_joueur_pht_id",
                table: "t_e_joueur_jou",
                column: "pht_id");

            migrationBuilder.AddCheckConstraint(
                name: "ck_col_codehexa",
                table: "t_e_couleur_col",
                sql: "col_codehexa ~ '^[0-9A-F]{6}$'");

            migrationBuilder.CreateIndex(
                name: "IX_joueurphoto_pht_id",
                table: "t_j_joueurphoto_jop",
                column: "pht_id");

            migrationBuilder.AddForeignKey(
                name: "FK_joueur_pht_id",
                table: "t_e_joueur_jou",
                column: "pht_id",
                principalTable: "t_e_photo_pht",
                principalColumn: "pht_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
