using LMSWebAppClean.Persistence.Context;
using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Domain.Interface;
using Microsoft.EntityFrameworkCore;

namespace LMSWebAppClean.Persistence.Repository
{
    public class DatabaseRepository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly DataDBContext context;
        private readonly DbSet<T> dbSet;

        public DatabaseRepository(DataDBContext dbContext)
        {
            context = dbContext;
            dbSet = context.Set<T>();
        }

        public T Add(T entity)
        {
            dbSet.Add(entity);
            return entity;
        }

        public T Get(int id)
        {
            return dbSet.Find(id);
        }

        public List<T> GetAll()
        {
            return dbSet.ToList();
        }

        public T Remove(int id)
        {
            var found = dbSet.Find(id);
            if (found == null)
                return null;

            dbSet.Remove(found);
            return found;
        }

        public T Update(T entity)
        {
            dbSet.Update(entity);
            return entity;
        }
    }
}