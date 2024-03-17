using System;
using System.Collections.Generic;
using FIFA_API.Models.EntityFramework;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FIFA_API.Migrations
{
    public partial class CreationBDD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:code_status_commande", "preparation,validation,expedition,livre,annule,refuse,refus_accepte")
                .Annotation("Npgsql:Enum:pied_joueur", "gaucher,droitier,ambidextre")
                .Annotation("Npgsql:Enum:poste_joueur", "attaquant,defenseur,gardien");

            migrationBuilder.CreateTable(
                name: "t_e_adresse_adr",
                columns: table => new
                {
                    adr_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    adr_ville = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    adr_rue = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    adr_codepostal = table.Column<string>(type: "character(5)", fixedLength: true, maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_adresse_adr", x => x.adr_id);
                    table.CheckConstraint("ck_adr_codepostal", "adr_codepostal ~ '^([0-9]{2}|2[AB])[0-9]{3}$'");
                });

            migrationBuilder.CreateTable(
                name: "t_e_categorieproduit_cpr",
                columns: table => new
                {
                    cpr_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cpr_nom = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    cpr_idparent = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_categorieproduit_cpr", x => x.cpr_id);
                    table.ForeignKey(
                        name: "FK_categorieproduit_cpr_idparent",
                        column: x => x.cpr_idparent,
                        principalTable: "t_e_categorieproduit_cpr",
                        principalColumn: "cpr_id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "t_e_couleur_col",
                columns: table => new
                {
                    col_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    col_nom = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    col_codehexa = table.Column<string>(type: "character(6)", fixedLength: true, maxLength: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_couleur_col", x => x.col_id);
                    table.CheckConstraint("ck_col_codehexa", "col_codehexa ~ '^[0-9A-F]{6}$'");
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
                name: "t_e_photo_pht",
                columns: table => new
                {
                    pht_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    pht_nom = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    pht_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_photo_pht", x => x.pht_id);
                });

            migrationBuilder.CreateTable(
                name: "t_e_tailleproduit_tpr",
                columns: table => new
                {
                    tpr_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tpr_nom = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_tailleproduit_tpr", x => x.tpr_id);
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
                    tli_prix = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_typelivraison_tli", x => x.tli_id);
                    table.CheckConstraint("ck_tli_prix", "tli_prix > 0");
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
                    cpr_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_produit_prd", x => x.prd_id);
                    table.ForeignKey(
                        name: "FK_produit_cmp_id",
                        column: x => x.cmp_id,
                        principalTable: "t_e_competition_cmp",
                        principalColumn: "cmp_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_produit_cpr_id",
                        column: x => x.cpr_id,
                        principalTable: "t_e_categorieproduit_cpr",
                        principalColumn: "cpr_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_produit_gen_id",
                        column: x => x.gen_id,
                        principalTable: "t_e_genre_gen",
                        principalColumn: "gen_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_produit_nat_id",
                        column: x => x.nat_id,
                        principalTable: "t_e_nation_nat",
                        principalColumn: "nat_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "t_e_utilisateur_utl",
                columns: table => new
                {
                    utl_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    utl_prenom = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    utl_surnom = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    utl_telephone = table.Column<string>(type: "text", nullable: false),
                    utl_mail = table.Column<string>(type: "text", nullable: false),
                    utl_stripeid = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    utl_datenaissance = table.Column<DateTime>(type: "date", nullable: false),
                    utl_motdepasse = table.Column<string>(type: "character(60)", fixedLength: true, maxLength: 60, nullable: false),
                    utl_derniereconnexion = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    utl_dateverificationemail = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    utl_doubleauthentification = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    utl_role = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    lan_id = table.Column<int>(type: "integer", nullable: false),
                    pys_idpays = table.Column<int>(type: "integer", nullable: false),
                    utl_idpaysfavori = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_utilisateur_utl", x => x.utl_id);
                    table.CheckConstraint("ck_utl_telephone", "utl_telephone ~ '^0[1-9][0-9]{8}$'");
                    table.ForeignKey(
                        name: "FK_utilisateur_lan_id",
                        column: x => x.lan_id,
                        principalTable: "t_e_langue_lan",
                        principalColumn: "lan_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_utilisateur_pys_idpays",
                        column: x => x.pys_idpays,
                        principalTable: "t_e_pays_pys",
                        principalColumn: "pys_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_utilisateur_utl_idpaysfavori",
                        column: x => x.utl_idpaysfavori,
                        principalTable: "t_e_pays_pys",
                        principalColumn: "pys_id",
                        onDelete: ReferentialAction.SetNull);
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
                    jou_poids = table.Column<int>(type: "integer", nullable: false),
                    jou_taille = table.Column<int>(type: "integer", nullable: false),
                    jou_biographie = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    pht_id = table.Column<int>(type: "integer", nullable: true),
                    clb_id = table.Column<int>(type: "integer", nullable: false),
                    pys_id = table.Column<int>(type: "integer", nullable: false),
                    jou_pied = table.Column<PiedJoueur>(type: "pied_joueur", nullable: false),
                    jou_poste = table.Column<PosteJoueur>(type: "poste_joueur", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_joueur_jou", x => x.jou_id);
                    table.CheckConstraint("ck_jou_poids", "jou_poids > 0");
                    table.CheckConstraint("ck_jou_taille", "jou_taille > 0");
                    table.ForeignKey(
                        name: "FK_joueur_clb_id",
                        column: x => x.clb_id,
                        principalTable: "t_e_club_clb",
                        principalColumn: "clb_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_joueur_pht_id",
                        column: x => x.pht_id,
                        principalTable: "t_e_photo_pht",
                        principalColumn: "pht_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_joueur_pys_id",
                        column: x => x.pys_id,
                        principalTable: "t_e_pays_pys",
                        principalColumn: "pys_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "t_g_publication_pub",
                columns: table => new
                {
                    pub_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    pub_titre = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    pub_resume = table.Column<string>(type: "character varying(600)", maxLength: 600, nullable: false),
                    pub_datepublication = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    pht_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_g_publication_pub", x => x.pub_id);
                    table.ForeignKey(
                        name: "FK_publication_pht_id",
                        column: x => x.pht_id,
                        principalTable: "t_e_photo_pht",
                        principalColumn: "pht_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "t_j_produitcouleur_prc",
                columns: table => new
                {
                    prd_id = table.Column<int>(type: "integer", nullable: false),
                    col_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_j_produitcouleur_prc", x => new { x.prd_id, x.col_id });
                    table.ForeignKey(
                        name: "FK_produitcouleur_col_id",
                        column: x => x.col_id,
                        principalTable: "t_e_couleur_col",
                        principalColumn: "col_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_produitcouleur_prd_id",
                        column: x => x.prd_id,
                        principalTable: "t_e_produit_prd",
                        principalColumn: "prd_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "t_j_produittailleproduit_prt",
                columns: table => new
                {
                    prd_id = table.Column<int>(type: "integer", nullable: false),
                    tpr_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_j_produittailleproduit_prt", x => new { x.prd_id, x.tpr_id });
                    table.ForeignKey(
                        name: "FK_produittailleproduit_prd_id",
                        column: x => x.prd_id,
                        principalTable: "t_e_produit_prd",
                        principalColumn: "prd_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_produittailleproduit_tpr_id",
                        column: x => x.tpr_id,
                        principalTable: "t_e_tailleproduit_tpr",
                        principalColumn: "tpr_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "t_j_variantecouleurproduit_vcp",
                columns: table => new
                {
                    prd_id = table.Column<int>(type: "integer", nullable: false),
                    col_id = table.Column<int>(type: "integer", nullable: false),
                    vcp_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    vcp_prix = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    vcp_images = table.Column<List<string>>(type: "varchar[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_j_variantecouleurproduit_vcp", x => x.vcp_id);
                    table.CheckConstraint("ck_vcp_prix", "vcp_prix > 0");
                    table.ForeignKey(
                        name: "FK_variantecouleurproduit_col_id",
                        column: x => x.col_id,
                        principalTable: "t_e_couleur_col",
                        principalColumn: "col_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_variantecouleurproduit_prd_id",
                        column: x => x.prd_id,
                        principalTable: "t_e_produit_prd",
                        principalColumn: "prd_id",
                        onDelete: ReferentialAction.Restrict);
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
                    cmd_prixlivraison = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    cmd_datecommande = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    cmd_dateexpedition = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    cmd_datelivraison = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    cmd_urlfacture = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_commande_cmd", x => x.cmd_id);
                    table.CheckConstraint("ck_cmd_prixlivraison", "cmd_prixlivraison > 0");
                    table.ForeignKey(
                        name: "FK_commande_adr_facuration_id",
                        column: x => x.adr_facuration_id,
                        principalTable: "t_e_adresse_adr",
                        principalColumn: "adr_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_commande_adr_livraison_id",
                        column: x => x.adr_livraison_id,
                        principalTable: "t_e_adresse_adr",
                        principalColumn: "adr_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_commande_tli_id",
                        column: x => x.tli_id,
                        principalTable: "t_e_typelivraison_tli",
                        principalColumn: "tli_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_commande_utl_id",
                        column: x => x.utl_id,
                        principalTable: "t_e_utilisateur_utl",
                        principalColumn: "utl_id",
                        onDelete: ReferentialAction.Restrict);
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
                        name: "FK_voteutilisateur_col_id",
                        column: x => x.col_id,
                        principalTable: "t_e_couleur_col",
                        principalColumn: "col_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_voteutilisateur_tpr_id",
                        column: x => x.tpr_id,
                        principalTable: "t_e_tailleproduit_tpr",
                        principalColumn: "tpr_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_voteutilisateur_utl_id",
                        column: x => x.utl_id,
                        principalTable: "t_e_utilisateur_utl",
                        principalColumn: "utl_id",
                        onDelete: ReferentialAction.Restrict);
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
                    table.ForeignKey(
                        name: "FK_faqjoueur_jou_id",
                        column: x => x.jou_id,
                        principalTable: "t_e_joueur_jou",
                        principalColumn: "jou_id",
                        onDelete: ReferentialAction.Restrict);
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
                    table.CheckConstraint("ck_stt_buts", "stt_buts > 0");
                    table.CheckConstraint("ck_stt_matchsjoues", "stt_matchsjoues > 0");
                    table.CheckConstraint("ck_stt_minutesjouees", "stt_minutesjouees > 0");
                    table.CheckConstraint("ck_stt_titularisations", "stt_titularisations > 0");
                    table.ForeignKey(
                        name: "FK_statistiques_jou_id",
                        column: x => x.jou_id,
                        principalTable: "t_e_joueur_jou",
                        principalColumn: "jou_id",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateTable(
                name: "t_j_joueurtrophee_jot",
                columns: table => new
                {
                    jou_id = table.Column<int>(type: "integer", nullable: false),
                    tph_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_j_joueurtrophee_jot", x => new { x.jou_id, x.tph_id });
                    table.ForeignKey(
                        name: "FK_joueurtrophee_jou_id",
                        column: x => x.jou_id,
                        principalTable: "t_e_joueur_jou",
                        principalColumn: "jou_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_joueurtrophee_tph_id",
                        column: x => x.tph_id,
                        principalTable: "t_e_trophee_tph",
                        principalColumn: "tph_id",
                        onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.CreateTable(
                name: "t_j_stockproduit_spr",
                columns: table => new
                {
                    vcp_id = table.Column<int>(type: "integer", nullable: false),
                    tpr_id = table.Column<int>(type: "integer", nullable: false),
                    spr_stocks = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_j_stockproduit_spr", x => new { x.vcp_id, x.tpr_id });
                    table.CheckConstraint("ck_spr_stocks", "spr_stocks > 0");
                    table.ForeignKey(
                        name: "FK_stockproduit_tpr_id",
                        column: x => x.tpr_id,
                        principalTable: "t_e_tailleproduit_tpr",
                        principalColumn: "tpr_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_stockproduit_vcp_id",
                        column: x => x.vcp_id,
                        principalTable: "t_j_variantecouleurproduit_vcp",
                        principalColumn: "vcp_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "t_e_lignecommande_lco",
                columns: table => new
                {
                    lco_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    lco_quantite = table.Column<int>(type: "integer", nullable: false),
                    lco_prixunitaire = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    prd_id = table.Column<int>(type: "integer", nullable: false),
                    tpr_id = table.Column<int>(type: "integer", nullable: false),
                    cmd_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_lignecommande_lco", x => x.lco_id);
                    table.CheckConstraint("ck_lco_prixunitaire", "lco_prixunitaire >= 0");
                    table.CheckConstraint("ck_lco_quantite", "lco_quantite > 0");
                    table.ForeignKey(
                        name: "FK_lignecommande_cmd_id",
                        column: x => x.cmd_id,
                        principalTable: "t_e_commande_cmd",
                        principalColumn: "cmd_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_lignecommande_prd_id",
                        column: x => x.prd_id,
                        principalTable: "t_j_variantecouleurproduit_vcp",
                        principalColumn: "vcp_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_lignecommande_tpr_id",
                        column: x => x.tpr_id,
                        principalTable: "t_e_tailleproduit_tpr",
                        principalColumn: "tpr_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "t_j_statuscommande_sco",
                columns: table => new
                {
                    cmd_id = table.Column<int>(type: "integer", nullable: false),
                    sco_code = table.Column<CodeStatusCommande>(type: "code_status_commande", nullable: false),
                    sco_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    sco_commentaire = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_j_statuscommande_sco", x => new { x.cmd_id, x.sco_code });
                    table.ForeignKey(
                        name: "FK_statuscommande_cmd_id",
                        column: x => x.cmd_id,
                        principalTable: "t_e_commande_cmd",
                        principalColumn: "cmd_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "t_j_albumphoto_alp",
                columns: table => new
                {
                    pub_id = table.Column<int>(type: "integer", nullable: false),
                    pht_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_j_albumphoto_alp", x => new { x.pub_id, x.pht_id });
                    table.ForeignKey(
                        name: "FK_albumphoto_pht_id",
                        column: x => x.pht_id,
                        principalTable: "t_e_photo_pht",
                        principalColumn: "pht_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_albumphoto_pub_id",
                        column: x => x.pub_id,
                        principalTable: "t_h_album_alb",
                        principalColumn: "pub_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "t_j_articlephoto_arp",
                columns: table => new
                {
                    pub_id = table.Column<int>(type: "integer", nullable: false),
                    pht_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_j_articlephoto_arp", x => new { x.pub_id, x.pht_id });
                    table.ForeignKey(
                        name: "FK_articlephoto_pht_id",
                        column: x => x.pht_id,
                        principalTable: "t_e_photo_pht",
                        principalColumn: "pht_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_articlephoto_pub_id",
                        column: x => x.pub_id,
                        principalTable: "t_h_article_art",
                        principalColumn: "pub_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "t_j_articlevideo_arv",
                columns: table => new
                {
                    pub_id = table.Column<int>(type: "integer", nullable: false),
                    vid_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_j_articlevideo_arv", x => new { x.pub_id, x.vid_id });
                    table.ForeignKey(
                        name: "FK_articlevideo_pub_id",
                        column: x => x.pub_id,
                        principalTable: "t_h_article_art",
                        principalColumn: "pub_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_articlevideo_vid_id",
                        column: x => x.vid_id,
                        principalTable: "t_e_video_vid",
                        principalColumn: "vid_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "t_j_blogphoto_blp",
                columns: table => new
                {
                    pub_id = table.Column<int>(type: "integer", nullable: false),
                    pht_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_j_blogphoto_blp", x => new { x.pub_id, x.pht_id });
                    table.ForeignKey(
                        name: "FK_blogphoto_pht_id",
                        column: x => x.pht_id,
                        principalTable: "t_e_photo_pht",
                        principalColumn: "pht_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_blogphoto_pub_id",
                        column: x => x.pub_id,
                        principalTable: "t_h_blog_blg",
                        principalColumn: "pub_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_categorieproduit_cpr_idparent",
                table: "t_e_categorieproduit_cpr",
                column: "cpr_idparent");

            migrationBuilder.CreateIndex(
                name: "IX_categorieproduit_cpr_nom",
                table: "t_e_categorieproduit_cpr",
                column: "cpr_nom",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_club_clb_nom",
                table: "t_e_club_clb",
                column: "clb_nom",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_commande_adr_facuration_id",
                table: "t_e_commande_cmd",
                column: "adr_facuration_id");

            migrationBuilder.CreateIndex(
                name: "IX_commande_adr_livraison_id",
                table: "t_e_commande_cmd",
                column: "adr_livraison_id");

            migrationBuilder.CreateIndex(
                name: "IX_commande_cmd_urlfacture",
                table: "t_e_commande_cmd",
                column: "cmd_urlfacture",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_commande_tli_id",
                table: "t_e_commande_cmd",
                column: "tli_id");

            migrationBuilder.CreateIndex(
                name: "IX_commande_utl_id",
                table: "t_e_commande_cmd",
                column: "utl_id");

            migrationBuilder.CreateIndex(
                name: "IX_faqjoueur_jou_id",
                table: "t_e_faqjoueur_faq",
                column: "jou_id");

            migrationBuilder.CreateIndex(
                name: "IX_joueur_clb_id",
                table: "t_e_joueur_jou",
                column: "clb_id");

            migrationBuilder.CreateIndex(
                name: "IX_joueur_pht_id",
                table: "t_e_joueur_jou",
                column: "pht_id");

            migrationBuilder.CreateIndex(
                name: "IX_joueur_pys_id",
                table: "t_e_joueur_jou",
                column: "pys_id");

            migrationBuilder.CreateIndex(
                name: "IX_lignecommande_cmd_id",
                table: "t_e_lignecommande_lco",
                column: "cmd_id");

            migrationBuilder.CreateIndex(
                name: "IX_lignecommande_prd_id",
                table: "t_e_lignecommande_lco",
                column: "prd_id");

            migrationBuilder.CreateIndex(
                name: "IX_lignecommande_tpr_id",
                table: "t_e_lignecommande_lco",
                column: "tpr_id");

            migrationBuilder.CreateIndex(
                name: "IX_produit_cmp_id",
                table: "t_e_produit_prd",
                column: "cmp_id");

            migrationBuilder.CreateIndex(
                name: "IX_produit_cpr_id",
                table: "t_e_produit_prd",
                column: "cpr_id");

            migrationBuilder.CreateIndex(
                name: "IX_produit_gen_id",
                table: "t_e_produit_prd",
                column: "gen_id");

            migrationBuilder.CreateIndex(
                name: "IX_produit_nat_id",
                table: "t_e_produit_prd",
                column: "nat_id");

            migrationBuilder.CreateIndex(
                name: "IX_tailleproduit_tpr_nom",
                table: "t_e_tailleproduit_tpr",
                column: "tpr_nom",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_trophee_tph_nom",
                table: "t_e_trophee_tph",
                column: "tph_nom",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_utilisateur_lan_id",
                table: "t_e_utilisateur_utl",
                column: "lan_id");

            migrationBuilder.CreateIndex(
                name: "IX_utilisateur_pys_idpays",
                table: "t_e_utilisateur_utl",
                column: "pys_idpays");

            migrationBuilder.CreateIndex(
                name: "IX_utilisateur_utl_idpaysfavori",
                table: "t_e_utilisateur_utl",
                column: "utl_idpaysfavori");

            migrationBuilder.CreateIndex(
                name: "IX_utilisateur_utl_mail",
                table: "t_e_utilisateur_utl",
                column: "utl_mail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_voteutilisateur_col_id",
                table: "t_e_voteutilisateur_vtl",
                column: "col_id");

            migrationBuilder.CreateIndex(
                name: "IX_voteutilisateur_tpr_id",
                table: "t_e_voteutilisateur_vtl",
                column: "tpr_id");

            migrationBuilder.CreateIndex(
                name: "IX_publication_pht_id",
                table: "t_g_publication_pub",
                column: "pht_id");

            migrationBuilder.CreateIndex(
                name: "IX_albumphoto_pht_id",
                table: "t_j_albumphoto_alp",
                column: "pht_id");

            migrationBuilder.CreateIndex(
                name: "IX_articlephoto_pht_id",
                table: "t_j_articlephoto_arp",
                column: "pht_id");

            migrationBuilder.CreateIndex(
                name: "IX_articlevideo_vid_id",
                table: "t_j_articlevideo_arv",
                column: "vid_id");

            migrationBuilder.CreateIndex(
                name: "IX_blogphoto_pht_id",
                table: "t_j_blogphoto_blp",
                column: "pht_id");

            migrationBuilder.CreateIndex(
                name: "IX_joueurphoto_pht_id",
                table: "t_j_joueurphoto_jop",
                column: "pht_id");

            migrationBuilder.CreateIndex(
                name: "IX_joueurtrophee_tph_id",
                table: "t_j_joueurtrophee_jot",
                column: "tph_id");

            migrationBuilder.CreateIndex(
                name: "IX_produitcouleur_col_id",
                table: "t_j_produitcouleur_prc",
                column: "col_id");

            migrationBuilder.CreateIndex(
                name: "IX_produittailleproduit_tpr_id",
                table: "t_j_produittailleproduit_prt",
                column: "tpr_id");

            migrationBuilder.CreateIndex(
                name: "IX_stockproduit_tpr_id",
                table: "t_j_stockproduit_spr",
                column: "tpr_id");

            migrationBuilder.CreateIndex(
                name: "IX_variantecouleurproduit_col_id",
                table: "t_j_variantecouleurproduit_vcp",
                column: "col_id");

            migrationBuilder.CreateIndex(
                name: "IX_variantecouleurproduit_prd_id_col_id",
                table: "t_j_variantecouleurproduit_vcp",
                columns: new[] { "prd_id", "col_id" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "t_e_faqjoueur_faq");

            migrationBuilder.DropTable(
                name: "t_e_lignecommande_lco");

            migrationBuilder.DropTable(
                name: "t_e_statistiques_stt");

            migrationBuilder.DropTable(
                name: "t_e_voteutilisateur_vtl");

            migrationBuilder.DropTable(
                name: "t_h_document_doc");

            migrationBuilder.DropTable(
                name: "t_j_albumphoto_alp");

            migrationBuilder.DropTable(
                name: "t_j_articlephoto_arp");

            migrationBuilder.DropTable(
                name: "t_j_articlevideo_arv");

            migrationBuilder.DropTable(
                name: "t_j_blogphoto_blp");

            migrationBuilder.DropTable(
                name: "t_j_joueurphoto_jop");

            migrationBuilder.DropTable(
                name: "t_j_joueurtrophee_jot");

            migrationBuilder.DropTable(
                name: "t_j_produitcouleur_prc");

            migrationBuilder.DropTable(
                name: "t_j_produittailleproduit_prt");

            migrationBuilder.DropTable(
                name: "t_j_statuscommande_sco");

            migrationBuilder.DropTable(
                name: "t_j_stockproduit_spr");

            migrationBuilder.DropTable(
                name: "t_h_album_alb");

            migrationBuilder.DropTable(
                name: "t_h_article_art");

            migrationBuilder.DropTable(
                name: "t_e_video_vid");

            migrationBuilder.DropTable(
                name: "t_h_blog_blg");

            migrationBuilder.DropTable(
                name: "t_e_joueur_jou");

            migrationBuilder.DropTable(
                name: "t_e_trophee_tph");

            migrationBuilder.DropTable(
                name: "t_e_commande_cmd");

            migrationBuilder.DropTable(
                name: "t_e_tailleproduit_tpr");

            migrationBuilder.DropTable(
                name: "t_j_variantecouleurproduit_vcp");

            migrationBuilder.DropTable(
                name: "t_g_publication_pub");

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
                name: "t_e_produit_prd");

            migrationBuilder.DropTable(
                name: "t_e_photo_pht");

            migrationBuilder.DropTable(
                name: "t_e_langue_lan");

            migrationBuilder.DropTable(
                name: "t_e_pays_pys");

            migrationBuilder.DropTable(
                name: "t_e_competition_cmp");

            migrationBuilder.DropTable(
                name: "t_e_categorieproduit_cpr");

            migrationBuilder.DropTable(
                name: "t_e_genre_gen");

            migrationBuilder.DropTable(
                name: "t_e_nation_nat");
        }
    }
}
