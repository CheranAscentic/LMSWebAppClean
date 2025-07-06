using LMSWebAppClean.Domain.Base;
using LMSWebAppClean.Domain.Model;

namespace LMSWebAppClean.Application.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync();

        int SaveChanges();

        Task BeginTransactionAsync();

        Task CommitTransactionAsync();

        Task RollbackTransactionAsync();
    }
}