using EasyWallet.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace EasyWallet.Data
{
    public class EasyWalletContext : DbContext
    {
        public DbSet<UserData> Users { get; set; }
        public DbSet<CategoryData> Categories { get; set; }
        public DbSet<CategoryTypeData> CategoryTypes { get; set; }
        public DbSet<TagData> Tags { get; set; }
        public DbSet<EntryData> Entries { get; set; }

        public EasyWalletContext(DbContextOptions<EasyWalletContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            /*
            modelBuilder.Entity<Entry>(eb =>
            {
                eb.Property(e => e.Amount).HasColumnType("decimal(12, 2)");
            });
            */
        }
    }
}
