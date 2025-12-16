using HV.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace HV.DAL;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Country> Countries { get; set; }
    public DbSet<Region> Regions { get; set; }
    public DbSet<City> Cities { get; set; }

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
        });
    }
}