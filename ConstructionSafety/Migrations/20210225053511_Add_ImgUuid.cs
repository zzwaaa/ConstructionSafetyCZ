using Microsoft.EntityFrameworkCore.Migrations;

namespace ConstructionSafety.Migrations
{
    public partial class Add_ImgUuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgUrl",
                table: "FileUploads");

            migrationBuilder.AddColumn<int>(
                name: "ImgUuId",
                table: "FileUploads",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgUuId",
                table: "FileUploads");

            migrationBuilder.AddColumn<string>(
                name: "ImgUrl",
                table: "FileUploads",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
