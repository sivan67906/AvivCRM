using AvivCRM.UI.Areas.Environment.Models;
using Microsoft.AspNetCore.Mvc;

namespace AvivCRM.UI.Areas.Environment.Controllers
{
    [Area("Environment")]
    public class LeadSourceController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LeadSourceController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IActionResult> LeadSource(string searchQuery = null)
        {
            ViewData["pTitle"] = "Lead Sources Profile";

            // Breadcrumb
            ViewData["bGParent"] = "Environment";
            ViewData["bParent"] = "Lead Source";
            ViewData["bChild"] = "Lead Source View";
            var client = _httpClientFactory.CreateClient("ApiGatewayCall");
            //var productList = await client.GetFromJsonAsync<List<ProductVM>>("Product/GetAll");

            List<LeadSourceVM> productList;

            if (string.IsNullOrEmpty(searchQuery))
            {
                // Fetch all products if no search query is provided
                productList = await client.GetFromJsonAsync<List<LeadSourceVM>>("LeadSource/GetAll");
            }
            else
            {
                // Fetch products matching the search query
                productList = await client.GetFromJsonAsync<List<LeadSourceVM>>($"LeadSource/SearchByName?name={searchQuery}");
            }
            ViewData["searchQuery"] = searchQuery; // Retain search query
            return View(productList);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            LeadSourceVM product = new();
            return PartialView("_Create", product);
        }

        [HttpPost]
        public async Task<IActionResult> Create(LeadSourceVM product)
        {
            var client = _httpClientFactory.CreateClient("ApiGatewayCall");
            await client.PostAsJsonAsync<LeadSourceVM>("LeadSource/Create", product);
            return RedirectToAction("LeadSource");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            if (Id == 0) return View();
            var client = _httpClientFactory.CreateClient("ApiGatewayCall");
            var product = await client.GetFromJsonAsync<LeadSourceVM>("LeadSource/GetById/?Id=" + Id);
            return PartialView("_Edit", product);
        }

        [HttpPost]
        public async Task<IActionResult> Update(LeadSourceVM product)
        {
            if (product.Id == 0) return View();
            var client = _httpClientFactory.CreateClient("ApiGatewayCall");
            await client.PutAsJsonAsync<LeadSourceVM>("LeadSource/Update/", product);
            return RedirectToAction("LeadSource");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            if (Id == 0) return View();
            var client = _httpClientFactory.CreateClient("ApiGatewayCall");
            var product = await client.GetFromJsonAsync<LeadSourceVM>("LeadSource/GetById/?Id=" + Id);
            return PartialView("_Delete", product);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(LeadStatusVM product)
        {
            if (product.Id == 0) return View();
            var client = _httpClientFactory.CreateClient("ApiGatewayCall");
            var productList = await client.DeleteAsync("LeadSource/Delete?Id=" + product.Id);
            return RedirectToAction("LeadSource");
        }
    }
}



