using LMSWebAppClean.Domain.Interface;

namespace LMSWebAppClean.Application.Interface
{
    public interface IRepository<E> where E : IEntity
    {
        E Add(E entity);
        E Get(int id);
        List<E> GetAll();
        E Remove(int id);
        E Update(E Entity);
    }
}
