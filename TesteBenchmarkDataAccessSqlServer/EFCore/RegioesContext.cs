using Microsoft.EntityFrameworkCore;

namespace TesteBenchmarkDataAccessSqlServer.EFCore
{
    public class RegioesContext : DbContext
    {
        public DbSet<Regiao> Regioes { get; set; }
        public DbSet<Estado> Estados { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Configurations.BaseEFCore);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Regiao>().HasMany(r => r.Estados);
            modelBuilder.Entity<Estado>().HasOne(e => e.Regiao);
        }
    }
}