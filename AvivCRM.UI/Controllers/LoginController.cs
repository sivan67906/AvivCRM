using Microsoft.AspNetCore.Mvc;

namespace AvivCRM.UI.Controllers;

public class LoginController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
    public IActionResult Login()
    {
        return View();
    }
    public IActionResult ForgotPassword()
    {
        return View();
    }
    public IActionResult RecoverPassword()
    {
        return View();
    }
    public IActionResult ResetPassword()
    {
        return View();
    }
    public IActionResult ResetSuccess()
    {
        return View();
    }
}