using System;
using Microsoft.EntityFrameworkCore;
using WebApiTest.Models;

namespace WebApiTest
{
    public sealed class WebApiDbContext : DbContext
    {
        public WebApiDbContext(DbContextOptions<WebApiDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<ProvidedProduct> ProvidedProducts { get; set; } = null!;
        public DbSet<SalesPoint> SalesPoints { get; set; } = null!;
        public DbSet<Buyer> Buyers { get; set; } = null!;
        public DbSet<SaleData> SalesData { get; set; } = null!;
        public DbSet<Sale> Sales { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Products Seed

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Nuka-Cola",
                    Price = 20.5
                },
                new Product
                {
                    Id = 2,
                    Name = "Brahmin steak",
                    Price = 5.75
                },
                new Product
                {
                    Id = 3,
                    Name = "Mutfruit",
                    Price = 3.32
                }
            );

            #endregion

            #region ProvidedProducts Seed

            modelBuilder.Entity<ProvidedProduct>().HasData(
                new ProvidedProduct
                {
                    Id = 1,
                    ProductId = 1,
                    SalesPointId = 1,
                    ProductQuantity = 5
                },
                new ProvidedProduct
                {
                    Id = 2,
                    ProductId = 2,
                    SalesPointId = 1,
                    ProductQuantity = 10
                }
                ,
                new ProvidedProduct
                {
                    Id = 3,
                    ProductId = 3,
                    SalesPointId = 1,
                    ProductQuantity = 8
                }
                ,
                new ProvidedProduct
                {
                    Id = 4,
                    ProductId = 1,
                    SalesPointId = 2,
                    ProductQuantity = 15
                }
                ,
                new ProvidedProduct
                {
                    Id = 5,
                    ProductId = 2,
                    SalesPointId = 2,
                    ProductQuantity = 2
                }
                ,
                new ProvidedProduct
                {
                    Id = 6,
                    ProductId = 3,
                    SalesPointId = 2,
                    ProductQuantity = 9
                });

            #endregion

            #region SalesPoints Seed

            modelBuilder.Entity<SalesPoint>().HasData(
                new SalesPoint
                {
                    Id = 1,
                    Name = "Crimson Caravan"
                },
                new SalesPoint
                {
                    Id = 2,
                    Name = "Cassidy Caravans"
                }
            );
            #endregion

            #region Buyers Seed

            modelBuilder.Entity<Buyer>().HasData(
                new Buyer
                {
                    Id = 1,
                    Name = "Courier"
                },
                new Buyer
                {
                    Id = 2,
                    Name = "Mr. H"
                });

            #endregion
            
            #region SalesData Seed

            modelBuilder.Entity<SaleData>().HasData(
                new SaleData
                {
                    Id = 1,
                    SaleId = 1,
                    ProductId = 1,
                    ProductQuantity = 3,
                    ProductIdAmount = 61.5
                },
                new SaleData
                {
                    Id = 2,
                    SaleId = 1,
                    ProductId = 2,
                    ProductQuantity = 2,
                    ProductIdAmount = 11.5
                });
            #endregion
            
            #region Sales Seed

            modelBuilder.Entity<Sale>().HasData(
                new Sale
                {
                    Id = 1,
                    SalesPointId = 1,
                    BuyerId = 1,
                    SaleDateTime = DateTime.Now - TimeSpan.FromDays(1)
                });
            #endregion
        }
    }
}