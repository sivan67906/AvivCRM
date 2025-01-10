using AvivCRM.UI.Areas.Configuration.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AvivCRM.UI.Areas.Configuration.Controllers;
[Area("Configuration")]
public class DepartmentController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public DepartmentController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Department()
    {
        // Page Title
        ViewData["pTitle"] = "Departments Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Configuration";
        ViewData["bParent"] = "Department";
        ViewData["bChild"] = "Department";
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        List<DepartmentVM>? departmentList = await client.GetFromJsonAsync<List<DepartmentVM>>("Department/GetAll");
        List<CompanyVM>? Companies = await client.GetFromJsonAsync<List<CompanyVM>>("Company/GetAll");
        ViewBag.CompanyList = Companies;
        return View(departmentList);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        DepartmentVM department = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        List<CompanyVM>? Companies = await client.GetFromJsonAsync<List<CompanyVM>>("Company/GetAll");
        ViewBag.CompanyList = Companies;
        return PartialView("_Create", department);
    }

    [HttpPost]
    public async Task<IActionResult> Create(DepartmentVM department)
    {
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PostAsJsonAsync<DepartmentVM>("Department/Create", department);
        return RedirectToAction("Department");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int Id)
    {
        if (Id == 0)
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        DepartmentVM? department = await client.GetFromJsonAsync<DepartmentVM>("Department/GetById/?Id=" + Id);
        List<CompanyVM>? Companies = await client.GetFromJsonAsync<List<CompanyVM>>("Company/GetAll");
        ViewBag.CompanyList = Companies;
        return PartialView("_Edit", department);
    }

    [HttpPost]
    public async Task<IActionResult> Update(DepartmentVM department)
    {
        if (department.Id == 0)
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PutAsJsonAsync<DepartmentVM>("Department/Update/", department);

        return RedirectToAction("Department");
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int Id)
    {
        if (Id == 0)
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        DepartmentVM? department = await client.GetFromJsonAsync<DepartmentVM>("Department/GetById/?Id=" + Id);
        List<CompanyVM>? Companies = await client.GetFromJsonAsync<List<CompanyVM>>("Company/GetAll");
        ViewBag.CompanyList = Companies;
        return PartialView("_Delete", department);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(DepartmentVM department)
    {
        if (department.Id == 0)
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.DeleteAsync("Department/Delete?Id=" + department.Id);
        return RedirectToAction("Department");
    }


    //[HttpPost]
    //public async Task<IActionResult> Delete(DepartmentVM department)
    //{
    //    JsonSerializerOptions options = new(JsonSerializerDefaults.Web)
    //    {
    //        WriteIndented = true
    //    };
    //    string forecastJson = JsonSerializer.Serialize<DepartmentVM>(department, options);

    //    if (department.Id == 0) return View();
    //    var client = _httpClientFactory.CreateClient("ApiGatewayCall");
    //    var departmentList = Deletewithresponse(client.BaseAddress.AbsoluteUri + "Department/Delete", department);
    //    return RedirectToAction("Department");
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