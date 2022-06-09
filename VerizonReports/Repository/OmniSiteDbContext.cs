using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using VerizonReports.Repository.Entities;
using VerizonReports.Repository.Models;

namespace VerizonReports.Repository;

public class OmniSiteDbContext : DbContext
{
    public DbSet<OmniSiteData> OmniSiteDataPoints { get; set; }

    public OmniSiteDbContext(DbContextOptions<OmniSiteDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(new ConfigurationBuilder().AddJsonFile("C:..\\..\\..\\appsettings.json").Build().GetSection("connectionStrings")["omniSiteConnectionString"]);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UnitDb>()
            .HasOne(x => x.Station)
            .WithOne(x => x.Unit)
            .HasForeignKey<StationDb>(x => x.UnitId);

        modelBuilder.Entity<AssemblyDb>()
            .HasOne(x => x.Unit)
            .WithOne(x => x.Assembly)
            .HasForeignKey<UnitDb>(x => x.AssemblyId);

        modelBuilder.Entity<RadioDb>()
            .HasOne(x => x.Assembly)
            .WithOne(x => x.Radio)
            .HasForeignKey<AssemblyDb>(x => x.RadioId);

        modelBuilder.Entity<RadioModelDb>()
            .HasMany(x => x.Radios)
            .WithOne(x => x.RadioModel)
            .HasForeignKey(x => x.RadioModelId);

        modelBuilder.Entity<RadioProtocolDb>()
            .HasMany(x => x.RadioModels)
            .WithOne(x => x.RadioProtocol)
            .HasForeignKey(x => x.RadioProtocolId);
            
        modelBuilder.Entity<SimDb>()
            .HasOne(x => x.Assembly)
            .WithOne(x => x.Sim)
            .HasForeignKey<AssemblyDb>(x => x.SimId);

        modelBuilder.Entity<ServiceProviderDb>()
            .HasMany(x => x.Sims)
            .WithOne(x => x.ServiceProvider)
            .HasForeignKey(x => x.ServiceId);
    }

    public DbSet<UnitDb> Unit { get; set; }
    public DbSet<StationDb> Station { get; set; }
    public DbSet<AssemblyDb> Assembly { get; set; }
    public DbSet<RadioDb> Radio { get; set; }
    public DbSet<RadioModelDb> RadioModel { get; set; }
    public DbSet<RadioProtocolDb> RadioProtocol { get; set; }
    public DbSet<SimDb> Sim { get; set; }
    public DbSet<ServiceProviderDb> ServiceProvider { get; set; }
}