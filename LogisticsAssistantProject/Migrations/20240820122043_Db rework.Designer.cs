﻿// <auto-generated />
using System;
using LogisticsAssistantProject.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LogisticsAssistantProject.Migrations
{
    [DbContext(typeof(AssistantDbContext))]
    [Migration("20240820122043_Db rework")]
    partial class Dbrework
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("LogisticsAssistantProject.Models.Domain.Transit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BreakDuration")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("MaxVelocity")
                        .HasColumnType("int");

                    b.Property<int>("MinutesUntilBreak")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("TruckId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Transit");
                });

            modelBuilder.Entity("LogisticsAssistantProject.Models.Domain.Truck", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BreakDuration")
                        .HasColumnType("int");

                    b.Property<int>("MaxVelocity")
                        .HasColumnType("int");

                    b.Property<int>("MinutesUntilBreak")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Truck");
                });

            modelBuilder.Entity("LogisticsAssistantProject.Models.Domain.TruckInTransit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("TransitId")
                        .HasColumnType("int");

                    b.Property<int>("TruckId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("TruckInTransit");
                });
#pragma warning restore 612, 618
        }
    }
}
