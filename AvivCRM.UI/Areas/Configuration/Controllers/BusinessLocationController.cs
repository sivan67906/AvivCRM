using System.Text.Json;
using AvivCRM.UI.Areas.Configuration.ViewModels;
using AvivCRM.UI.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace AvivCRM.UI.Areas.Configuration.Controllers;
[Area("Configuration")]
public class BusinessLocationController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly JsonSerializerOptions _options;

    public BusinessLocationController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }
    public async Task<IActionResult> Index()
    {
        return View();
    }
    public async Task<IActionResult> BusinessLocation()
    {
        // Page Title
        ViewData["pTitle"] = "Business Locations Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Configuration";
        ViewData["bParent"] = "Business Location";
        ViewData["bChild"] = "Business Location";

        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        var countries = await client.GetFromJsonAsync<List<CountryVM>>("Country/GetAll");
        ViewBag.CountryList = countries;
        var businessLocationList = await client.GetFromJsonAsync<List<BusinessLocationVM>>("BusinessLocation/GetAll");
        return View(businessLocationList);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        BusinessLocationVM businessLocation = new();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        var companies = await client.GetFromJsonAsync<List<CompanyVM>>("Company/GetAll");
        var countries = await client.GetFromJsonAsync<List<CountryVM>>("Country/GetAll");
        ViewBag.CompanyList = companies;
        ViewBag.CountryList = countries;
        return PartialView("_Create", businessLocation);
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
    public async Task<IActionResult> Create(BusinessLocationVM businessLocation)
    {
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        var businesslocation = await client.PostAsJsonAsync("BusinessLocation/Create", businessLocation);
        return RedirectToAction("BusinessLocation");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int Id)
    {
        if (Id == 0) return View();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        var companies = await client.GetFromJsonAsync<List<CompanyVM>>("Company/GetAll");
        var countries = await client.GetFromJsonAsync<List<CountryVM>>("Country/GetAll");
        var states = await client.GetFromJsonAsync<List<StateVM>>("State/GetAll");
        var cities = await client.GetFromJsonAsync<List<CityVM>>("City/GetAll");
        ViewBag.CompanyList = companies;
        ViewBag.CountryList = countries;
        ViewBag.StateList = states;
        ViewBag.CityList = cities;
        var businessLocation = await client.GetFromJsonAsync<BusinessLocationVM>("BusinessLocation/GetById/?Id=" + Id);
        return PartialView("_Edit", businessLocation);
    }

    [HttpPost]
    public async Task<IActionResult> Update(BusinessLocationVM businessLocation)
    {
        if (businessLocation.Id == 0) return View();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PutAsJsonAsync<BusinessLocationVM>("BusinessLocation/Update/", businessLocation);
        return RedirectToAction("BusinessLocation");
    }

    //[HttpGet]
    //public async Task<IActionResult> Delete1(int Id)
    //{
    //    if (Id == 0) return View();
    //    var client = _httpClientFactory.CreateClient("ApiGatewayCall");
    //    var companies = await client.GetFromJsonAsync<List<CompanyVM>>("Company/GetAll");
    //    var countries = await client.GetFromJsonAsync<List<CountryVM>>("Country/GetAll");
    //    var states = await client.GetFromJsonAsync<List<StateVM>>("State/GetAll");
    //    var cities = await client.GetFromJsonAsync<List<CityVM>>("City/GetAll");
    //    ViewBag.CompanyList = companies;
    //    ViewBag.CountryList = countries;
    //    ViewBag.StateList = states;
    //    ViewBag.CityList = cities;
    //    var businessLocation = await client.GetFromJsonAsync<BusinessLocationVM>("BusinessLocation/GetById/?Id=" + Id);
    //    return PartialView("_Delete", businessLocation);
    //}

    [HttpPost]
    public async Task<IActionResult> Delete(int Id)
    {
        if (Id == 0) return View();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.DeleteAsync("BusinessLocation/Delete?Id=" + Id);
        return RedirectToAction("BusinessLocation");
    }

    //[HttpPost]
    //public async Task<IActionResult> Delete(BusinessLocationVM businessLocation)
    //{
    //    JsonSerializerOptions options = new(JsonSerializerDefaults.Web)
    //    {
    //        WriteIndented = true
    //    };
    //    string forecastJson = JsonSerializer.Serialize<BusinessLocationVM>(businessLocation, options);

    //    if (businessLocation.Id == 0) return View();
    //    var client = _httpClientFactory.CreateClient("ApiGatewayCall");
    //    var businessLocationList = Deletewithresponse(client.BaseAddress.AbsoluteUri + "BusinessLocation/Delete", businessLocation);
    //    return RedirectToAction("BusinessLocation");
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



