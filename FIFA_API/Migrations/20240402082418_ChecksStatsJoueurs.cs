using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FIFA_API.Migrations
{
    public partial class ChecksStatsJoueurs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "ck_stt_buts",
                table: "t_e_statistiques_stt");

            migrationBuilder.DropCheckConstraint(
                name: "ck_stt_matchsjoues",
                table: "t_e_statistiques_stt");

            migrationBuilder.DropCheckConstraint(
                name: "ck_stt_minutesjouees",
                table: "t_e_statistiques_stt");

            migrationBuilder.DropCheckConstraint(
                name: "ck_stt_titularisations",
                table: "t_e_statistiques_stt");

            migrationBuilder.AddCheckConstraint(
                name: "ck_stt_buts",
                table: "t_e_statistiques_stt",
                sql: "stt_buts >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "ck_stt_matchsjoues",
                table: "t_e_statistiques_stt",
                sql: "stt_matchsjoues >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "ck_stt_minutesjouees",
                table: "t_e_statistiques_stt",
                sql: "stt_minutesjouees >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "ck_stt_titularisations",
                table: "t_e_statistiques_stt",
                sql: "stt_titularisations >= 0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "ck_stt_buts",
                table: "t_e_statistiques_stt");

            migrationBuilder.DropCheckConstraint(
                name: "ck_stt_matchsjoues",
                table: "t_e_statistiques_stt");

            migrationBuilder.DropCheckConstraint(
                name: "ck_stt_minutesjouees",
                table: "t_e_statistiques_stt");

            migrationBuilder.DropCheckConstraint(
                name: "ck_stt_titularisations",
                table: "t_e_statistiques_stt");

            migrationBuilder.AddCheckConstraint(
                name: "ck_stt_buts",
                table: "t_e_statistiques_stt",
                sql: "stt_buts > 0");

            migrationBuilder.AddCheckConstraint(
                name: "ck_stt_matchsjoues",
                table: "t_e_statistiques_stt",
                sql: "stt_matchsjoues > 0");

            migrationBuilder.AddCheckConstraint(
                name: "ck_stt_minutesjouees",
                table: "t_e_statistiques_stt",
                sql: "stt_minutesjouees > 0");

            migrationBuilder.AddCheckConstraint(
                name: "ck_stt_titularisations",
                table: "t_e_statistiques_stt",
                sql: "stt_titularisations > 0");
        }
    }
}
