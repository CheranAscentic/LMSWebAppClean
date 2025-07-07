namespace LMSWebAppClean.Application.DTO
{
    public class GetByIdRequestDTO
    {
        public int Id { get; set; }

        public GetByIdRequestDTO()
        {
        }

        public GetByIdRequestDTO(int id)
        {
            Id = id;
        }
    }
}