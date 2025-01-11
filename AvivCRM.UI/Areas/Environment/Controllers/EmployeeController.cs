using System.Net;
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

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        List<CountryVM>? countries = await client.GetFromJsonAsync<List<CountryVM>>("Country/GetAll");
        ViewBag.CountryList = countries;
        List<DepartmentVM>? departments = await client.GetFromJsonAsync<List<DepartmentVM>>("Department/GetAll");
        ViewBag.DepartmentList = departments;
        List<EmployeeVM>? businessLocationList = await client.GetFromJsonAsync<List<EmployeeVM>>("Company/GetAll");
        return View(businessLocationList);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        EmployeeVM Employee = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        List<CompanyVM>? companies = await client.GetFromJsonAsync<List<CompanyVM>>("Company/GetAll");
        List<CountryVM>? countries = await client.GetFromJsonAsync<List<CountryVM>>("Country/GetAll");
        List<DepartmentVM>? departments = await client.GetFromJsonAsync<List<DepartmentVM>>("Department/GetAll");

        ViewBag.CompanyList = companies;
        ViewBag.CountryList = countries;
        ViewBag.DepartmentList = departments;
        return PartialView("_Create", Employee);
    }

    private void WriteExtractedError(Stream stream)
    {
        Dictionary<string, List<string>>? errorsFromWebAPI = Utility.ExtractErrorsFromWebAPIResponse(stream.ToString());

        foreach (KeyValuePair<string, List<string>> fieldWithErrors in errorsFromWebAPI)
        {
            Console.WriteLine($"-{fieldWithErrors.Key}");
            foreach (string? error in fieldWithErrors.Value)
            {
                Console.WriteLine($"  {error}");
            }
        }
    }

    [HttpPost]
    [ActionName("GetStatesByCountryId")]
    public async Task<IActionResult> GetStatesByCountryId(string countryId)
    {
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        List<StateVM>? states = [];

        using (HttpResponseMessage? response = await client.GetAsync("State/GetByParentId/?parentId=" + countryId
                   , HttpCompletionOption.ResponseHeadersRead))
        {
            Stream? stream = await response.Content.ReadAsStreamAsync();

            if (response.StatusCode == HttpStatusCode.BadRequest)
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

    [HttpPost]
    [ActionName("GetCitiesByStateId")]
    public async Task<IActionResult> GetCitiesByStateId(string stateId)
    {
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        List<CityVM>? cities = [];
        using (HttpResponseMessage? response = await client.GetAsync("City/GetByParentId/?parentId=" + stateId
                   , HttpCompletionOption.ResponseHeadersRead))
        {
            if (response.IsSuccessStatusCode)
            {
                Stream? stream = await response.Content.ReadAsStreamAsync();
                cities = await JsonSerializer.DeserializeAsync<List<CityVM>>(stream, _options);
            }

            return Json(cities);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(EmployeeVM Employee)
    {
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PostAsJsonAsync("Employee/Create", Employee);
        return RedirectToAction("Employee");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int Id)
    {
        if (Id == 0)
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        List<CompanyVM>? companies = await client.GetFromJsonAsync<List<CompanyVM>>("Company/GetAll");
        List<DepartmentVM>? departments = await client.GetFromJsonAsync<List<DepartmentVM>>("Department/GetAll");
        List<CountryVM>? countries = await client.GetFromJsonAsync<List<CountryVM>>("Country/GetAll");
        List<StateVM>? states = await client.GetFromJsonAsync<List<StateVM>>("State/GetAll");
        List<CityVM>? cities = await client.GetFromJsonAsync<List<CityVM>>("City/GetAll");
        ViewBag.CompanyList = companies;
        ViewBag.DepartmentList = departments;
        ViewBag.CountryList = countries;
        ViewBag.StateList = states;
        ViewBag.CityList = cities;
        EmployeeVM? Employee = await client.GetFromJsonAsync<EmployeeVM>("Employee/GetById/?Id=" + Id);
        return PartialView("_Edit", Employee);
    }

    [HttpPost]
    public async Task<IActionResult> Update(EmployeeVM Employee)
    {
        if (Employee.Id == 0)
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PutAsJsonAsync<EmployeeVM>("Employee/Update/", Employee);
        return RedirectToAction("Employee");
    }


    [HttpPost]
    public async Task<IActionResult> Delete(int Id)
    {
        if (Id == 0)
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.DeleteAsync("Employee/Delete?Id=" + Id);
        return RedirectToAction("Employee");
    }
}