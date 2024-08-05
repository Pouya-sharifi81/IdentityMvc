using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcBuggetoEx.Models;
using System.Diagnostics;

namespace MvcBuggetoEx.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var TehranGroup = new SelectListGroup { Name = "??? ??? ??????" };
            var otherGroup = new SelectListGroup { Name = "??? ??? ???????" };

            TeamViewModel model = new TeamViewModel()
            {
                Teams = new List<SelectListItem>()
                 {
                      new SelectListItem { Value ="1" , Text = "????????"},
                      new SelectListItem { Value ="2" , Text = "??????"},
                      new SelectListItem { Value ="3" , Text = "???????????"},
                      new SelectListItem { Value ="4" , Text = "?????"},
                      new SelectListItem { Value ="5" , Text = "???????"},
                 },
                TeamOptionGroup = new List<SelectListItem>()
                {
                     new SelectListItem  {  Group=TehranGroup, Value ="1" , Text = "????????"},
                      new SelectListItem {Group=otherGroup, Value ="2" , Text = "??????"},
                      new SelectListItem {Group=otherGroup, Value ="3" , Text = "???????????"},
                      new SelectListItem { Group=TehranGroup,Value ="4" , Text = "?????"},
                      new SelectListItem { Group=TehranGroup,Value ="5" , Text = "???????"},
                },
                TeamMltipleItem = new List<SelectListItem>()
                {
                      new SelectListItem { Value ="1" , Text = "????????"},
                      new SelectListItem { Value ="2" , Text = "??????"},
                      new SelectListItem { Value ="3" , Text = "???????????"},
                      new SelectListItem { Value ="4" , Text = "?????"},
                      new SelectListItem { Value ="5" , Text = "???????"},
                }
            };
            //model.Team = "4";
            model.TeamOptionGroup.Insert(0, new SelectListItem { Value = "", Text = "None" });
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
