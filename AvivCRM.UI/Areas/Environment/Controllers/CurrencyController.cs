using AvivCRM.UI.Areas.Environment.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class CurrencyController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CurrencyController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        return View();
    }

    public async Task<IActionResult> Currency()
    {
        // Page Title
        ViewData["pTitle"] = "Currencies Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "Currency";
        ViewData["bChild"] = "Currency View";

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        List<CurrencyVM>? currencyList = await client.GetFromJsonAsync<List<CurrencyVM>>("Currency/GetAll");

        return View(currencyList);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        CurrencyVM company = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        return PartialView("_Create", company);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CurrencyVM currency)
    {
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PostAsJsonAsync<CurrencyVM>("Currency/Create", currency);
        return RedirectToAction("Currency");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int Id)
    {
        if (Id == 0)
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        CurrencyVM? currency = await client.GetFromJsonAsync<CurrencyVM>("Currency/GetById/?Id=" + Id);
        return PartialView("_Edit", currency);
    }

    [HttpPost]
    public async Task<IActionResult> Update(CurrencyVM currency)
    {
        if (currency.Id == 0)
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PutAsJsonAsync<CurrencyVM>("Currency/Update/", currency);
        return RedirectToAction("Currency");
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int Id)
    {
        if (Id == 0)
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        CurrencyVM? currency = await client.GetFromJsonAsync<CurrencyVM>("Currency/GetById/?Id=" + Id);
        return PartialView("_Delete", currency);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(CurrencyVM currency)
    {
        if (currency.Id == 0)
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.DeleteAsync("Currency/Delete?Id=" + currency.Id);
        return RedirectToAction("Currency");
    }
}