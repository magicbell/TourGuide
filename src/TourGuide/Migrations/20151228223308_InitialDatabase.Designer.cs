using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using TourGuide.Models;

namespace TourGuide.Migrations
{
    [DbContext(typeof(TripContext))]
    [Migration("20151228223308_InitialDatabase")]
    partial class InitialDatabase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TourGuide.Models.Point", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Arrival");

                    b.Property<DateTime>("Created");

                    b.Property<string>("Description");

                    b.Property<double>("Latitude");

                    b.Property<double>("Longitude");

                    b.Property<string>("Name");

                    b.Property<int?>("PointId");

                    b.Property<int?>("RouteId");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("TourGuide.Models.Route", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<string>("Description");

                    b.Property<DateTime>("EndTime");

                    b.Property<string>("Name");

                    b.Property<DateTime>("StartDate");

                    b.Property<int>("UserId");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("TourGuide.Models.Point", b =>
                {
                    b.HasOne("TourGuide.Models.Point")
                        .WithMany()
                        .HasForeignKey("PointId");

                    b.HasOne("TourGuide.Models.Route")
                        .WithMany()
                        .HasForeignKey("RouteId");
                });
        }
    }
}
