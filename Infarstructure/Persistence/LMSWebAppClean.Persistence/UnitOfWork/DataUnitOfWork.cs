using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Persistence.Context;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;

namespace LMSWebAppClean.Persistence.UnitOfWork
{
    public class DataUnitOfWork : IUnitOfWork
    {
        private readonly DataDBContext context;
        private IDbContextTransaction transaction;

        public DataUnitOfWork(DataDBContext context)
        {
            this.context = context;
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
            await transaction.CommitAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await transaction.RollbackAsync();
        }

        public void Dispose()
        {
            context.Dispose();
            transaction?.Dispose();
        }
    }
}