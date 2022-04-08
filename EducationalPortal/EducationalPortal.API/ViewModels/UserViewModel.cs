using EducationalPortal.Application.DTO;

namespace EducationalPortal.API.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Position { get; set; }

        public string Avatar { get; set; }

        public string Email { get; set; }

        public int Balance { get; set; }

        public List<RoleDTO> Roles { get; set; }
    }
}
