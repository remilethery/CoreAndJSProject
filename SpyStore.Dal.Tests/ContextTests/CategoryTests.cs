using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SpyStore.Dal.EFStructures;
using SpyStore.Dal.Initialization;
using SpyStore.Models.Entities;
using Xunit;

namespace SpyStore.Dal.Tests
{

    [Collection("SpyStore.Dal")]
    public class CategoryTests : IDisposable
    {
        private readonly StoreContext _db;
        public CategoryTests()
        {
            _db = new StoreContextFactory()
                    .CreateDbContext(new string[0]);
                                CleanDatabase();
        }
        public void Dispose()
        {
            CleanDatabase();
            _db.Dispose();
        }

        private void CleanDatabase()
        {
            SampleDataInitializer.ClearData(_db);
        }

        [Fact]
        public void FirstTest()
        {
            Assert.True(true);
        }

        [Fact]
        public void ShouldAddACategoryWithDbSet()
        {
            var category = new Category { CategoryName = "Foo" };
            _db.Categories.Add(category);

            Assert.Equal(EntityState.Added, _db.Entry(category).State);
            Assert.True(category.Id < 0);
            Assert.Null(category.TimeStamp);
            _db.SaveChanges();
            Assert.Equal(EntityState.Unchanged, _db.Entry(category).State);
            Assert.NotNull(category.TimeStamp);
            Assert.Equal(1, _db.Categories.Count());
        }

        [Fact]
        public void ShouldAddACategoryWithContext()
        {
            var category = new Category { CategoryName = "Foo" };
            _db.Add(category);
 
            Assert.Equal(EntityState.Added, _db.Entry(category).State);
            Assert.True(category.Id < 0);
            Assert.Null(category.TimeStamp);
            _db.SaveChanges();
            Assert.Equal(EntityState.Unchanged, _db.Entry(category).State);
            Assert.NotNull(category.TimeStamp);
            Assert.Equal(1, _db.Categories.Count());

        }

        [Fact]
        public void ShouldGetAllCategoriesOrderedByName()
        {
            _db.Categories.Add(new Category { CategoryName = "Foo" });
            _db.Categories.Add(new Category { CategoryName = "Bar" });
            _db.SaveChanges();

            var categories = _db.Categories.OrderBy(c => c.CategoryName).ToList();
            Assert.Equal(2, _db.Categories.Count());
            Assert.Equal("Bar", categories[0].CategoryName);
            Assert.Equal("Foo", categories[1].CategoryName);

        }

        [Fact]
        public void ShouldUpdateACategory()
        {
            var category = new Category { CategoryName = "Foo" };
            _db.Categories.Add(category);
            _db.SaveChanges();
            category.CategoryName = "Bar";
            _db.Categories.Update(category);
            Assert.Equal(EntityState.Modified, _db.Entry(category).State);
            _db.SaveChanges();
            Assert.Equal(EntityState.Unchanged, _db.Entry(category).State);
            StoreContext context;
            using (context = new StoreContextFactory().CreateDbContext( null))
            {
                Assert.Equal("Bar", context.Categories.First().CategoryName);
            }


        }

    }

}