using Microsoft.AspNetCore.Mvc;
using AvivCRM.UI.Areas.Environment.ViewModels;

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class PaymentController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    public PaymentController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;

    }
    public async Task<IActionResult> Index()
    {
        return View();
    }

    public async Task<IActionResult> Payment()
    {
        // Page Title
        ViewData["pTitle"] = "Payments Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "Payment";
        ViewData["bChild"] = "Payment View";

        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        var PaymentList = await client.GetFromJsonAsync<List<PaymentVM>>("Payment/GetAll");

        return View(PaymentList);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        PaymentVM company = new();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        return PartialView("_Create", company);
    }

    [HttpPost]
    public async Task<IActionResult> Create(PaymentVM Payment)
    {
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PostAsJsonAsync<PaymentVM>("Payment/Create", Payment);
        return RedirectToAction("Payment");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int Id)
    {
        if (Id == 0) return View();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        var Payment = await client.GetFromJsonAsync<PaymentVM>("Payment/GetById/?Id=" + Id);
        return PartialView("_Edit", Payment);
    }

    [HttpPost]
    public async Task<IActionResult> Update(PaymentVM Payment)
    {
        if (Payment.Id == 0) return View();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PutAsJsonAsync<PaymentVM>("Payment/Update/", Payment);
        return RedirectToAction("Payment");
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int Id)
    {
        if (Id == 0) return View();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        var Payment = await client.GetFromJsonAsync<PaymentVM>("Payment/GetById/?Id=" + Id);
        return PartialView("_Delete", Payment);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(PaymentVM Payment)
    {
        if (Payment.Id == 0) return View();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.DeleteAsync("Payment/Delete?Id=" + Payment.Id);
        return RedirectToAction("Payment");
    }
}



