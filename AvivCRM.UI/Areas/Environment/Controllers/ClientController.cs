using Microsoft.AspNetCore.Mvc;
using AvivCRM.UI.Areas.Environment.ViewModels;
using AvivCRM.UI.Utilities;
using System.Text.Json;
using CityVM = AvivCRM.UI.Areas.Environment.ViewModels.CityVM;
using CountryVM = AvivCRM.UI.Areas.Environment.ViewModels.CountryVM;
using StateVM = AvivCRM.UI.Areas.Environment.ViewModels.StateVM;

namespace AvivCRM.UI.Areas.Environment.Controllers
{
    [Area("Environment")]
    public class ClientController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _options;
        public ClientController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }


        public async Task<IActionResult> Client()
        {
            // Page Title
            ViewData["pTitle"] = "Clients Profile";

            // Breadcrumb
            ViewData["bGParent"] = "Environment";
            ViewData["bParent"] = "Client";
            ViewData["bChild"] = "Client View";

            var client = _httpClientFactory.CreateClient("ApiGatewayCall");
            var countries = await client.GetFromJsonAsync<List<CountryVM>>("Country/GetAll");
            ViewBag.CountryList = countries;
            var businessLocationList = await client.GetFromJsonAsync<List<ClientVM>>("Client/GetAll");
            return View(businessLocationList);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ClientVM clients = new();
            var client = _httpClientFactory.CreateClient("ApiGatewayCall");
            var companies = await client.GetFromJsonAsync<List<CompanyVM>>("Company/GetAll");
            var countries = await client.GetFromJsonAsync<List<CountryVM>>("Country/GetAll");
            ViewBag.CompanyList = companies;
            ViewBag.CountryList = countries;
            return PartialView("_Create", clients);
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
        public async Task<IActionResult> Create(ClientVM clients)
        {
            var client = _httpClientFactory.CreateClient("ApiGatewayCall");
            await client.PostAsJsonAsync("Client/Create", clients);
            return RedirectToAction("Client");
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
            var clients = await client.GetFromJsonAsync<ClientVM>("Client/GetById/?Id=" + Id);
            return PartialView("_Edit", clients);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ClientVM clients)
        {
            if (clients.Id == 0) return View();
            var client = _httpClientFactory.CreateClient("ApiGatewayCall");
            await client.PutAsJsonAsync<ClientVM>("Client/Update/", clients);
            return RedirectToAction("Client");
        }



        [HttpPost]
        public async Task<IActionResult> Delete(int Id)
        {
            if (Id == 0) return View();
            var client = _httpClientFactory.CreateClient("ApiGatewayCall");
            await client.DeleteAsync("Client/Delete?Id=" + Id);
            return RedirectToAction("Client");
        }

























        //    public async Task<IActionResult> Client(string searchQuery = null)
        //    {
        //        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        //        //var productList = await client.GetFromJsonAsync<List<ProductVM>>("Product/GetAll");

        //        List<ClientVM> productList;

        //        if (string.IsNullOrEmpty(searchQuery))
        //        {
        //            // Fetch all products if no search query is provided
        //            productList = await client.GetFromJsonAsync<List<ClientVM>>("Client/GetAll");
        //        }
        //        else
        //        {
        //            // Fetch products matching the search query
        //            productList = await client.GetFromJsonAsync<List<ClientVM>>($"Client/SearchByName?name={searchQuery}");
        //        }
        //        ViewData["searchQuery"] = searchQuery; // Retain search query
        //        return View(productList);
        //    }

        //    [HttpGet]
        //    public async Task<IActionResult> Create()
        //    {
        //        ClientVM product = new();
        //        return PartialView("_Create", product);
        //    }

        //    [HttpPost]
        //    public async Task<IActionResult> Create(ClientVM product)
        //    {
        //        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        //        await client.PostAsJsonAsync<ClientVM>("Client/Create", product);
        //        return RedirectToAction("Client");
        //    }

        //    [HttpGet]
        //    public async Task<IActionResult> Edit(int Id)
        //    {
        //        if (Id == 0) return View();
        //        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        //        var product = await client.GetFromJsonAsync<ClientVM>("Client/GetById/?Id=" + Id);
        //        return PartialView("_Edit", product);
        //    }

        //    [HttpPost]
        //    public async Task<IActionResult> Update(ClientVM product)
        //    {
        //        if (product.Id == 0) return View();
        //        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        //        await client.PutAsJsonAsync<ClientVM>("Client/Update/", product);
        //        return RedirectToAction("Client");
        //    }

        //    [HttpGet]
        //    public async Task<IActionResult> Delete(int Id)
        //    {
        //        if (Id == 0) return View();
        //        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        //        var product = await client.GetFromJsonAsync<ClientVM>("Client/GetById/?Id=" + Id);
        //        return PartialView("_Delete", product);
        //    }

        //    [HttpPost]
        //    public async Task<IActionResult> Delete(ClientVM product)
        //    {
        //        if (product.Id == 0) return View();
        //        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        //        var productList = await client.DeleteAsync("Client/Delete?Id=" + product.Id);
        //        return RedirectToAction("Client");
        //    }
    }
}



