﻿// <auto-generated />
using System;
using Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infra.Data.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.Person", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Id")
                        .HasDefaultValueSql("newid()");

                    b.Property<DateTime?>("Birthday")
                        .HasColumnType("date")
                        .HasColumnName("Birthday");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("varchar")
                        .HasColumnName("Email");

                    b.Property<int>("Gender")
                        .HasColumnType("int")
                        .HasColumnName("Gender");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("varchar")
                        .HasColumnName("Name");

                    b.Property<string>("PersonalCode")
                        .HasMaxLength(11)
                        .HasColumnType("varchar")
                        .HasColumnName("CPF");

                    b.Property<string>("Phone")
                        .HasMaxLength(11)
                        .HasColumnType("varchar")
                        .HasColumnName("Phone");

                    b.HasKey("Id");

                    b.ToTable("People", "AF");
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Id")
                        .HasDefaultValueSql("newid()");

                    b.Property<string>("Hash")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar")
                        .HasColumnName("Hash");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar")
                        .HasColumnName("Login");

                    b.Property<Guid>("PersonId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("IdPerson");

                    b.Property<int>("Role")
                        .HasColumnType("int")
                        .HasColumnName("Role");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar")
                        .HasColumnName("Salt");

                    b.HasKey("Id");

                    b.HasIndex("PersonId");

                    b.ToTable("Users", "AF");
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.HasOne("Domain.Entities.Person", "Person")
                        .WithMany("Users")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");
                });

            modelBuilder.Entity("Domain.Entities.Person", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
