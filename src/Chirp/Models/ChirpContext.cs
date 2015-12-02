using System;
using Microsoft.Data.Entity;

namespace Chirp.Models
{
    public class ChirpContext : DbContext
    {
        public DbSet <ChirpMessage> ChirpMessages { get; set; }

        public ChirpContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connString = Startup.Configuration["Data:ChirpContextConnection"];
            optionsBuilder.UseNpgsql(connString);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
