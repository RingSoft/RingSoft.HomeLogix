﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RingSoft.HomeLogix.Sqlite;

namespace RingSoft.HomeLogix.Sqlite.Migrations
{
    [DbContext(typeof(HomeLogixDbContext))]
    [Migration("20201130215928_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("RingSoft.HomeLogix.DataAccess.Model.SystemMaster", b =>
                {
                    b.Property<string>("HouseholdName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.HasKey("HouseholdName");

                    b.ToTable("SystemMaster");
                });
#pragma warning restore 612, 618
        }
    }
}