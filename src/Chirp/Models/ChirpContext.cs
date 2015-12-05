using System;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;

namespace Chirp.Models
{
    public class ChirpContext : IdentityDbContext<ChirpUser>
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
