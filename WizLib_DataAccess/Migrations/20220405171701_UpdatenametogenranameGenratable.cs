using Microsoft.EntityFrameworkCore.Migrations;

namespace WizLib_DataAccess.Migrations
{
    public partial class UpdatenametogenranameGenratable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.AddColumn<string>(
                name: "GenreName",
                table: "Genres",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.Sql("Update dbo.genres set GenreName=Name");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Genres");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Genres",
                type: "nvarchar(max)",
                nullable: true);
            migrationBuilder.Sql("Update dbo.genres set Name=GenreName");
            migrationBuilder.DropColumn(
                name: "GenreName",
                table: "Genres");
        }
    }
}
