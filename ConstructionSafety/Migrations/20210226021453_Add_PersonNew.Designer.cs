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
    [Migration("20210226021453_Add_PersonNew")]
    partial class Add_PersonNew
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Models.Models.FileImg", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImgUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImgUuId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("FileImgs");
                });

            modelBuilder.Entity("Models.Models.FileUpload", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContentLable")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("DeleteMark")
                        .HasColumnType("int");

                    b.Property<string>("ImgName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImgUuId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NoticeId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NoticeType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProjectInfoId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Subtitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("author")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("FileUploads");
                });

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

            modelBuilder.Entity("Models.Models.PersonNew", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("MID")
                        .HasColumnType("int");

                    b.Property<string>("NoticeId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProjectInfoId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("PersonNews");
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
