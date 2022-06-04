namespace EducationalPortal.Application.Models.DTO
{
    public class UserDto
    {
        public string Name { get; set; }

        public string Position { get; set; } = "";

        public string Email { get; set; }

        public string Avatar { get; set; }

        public string? Password { get; set; }
    }
}
