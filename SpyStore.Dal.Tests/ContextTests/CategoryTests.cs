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

    }

}