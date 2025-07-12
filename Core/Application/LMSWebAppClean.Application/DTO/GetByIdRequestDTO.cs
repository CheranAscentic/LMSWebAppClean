using LMSWebAppClean.Application.Interface;

namespace LMSWebAppClean.Application.DTO
{
    public class GetByIdRequestDTO : IQuery
    {
        public int Id { get; set; }

        public GetByIdRequestDTO() { }
        public GetByIdRequestDTO(int id) { Id = id; }
    }
}