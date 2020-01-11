using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;

// Class to hold StoreContext method builder within a Database
public class StoreContextFactory : IDesignTimeDbContextFactory<StoreContext>
{

    public StoreContext CreateDbContext(string[] args) {
        // Get optionsBuilder from a new DbContext with options
        var optionsBuilder = new DbContextOptionsBuilder<StoreContext>();
        // Connection String, here, we use a docker based SQL Express Server
        var connectionString = @"Server=.,5433;Database=SpyStore21;User ID=sa;Password=P@ssw0rd;MultipleActiveResultSets=true;";
        // Sets options Builder with the Connection String
        optionsBuilder.UseSqlServer(connectionString, options => options.EnableRetryOnFailure());
        // Sets the Warnings to throw exception when querying goes wrong
        optionsBuilder.ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
        // Displays connectionString to console for debugging and maintenance
        Console.WriteLine(connectionString);
        // returns the awaited StoreContext with the set options
        return new StoreContext(optionsBuilder.Options);
    }

}