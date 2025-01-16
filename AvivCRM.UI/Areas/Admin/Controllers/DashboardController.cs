using Microsoft.AspNetCore.Mvc;

namespace AvivCRM.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            // Page Titles
            ViewData["pTitle"] = "Dashboard";

            // Breadcrumb
            ViewData["bGParent"] = "Admin";
            ViewData["bParent"] = "Dashboard";
            ViewData["bChild"] = "Index";

            return View();
        }
    }
}
