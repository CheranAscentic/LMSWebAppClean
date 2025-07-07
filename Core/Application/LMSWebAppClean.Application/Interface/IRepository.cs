using LMSWebAppClean.Domain.Interface;

namespace LMSWebAppClean.Application.Interface
{
    public interface IRepository<E> where E : IEntity
    {
        E Add(E entity);
        E Get(int id);
        E GetWithIncludes(int id, params string[] includes); // New method for loading related data
        List<E> GetAll();
        List<E> GetAllWithIncludes(params string[] includes); // New method for loading all with related data
        E Remove(int id);
        E Update(E Entity);
    }
}
