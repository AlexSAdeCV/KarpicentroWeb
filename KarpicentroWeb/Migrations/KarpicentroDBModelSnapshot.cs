﻿// <auto-generated />
using System;
using KarpicentroWeb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace KarpicentroWeb.Migrations
{
    [DbContext(typeof(KarpicentroDB))]
    partial class KarpicentroDBModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("KarpicentroWeb.Models.CartBuys", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("SwitchCartBuy")
                        .HasColumnType("int");

                    b.Property<int>("idProduct")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("idProduct");

                    b.ToTable("Cart");
                });

            modelBuilder.Entity("KarpicentroWeb.Models.Directions", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("Active")
                        .HasColumnType("int");

                    b.Property<string>("Delegations")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameUser")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Neighborhood")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Street")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ZipCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("idUser")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("idUser");

                    b.ToTable("Direction");
                });

            modelBuilder.Entity("KarpicentroWeb.Models.Product", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("Active")
                        .HasColumnType("int");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductFeatures")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("PurchaseProduct")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("SalePrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Stock")
                        .HasColumnType("int");

                    b.Property<int>("idSupplier")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("idSupplier");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("KarpicentroWeb.Models.Supplier", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("Active")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameContact")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameSupplier")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("idDirections")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("idDirections");

                    b.ToTable("Proveedores");
                });

            modelBuilder.Entity("KarpicentroWeb.Models.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("Activate")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("User");
                });

            modelBuilder.Entity("KarpicentroWeb.Models.CartBuys", b =>
                {
                    b.HasOne("KarpicentroWeb.Models.Product", "Products")
                        .WithMany()
                        .HasForeignKey("idProduct")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Products");
                });

            modelBuilder.Entity("KarpicentroWeb.Models.Directions", b =>
                {
                    b.HasOne("KarpicentroWeb.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("idUser")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("KarpicentroWeb.Models.Product", b =>
                {
                    b.HasOne("KarpicentroWeb.Models.Supplier", "Suppliers")
                        .WithMany()
                        .HasForeignKey("idSupplier")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Suppliers");
                });

            modelBuilder.Entity("KarpicentroWeb.Models.Supplier", b =>
                {
                    b.HasOne("KarpicentroWeb.Models.Directions", "Directions")
                        .WithMany()
                        .HasForeignKey("idDirections")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Directions");
                });
#pragma warning restore 612, 618
        }
    }
}