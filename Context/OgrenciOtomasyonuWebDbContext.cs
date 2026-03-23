using Microsoft.EntityFrameworkCore;
using OgrenciOtomasyonWeb.Models;

namespace OgrenciOtomasyonWeb.Context
{
    public class OgrenciOtomasyonuWebDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }

        public DbSet<Dersler> Dersler { get; set; }
        public DbSet<Ogrenci> Ogrenci { get; set; }
        public DbSet<OgrenciDers> OgrenciDers { get; set; }
        public DbSet<OgretimUyesi> OgretimUyesi { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dersler>().HasKey(d => d.DersId);

            modelBuilder.Entity<OgrenciDers>()
            .ToTable(tb => tb.HasTrigger("HarfNotuHesaplaTrigger"));
        }
    }
}
