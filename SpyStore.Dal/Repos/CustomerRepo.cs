
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SpyStore.Dal.EFStructures;
using SpyStore.Dal.Repos.Base;
using SpyStore.Dal.Repos.Interfaces;
using SpyStore.Models.Entities;


namespace SpyStore.Dal.Repos
{

    public class CustomerRepo : RepoBase<Customer>, ICustomerRepo
    {

        public CustomerRepo(StoreContext context) : base(context)
        {
        }

        public CustomerRepo(DbContextOptions<StoreContext> options) : base(options)
        {
        }

        public override IEnumerable<Customer> GetAll()
            => base.GetAll(x => x.FullName);

    }
}