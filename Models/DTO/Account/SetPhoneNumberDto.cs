using System.ComponentModel.DataAnnotations;

namespace MvcBuggetoEx.Models.DTO.Account
{
    public class SetPhoneNumberDto
    {
        [Required]
        [RegularExpression(@"(\+98|0)?9\d{9}")]
        public string PhoneNumber { get; set; }
    }
}
