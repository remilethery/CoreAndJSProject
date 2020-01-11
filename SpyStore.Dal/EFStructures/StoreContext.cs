using System;
using Microsoft.EntityFrameworkCore;
using SpyStore.Models.Entities;
using SpyStore.Models.Entities.Base;

public class StoreContext: DbContext
{

    public StoreContext(DbContextOptions<StoreContext>options): base(options) 
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }
}