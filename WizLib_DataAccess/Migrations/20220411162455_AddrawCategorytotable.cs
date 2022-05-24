using Microsoft.EntityFrameworkCore.Migrations;

namespace WizLib_DataAccess.Migrations
{
    public partial class AddrawCategorytotable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("insert into tbl_category values ('cat1')");
            migrationBuilder.Sql("insert into tbl_category values ('cat2')");
            migrationBuilder.Sql("insert into tbl_category values ('cat3')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
