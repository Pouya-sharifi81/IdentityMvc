using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MvcBuggetoEx.Controllers
{
    public class AuthorizeTestController : Controller
    {
        //[Authorize(Roles ="Admin")]
        public string Index()
        {
            return "Index";
        }
       // [Authorize(Policy = "Blood")]
        public string Edit()
        {
            return "Ap and o";
        }
    }
}
