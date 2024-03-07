using Mango.Services.ProductAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ProductAPI.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasData(new Product()
            {
                ProductId = 1,
                Name = "Fish",
                Price = 15,
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                ImageUrl = "https://dotnetmasteryskye.blob.core.windows.net/mango/1.jpg",
                CategoryName = "Appetizer"
            });

            modelBuilder.Entity<Product>().HasData(new Product()
            {
                ProductId = 2,
                Name = "Meet",
                Price = 13.99,
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                ImageUrl = "https://dotnetmasteryskye.blob.core.windows.net/mango/2.jpg",
                CategoryName = "Appetizer"
            });

            modelBuilder.Entity<Product>().HasData(new Product()
            {
                ProductId = 3,
                Name = "Khachapuri in Adjara",
                Price = 10.99,
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                ImageUrl = "https://dotnetmasteryskye.blob.core.windows.net/mango/3.jpg",
                CategoryName = "Appetizer"
            });

            modelBuilder.Entity<Product>().HasData(new Product()
            {
                ProductId = 4,
                Name = "Khinkali",
                Price = 15,
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                ImageUrl = "https://dotnetmasteryskye.blob.core.windows.net/mango/4.jpg",
                CategoryName = "Entree"
            });
        }
    }
}
