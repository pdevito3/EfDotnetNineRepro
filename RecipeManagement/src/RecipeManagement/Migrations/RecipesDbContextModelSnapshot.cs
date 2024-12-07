﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RecipeManagement.Databases;

#nullable disable

namespace RecipeManagement.Migrations
{
    [DbContext(typeof(RecipesDbContext))]
    partial class RecipesDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("RecipeManagement.Domain.Recipes.Recipe", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateOnly?>("DateOfOrigin")
                        .HasColumnType("date")
                        .HasColumnName("date_of_origin");

                    b.Property<string>("Directions")
                        .HasColumnType("text")
                        .HasColumnName("directions");

                    b.Property<bool>("HaveMadeItMyself")
                        .HasColumnType("boolean")
                        .HasColumnName("have_made_it_myself");

                    b.PrimitiveCollection<string[]>("Tags")
                        .HasColumnType("text[]")
                        .HasColumnName("tags");

                    b.Property<string>("Title")
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("pk_recipes");

                    b.ToTable("recipes", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
