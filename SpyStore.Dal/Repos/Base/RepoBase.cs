using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SpyStore.Dal.EFStructures;
using SpyStore.Dal.Exceptions;
using SpyStore.Models.Entities.Base;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage;

namespace SpyStore.Dal.Repos.Base
{
    public abstract class RepoBase<T> : IRepo<T> where T : EntityBase, new()
    {

        public StoreContext Context { get; }

        protected RepoBase(StoreContext context)
        {
            Context = context;
        }

        private readonly bool _disposeContext;
        protected RepoBase(DbContextOptions<StoreContext> options) : this(new StoreContext(options))
        {
            _disposeContext = true;
        }

        public virtual void Dispose()
        {
            if (_disposeContext)
            {
                Context.Dispose();
            }
        }

        public DbSet<T> Table { get; }

        public int SaveChanges()
        {
            try
            {
                return Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new SpyStoreConcurrencyException("A concurrency error happened.", ex);
            }
            catch (RetryLimitExceededException ex)
            {
                throw new SpyStoreRetryLimitException("There is a problem with your connection", ex);
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlException)
                {
                    if (sqlException.Message.Contains(
                        "FOREIGN KEY constraint", StringComparison.OrdinalIgnoreCase
                    ))
                    {
                        if (sqlException.Message.Contains("table \"Store.Products\", column 'Id'", StringComparison.OrdinalIgnoreCase))
                        {
                            throw new SpyStoreInvalidProductException($"Invalid Product Id\r\n{ex.Message}", ex);
                        }
                        if (sqlException.Message.Contains("table \"Store.Customers\", column 'Id'", StringComparison.OrdinalIgnoreCase))
                        {
                            throw new SpyStoreInvalidCustomerException($"Invalid Customer Id\r\n{ex.Message}", ex);
                        }
                    }
                }
                throw new SpyStoreException("An error occured updating the database", ex);
            }
            catch (Exception ex)
            {
                throw new SpyStoreException("An error occured updating the database", ex);

            }

        }

        public virtual int Add(T entity, bool persist = true)
        {
            Table.Add(entity);
            return persist ? SaveChanges() : 0;
        }

        public virtual int AddRange(IEnumerable<T> entities, bool persist = true)
        {
            Table.AddRange(entities);
            return persist ? SaveChanges() : 0;
        }

        public virtual int Update(T entity, bool persist = true)
        {
            Table.Update(entity);
            return persist ? SaveChanges() : 0;
        }

        public virtual int UpdateRange(IEnumerable<T> entities, bool persist = true)
        {
            Table.AddRange(entities);
            return persist ? SaveChanges() : 0;
        }

        public virtual int Delete(T entity, bool persist = true)
        {
            Table.Remove(entity);
            return persist ? SaveChanges() : 0;
        }

        public virtual int DeleteRange(IEnumerable<T> entities, bool persist = true)
        {
            Table.RemoveRange(entities);
            return persist ? SaveChanges() : 0;
        }


        public T Find(int? id) => Table.Find(id);

        public T FindAsNoTracking(int id)
            => Table.Where(x => x.Id == id)
                .AsNoTracking().FirstOrDefault();

        public T FindIgnoreQueryFilters(int id)
            => Table.IgnoreQueryFilters().FirstOrDefault(x => x.Id == id);


        public virtual IEnumerable<T> GetAll() => Table;
        public virtual IEnumerable<T> GetAll(Expression<Func<T, object>> orderBy)
            => Table.OrderBy(orderBy);

        public IEnumerable<T> GetRange(IQueryable<T> query, int skip, int take)
            => query.Skip(skip).Take(take);

        //Helpers
        public bool HasChanges
            => Context.ChangeTracker.HasChanges();

        public (string Schema, string TableName) TableSchemaAndName
        {
            get
            {
                var metaData = Context.Model.FindEntityType(typeof(T).FullName).SqlServer();
                return (metaData.Schema, metaData.TableName);
            }


        }


    }

}