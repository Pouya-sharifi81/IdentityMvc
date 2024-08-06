using System.ComponentModel.DataAnnotations;

namespace MvcBuggetoEx.Models.DTO.Account
{
    public class ForgotPasswordConfirmationDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
