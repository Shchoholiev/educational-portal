namespace EducationalPortal.API.ViewModels
{
    public class LoginViewModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; } = false;

        public string ReturnUrl { get; set; }
    }
}
