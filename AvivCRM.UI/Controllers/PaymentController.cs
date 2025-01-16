using Microsoft.AspNetCore.Mvc;

namespace AvivCRM.UI.Controllers;

public class PaymentController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Payment()
    {
        return View();
    }
    public IActionResult PaymentSuccess()
    {
        return View();
    }
}
