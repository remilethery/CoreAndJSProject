using System;
using Microsoft.EntityFrameworkCore;


public class StoreContext : DbContext
{

    public int CustomerId;

    public StoreContext(DbContextOptions<StoreContext> options) : base(options)
    {

    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Makes so every Email Address in Customer Table is unique
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasIndex(e =>
                e.EmailAddress).HasName("IX_Customers").IsUnique();
        });

        // Makes so OrderDate and ShipDate are set by system
        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e =>
                e.OrderDate).HasColumnType("datetime").HasDefaultValueSql("getdate()");
            entity.Property(e =>
                e.ShipDate).HasColumnType("datetime").HasDefaultValueSql("getdate()");
        });

        // Makes so every query is built upon customer ID
        // No need to look for it every time
        modelBuilder.Entity<Order>()
            .HasQueryFilter(x => x.CustomerId == CustomerId);

        // Makes so UnitCost is of money column type
        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.Property(e => e.UnitCost).HasColumnType("money");
        });
    }
}