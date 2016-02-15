using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using Chirp.Database;

namespace Chirp.Migrations
{
    [DbContext(typeof(ChirpContext))]
    [Migration("20151202045221_InitalDatabase")]
    partial class InitalDatabase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348");

            modelBuilder.Entity("Chirp.Models.ChirpPost", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Message");

                    b.Property<DateTime>("PostTime");

                    b.Property<int>("UserId");

                    b.HasKey("Id");
                });
        }
    }
}
