using LMSWebAppClean.Application.Interface;

namespace LMSWebAppClean.Application.DTO
{
    public class StandardRequestObject<T> where T : class, IRequestData
    {
        public T? Data { get; set; }

        public StandardRequestObject() { }
        public StandardRequestObject(T? data) { Data = data; }
    }
}
