using LMSWebAppClean.Domain.Enum;

namespace LMSWebAppClean.Application.DTO
{
    public class CreateUserDTO
    {
        public string Name { get; set; }
        public UserType Type { get; set; }
    }
}
