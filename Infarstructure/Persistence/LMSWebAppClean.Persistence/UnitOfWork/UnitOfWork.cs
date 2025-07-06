using LMSWebAppClean.Persistence.Context;
using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Persistence.Repository;
using LMSWebAppClean.Domain.Base;
using LMSWebAppClean.Domain.Model;
using Microsoft.EntityFrameworkCore.Storage;

namespace LMSWebAppClean.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataDBContext context;
        private IDbContextTransaction? transaction;

        /*public IRepository<Book> Books { get; }
        public IRepository<BaseUser> Users { get; }*/

        public UnitOfWork(DataDBContext context/*, IRepository<Book> books, IRepository<BaseUser> users*/)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            /*this.Books = books ?? throw new ArgumentNullException(nameof(books));
            this.Users = users ?? throw new ArgumentNullException(nameof(users));*/
        }

        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }

        public int SaveChanges()
        {
            return context.SaveChanges();
        }

        public async Task BeginTransactionAsync()
        {
            transaction = await context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (transaction != null)
            {
                await transaction.CommitAsync();
                await transaction.DisposeAsync();
                transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (transaction != null)
            {
                await transaction.RollbackAsync();
                await transaction.DisposeAsync();
                transaction = null;
            }
        }

        public void Dispose()
        {
            transaction?.Dispose();
            context.Dispose();
        }
    }
}