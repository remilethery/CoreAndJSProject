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


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasIndex(e =>
                e.EmailAddress).HasName("IX_Customers").IsUnique();
        });
        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e =>
                e.OrderDate).HasColumnType("datetime").HasDefaultValueSql("getdate()");
            entity.Property(e =>
                e.ShipDate).HasColumnType("datetime").HasDefaultValueSql("getdate()");
        });
        modelBuilder.Entity<Order>()
            .HasQueryFilter(x => x.CustomerId == CustomerId);
    }
}