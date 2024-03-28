using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FIFA_API.Migrations
{
    public partial class FixStocks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "ck_spr_stocks",
                table: "t_j_stockproduit_spr");

            migrationBuilder.AddCheckConstraint(
                name: "ck_spr_stocks",
                table: "t_j_stockproduit_spr",
                sql: "spr_stocks >= 0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "ck_spr_stocks",
                table: "t_j_stockproduit_spr");

            migrationBuilder.AddCheckConstraint(
                name: "ck_spr_stocks",
                table: "t_j_stockproduit_spr",
                sql: "spr_stocks > 0");
        }
    }
}
