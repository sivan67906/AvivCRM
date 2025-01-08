using AvivCRM.UI.Areas.Environment.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class TaxController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public TaxController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        return View();
    }

    public async Task<IActionResult> Tax()
    {
        // Page Title
        ViewData["pTitle"] = "Taxes Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "Tax";
        ViewData["bChild"] = "Tax View";

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        List<TaxVM>? taxList = await client.GetFromJsonAsync<List<TaxVM>>("Tax/GetAll");

        return View(taxList);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        TaxVM tax = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        return PartialView("_Create", tax);
    }

    [HttpPost]
    public async Task<IActionResult> Create(TaxVM tax)
    {
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PostAsJsonAsync<TaxVM>("Tax/Create", tax);
        return RedirectToAction("Tax");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int Id)
    {
        if (Id == 0)
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        TaxVM? tax = await client.GetFromJsonAsync<TaxVM>("Tax/GetById/?Id=" + Id);
        return PartialView("_Edit", tax);
    }

    [HttpPost]
    public async Task<IActionResult> Update(TaxVM tax)
    {
        if (tax.Id == 0)
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PutAsJsonAsync<TaxVM>("Tax/Update/", tax);
        return RedirectToAction("Tax");
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int Id)
    {
        if (Id == 0)
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        TaxVM? tax = await client.GetFromJsonAsync<TaxVM>("Tax/GetById/?Id=" + Id);
        return PartialView("_Delete", tax);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(TaxVM tax)
    {
        if (tax.Id == 0)
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.DeleteAsync("Tax/Delete?Id=" + tax.Id);
        return RedirectToAction("Tax");
    }
}