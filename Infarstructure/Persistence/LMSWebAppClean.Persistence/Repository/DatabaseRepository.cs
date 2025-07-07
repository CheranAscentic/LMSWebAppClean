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

        // New method to load entity with related data
        public T GetWithIncludes(int id, params string[] includes)
        {
            IQueryable<T> query = dbSet;
            
            // Add includes for each specified navigation property
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            
            return query.FirstOrDefault(e => e.Id == id);
        }

        public List<T> GetAll()
        {
            return dbSet.ToList();
        }

        // New method to load all entities with related data
        public List<T> GetAllWithIncludes(params string[] includes)
        {
            IQueryable<T> query = dbSet;
            
            // Add includes for each specified navigation property
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            
            return query.ToList();
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