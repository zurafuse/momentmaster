﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MomentMaster.Models;

namespace MomentMaster.Migrations
{
    [DbContext(typeof(TimeObjectContext))]
    [Migration("20191214044430_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MomentMaster.Models.TimeObject", b =>
                {
                    b.Property<int>("Project_Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("HoursBudgeted")
                        .HasColumnType("int");

                    b.Property<int>("OriginalEstimate")
                        .HasColumnType("int");

                    b.Property<int>("Parent_Id")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("Tier")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("User_Id")
                        .HasColumnType("int");

                    b.HasKey("Project_Id");

                    b.ToTable("TimeObject");
                });
#pragma warning restore 612, 618
        }
    }
}
