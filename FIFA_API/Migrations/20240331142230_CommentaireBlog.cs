using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FIFA_API.Migrations
{
    public partial class CommentaireBlog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "t_j_commentaireblog_cbl",
                columns: table => new
                {
                    cbl_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    pub_id = table.Column<int>(type: "integer", nullable: false),
                    utl_id = table.Column<int>(type: "integer", nullable: true),
                    cbl_id_original = table.Column<int>(type: "integer", nullable: true),
                    cbl_estreponse = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    cbl_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    cbl_texte = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_j_commentaireblog_cbl", x => x.cbl_id);
                    table.ForeignKey(
                        name: "FK_commentaireblog_cbl_id_original",
                        column: x => x.cbl_id_original,
                        principalTable: "t_j_commentaireblog_cbl",
                        principalColumn: "cbl_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_commentaireblog_pub_id",
                        column: x => x.pub_id,
                        principalTable: "t_h_blog_blg",
                        principalColumn: "pub_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_commentaireblog_utl_id",
                        column: x => x.utl_id,
                        principalTable: "t_e_utilisateur_utl",
                        principalColumn: "utl_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_commentaireblog_cbl_id_original",
                table: "t_j_commentaireblog_cbl",
                column: "cbl_id_original");

            migrationBuilder.CreateIndex(
                name: "IX_commentaireblog_pub_id",
                table: "t_j_commentaireblog_cbl",
                column: "pub_id");

            migrationBuilder.CreateIndex(
                name: "IX_commentaireblog_utl_id",
                table: "t_j_commentaireblog_cbl",
                column: "utl_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "t_j_commentaireblog_cbl");
        }
    }
}
