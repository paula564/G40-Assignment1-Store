using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using psH60A01.Models;

namespace psH60A01.Models;

public partial class H60AssignmentDbPsContext : DbContext
{
    public H60AssignmentDbPsContext()
    {
    }

    public H60AssignmentDbPsContext(DbContextOptions<H60AssignmentDbPsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tcp:csdevdb-heritagecs.database.windows.net,1433; Database=H60_AssignmentDB_ps;Authentication=Active Directory Interactive; Encrypt=True; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.HasIndex(e => e.ProdCatId, "IX_Product_ProdCatId");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.BuyPrice).HasColumnType("numeric(8, 2)");
            entity.Property(e => e.Description)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.Manufacturer)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.SellPrice).HasColumnType("numeric(8, 2)");

            entity.HasOne(d => d.ProdCat).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProdCatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_ProductCategory");
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId);

            entity.ToTable("ProductCategory");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.ProdCat)
                .HasMaxLength(60)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ShoppingCart>()
            .HasKey(c => c.CartId);

        modelBuilder.Entity<CartItem>()
        .HasOne(c => c.ShoppingCart)
        .WithMany()
        .HasForeignKey(c => c.CartId);

        modelBuilder.Entity<ProductCategory>().HasData(
            new ProductCategory { CategoryId = 1, ProdCat = "Crochet hooks" },
            new ProductCategory { CategoryId = 2, ProdCat = "Knitting needles" },
            new ProductCategory { CategoryId = 3, ProdCat = "Yarn" },
            new ProductCategory { CategoryId = 4, ProdCat = "Patterns" },
            new ProductCategory { CategoryId = 5, ProdCat = "Felting tools" }

            );

        modelBuilder.Entity<Product>().HasData(
            new Product { ProductId = 1, ProdCatId = 1, Description = "Wooden hook", Manufacturer = "Red Heart", Stock = 25, BuyPrice = 4.50m, SellPrice = 5.99m },
            new Product { ProductId =2, ProdCatId = 1, Description = "Plastic hook", Manufacturer = "Red Heart", Stock = 40, BuyPrice = 2.50m, SellPrice = 3.99m },
            new Product { ProductId =3, ProdCatId = 1, Description = "Metal hook", Manufacturer = "Lion Brand", Stock = 36, BuyPrice = 3.60m, SellPrice = 4.99m },
            new Product { ProductId = 4, ProdCatId = 1, Description = "Ergonomic hook", Manufacturer = "Clover Armour", Stock = 10, BuyPrice = 6.50m, SellPrice = 10.99m },

            new Product { ProductId =5, ProdCatId = 2, Description = "Wooden needles", Manufacturer = "Red Heart", Stock = 25, BuyPrice = 4.50m, SellPrice = 5.99m },
            new Product { ProductId = 6, ProdCatId = 2, Description = "Plastic needles", Manufacturer = "Red Heart", Stock = 25, BuyPrice = 4.50m, SellPrice = 5.99m },
            new Product { ProductId = 7, ProdCatId = 2, Description = "Metal needles", Manufacturer = "Red Heart", Stock = 25, BuyPrice = 4.50m, SellPrice = 5.99m },
            new Product { ProductId = 8, ProdCatId = 2, Description = "Circular needles", Manufacturer = "Red Heart", Stock = 25, BuyPrice = 4.50m, SellPrice = 5.99m },

            new Product { ProductId = 9, ProdCatId = 3, Description = "Merino wool", Manufacturer = "Red Heart", Stock = 25, BuyPrice = 4.50m, SellPrice = 5.99m },
            new Product { ProductId = 10, ProdCatId = 3, Description = "Cotton yarn", Manufacturer = "Red Heart", Stock = 25, BuyPrice = 4.50m, SellPrice = 5.99m },
            new Product { ProductId = 11, ProdCatId = 3, Description = "Alpacca wool", Manufacturer = "Red Heart", Stock = 25, BuyPrice = 4.50m, SellPrice = 5.99m },
            new Product { ProductId = 12, ProdCatId = 3, Description = "Silk yarn", Manufacturer = "Red Heart", Stock = 25, BuyPrice = 4.50m, SellPrice = 5.99m },

            new Product { ProductId = 13, ProdCatId = 4, Description = "Knit socks", Manufacturer = "Red Heart", Stock = 25, BuyPrice = 4.50m, SellPrice = 5.99m },
            new Product { ProductId = 14, ProdCatId = 4, Description = "Cat amigurumi", Manufacturer = "Red Heart", Stock = 25, BuyPrice = 4.50m, SellPrice = 5.99m },
            new Product { ProductId = 15, ProdCatId = 4, Description = "Crochet blanket", Manufacturer = "Red Heart", Stock = 25, BuyPrice = 4.50m, SellPrice = 5.99m },
            new Product { ProductId = 16, ProdCatId = 4, Description = "Cozy sweather", Manufacturer = "Red Heart", Stock = 25, BuyPrice = 4.50m, SellPrice = 5.99m },

            new Product { ProductId = 17, ProdCatId = 5, Description = "Felting needle", Manufacturer = "Red Heart", Stock = 25, BuyPrice = 4.50m, SellPrice = 5.99m },
            new Product { ProductId = 18, ProdCatId = 5, Description = "Felting mat", Manufacturer = "Red Heart", Stock = 25, BuyPrice = 4.50m, SellPrice = 5.99m },
            new Product { ProductId = 19, ProdCatId = 5, Description = "Needle handle", Manufacturer = "Red Heart", Stock = 25, BuyPrice = 4.50m, SellPrice = 5.99m },
            new Product { ProductId = 20, ProdCatId = 5, Description = "Finger guards", Manufacturer = "Red Heart", Stock = 25, BuyPrice = 4.50m, SellPrice = 5.99m }
            );

        modelBuilder.Entity<Customer>().HasData(
            new Customer {CustomerId = 1, FirstName = "Paula", LastName = "Selskiy", Email = "paula@example.com", PhoneNumber = "8195551234",Province = "QC" ,CreditCard = "1234567890123456"},
            new Customer {CustomerId = 2, FirstName = "Stella", LastName = "Hatton", Email = "stella@example.com", PhoneNumber = "8195555678", Province = "ON", CreditCard = "2345678901234567"},
            new Customer {CustomerId = 3,FirstName = "Chai", LastName = "Hatton", Email = "chaicat@example.com", PhoneNumber = "8195559012",Province = "QC", CreditCard = "3456789012345678"}
        );

        modelBuilder.Entity<ShoppingCart>().HasData(
            new ShoppingCart {CartId = 1, CustomerId = 2, DateCreated = new DateTime(2026, 9, 20)}
        );

        modelBuilder.Entity<CartItem>().HasData(
            new CartItem { CartItemId = 1, CartId = 1, ProductId = 4, Quantity = 2,Price = 10.99m},
            new CartItem { CartItemId = 2, CartId = 1, ProductId = 5, Quantity = 1, Price = 5.99m}
        );

        modelBuilder.Entity<Order>().HasData(
            new Order {OrderId = 1,CustomerId = 1,DateCreated = new DateTime(2026, 9, 18),DateFulfilled = new DateTime(2026, 9, 19), Total = 15.97m,Taxes = 1.60m}
        );

        modelBuilder.Entity<OrderItem>().HasData(
            new OrderItem { OrderItemId = 1, OrderId = 1, ProductId = 1,Quantity = 1, Price = 5.99m},
            new OrderItem {OrderItemId = 2, OrderId = 1, ProductId = 2, Quantity = 1, Price = 3.99m},
            new OrderItem { OrderItemId = 3, OrderId = 1, ProductId = 3, Quantity = 1,Price = 4.99m}
        );
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
