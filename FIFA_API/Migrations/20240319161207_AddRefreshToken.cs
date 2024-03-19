using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FIFA_API.Migrations
{
    public partial class AddRefreshToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "utl_refreshtoken",
                table: "t_e_utilisateur_utl",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "utl_refreshtoken",
                table: "t_e_utilisateur_utl");
        }
    }
}
