using System;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Chirp.Models;

namespace Chirp.Database
{
    public class ChirpContext : IdentityDbContext<ChirpUser>
    {
        public DbSet <ChirpPost> ChirpPosts { get; set; }

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
