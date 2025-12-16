using HV.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace HV.DAL;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Country> Countries { get; set; }

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
    }
}