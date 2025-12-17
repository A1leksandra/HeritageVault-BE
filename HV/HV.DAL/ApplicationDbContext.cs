using HV.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace HV.DAL;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Country> Countries { get; set; }
    public DbSet<Region> Regions { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Landmark> Landmarks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(120);

            entity.Property(e => e.NormalizedName)
                .IsRequired()
                .HasMaxLength(120);

            entity.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(3);

            entity.Property(e => e.IsDeleted)
                .IsRequired();

            entity.HasIndex(e => e.Code)
                .IsUnique();

            entity.HasIndex(e => e.NormalizedName)
                .IsUnique();
        });

        modelBuilder.Entity<Region>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.CountryId)
                .IsRequired();

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(160);

            entity.Property(e => e.NormalizedName)
                .IsRequired()
                .HasMaxLength(160);

            entity.Property(e => e.Type)
                .HasMaxLength(40);

            entity.Property(e => e.IsDeleted)
                .IsRequired();

            entity.HasIndex(e => new { e.CountryId, e.NormalizedName })
                .IsUnique();

            entity.HasOne(e => e.Country)
                .WithMany(c => c.Regions)
                .HasForeignKey(e => e.CountryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.CountryId)
                .IsRequired();

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(160);

            entity.Property(e => e.NormalizedName)
                .IsRequired()
                .HasMaxLength(160);

            entity.Property(e => e.Latitude)
                .HasPrecision(9, 6);

            entity.Property(e => e.Longitude)
                .HasPrecision(9, 6);

            entity.Property(e => e.IsDeleted)
                .IsRequired();

            entity.HasIndex(e => new { e.CountryId, e.RegionId, e.NormalizedName });

            entity.HasOne(e => e.Country)
                .WithMany(c => c.Cities)
                .HasForeignKey(e => e.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Region)
                .WithMany(r => r.Cities)
                .HasForeignKey(e => e.RegionId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Landmark>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.CityId)
                .IsRequired();

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.NormalizedName)
                .HasMaxLength(200);

            entity.Property(e => e.Description)
                .HasMaxLength(5000);

            entity.Property(e => e.Address)
                .HasMaxLength(300);

            entity.Property(e => e.Latitude)
                .HasPrecision(9, 6);

            entity.Property(e => e.Longitude)
                .HasPrecision(9, 6);

            entity.Property(e => e.ExternalRegistryUrl)
                .HasMaxLength(500);

            entity.Property(e => e.UploadedImagePath)
                .HasMaxLength(500);

            entity.Property(e => e.ImageUrl)
                .HasMaxLength(500);

            entity.Property(e => e.ProtectionStatus)
                .IsRequired();

            entity.Property(e => e.PhysicalCondition)
                .IsRequired();

            entity.Property(e => e.AccessibilityStatus)
                .IsRequired();

            entity.HasOne(e => e.City)
                .WithMany(c => c.Landmarks)
                .HasForeignKey(e => e.CityId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}