﻿// <auto-generated />
using System;
using EFCoreSample.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EFCoreSample.Migrations
{
    [DbContext(typeof(EFcoreSampleContext))]
    [Migration("20240711074741_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EFCoreSample.DbContext.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasComment("訂單編號");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2")
                        .HasComment("建立時間");

                    b.Property<Guid>("OrderUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2")
                        .HasComment("更新時間");

                    b.HasKey("Id");

                    b.HasIndex("OrderUserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("EFCoreSample.DbContext.OrderDetail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasComment("訂單明細編號");

                    b.Property<Guid?>("OrderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)")
                        .HasComment("價格");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Quantity")
                        .HasColumnType("int")
                        .HasComment("數量");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("EFCoreSample.DbContext.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasComment("商品編號");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2")
                        .HasComment("建立時間");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)")
                        .HasComment("商品名稱");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)")
                        .HasComment("價格");

                    b.Property<int>("Quantity")
                        .HasColumnType("int")
                        .HasComment("數量");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("datetime2")
                        .HasComment("更新時間");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("EFCoreSample.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasComment("使用者編號");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2")
                        .HasComment("建立時間");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)")
                        .HasComment("使用者名稱");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasComment("密碼");

                    b.Property<string>("Roles")
                        .HasColumnType("nvarchar(max)")
                        .HasComment("角色");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2")
                        .HasComment("更新時間");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("EFCoreSample.DbContext.Order", b =>
                {
                    b.HasOne("EFCoreSample.Models.User", "OrderUser")
                        .WithMany()
                        .HasForeignKey("OrderUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OrderUser");
                });

            modelBuilder.Entity("EFCoreSample.DbContext.OrderDetail", b =>
                {
                    b.HasOne("EFCoreSample.DbContext.Order", null)
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderId");

                    b.HasOne("EFCoreSample.DbContext.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("EFCoreSample.DbContext.Order", b =>
                {
                    b.Navigation("OrderDetails");
                });
#pragma warning restore 612, 618
        }
    }
}
