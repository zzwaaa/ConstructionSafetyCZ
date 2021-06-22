using Microsoft.EntityFrameworkCore.Migrations;

namespace ConstructionSafety.Migrations
{
    public partial class Add_FileUpload : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileUploads",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    author = table.Column<string>(nullable: true),
                    NoticeId = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Subtitle = table.Column<string>(nullable: true),
                    NoticeType = table.Column<string>(nullable: true),
                    ImgUrl = table.Column<string>(nullable: true),
                    ImgName = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    ContentLable = table.Column<string>(nullable: true),
                    CreateDate = table.Column<string>(nullable: true),
                    DeleteMark = table.Column<int>(nullable: false),
                    ProjectInfoId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileUploads", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileUploads");
        }
    }
}
