using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Identity.Context;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;

namespace LMSWebAppClean.Identity.UnitOfWork
{
    public class IdentityUnitOfWork : IUnitOfWork
    {
        private readonly IdentityDBContext context;
        private IDbContextTransaction? transaction;

        public IdentityUnitOfWork(IdentityDBContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
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