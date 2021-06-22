using Microsoft.EntityFrameworkCore.Migrations;

namespace ConstructionSafety.Migrations
{
    public partial class Add_RemoveMid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MID",
                table: "FileUploads");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MID",
                table: "FileUploads",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
