using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ConstructionSafety.Migrations
{
    public partial class Add_CompanyPro : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectUsers",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyProId = table.Column<string>(nullable: true),
                    ProjectCompany = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    ProjPerson = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    CompanyName = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    DeleteMark = table.Column<int>(nullable: true),
                    PersonId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectUsers", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectUsers");
        }
    }
}
