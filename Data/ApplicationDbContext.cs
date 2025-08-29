using MVCBarApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace MVCBarApplication.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cocktail> Cocktails { get; set; }
        public DbSet<Order> Orders { get; set; }

        // This method seeds the database with initial data (our cocktail menu)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cocktail>().HasData(
                new Cocktail { Id = 1, Name = "Margarita", Description = "Tequila, lime juice, and triple sec.", Price = 12.50m },
                new Cocktail { Id = 2, Name = "Mojito", Description = "White rum, sugar, lime juice, soda water, and mint.", Price = 11.00m },
                new Cocktail { Id = 3, Name = "Old Fashioned", Description = "Whiskey, sugar, bitters, and a twist of citrus rind.", Price = 14.00m },
                new Cocktail { Id = 4, Name = "Espresso Martini", Description = "Vodka, espresso coffee, and coffee liqueur.", Price = 13.50m }
            );
        }
    }
}