using AvivCRM.UI.Areas.Configuration.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AvivCRM.UI.Areas.Configuration.Controllers;
[Area("Configuration")]
public class RoleController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public RoleController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    public async Task<IActionResult> Index()
    {
        return View();
    }
    public async Task<IActionResult> Role()
    {
        // Page Title
        ViewData["pTitle"] = "Roles Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Configuration";
        ViewData["bParent"] = "Role";
        ViewData["bChild"] = "Role";

        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        var roleList = await client.GetFromJsonAsync<List<RoleVM>>("Role/GetAll");
        return View(roleList);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        RoleVM role = new();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        var companies = await client.GetFromJsonAsync<List<CompanyVM>>("Company/GetAll");
        var departments = await client.GetFromJsonAsync<List<DepartmentVM>>("Department/GetAll");
        var designations = await client.GetFromJsonAsync<List<DesignationVM>>("Designation/GetAll");
        ViewBag.CompanyList = companies;
        ViewBag.DepartmentList = departments;
        ViewBag.DesignationList = designations;
        return PartialView("_Create", role);
    }

    [HttpPost]
    public async Task<IActionResult> Create(RoleVM role)
    {
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PostAsJsonAsync<RoleVM>("Role/Create", role);
        return RedirectToAction("Role");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int Id)
    {
        if (Id == 0) return View();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        var companies = await client.GetFromJsonAsync<List<CompanyVM>>("Company/GetAll");
        var departments = await client.GetFromJsonAsync<List<DepartmentVM>>("Department/GetAll");
        var designations = await client.GetFromJsonAsync<List<DesignationVM>>("Designation/GetAll");
        ViewBag.CompanyList = companies;
        ViewBag.DepartmentList = departments;
        ViewBag.DesignationList = designations;
        var role = await client.GetFromJsonAsync<RoleVM>("Role/GetById/?Id=" + Id);
        return PartialView("_Edit", role);
    }

    [HttpPost]
    public async Task<IActionResult> Update(RoleVM role)
    {
        if (role.Id == 0) return View();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PutAsJsonAsync<RoleVM>("Role/Update/", role);
        return RedirectToAction("Role");
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int Id)
    {
        if (Id == 0) return View();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        var companies = await client.GetFromJsonAsync<List<CompanyVM>>("Company/GetAll");
        var departments = await client.GetFromJsonAsync<List<DepartmentVM>>("Department/GetAll");
        var designations = await client.GetFromJsonAsync<List<DesignationVM>>("Designation/GetAll");
        ViewBag.CompanyList = companies;
        ViewBag.DepartmentList = departments;
        ViewBag.DesignationList = designations;
        var role = await client.GetFromJsonAsync<RoleVM>("Role/GetById/?Id=" + Id);
        return PartialView("_Delete", role);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(RoleVM role)
    {
        if (role.Id == 0) return View();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.DeleteAsync("Role/Delete?Id=" + role.Id);
        return RedirectToAction("Role");
    }

    //[HttpPost]
    //public async Task<IActionResult> Delete(RoleVM role)
    //{
    //    JsonSerializerOptions options = new(JsonSerializerDefaults.Web)
    //    {
    //        WriteIndented = true
    //    };
    //    string forecastJson = JsonSerializer.Serialize<RoleVM>(role, options);

    //    if (role.Id == 0) return View();
    //    var client = _httpClientFactory.CreateClient("ApiGatewayCall");
    //    var roleList = Deletewithresponse(client.BaseAddress.AbsoluteUri + "Role/Delete", role);
    //    return RedirectToAction("Role");
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



