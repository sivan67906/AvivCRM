using System.Text.Json;
using AvivCRM.UI.Areas.Environment.ViewModels;
using AvivCRM.UI.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class EmployeeController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly JsonSerializerOptions _options;
    public EmployeeController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }


    public async Task<IActionResult> Employee()
    {
        // Page Title
        ViewData["pTitle"] = "Employee Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "Employee";
        ViewData["bChild"] = "Employee View";

        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        var countries = await client.GetFromJsonAsync<List<CountryVM>>("Country/GetAll");
        ViewBag.CountryList = countries;
        var departments = await client.GetFromJsonAsync<List<DepartmentVM>>("Department/GetAll");
        ViewBag.DepartmentList = departments;
        var businessLocationList = await client.GetFromJsonAsync<List<EmployeeVM>>("Company/GetAll");
        return View(businessLocationList);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        EmployeeVM Employee = new();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        var companies = await client.GetFromJsonAsync<List<CompanyVM>>("Company/GetAll");
        var countries = await client.GetFromJsonAsync<List<CountryVM>>("Country/GetAll");
        var departments = await client.GetFromJsonAsync<List<DepartmentVM>>("Department/GetAll");

        ViewBag.CompanyList = companies;
        ViewBag.CountryList = countries;
        ViewBag.DepartmentList = departments;
        return PartialView("_Create", Employee);
    }
    private void WriteExtractedError(Stream stream)
    {

        var errorsFromWebAPI = Utility.ExtractErrorsFromWebAPIResponse(stream.ToString());

        foreach (var fieldWithErrors in errorsFromWebAPI)
        {
            Console.WriteLine($"-{fieldWithErrors.Key}");
            foreach (var error in fieldWithErrors.Value)
            {
                Console.WriteLine($"  {error}");
            }
        }

    }
    [HttpPost, ActionName("GetStatesByCountryId")]
    public async Task<IActionResult> GetStatesByCountryId(string countryId)
    {
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        var states = new List<StateVM>();

        using (var response = await client.GetAsync("State/GetByParentId/?parentId=" + countryId
            , HttpCompletionOption.ResponseHeadersRead))
        {
            var stream = await response.Content.ReadAsStreamAsync();

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                WriteExtractedError(stream);
            }
            else
            {
                states = await JsonSerializer.DeserializeAsync<List<StateVM>>(stream, _options);
            }
            return Json(states);
        }
    }

    [HttpPost, ActionName("GetCitiesByStateId")]
    public async Task<IActionResult> GetCitiesByStateId(string stateId)
    {
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        var cities = new List<CityVM>();
        using (var response = await client.GetAsync("City/GetByParentId/?parentId=" + stateId
            , HttpCompletionOption.ResponseHeadersRead))
        {
            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();
                cities = await JsonSerializer.DeserializeAsync<List<CityVM>>(stream, _options);
            }
            return Json(cities);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(EmployeeVM Employee)
    {
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PostAsJsonAsync("Employee/Create", Employee);
        return RedirectToAction("Employee");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int Id)
    {
        if (Id == 0) return View();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        var companies = await client.GetFromJsonAsync<List<CompanyVM>>("Company/GetAll");
        var departments = await client.GetFromJsonAsync<List<DepartmentVM>>("Department/GetAll");
        var countries = await client.GetFromJsonAsync<List<CountryVM>>("Country/GetAll");
        var states = await client.GetFromJsonAsync<List<StateVM>>("State/GetAll");
        var cities = await client.GetFromJsonAsync<List<CityVM>>("City/GetAll");
        ViewBag.CompanyList = companies;
        ViewBag.DepartmentList = departments;
        ViewBag.CountryList = countries;
        ViewBag.StateList = states;
        ViewBag.CityList = cities;
        var Employee = await client.GetFromJsonAsync<EmployeeVM>("Employee/GetById/?Id=" + Id);
        return PartialView("_Edit", Employee);
    }

    [HttpPost]
    public async Task<IActionResult> Update(EmployeeVM Employee)
    {
        if (Employee.Id == 0) return View();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PutAsJsonAsync<EmployeeVM>("Employee/Update/", Employee);
        return RedirectToAction("Employee");
    }



    [HttpPost]
    public async Task<IActionResult> Delete(int Id)
    {
        if (Id == 0) return View();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.DeleteAsync("Employee/Delete?Id=" + Id);
        return RedirectToAction("Employee");
    }


}



