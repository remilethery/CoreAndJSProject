using System;
using Microsoft.EntityFrameworkCore;


public class StoreContext: DbContext
{

    public StoreContext(DbContextOptions<StoreContext>options): base(options) 
    {

    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Customer> Customers { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasIndex(e =>
           e.EmailAddress).HasName("IX_Customers").IsUnique();
        });
    }
}