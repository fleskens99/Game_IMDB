using System.ComponentModel.DataAnnotations;

namespace Presentation.ViewModels.Account
{
    public class LogedInVm
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, MinLength(6)]
        public string Password { get; set; }
        public bool Admin { get; set; }
    }
}
