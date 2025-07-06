using LMSWebAppClean.Domain.Base;
using LMSWebAppClean.Domain.Interface;
using Microsoft.EntityFrameworkCore;
using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Persistence.Context;

namespace LMSWebAppClean.Persistence.Repository
{
    public class DropInDatabaseRepository<T> : IRepository<T> where T : class, IEntity // Fix: Added 'class' constraint to ensure T is a reference type
    {
        private readonly DataDBContext _context;
        private readonly DbSet<T> _dbSet;

        public DropInDatabaseRepository(DataDBContext dbContext)
        {
            _context = dbContext;
            _dbSet = _context.Set<T>();
        }

        // Adds new entity and returns the same entity after persistence
        public T Add(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        // Retrieves a single entity by id
        public T Get(int id)
        {
            return _dbSet.Find(id);
        }

        // Retrieves all entities
        public List<T> GetAll()
        {
            return _dbSet.ToList();
        }

        // Removes an entity by id and returns the removed entity
        public T Remove(int id)
        {
            var found = _dbSet.Find(id);
            if (found == null)
                return null; // or throw an exception if preferred

            _dbSet.Remove(found);
            _context.SaveChanges();
            return found;
        }

        // Updates an existing entity and returns the updated entity
        public T Update(T entity)
        {
            _dbSet.Update(entity);
            _context.SaveChanges();
            return entity;
        }
    }
}