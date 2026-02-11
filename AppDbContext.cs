using Microsoft.EntityFrameworkCore;

namespace ZaposleniAPI; // OVO MORA DA BUDE ISTO SVUDA

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Zaposleni> Zaposleni { get; set; }
    public DbSet<Pozicija> Pozicije { get; set; }
    public DbSet<Projekat> Projekti { get; set; }
    public DbSet<RadioNa> RadioNa { get; set; }
    public DbSet<Plata> Plate { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Zaposleni>().HasKey(z => z.Jmbg);
    }
}
