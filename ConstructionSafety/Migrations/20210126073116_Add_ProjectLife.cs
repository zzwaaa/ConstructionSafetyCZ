using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ConstructionSafety.Migrations
{
    public partial class Add_ProjectLife : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IntegralInfos",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonId = table.Column<string>(nullable: true),
                    IntegralNums = table.Column<int>(nullable: false),
                    UnIntegralNums = table.Column<int>(nullable: false),
                    ProjectInfoId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegralInfos", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "IntegralRecords",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IntegralType = table.Column<string>(nullable: true),
                    IntegralNums = table.Column<int>(nullable: false),
                    PersonId = table.Column<string>(nullable: true),
                    ProjectInfoId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegralRecords", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LifeInUserInfos",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserInfoId = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Telephone = table.Column<string>(maxLength: 11, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    DeleteMark = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LifeInUserInfos", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ProjectPoints",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectInfoId = table.Column<string>(nullable: true),
                    ProjectName = table.Column<string>(nullable: true),
                    PersonId = table.Column<string>(nullable: true),
                    PersonName = table.Column<string>(nullable: true),
                    AllRanks = table.Column<string>(nullable: true),
                    ProjectRanks = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectPoints", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IntegralInfos");

            migrationBuilder.DropTable(
                name: "IntegralRecords");

            migrationBuilder.DropTable(
                name: "LifeInUserInfos");

            migrationBuilder.DropTable(
                name: "ProjectPoints");
        }
    }
}
