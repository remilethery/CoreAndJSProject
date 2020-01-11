using System;
using Microsoft.EntityFrameworkCore;


public class StoreContext: DbContext
{

    public StoreContext(DbContextOptions<StoreContext>options): base(options) 
    {

    }

    public DbSet<Category> Categories { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }
}