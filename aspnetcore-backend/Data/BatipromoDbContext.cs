using Microsoft.EntityFrameworkCore;
using Batipromo.Api.Models;

namespace Batipromo.Api.Data;

public class BatipromoDbContext : DbContext
{
    public BatipromoDbContext(DbContextOptions<BatipromoDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Chantier> Chantiers => Set<Chantier>();
    public DbSet<Caisse> Caisses => Set<Caisse>();
    public DbSet<CaisseMouvement> CaisseMouvements => Set<CaisseMouvement>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Materiel> Materiels => Set<Materiel>();
    public DbSet<StockArticle> StockArticles => Set<StockArticle>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure indexes and relationships
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Chantier>()
            .HasIndex(c => c.Code)
            .IsUnique();

        modelBuilder.Entity<Caisse>()
            .HasIndex(c => c.Code)
            .IsUnique();

        modelBuilder.Entity<CaisseMouvement>()
            .HasOne(m => m.Caisse)
            .WithMany(c => c.Mouvements)
            .HasForeignKey(m => m.CaisseId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Employee>()
            .HasIndex(e => e.Matricule)
            .IsUnique();

        modelBuilder.Entity<Materiel>()
            .HasIndex(m => m.Code)
            .IsUnique();

        modelBuilder.Entity<StockArticle>()
            .HasIndex(s => s.Code)
            .IsUnique();
    }
}
