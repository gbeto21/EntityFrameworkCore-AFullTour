using EntityFrameWorkCore.Domain;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameWorkCore.Data
{
    public class FootballLeageDbContext : DbContext
    {
        public DbSet<Team> Teams { get; set; }

        public DbSet<Coach> Coaches { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

    }
}
