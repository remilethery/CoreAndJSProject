using System;
using Microsoft.EntityFrameworkCore;
using SpyStore.Models.Entities;
using SpyStore.Models.Entities.Base;
using SpyStore.Models.ViewModels;

namespace SpyStore.Dal.EFStructures
{
    public class StoreContext : DbContext
    {

        public int CustomerId;

        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {

        }

        // Maps DB Based function to C# functions, allowing it to be used
        [DbFunction("GetOrderTotal", Schema = "Store")]
        public static int GetOrderTotal(int orderId)
        {
            //Code in here doesn't matter
            throw new Exception();
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ShoppingCartRecord> ShoppingCartRecords { get; set; }
        public DbQuery<CartRecordWithProductInfo> CartRecordWithProductInfos { get; set; }
        public DbQuery<OrderDetailWithProductInfo> OrderDetailWithProductInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Makes so every Email Address in Customer Table is unique
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasIndex(e =>
                    e.EmailAddress).HasName("IX_Customers").IsUnique();
            });

            // Makes so every query is built upon customer ID
            // No need to look for it every time
            modelBuilder.Entity<Order>()
                .HasQueryFilter(x => x.CustomerId == CustomerId);

            // Makes so OrderDate and ShipDate are set by system
            // Also sets OrderTotal to money and computed with Stored Function
            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e =>
                    e.OrderDate).HasColumnType("datetime").HasDefaultValueSql("getdate()");
                entity.Property(e =>
                    e.ShipDate).HasColumnType("datetime").HasDefaultValueSql("getdate()");
                entity.Property(e => e.OrderTotal)
                    .HasColumnType("money")
                    .HasComputedColumnSql("Store.GetOrderTotal([Id])");
            });

            // Makes so UnitCost is of money column type
            // Same for LineItemTotal + Configure it to computed with formula
            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.Property(e => e.UnitCost).HasColumnType("money");
                entity.Property(e => e.LineItemTotal)
                    .HasColumnType("money")
                    .HasComputedColumnSql("[Quantity]*[UnitCost]");
            });

            // Makes so both UnitCost and CurrentPrice are of money type
            // Also makes the Product owns a Product Detail
            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.UnitCost).HasColumnType("money");
                entity.Property(e => e.CurrentPrice).HasColumnType("money");
                entity.OwnsOne(o => o.Details,
                productDetails =>
                {
                    productDetails.Property(product =>
                       product.Description).HasColumnName(nameof(ProductDetails.Description));
                    productDetails.Property(product =>
                       product.ModelName).HasColumnName(nameof(ProductDetails.ModelName));
                    productDetails.Property(product =>
                       product.ModelNumber).HasColumnName(nameof(ProductDetails.ModelNumber));
                    productDetails.Property(product =>
                       product.ProductImage).HasColumnName(nameof(ProductDetails.ProductImage));
                    productDetails.Property(product =>
                       product.ProductImageLarge).HasColumnName(nameof(ProductDetails.ProductImageLarge));
                    productDetails.Property(product =>
                       product.ProductImageThumb).HasColumnName(nameof(ProductDetails.ProductImageThumb));
                });
            });

            // Makes so the Shopping Cart is filtered with the current customer
            modelBuilder.Entity<ShoppingCartRecord>()
                .HasQueryFilter(x => x.CustomerId == CustomerId);

            // Makes so the ShoppingCartRecord sets datetime to system by default
            // And quantity of one product to 1 by default
            // Also sets the index with the productId and customerID
            modelBuilder.Entity<ShoppingCartRecord>(entity =>
            {
                entity.HasIndex(
                    e => new { ShoppingCartRecordId = e.Id, e.ProductId, e.CustomerId })
                    .HasName("IX_ShoppingCart").IsUnique();
                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime").HasDefaultValueSql("getdate()");
                entity.Property(e => e.Quantity).HasDefaultValue(1);
            });

            modelBuilder.Query<CartRecordWithProductInfo>()
                .ToView("CartRecordWithProductInfo", "Store");

            modelBuilder.Query<OrderDetailWithProductInfo>()
                .ToView("OrderDetailWithProductInfo", "Store");

        }
    }
}