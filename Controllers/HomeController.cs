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
            var TehranGroup = new SelectListGroup { Name = "گروه تهرانی" };
            var otherGroup = new SelectListGroup { Name = "گروه شهرستانی" };

            TeamViewModel model = new TeamViewModel()
            {
                Teams = new List<SelectListItem>()
                 {
                      new SelectListItem { Value ="1" , Text = "استقلال"},
                      new SelectListItem { Value ="2" , Text = "پرسپولیس"},
                      new SelectListItem { Value ="3" , Text = "ذوب اهن"},
                      new SelectListItem { Value ="4" , Text = "سپاهان"},
                      new SelectListItem { Value ="5" , Text = "سایپا"},
                 },
                TeamOptionGroup = new List<SelectListItem>()
                {
                     new SelectListItem  {  Group=TehranGroup, Value ="1" , Text = "استقلال"},
                      new SelectListItem {Group=otherGroup, Value ="2" , Text = "سپاهان"},
                      new SelectListItem {Group=otherGroup, Value ="3" , Text = "ذوب اهن"},
                      new SelectListItem { Group=TehranGroup,Value ="4" , Text = "پرسپولیس"},
                      new SelectListItem { Group=TehranGroup,Value ="5" , Text = "ذوب اهن"}  ,
                },
                TeamMltipleItem = new List<SelectListItem>()
                {
                    new SelectListItem  {   Value ="1" , Text = "استقلال"},
                      new SelectListItem { Value ="2" , Text = "سپاهان"},
                      new SelectListItem { Value ="3" , Text = "ذوب اهن"},
                      new SelectListItem {   Value ="4" , Text = "پرسپولیس"},
                      new SelectListItem {   Value ="5" , Text = "ذوب اهن"},
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
