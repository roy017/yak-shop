﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using yak_shop.Models;

namespace yak_shop.Migrations
{
    [DbContext(typeof(YakContext))]
    partial class YakContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("yak_shop.DetailsAndUtilities.YakDetails", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<float>("Age")
                        .HasColumnType("real");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Sex")
                        .IsRequired()
                        .HasColumnType("nvarchar(1)");

                    b.Property<float>("ageLastShaved")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.ToTable("YakItems");

                    //b.HasData(
                    //    new
                    //    {
                    //        Id = 1,
                    //        Age = 3f,
                    //        Name = "Billy",
                    //        Sex = "f",
                    //        ageLastShaved = 3f
                    //    });
                });
#pragma warning restore 612, 618
        }
    }
}
