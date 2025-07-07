using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSWebAppClean.Persistence.Repository
{
    public class InMemoryRepository<E> : IRepository<E> where E : class, IEntity
    {
        private readonly Dictionary<int, E> entities = new Dictionary<int, E>();
        private int nextId = 1;
        public E Add(E entity)
        {
            entity.Id = nextId++;
            entities.Add(entity.Id, entity);
            return entity;
        }

        public E Get(int id)
        {
            return entities[id];
        }

        public List<E> GetAll()
        {               
            return entities.Values.ToList();
        }

        public List<E> GetAllWithIncludes(params string[] includes)
        {
            throw new NotImplementedException();
        }

        public E GetWithIncludes(int id, params string[] includes)
        {
            throw new NotImplementedException();
        }

        public E Remove(int id)
        {
            var entity = Get(id);
            entities.Remove(id);
            return entity;
        }

        public E Update(E entity)
        {
            entities[entity.Id] = entity;
            return entity;
        }
    }
}
