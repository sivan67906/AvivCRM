using AvivCRM.UI.Areas.Configuration.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AvivCRM.UI.Areas.Configuration.Controllers;
[Area("Configuration")]
public class BusinessCategoryController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public BusinessCategoryController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> BusinessCategory()
    {
        // Page Title
        ViewData["pTitle"] = "BusinessCategories Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Configuration";
        ViewData["bParent"] = "BusinessCategory";
        ViewData["bChild"] = "BusinessCategory";

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        List<BusinessTypeVM>? businessTypeList =
            await client.GetFromJsonAsync<List<BusinessTypeVM>>("BusinessType/GetAll");
        ViewBag.BusinessTypeList = businessTypeList;
        List<BusinessCategoryVM>? businessCategoryList =
            await client.GetFromJsonAsync<List<BusinessCategoryVM>>("BusinessCategory/GetAll");

        return View(businessCategoryList);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        BusinessCategoryVM businessCategory = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        List<BusinessTypeVM>? businessTypeList =
            await client.GetFromJsonAsync<List<BusinessTypeVM>>("BusinessType/GetAll");
        ViewBag.BusinessTypeList = businessTypeList;
        return PartialView("_Create", businessCategory);
    }

    [HttpPost]
    public async Task<IActionResult> Create(BusinessCategoryVM businessCategory)
    {
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PostAsJsonAsync<BusinessCategoryVM>("BusinessCategory/Create", businessCategory);
        return RedirectToAction("BusinessCategory");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int Id)
    {
        if (Id == 0)
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        List<BusinessTypeVM>? businessTypeList =
            await client.GetFromJsonAsync<List<BusinessTypeVM>>("BusinessType/GetAll");
        ViewBag.BusinessTypeList = businessTypeList;
        BusinessCategoryVM? businessCategory =
            await client.GetFromJsonAsync<BusinessCategoryVM>("BusinessCategory/GetById/?Id=" + Id);
        return PartialView("_Edit", businessCategory);
    }

    [HttpPost]
    public async Task<IActionResult> Update(BusinessCategoryVM businessCategory)
    {
        if (businessCategory.Id == 0)
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PutAsJsonAsync<BusinessCategoryVM>("BusinessCategory/Update/", businessCategory);
        return RedirectToAction("BusinessCategory");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int Id)
    {
        if (Id == 0)
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.DeleteAsync("BusinessCategory/Delete?Id=" + Id);
        return RedirectToAction("BusinessCategory");
    }
}