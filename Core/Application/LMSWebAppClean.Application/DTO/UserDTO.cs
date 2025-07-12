using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Domain.Enum;

namespace LMSWebAppClean.Application.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Type { get; set; }
    }
}
