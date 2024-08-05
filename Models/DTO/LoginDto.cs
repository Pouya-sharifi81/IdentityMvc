using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MvcBuggetoEx.Models.DTO
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password  { get; set; }
        [DisplayName("Rememmer me")]
        public bool IsPersistens { get; set; } = false;

        public string ReturnUrl { get; set; }
    }
}
