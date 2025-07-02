using LMSWebAppClean.Domain.Base;
using LMSWebAppClean.Domain.Model;

namespace LMSWebAppClean.Application.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        /*IRepository<Book> Books { get; }
        IRepository<BaseUser> Users { get; }*/
        
        Task<int> SaveChangesAsync();
        int SaveChanges();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}