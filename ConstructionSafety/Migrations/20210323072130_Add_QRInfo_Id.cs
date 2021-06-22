using Microsoft.EntityFrameworkCore.Migrations;

namespace ConstructionSafety.Migrations
{
    public partial class Add_QRInfo_Id : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PersonId",
                table: "ProjectQRinfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectInfoId",
                table: "ProjectQRinfos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "ProjectQRinfos");

            migrationBuilder.DropColumn(
                name: "ProjectInfoId",
                table: "ProjectQRinfos");
        }
    }
}
