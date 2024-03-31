using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FIFA_API.Migrations
{
    public partial class DeletesMedias : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_publication_pht_id",
                table: "t_g_publication_pub");

            migrationBuilder.DropForeignKey(
                name: "FK_albumphoto_pht_id",
                table: "t_j_albumphoto_alp");

            migrationBuilder.DropForeignKey(
                name: "FK_albumphoto_pub_id",
                table: "t_j_albumphoto_alp");

            migrationBuilder.DropForeignKey(
                name: "FK_articlephoto_pht_id",
                table: "t_j_articlephoto_arp");

            migrationBuilder.DropForeignKey(
                name: "FK_articlephoto_pub_id",
                table: "t_j_articlephoto_arp");

            migrationBuilder.DropForeignKey(
                name: "FK_articlevideo_pub_id",
                table: "t_j_articlevideo_arv");

            migrationBuilder.DropForeignKey(
                name: "FK_articlevideo_vid_id",
                table: "t_j_articlevideo_arv");

            migrationBuilder.DropForeignKey(
                name: "FK_blogphoto_pht_id",
                table: "t_j_blogphoto_blp");

            migrationBuilder.DropForeignKey(
                name: "FK_blogphoto_pub_id",
                table: "t_j_blogphoto_blp");

            migrationBuilder.DropForeignKey(
                name: "FK_joueurtrophee_jou_id",
                table: "t_j_joueurtrophee_jot");

            migrationBuilder.DropForeignKey(
                name: "FK_joueurtrophee_tph_id",
                table: "t_j_joueurtrophee_jot");

            migrationBuilder.DropForeignKey(
                name: "FK_produittailleproduit_prd_id",
                table: "t_j_produittailleproduit_prt");

            migrationBuilder.DropForeignKey(
                name: "FK_produittailleproduit_tpr_id",
                table: "t_j_produittailleproduit_prt");

            migrationBuilder.AddForeignKey(
                name: "FK_publication_pht_id",
                table: "t_g_publication_pub",
                column: "pht_id",
                principalTable: "t_e_photo_pht",
                principalColumn: "pht_id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_albumphoto_pht_id",
                table: "t_j_albumphoto_alp",
                column: "pht_id",
                principalTable: "t_e_photo_pht",
                principalColumn: "pht_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_albumphoto_pub_id",
                table: "t_j_albumphoto_alp",
                column: "pub_id",
                principalTable: "t_h_album_alb",
                principalColumn: "pub_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_articlephoto_pht_id",
                table: "t_j_articlephoto_arp",
                column: "pht_id",
                principalTable: "t_e_photo_pht",
                principalColumn: "pht_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_articlephoto_pub_id",
                table: "t_j_articlephoto_arp",
                column: "pub_id",
                principalTable: "t_h_article_art",
                principalColumn: "pub_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_articlevideo_pub_id",
                table: "t_j_articlevideo_arv",
                column: "pub_id",
                principalTable: "t_h_article_art",
                principalColumn: "pub_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_articlevideo_vid_id",
                table: "t_j_articlevideo_arv",
                column: "vid_id",
                principalTable: "t_e_video_vid",
                principalColumn: "vid_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_blogphoto_pht_id",
                table: "t_j_blogphoto_blp",
                column: "pht_id",
                principalTable: "t_e_photo_pht",
                principalColumn: "pht_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_blogphoto_pub_id",
                table: "t_j_blogphoto_blp",
                column: "pub_id",
                principalTable: "t_h_blog_blg",
                principalColumn: "pub_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_joueurtrophee_jou_id",
                table: "t_j_joueurtrophee_jot",
                column: "jou_id",
                principalTable: "t_e_joueur_jou",
                principalColumn: "jou_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_joueurtrophee_tph_id",
                table: "t_j_joueurtrophee_jot",
                column: "tph_id",
                principalTable: "t_e_trophee_tph",
                principalColumn: "tph_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_produittailleproduit_prd_id",
                table: "t_j_produittailleproduit_prt",
                column: "prd_id",
                principalTable: "t_e_produit_prd",
                principalColumn: "prd_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_produittailleproduit_tpr_id",
                table: "t_j_produittailleproduit_prt",
                column: "tpr_id",
                principalTable: "t_e_tailleproduit_tpr",
                principalColumn: "tpr_id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_publication_pht_id",
                table: "t_g_publication_pub");

            migrationBuilder.DropForeignKey(
                name: "FK_albumphoto_pht_id",
                table: "t_j_albumphoto_alp");

            migrationBuilder.DropForeignKey(
                name: "FK_albumphoto_pub_id",
                table: "t_j_albumphoto_alp");

            migrationBuilder.DropForeignKey(
                name: "FK_articlephoto_pht_id",
                table: "t_j_articlephoto_arp");

            migrationBuilder.DropForeignKey(
                name: "FK_articlephoto_pub_id",
                table: "t_j_articlephoto_arp");

            migrationBuilder.DropForeignKey(
                name: "FK_articlevideo_pub_id",
                table: "t_j_articlevideo_arv");

            migrationBuilder.DropForeignKey(
                name: "FK_articlevideo_vid_id",
                table: "t_j_articlevideo_arv");

            migrationBuilder.DropForeignKey(
                name: "FK_blogphoto_pht_id",
                table: "t_j_blogphoto_blp");

            migrationBuilder.DropForeignKey(
                name: "FK_blogphoto_pub_id",
                table: "t_j_blogphoto_blp");

            migrationBuilder.DropForeignKey(
                name: "FK_joueurtrophee_jou_id",
                table: "t_j_joueurtrophee_jot");

            migrationBuilder.DropForeignKey(
                name: "FK_joueurtrophee_tph_id",
                table: "t_j_joueurtrophee_jot");

            migrationBuilder.DropForeignKey(
                name: "FK_produittailleproduit_prd_id",
                table: "t_j_produittailleproduit_prt");

            migrationBuilder.DropForeignKey(
                name: "FK_produittailleproduit_tpr_id",
                table: "t_j_produittailleproduit_prt");

            migrationBuilder.AddForeignKey(
                name: "FK_publication_pht_id",
                table: "t_g_publication_pub",
                column: "pht_id",
                principalTable: "t_e_photo_pht",
                principalColumn: "pht_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_albumphoto_pht_id",
                table: "t_j_albumphoto_alp",
                column: "pht_id",
                principalTable: "t_e_photo_pht",
                principalColumn: "pht_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_albumphoto_pub_id",
                table: "t_j_albumphoto_alp",
                column: "pub_id",
                principalTable: "t_h_album_alb",
                principalColumn: "pub_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_articlephoto_pht_id",
                table: "t_j_articlephoto_arp",
                column: "pht_id",
                principalTable: "t_e_photo_pht",
                principalColumn: "pht_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_articlephoto_pub_id",
                table: "t_j_articlephoto_arp",
                column: "pub_id",
                principalTable: "t_h_article_art",
                principalColumn: "pub_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_articlevideo_pub_id",
                table: "t_j_articlevideo_arv",
                column: "pub_id",
                principalTable: "t_h_article_art",
                principalColumn: "pub_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_articlevideo_vid_id",
                table: "t_j_articlevideo_arv",
                column: "vid_id",
                principalTable: "t_e_video_vid",
                principalColumn: "vid_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_blogphoto_pht_id",
                table: "t_j_blogphoto_blp",
                column: "pht_id",
                principalTable: "t_e_photo_pht",
                principalColumn: "pht_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_blogphoto_pub_id",
                table: "t_j_blogphoto_blp",
                column: "pub_id",
                principalTable: "t_h_blog_blg",
                principalColumn: "pub_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_joueurtrophee_jou_id",
                table: "t_j_joueurtrophee_jot",
                column: "jou_id",
                principalTable: "t_e_joueur_jou",
                principalColumn: "jou_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_joueurtrophee_tph_id",
                table: "t_j_joueurtrophee_jot",
                column: "tph_id",
                principalTable: "t_e_trophee_tph",
                principalColumn: "tph_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_produittailleproduit_prd_id",
                table: "t_j_produittailleproduit_prt",
                column: "prd_id",
                principalTable: "t_e_produit_prd",
                principalColumn: "prd_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_produittailleproduit_tpr_id",
                table: "t_j_produittailleproduit_prt",
                column: "tpr_id",
                principalTable: "t_e_tailleproduit_tpr",
                principalColumn: "tpr_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
