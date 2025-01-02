using AvivCRM.UI.Areas.Environment.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AvivCRM.UI.Areas.Environment.Controllers
{
    [Area("Environment")]
    public class LeadCategoryController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LeadCategoryController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IActionResult> LeadCategory(string searchQuery = null)
        {
            ViewData["pTitle"] = "Lead Categories Profile";

            // Breadcrumb
            ViewData["bGParent"] = "Environment";
            ViewData["bParent"] = "Lead Category";
            ViewData["bChild"] = "Lead Category View";
            var client = _httpClientFactory.CreateClient("ApiGatewayCall");
            //var productList = await client.GetFromJsonAsync<List<ProductVM>>("Product/GetAll");

            List<LeadCategoryVM> productList;

            if (string.IsNullOrEmpty(searchQuery))
            {
                // Fetch all products if no search query is provided
                productList = await client.GetFromJsonAsync<List<LeadCategoryVM>>("LeadCategory/GetAll");
            }
            else
            {
                // Fetch products matching the search query
                productList = await client.GetFromJsonAsync<List<LeadCategoryVM>>($"LeadCategory/SearchByName?name={searchQuery}");
            }
            ViewData["searchQuery"] = searchQuery; // Retain search query
            return View(productList);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            LeadCategoryVM product = new();
            return PartialView("_Create", product);
        }

        [HttpPost]
        public async Task<IActionResult> Create(LeadCategoryVM product)
        {
            var client = _httpClientFactory.CreateClient("ApiGatewayCall");
            await client.PostAsJsonAsync<LeadCategoryVM>("LeadCategory/Create", product);
            return RedirectToAction("LeadCategory");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            if (Id == 0) return View();
            var client = _httpClientFactory.CreateClient("ApiGatewayCall");
            var product = await client.GetFromJsonAsync<LeadCategoryVM>("LeadCategory/GetById/?Id=" + Id);
            return PartialView("_Edit", product);
        }

        [HttpPost]
        public async Task<IActionResult> Update(LeadCategoryVM product)
        {
            if (product.Id == 0) return View();
            var client = _httpClientFactory.CreateClient("ApiGatewayCall");
            await client.PutAsJsonAsync<LeadCategoryVM>("LeadCategory/Update/", product);
            return RedirectToAction("LeadCategory");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            if (Id == 0) return View();
            var client = _httpClientFactory.CreateClient("ApiGatewayCall");
            var product = await client.GetFromJsonAsync<LeadCategoryVM>("LeadCategory/GetById/?Id=" + Id);
            return PartialView("_Delete", product);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(LeadCategoryVM product)
        {
            if (product.Id == 0) return View();
            var client = _httpClientFactory.CreateClient("ApiGatewayCall");
            var productList = await client.DeleteAsync("LeadCategory/Delete?Id=" + product.Id);
            return RedirectToAction("LeadCategory");
        }
    }
}



