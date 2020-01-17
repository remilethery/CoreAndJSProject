using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SpyStore.Dal.EFStructures;
using SpyStore.Models.Entities;
using SpyStore.Dal.Exceptions;

namespace SpyStore.Dal.Initialization
{

    public class SampleDataInitializer
    {
        public static void DropAndCreateDatabase(StoreContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.Migrate();
        }

        internal static void ResetIdentity(StoreContext context)
        {
            var tables = new[] {"Categories", "Customers", "OrderDetails", "Orders", "Products", "ShoppingCartRecords"};

            foreach (var item in tables)
            {
                var rawSqlString = $"DBCC CHECKIDENT (\"Store.{item}\", RESEED, 0);";
                #pragma warning disable EF1000 // Possible SQL injection vulnerability
                context.Database.ExecuteSqlCommand(rawSqlString);
                #pragma warning restore EF1000 //  Possible SQL injection vulnerability
            }
        }

        public static void ClearData(StoreContext context)
        {
            context.Database.ExecuteSqlCommand("Delete from Store.Categories");
            context.Database.ExecuteSqlCommand("Delete from Store.Customers");
            ResetIdentity(context);
        }

        internal static void SeedData(StoreContext context)
        {
            try
            {
                if (!context.Categories.Any())
                {
                    context.Categories.AddRange(SampleData.GetCategories());
                    context.SaveChanges();
                }
                if (!context.Customers.Any())
                {
                    var prod1 = context
                            .Categories
                            .Include(c => c.Products)
                            .FirstOrDefault()?
                            .Products.Skip(3).FirstOrDefault();
                    var prod2 = context
                            .Categories.Skip(2)
                            .Include(c => c.Products)
                            .FirstOrDefault()?
                            .Products.Skip(2).FirstOrDefault();
                    var prod3 = context
                            .Categories.Skip(5)
                            .Include(c => c.Products)
                            .FirstOrDefault()?
                            .Products.Skip(1).FirstOrDefault();
                    var prod4 = context
                            .Categories.Skip(2)
                            .Include(c => c.Products)
                            .FirstOrDefault()?
                            .Products.Skip(1).FirstOrDefault();

                    context.Customers.AddRange(SampleData.GetAllCustomerRecords(
                        new List<Product> {prod1, prod2, prod3, prod4}
                    ));
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        public static void InializeData(StoreContext context)
        {
            //Ensure database exists and is up to date
            context.Database.Migrate();
            ClearData(context);
            SeedData(context);
        }

    }
}