using Microsoft.EntityFrameworkCore.Migrations;

namespace ConstructionSafety.Migrations
{
    public partial class Add_MID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MID",
                table: "FileUploads",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MID",
                table: "FileUploads");
        }
    }
}
