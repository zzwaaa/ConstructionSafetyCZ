﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Models.Models;

namespace ConstructionSafety.Migrations
{
    [DbContext(typeof(ProjLiefInsDBContext))]
    [Migration("20210128073015_Add_Avatar_LifeInUserInfo")]
    partial class Add_Avatar_LifeInUserInfo
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Models.Models.IntegralInfo", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IntegralNums")
                        .HasColumnType("int");

                    b.Property<string>("PersonId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProjectInfoId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UnIntegralNums")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("IntegralInfos");
                });

            modelBuilder.Entity("Models.Models.IntegralRecord", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IntegralNums")
                        .HasColumnType("int");

                    b.Property<string>("IntegralType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PersonId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProjectInfoId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("IntegralRecords");
                });

            modelBuilder.Entity("Models.Models.LifeInUserInfo", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Avatar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreateTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("DeleteMark")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PersonId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telephone")
                        .HasColumnType("nvarchar(11)")
                        .HasMaxLength(11);

                    b.Property<string>("UserInfoId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("LifeInUserInfos");
                });

            modelBuilder.Entity("Models.Models.ProjectPoint", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AllRanks")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PersonId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PersonName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProjectInfoId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProjectName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProjectRanks")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("ProjectPoints");
                });
#pragma warning restore 612, 618
        }
    }
}
