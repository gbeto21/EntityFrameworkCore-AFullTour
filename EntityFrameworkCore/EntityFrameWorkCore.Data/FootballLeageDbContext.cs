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
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=FootballLeage_EfCore; Encrypt=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Team>().HasData(
                new Team
                {
                    Id = 1,
                    Name = "Tivoli Gardens FC",
                    CreatedDate = DateTimeOffset.UtcNow.DateTime
                },
                new Team
                {
                    Id = 2,
                    Name = "Waterhouse FC",
                    CreatedDate = DateTimeOffset.UtcNow.DateTime
                },
                new Team
                {
                    Id = 3,
                    Name = "Humble Lions FC",
                    CreatedDate = DateTimeOffset.UtcNow.DateTime
                }
            );

        }

    }
}
