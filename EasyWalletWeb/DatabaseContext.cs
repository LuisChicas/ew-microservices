using EasyWalletWeb.Models;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyWalletWeb
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryType> CategoryTypes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Entry> Entries { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Entry>(eb =>
            {
                eb.Property(e => e.Amount).HasColumnType("decimal(12, 2)");
            });
        }
    }
}
