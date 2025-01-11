using AvivCRM.UI.Areas.Configuration.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AvivCRM.UI.Areas.Configuration.Controllers;
[Area("Configuration")]
public class CompanyController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CompanyController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Company()
    {
        // Page Title
        ViewData["pTitle"] = "Companies Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Configuration";
        ViewData["bParent"] = "Company";
        ViewData["bChild"] = "Company";

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        List<CompanyVM>? companyList = await client.GetFromJsonAsync<List<CompanyVM>>("Company/GetAll");

        return View(companyList);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        CompanyVM company = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        ViewBag.BusinessTypes = await client.GetFromJsonAsync<List<BusinessTypeVM>>("BusinessType/GetAll");
        ViewBag.CategoryTypes = await client.GetFromJsonAsync<List<BusinessCategoryVM>>("BusinessCategory/GetAll");
        return PartialView("_Create", company);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CompanyVM company)
    {
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PostAsJsonAsync<CompanyVM>("Company/Create", company);
        return RedirectToAction("Company");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int Id)
    {
        if (Id == 0)
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        ViewBag.BusinessTypes = await client.GetFromJsonAsync<List<BusinessTypeVM>>("BusinessType/GetAll");
        ViewBag.CategoryTypes = await client.GetFromJsonAsync<List<BusinessCategoryVM>>("Category/GetAll");
        CompanyVM? company = await client.GetFromJsonAsync<CompanyVM>("Company/GetById/?Id=" + Id);
        return PartialView("_Edit", company);
    }

    [HttpPost]
    public async Task<IActionResult> Update(CompanyVM company)
    {
        if (company.Id == 0)
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PutAsJsonAsync<CompanyVM>("Company/Update/", company);
        return RedirectToAction("Company");
    }

    //[HttpGet]
    //public async Task<IActionResult> Delete(int Id)
    //{
    //    if (Id == 0) return View();
    //    var client = _httpClientFactory.CreateClient("ApiGatewayCall");
    //    ViewBag.BusinessTypes = await client.GetFromJsonAsync<List<BusinessTypeVM>>("BusinessType/GetAll");
    //    ViewBag.CategoryTypes = await client.GetFromJsonAsync<List<BusinessCategoryVM>>("Category/GetAll");
    //    var company = await client.GetFromJsonAsync<CompanyVM>("Company/GetById/?{Id}=" + Id);
    //    return PartialView("_Delete", company);
    //}

    [HttpPost]
    public async Task<IActionResult> Delete(int Id)
    {
        if (Id == 0)
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.DeleteAsync("Company/Delete?Id=" + Id);
        return RedirectToAction("Company");
    }

    //[HttpPost]
    //public async Task<IActionResult> Delete(CompanyVM company)
    //{
    //    JsonSerializerOptions options = new(JsonSerializerDefaults.Web)
    //    {
    //        WriteIndented = true
    //    };
    //    string forecastJson = JsonSerializer.Serialize<CompanyVM>(company, options);

    //    if (company.Id == 0) return View();
    //    var client = _httpClientFactory.CreateClient("ApiGatewayCall");
    //    var companyList = Deletewithresponse(client.BaseAddress.AbsoluteUri + "Company/Delete", company);
    //    return RedirectToAction("Company");
    //}

    //public async Task<HttpResponseMessage> Deletewithresponse(string url, object entity)
    //{
    //    using (var client = new HttpClient())
    //    {
    //        var json = JsonSerializer.Serialize(entity);
    //        var content = new StringContent(json, Encoding.UTF8, "application/json");

    //        var request = new HttpRequestMessage
    //        {
    //            Method = HttpMethod.Delete,
    //            RequestUri = new Uri(url),
    //            Content = content
    //        };
    //        return await client.SendAsync(request);
    //    }
    //}
}