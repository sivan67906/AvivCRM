using AvivCRM.UI.Areas.Environment.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class NotificationController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public NotificationController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        return View();
    }

    public async Task<IActionResult> Notification()
    {
        // Page Title
        ViewData["pTitle"] = "Notifications Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "Notification";
        ViewData["bChild"] = "Notification View";

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        List<NotificationVM>? currencyList = await client.GetFromJsonAsync<List<NotificationVM>>("Notification/GetAll");

        return View(currencyList);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        NotificationVM company = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        return PartialView("_Create", company);
    }

    [HttpPost]
    public async Task<IActionResult> Create(NotificationVM notification)
    {
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PostAsJsonAsync<NotificationVM>("Notification/Create", notification);
        return RedirectToAction("Notification");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int Id)
    {
        if (Id == 0)
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        NotificationVM? notification = await client.GetFromJsonAsync<NotificationVM>("Notification/GetById/?Id=" + Id);
        return PartialView("_Edit", notification);
    }

    [HttpPost]
    public async Task<IActionResult> Update(NotificationVM notification)
    {
        if (notification.Id == 0)
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PutAsJsonAsync<NotificationVM>("Notification/Update/", notification);
        return RedirectToAction("Notification");
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int Id)
    {
        if (Id == 0)
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        NotificationVM? notification = await client.GetFromJsonAsync<NotificationVM>("Notification/GetById/?Id=" + Id);
        return PartialView("_Delete", notification);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(NotificationVM notification)
    {
        if (notification.Id == 0)
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.DeleteAsync("Notification/Delete?Id=" + notification.Id);
        return RedirectToAction("Notification");
    }
}