using AvivCRM.UI.Areas.Configuration.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AvivCRM.UI.Areas.Configuration.Controllers;
[Area("Configuration")]
public class BusinessTypeController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public BusinessTypeController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> BusinessType()
    {
        // Page Title
        ViewData["pTitle"] = "BusinessTypes Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Configuration";
        ViewData["bParent"] = "BusinessType";
        ViewData["bChild"] = "BusinessType";

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        List<BusinessTypeVM>? businessTypeList =
            await client.GetFromJsonAsync<List<BusinessTypeVM>>("BusinessType/GetAll");

        return View(businessTypeList);
    }

    [HttpGet]
    public IActionResult Create()
    {
        BusinessTypeVM businessType = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        return PartialView("_Create", businessType);
    }

    [HttpPost]
    public async Task<IActionResult> Create(BusinessTypeVM businessType)
    {
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PostAsJsonAsync<BusinessTypeVM>("BusinessType/Create", businessType);
        return RedirectToAction("BusinessType");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid Id)
    {
        //if (Id == 0)
        //{
        //    return View();
        //}

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        BusinessTypeVM? businessType = await client.GetFromJsonAsync<BusinessTypeVM>("BusinessType/GetById/?Id=" + Id);
        return PartialView("_Edit", businessType);
    }

    [HttpPost]
    public async Task<IActionResult> Update(BusinessTypeVM businessType)
    {
        //if (businessType.Id == 0)
        //{
        //    return View();
        //}

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PutAsJsonAsync<BusinessTypeVM>("BusinessType/Update/", businessType);
        return RedirectToAction("BusinessType");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int Id)
    {
        if (Id == 0)
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.DeleteAsync("BusinessType/Delete?Id=" + Id);
        return RedirectToAction("BusinessType");
    }
}