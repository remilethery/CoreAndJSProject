using System;
using System.Collections.Generic;
using System.Text;
using SpyStore.Dal.EFStructures;
using SpyStore.Dal.Initialization;

namespace SpyStore.Service.Tests.TestClasses.Base
{
    public abstract class BaseTestClass: IDisposable
    {
        protected string ServiceAddress = "http://localhost:7745/";
        protected string RootAddress = string.Empty;

        public virtual void Dispose()
        {
        }

        protected void ResetTheDatabase()
        {
            SampleDataInitializer.InializeData(new StoreContextFactory().CreateDbContext(null));
        }

    }
}
