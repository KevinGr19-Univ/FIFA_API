using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FIFA_API.Migrations
{
    public partial class CreationBDDMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "t_e_adresse_adr",
                columns: table => new
                {
                    adr_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    adr_ville = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    adr_rue = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    adr_codepostal = table.Column<string>(type: "char(5)", maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_adresse_adr", x => x.adr_id);
                });

            migrationBuilder.CreateTable(
                name: "t_e_categorieproduit_cpr",
                columns: table => new
                {
                    cpr_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cpr_nom = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    cpr_idparent = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_categorieproduit_cpr", x => x.cpr_id);
                    table.ForeignKey(
                        name: "FK_t_e_categorieproduit_cpr_t_e_categorieproduit_cpr_cpr_idpar~",
                        column: x => x.cpr_idparent,
                        principalTable: "t_e_categorieproduit_cpr",
                        principalColumn: "cpr_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "t_e_club_clb",
                columns: table => new
                {
                    clb_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    clb_nom = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_club_clb", x => x.clb_id);
                });

            migrationBuilder.CreateTable(
                name: "t_e_competition_cmp",
                columns: table => new
                {
                    cmp_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cmp_nom = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_competition_cmp", x => x.cmp_id);
                });

            migrationBuilder.CreateTable(
                name: "t_e_genre_gen",
                columns: table => new
                {
                    gen_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    gen_nom = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_genre_gen", x => x.gen_id);
                });

            migrationBuilder.CreateTable(
                name: "t_e_langue_lan",
                columns: table => new
                {
                    lan_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    lan_nom = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_langue_lan", x => x.lan_id);
                });

            migrationBuilder.CreateTable(
                name: "t_e_nation_nat",
                columns: table => new
                {
                    nat_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nat_nom = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_nation_nat", x => x.nat_id);
                });

            migrationBuilder.CreateTable(
                name: "t_e_pays_pys",
                columns: table => new
                {
                    pys_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    pys_nom = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_pays_pys", x => x.pys_id);
                });

            migrationBuilder.CreateTable(
                name: "t_e_trophee_tph",
                columns: table => new
                {
                    tph_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tph_nom = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_trophee_tph", x => x.tph_id);
                });

            migrationBuilder.CreateTable(
                name: "t_e_typelivraison_tli",
                columns: table => new
                {
                    tli_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tli_nom = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    tli_maxbusinessdays = table.Column<int>(type: "integer", nullable: false),
                    tli_prix = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_typelivraison_tli", x => x.tli_id);
                });

            migrationBuilder.CreateTable(
                name: "t_e_video_vid",
                columns: table => new
                {
                    vid_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    vid_nom = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    vid_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_video_vid", x => x.vid_id);
                });

            migrationBuilder.CreateTable(
                name: "t_e_produit_prd",
                columns: table => new
                {
                    prd_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    prd_titre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    prd_description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    cmp_id = table.Column<int>(type: "integer", nullable: false),
                    nat_id = table.Column<int>(type: "integer", nullable: false),
                    gen_id = table.Column<int>(type: "integer", nullable: false),
                    cpr_id = table.Column<int>(type: "integer", nullable: false),
                    ProduitId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_produit_prd", x => x.prd_id);
                    table.ForeignKey(
                        name: "FK_t_e_produit_prd_t_e_categorieproduit_cpr_cpr_id",
                        column: x => x.cpr_id,
                        principalTable: "t_e_categorieproduit_cpr",
                        principalColumn: "cpr_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_t_e_produit_prd_t_e_competition_cmp_cmp_id",
                        column: x => x.cmp_id,
                        principalTable: "t_e_competition_cmp",
                        principalColumn: "cmp_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_t_e_produit_prd_t_e_genre_gen_gen_id",
                        column: x => x.gen_id,
                        principalTable: "t_e_genre_gen",
                        principalColumn: "gen_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_t_e_produit_prd_t_e_nation_nat_nat_id",
                        column: x => x.nat_id,
                        principalTable: "t_e_nation_nat",
                        principalColumn: "nat_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_t_e_produit_prd_t_e_produit_prd_ProduitId",
                        column: x => x.ProduitId,
                        principalTable: "t_e_produit_prd",
                        principalColumn: "prd_id");
                });

            migrationBuilder.CreateTable(
                name: "t_e_utilisateur_utl",
                columns: table => new
                {
                    utl_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    lan_id = table.Column<int>(type: "integer", nullable: false),
                    pys_idpays = table.Column<int>(type: "integer", nullable: false),
                    utl_idpaysfavori = table.Column<int>(type: "integer", nullable: false),
                    utl_stripeid = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    utl_telephone = table.Column<string>(type: "text", nullable: false),
                    utl_prenom = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    utl_mail = table.Column<string>(type: "text", nullable: false),
                    utl_surnom = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    utl_datenaissance = table.Column<DateTime>(type: "date", nullable: false),
                    utl_motdepasse = table.Column<string>(type: "text", nullable: false),
                    utl_role = table.Column<int>(type: "integer", nullable: false),
                    utl_derniereconnexion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    utl_dateverificationemail = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    utl_doubleauthentification = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_utilisateur_utl", x => x.utl_id);
                    table.ForeignKey(
                        name: "FK_t_e_utilisateur_utl_t_e_langue_lan_lan_id",
                        column: x => x.lan_id,
                        principalTable: "t_e_langue_lan",
                        principalColumn: "lan_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_t_e_utilisateur_utl_t_e_pays_pys_pys_idpays",
                        column: x => x.pys_idpays,
                        principalTable: "t_e_pays_pys",
                        principalColumn: "pys_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_t_e_utilisateur_utl_t_e_pays_pys_utl_idpaysfavori",
                        column: x => x.utl_idpaysfavori,
                        principalTable: "t_e_pays_pys",
                        principalColumn: "pys_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "t_e_couleur_col",
                columns: table => new
                {
                    col_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    col_nom = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    col_codehexa = table.Column<string>(type: "text", nullable: false),
                    ProduitId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_couleur_col", x => x.col_id);
                    table.ForeignKey(
                        name: "FK_t_e_couleur_col_t_e_produit_prd_ProduitId",
                        column: x => x.ProduitId,
                        principalTable: "t_e_produit_prd",
                        principalColumn: "prd_id");
                });

            migrationBuilder.CreateTable(
                name: "t_e_tailleproduit_tpr",
                columns: table => new
                {
                    tpr_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tpr_nom = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ProduitId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_tailleproduit_tpr", x => x.tpr_id);
                    table.ForeignKey(
                        name: "FK_t_e_tailleproduit_tpr_t_e_produit_prd_ProduitId",
                        column: x => x.ProduitId,
                        principalTable: "t_e_produit_prd",
                        principalColumn: "prd_id");
                });

            migrationBuilder.CreateTable(
                name: "t_e_commande_cmd",
                columns: table => new
                {
                    cmd_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tli_id = table.Column<int>(type: "integer", nullable: false),
                    utl_id = table.Column<int>(type: "integer", nullable: false),
                    adr_livraison_id = table.Column<int>(type: "integer", nullable: false),
                    adr_facuration_id = table.Column<int>(type: "integer", nullable: false),
                    cmd_prixlivraison = table.Column<decimal>(type: "numeric", nullable: false),
                    cmd_dateexpedition = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    cmd_datecommande = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    cmd_datelivraison = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    cmd_urlfacture = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_commande_cmd", x => x.cmd_id);
                    table.ForeignKey(
                        name: "FK_t_e_commande_cmd_t_e_adresse_adr_adr_facuration_id",
                        column: x => x.adr_facuration_id,
                        principalTable: "t_e_adresse_adr",
                        principalColumn: "adr_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_t_e_commande_cmd_t_e_adresse_adr_adr_livraison_id",
                        column: x => x.adr_livraison_id,
                        principalTable: "t_e_adresse_adr",
                        principalColumn: "adr_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_t_e_commande_cmd_t_e_typelivraison_tli_tli_id",
                        column: x => x.tli_id,
                        principalTable: "t_e_typelivraison_tli",
                        principalColumn: "tli_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_t_e_commande_cmd_t_e_utilisateur_utl_utl_id",
                        column: x => x.utl_id,
                        principalTable: "t_e_utilisateur_utl",
                        principalColumn: "utl_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "t_j_variantecouleurproduit_vcp",
                columns: table => new
                {
                    prd_id = table.Column<int>(type: "integer", nullable: false),
                    col_id = table.Column<int>(type: "integer", nullable: false),
                    vcp_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    vcp_prix = table.Column<decimal>(type: "numeric", nullable: false),
                    vcp_images = table.Column<List<string>>(type: "varchar[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_j_variantecouleurproduit_vcp", x => x.vcp_id);
                    table.ForeignKey(
                        name: "FK_t_j_variantecouleurproduit_vcp_t_e_couleur_col_col_id",
                        column: x => x.col_id,
                        principalTable: "t_e_couleur_col",
                        principalColumn: "col_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_t_j_variantecouleurproduit_vcp_t_e_produit_prd_prd_id",
                        column: x => x.prd_id,
                        principalTable: "t_e_produit_prd",
                        principalColumn: "prd_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "t_e_voteutilisateur_vtl",
                columns: table => new
                {
                    utl_id = table.Column<int>(type: "integer", nullable: false),
                    col_id = table.Column<int>(type: "integer", nullable: false),
                    tpr_id = table.Column<int>(type: "integer", nullable: false),
                    vtl_rankvote = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_voteutilisateur_vtl", x => new { x.utl_id, x.col_id, x.tpr_id });
                    table.ForeignKey(
                        name: "FK_t_e_voteutilisateur_vtl_t_e_couleur_col_col_id",
                        column: x => x.col_id,
                        principalTable: "t_e_couleur_col",
                        principalColumn: "col_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_t_e_voteutilisateur_vtl_t_e_tailleproduit_tpr_tpr_id",
                        column: x => x.tpr_id,
                        principalTable: "t_e_tailleproduit_tpr",
                        principalColumn: "tpr_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_t_e_voteutilisateur_vtl_t_e_utilisateur_utl_utl_id",
                        column: x => x.utl_id,
                        principalTable: "t_e_utilisateur_utl",
                        principalColumn: "utl_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "t_j_statuscommande_sco",
                columns: table => new
                {
                    cmd_id = table.Column<int>(type: "integer", nullable: false),
                    sco_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    sco_commentaire = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Code = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_j_statuscommande_sco", x => x.cmd_id);
                    table.ForeignKey(
                        name: "FK_t_j_statuscommande_sco_t_e_commande_cmd_cmd_id",
                        column: x => x.cmd_id,
                        principalTable: "t_e_commande_cmd",
                        principalColumn: "cmd_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "t_e_lignecommande_lco",
                columns: table => new
                {
                    lco_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    lco_quantite = table.Column<int>(type: "integer", nullable: false),
                    lco_prixunitaire = table.Column<decimal>(type: "numeric(2)", precision: 2, nullable: false),
                    prd_id = table.Column<int>(type: "integer", nullable: false),
                    tpr_id = table.Column<int>(type: "integer", nullable: false),
                    cmd_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_lignecommande_lco", x => x.lco_id);
                    table.ForeignKey(
                        name: "FK_t_e_lignecommande_lco_t_e_commande_cmd_cmd_id",
                        column: x => x.cmd_id,
                        principalTable: "t_e_commande_cmd",
                        principalColumn: "cmd_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_t_e_lignecommande_lco_t_e_tailleproduit_tpr_tpr_id",
                        column: x => x.tpr_id,
                        principalTable: "t_e_tailleproduit_tpr",
                        principalColumn: "tpr_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_t_e_lignecommande_lco_t_j_variantecouleurproduit_vcp_prd_id",
                        column: x => x.prd_id,
                        principalTable: "t_j_variantecouleurproduit_vcp",
                        principalColumn: "vcp_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "t_j_stockproduit_spr",
                columns: table => new
                {
                    vcp_id = table.Column<int>(type: "integer", nullable: false),
                    tpr_id = table.Column<int>(type: "integer", nullable: false),
                    spr_stocks = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_j_stockproduit_spr", x => new { x.vcp_id, x.tpr_id });
                    table.ForeignKey(
                        name: "FK_t_j_stockproduit_spr_t_e_tailleproduit_tpr_tpr_id",
                        column: x => x.tpr_id,
                        principalTable: "t_e_tailleproduit_tpr",
                        principalColumn: "tpr_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_t_j_stockproduit_spr_t_j_variantecouleurproduit_vcp_vcp_id",
                        column: x => x.vcp_id,
                        principalTable: "t_j_variantecouleurproduit_vcp",
                        principalColumn: "vcp_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JoueurTrophee",
                columns: table => new
                {
                    JoueursId = table.Column<int>(type: "integer", nullable: false),
                    TropheesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JoueurTrophee", x => new { x.JoueursId, x.TropheesId });
                    table.ForeignKey(
                        name: "FK_JoueurTrophee_t_e_trophee_tph_TropheesId",
                        column: x => x.TropheesId,
                        principalTable: "t_e_trophee_tph",
                        principalColumn: "tph_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "t_e_faqjoueur_faq",
                columns: table => new
                {
                    faq_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    jou_id = table.Column<int>(type: "integer", nullable: false),
                    faq_question = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    faq_reponse = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_faqjoueur_faq", x => x.faq_id);
                });

            migrationBuilder.CreateTable(
                name: "t_e_joueur_jou",
                columns: table => new
                {
                    jou_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    jou_nom = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    jou_prenom = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    jou_datenaissance = table.Column<DateTime>(type: "date", nullable: true),
                    jou_lieunaissance = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    jou_pied = table.Column<int>(type: "integer", nullable: false),
                    jou_poids = table.Column<int>(type: "integer", nullable: false),
                    jou_taille = table.Column<int>(type: "integer", nullable: false),
                    jou_poste = table.Column<int>(type: "integer", nullable: false),
                    jou_biographie = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    pht_id = table.Column<int>(type: "integer", nullable: false),
                    clb_id = table.Column<int>(type: "integer", nullable: false),
                    pys_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_joueur_jou", x => x.jou_id);
                    table.ForeignKey(
                        name: "FK_t_e_joueur_jou_t_e_club_clb_clb_id",
                        column: x => x.clb_id,
                        principalTable: "t_e_club_clb",
                        principalColumn: "clb_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_t_e_joueur_jou_t_e_pays_pys_pys_id",
                        column: x => x.pys_id,
                        principalTable: "t_e_pays_pys",
                        principalColumn: "pys_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "t_e_statistiques_stt",
                columns: table => new
                {
                    jou_id = table.Column<int>(type: "integer", nullable: false),
                    stt_matchsjoues = table.Column<int>(type: "integer", nullable: false),
                    stt_titularisations = table.Column<int>(type: "integer", nullable: false),
                    stt_minutesjouees = table.Column<int>(type: "integer", nullable: false),
                    stt_buts = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_statistiques_stt", x => x.jou_id);
                    table.ForeignKey(
                        name: "FK_t_e_statistiques_stt_t_e_joueur_jou_jou_id",
                        column: x => x.jou_id,
                        principalTable: "t_e_joueur_jou",
                        principalColumn: "jou_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "t_e_photo_pht",
                columns: table => new
                {
                    pht_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    pht_nom = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    pht_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    AlbumId = table.Column<int>(type: "integer", nullable: true),
                    BlogId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_photo_pht", x => x.pht_id);
                });

            migrationBuilder.CreateTable(
                name: "t_g_publication_pub",
                columns: table => new
                {
                    pub_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    pub_titre = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    pub_resume = table.Column<string>(type: "character varying(600)", maxLength: 600, nullable: false),
                    pub_datepublication = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    pht_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_g_publication_pub", x => x.pub_id);
                    table.ForeignKey(
                        name: "FK_t_g_publication_pub_t_e_photo_pht_pht_id",
                        column: x => x.pht_id,
                        principalTable: "t_e_photo_pht",
                        principalColumn: "pht_id");
                });

            migrationBuilder.CreateTable(
                name: "t_h_album_alb",
                columns: table => new
                {
                    pub_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_h_album_alb", x => x.pub_id);
                    table.ForeignKey(
                        name: "FK_t_h_album_alb_t_g_publication_pub_pub_id",
                        column: x => x.pub_id,
                        principalTable: "t_g_publication_pub",
                        principalColumn: "pub_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "t_h_article_art",
                columns: table => new
                {
                    pub_id = table.Column<int>(type: "integer", nullable: false),
                    art_texte = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_h_article_art", x => x.pub_id);
                    table.ForeignKey(
                        name: "FK_t_h_article_art_t_g_publication_pub_pub_id",
                        column: x => x.pub_id,
                        principalTable: "t_g_publication_pub",
                        principalColumn: "pub_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "t_h_blog_blg",
                columns: table => new
                {
                    pub_id = table.Column<int>(type: "integer", nullable: false),
                    blg_texte = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_h_blog_blg", x => x.pub_id);
                    table.ForeignKey(
                        name: "FK_t_h_blog_blg_t_g_publication_pub_pub_id",
                        column: x => x.pub_id,
                        principalTable: "t_g_publication_pub",
                        principalColumn: "pub_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "t_h_document_doc",
                columns: table => new
                {
                    pub_id = table.Column<int>(type: "integer", nullable: false),
                    doc_urlpdf = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_h_document_doc", x => x.pub_id);
                    table.ForeignKey(
                        name: "FK_t_h_document_doc_t_g_publication_pub_pub_id",
                        column: x => x.pub_id,
                        principalTable: "t_g_publication_pub",
                        principalColumn: "pub_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JoueurTrophee_TropheesId",
                table: "JoueurTrophee",
                column: "TropheesId");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_categorieproduit_cpr_cpr_idparent",
                table: "t_e_categorieproduit_cpr",
                column: "cpr_idparent");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_categorieproduit_cpr_cpr_nom",
                table: "t_e_categorieproduit_cpr",
                column: "cpr_nom",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_t_e_club_clb_clb_nom",
                table: "t_e_club_clb",
                column: "clb_nom",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_t_e_commande_cmd_adr_facuration_id",
                table: "t_e_commande_cmd",
                column: "adr_facuration_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_commande_cmd_adr_livraison_id",
                table: "t_e_commande_cmd",
                column: "adr_livraison_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_commande_cmd_cmd_urlfacture",
                table: "t_e_commande_cmd",
                column: "cmd_urlfacture",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_t_e_commande_cmd_tli_id",
                table: "t_e_commande_cmd",
                column: "tli_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_commande_cmd_utl_id",
                table: "t_e_commande_cmd",
                column: "utl_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_couleur_col_ProduitId",
                table: "t_e_couleur_col",
                column: "ProduitId");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_faqjoueur_faq_jou_id",
                table: "t_e_faqjoueur_faq",
                column: "jou_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_joueur_jou_clb_id",
                table: "t_e_joueur_jou",
                column: "clb_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_joueur_jou_pht_id",
                table: "t_e_joueur_jou",
                column: "pht_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_joueur_jou_pys_id",
                table: "t_e_joueur_jou",
                column: "pys_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_lignecommande_lco_cmd_id",
                table: "t_e_lignecommande_lco",
                column: "cmd_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_lignecommande_lco_prd_id",
                table: "t_e_lignecommande_lco",
                column: "prd_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_lignecommande_lco_tpr_id",
                table: "t_e_lignecommande_lco",
                column: "tpr_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_photo_pht_AlbumId",
                table: "t_e_photo_pht",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_photo_pht_BlogId",
                table: "t_e_photo_pht",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_produit_prd_cmp_id",
                table: "t_e_produit_prd",
                column: "cmp_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_produit_prd_cpr_id",
                table: "t_e_produit_prd",
                column: "cpr_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_produit_prd_gen_id",
                table: "t_e_produit_prd",
                column: "gen_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_produit_prd_nat_id",
                table: "t_e_produit_prd",
                column: "nat_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_produit_prd_ProduitId",
                table: "t_e_produit_prd",
                column: "ProduitId");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_tailleproduit_tpr_ProduitId",
                table: "t_e_tailleproduit_tpr",
                column: "ProduitId");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_tailleproduit_tpr_tpr_nom",
                table: "t_e_tailleproduit_tpr",
                column: "tpr_nom",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_t_e_trophee_tph_tph_nom",
                table: "t_e_trophee_tph",
                column: "tph_nom",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_t_e_utilisateur_utl_lan_id",
                table: "t_e_utilisateur_utl",
                column: "lan_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_utilisateur_utl_pys_idpays",
                table: "t_e_utilisateur_utl",
                column: "pys_idpays");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_utilisateur_utl_utl_idpaysfavori",
                table: "t_e_utilisateur_utl",
                column: "utl_idpaysfavori");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_voteutilisateur_vtl_col_id",
                table: "t_e_voteutilisateur_vtl",
                column: "col_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_voteutilisateur_vtl_tpr_id",
                table: "t_e_voteutilisateur_vtl",
                column: "tpr_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_g_publication_pub_pht_id",
                table: "t_g_publication_pub",
                column: "pht_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_j_stockproduit_spr_tpr_id",
                table: "t_j_stockproduit_spr",
                column: "tpr_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_j_variantecouleurproduit_vcp_col_id",
                table: "t_j_variantecouleurproduit_vcp",
                column: "col_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_j_variantecouleurproduit_vcp_prd_id_col_id",
                table: "t_j_variantecouleurproduit_vcp",
                columns: new[] { "prd_id", "col_id" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_JoueurTrophee_t_e_joueur_jou_JoueursId",
                table: "JoueurTrophee",
                column: "JoueursId",
                principalTable: "t_e_joueur_jou",
                principalColumn: "jou_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_t_e_faqjoueur_faq_t_e_joueur_jou_jou_id",
                table: "t_e_faqjoueur_faq",
                column: "jou_id",
                principalTable: "t_e_joueur_jou",
                principalColumn: "jou_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_t_e_joueur_jou_t_e_photo_pht_pht_id",
                table: "t_e_joueur_jou",
                column: "pht_id",
                principalTable: "t_e_photo_pht",
                principalColumn: "pht_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_t_e_photo_pht_t_h_album_alb_AlbumId",
                table: "t_e_photo_pht",
                column: "AlbumId",
                principalTable: "t_h_album_alb",
                principalColumn: "pub_id");

            migrationBuilder.AddForeignKey(
                name: "FK_t_e_photo_pht_t_h_blog_blg_BlogId",
                table: "t_e_photo_pht",
                column: "BlogId",
                principalTable: "t_h_blog_blg",
                principalColumn: "pub_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_t_g_publication_pub_t_e_photo_pht_pht_id",
                table: "t_g_publication_pub");

            migrationBuilder.DropTable(
                name: "JoueurTrophee");

            migrationBuilder.DropTable(
                name: "t_e_faqjoueur_faq");

            migrationBuilder.DropTable(
                name: "t_e_lignecommande_lco");

            migrationBuilder.DropTable(
                name: "t_e_statistiques_stt");

            migrationBuilder.DropTable(
                name: "t_e_video_vid");

            migrationBuilder.DropTable(
                name: "t_e_voteutilisateur_vtl");

            migrationBuilder.DropTable(
                name: "t_h_article_art");

            migrationBuilder.DropTable(
                name: "t_h_document_doc");

            migrationBuilder.DropTable(
                name: "t_j_statuscommande_sco");

            migrationBuilder.DropTable(
                name: "t_j_stockproduit_spr");

            migrationBuilder.DropTable(
                name: "t_e_trophee_tph");

            migrationBuilder.DropTable(
                name: "t_e_joueur_jou");

            migrationBuilder.DropTable(
                name: "t_e_commande_cmd");

            migrationBuilder.DropTable(
                name: "t_e_tailleproduit_tpr");

            migrationBuilder.DropTable(
                name: "t_j_variantecouleurproduit_vcp");

            migrationBuilder.DropTable(
                name: "t_e_club_clb");

            migrationBuilder.DropTable(
                name: "t_e_adresse_adr");

            migrationBuilder.DropTable(
                name: "t_e_typelivraison_tli");

            migrationBuilder.DropTable(
                name: "t_e_utilisateur_utl");

            migrationBuilder.DropTable(
                name: "t_e_couleur_col");

            migrationBuilder.DropTable(
                name: "t_e_langue_lan");

            migrationBuilder.DropTable(
                name: "t_e_pays_pys");

            migrationBuilder.DropTable(
                name: "t_e_produit_prd");

            migrationBuilder.DropTable(
                name: "t_e_categorieproduit_cpr");

            migrationBuilder.DropTable(
                name: "t_e_competition_cmp");

            migrationBuilder.DropTable(
                name: "t_e_genre_gen");

            migrationBuilder.DropTable(
                name: "t_e_nation_nat");

            migrationBuilder.DropTable(
                name: "t_e_photo_pht");

            migrationBuilder.DropTable(
                name: "t_h_album_alb");

            migrationBuilder.DropTable(
                name: "t_h_blog_blg");

            migrationBuilder.DropTable(
                name: "t_g_publication_pub");
        }
    }
}
