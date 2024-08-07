using Microsoft.AspNetCore.Identity;

namespace MvcBuggetoEx.Models
{
    public class User: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        #region relation
        public ICollection<Blog> Blogs { get; set; }
        #endregion
    }
}
