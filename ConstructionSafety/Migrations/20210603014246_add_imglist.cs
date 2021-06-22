using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ConstructionSafety.Migrations
{
    public partial class add_imglist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Imagelist",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<byte[]>(nullable: true),
                    ImageName = table.Column<string>(nullable: true),
                    StartX = table.Column<int>(nullable: false),
                    StartY = table.Column<int>(nullable: false),
                    EndX = table.Column<int>(nullable: false),
                    EndY = table.Column<int>(nullable: false),
                    Width = table.Column<int>(nullable: true),
                    Height = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imagelist", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Imagelist");
        }
    }
}
