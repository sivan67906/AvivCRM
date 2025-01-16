using AvivCRM.UI.Areas.Configuration.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AvivCRM.UI.Areas.Configuration.Controllers;
[Area("Configuration")]
public class PlanTypeController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public PlanTypeController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> PlanType()
    {
        // Page Title
        ViewData["pTitle"] = "PlanTypes Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Configuration";
        ViewData["bParent"] = "PlanType";
        ViewData["bChild"] = "PlanType";

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        List<PlanTypeVM>? planTypeList = await client.GetFromJsonAsync<List<PlanTypeVM>>("PlanType/GetAll");

        return View(planTypeList);
    }

    [HttpGet]
    public IActionResult Create()
    {
        PlanTypeVM planType = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        return PartialView("_Create", planType);
    }

    [HttpPost]
    public async Task<IActionResult> Create(PlanTypeVM planType)
    {
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PostAsJsonAsync<PlanTypeVM>("PlanType/Create", planType);
        return RedirectToAction("PlanType");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int Id)
    {
        if (Id == 0)
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        PlanTypeVM? planType = await client.GetFromJsonAsync<PlanTypeVM>("PlanType/GetById/?Id=" + Id);
        return PartialView("_Edit", planType);
    }

    [HttpPost]
    public async Task<IActionResult> Update(PlanTypeVM planType)
    {
        //if (planType.Id == 0)
        //{
        //    return View();
        //}

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PutAsJsonAsync<PlanTypeVM>("PlanType/Update/", planType);
        return RedirectToAction("PlanType");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int Id)
    {
        if (Id == 0)
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.DeleteAsync("PlanType/Delete?Id=" + Id);
        return RedirectToAction("PlanType");
    }
}