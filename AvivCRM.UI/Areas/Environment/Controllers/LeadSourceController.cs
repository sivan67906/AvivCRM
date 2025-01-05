using System.Text;
using AvivCRM.UI.Areas.Environment.ViewModels;
using AvivCRM.UI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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

            ApiResultResponse<List<LeadSourceVM>> productList = new();

            if (string.IsNullOrEmpty(searchQuery))
            {
                // Fetch all products if no search query is provided
                productList = await client.GetFromJsonAsync<ApiResultResponse<List<LeadSourceVM>>>("LeadSource/all-leadsource");
            }
            else
            {
                // Fetch products matching the search query
                productList = await client.GetFromJsonAsync<ApiResultResponse<List<LeadSourceVM>>>($"LeadSource/SearchByName?name={searchQuery}");
            }
            ViewData["searchQuery"] = searchQuery; // Retain search query

            ViewBag.ListResultData = productList!.Data;
            ViewBag.ApiResult = productList!.Data;
            ViewBag.ApiMessage = productList!.Message;
            ViewBag.ApiStatus = productList.IsSuccess;
            return View(ViewBag.ApiResult);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            LeadSourceVM product = new();
            return PartialView("_Create", product);
        }

        [HttpPost]
        public async Task<IActionResult> Create(LeadSourceVM leadSource)
        {
            ApiResultResponse<LeadSourceVM> source = new();

            if (!ModelState.IsValid)
            {
                return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }
            if (leadSource.Name == null) leadSource.Name = "";

            var client = _httpClientFactory.CreateClient("ApiGatewayCall");

            var jsonleadSource = JsonConvert.SerializeObject(leadSource);
            var leadSourcecontent = new StringContent(jsonleadSource, Encoding.UTF8, "application/json");
            var responseLeadSource = await client.PostAsync("LeadSource/create-leadsource", leadSourcecontent);

            if (responseLeadSource.IsSuccessStatusCode)
            {
                var jsonResponseLeadSource = await responseLeadSource.Content.ReadAsStringAsync();
                source = JsonConvert.DeserializeObject<ApiResultResponse<LeadSourceVM>>(jsonResponseLeadSource);
            }
            else
            {
                var errorContent = await responseLeadSource.Content.ReadAsStringAsync();
                source = new ApiResultResponse<LeadSourceVM>
                {
                    IsSuccess = false,
                    Message = responseLeadSource.StatusCode.ToString() + "ErrorContent: " + errorContent.ToString()
                };
            }

            ViewBag.ApiResult = source!.Data;
            ViewBag.ApiMessage = source!.Message;
            ViewBag.ApiStatus = source.IsSuccess;

            //Server side Validation
            //List<string> serverErrorMessageList = new List<string>();
            //string serverErrorMessage = source!.Message!;
            //serverErrorMessageList.Add(serverErrorMessage);

            if (!source.IsSuccess)
                return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            else
                return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid Id)
        {
            if (GuidExtensions.IsNullOrEmpty(Id)) return View();
            ApiResultResponse<LeadSourceVM> leadSource = new();

            var client = _httpClientFactory.CreateClient("ApiGatewayCall");
            leadSource = await client.GetFromJsonAsync<ApiResultResponse<LeadSourceVM>>("LeadSource/byid-leadsource/?Id=" + Id);

            ViewBag.ApiResult = leadSource!.Data;
            ViewBag.ApiMessage = leadSource!.Message;
            ViewBag.ApiStatus = leadSource.IsSuccess;

            if (!leadSource.IsSuccess) return View();
            else return PartialView("_Edit", ViewBag.ApiResult);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(LeadSourceVM leadSource)
        {

            if (!ModelState.IsValid)
            {
                return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }
            if (leadSource.Name == null) leadSource.Name = "";

            ApiResultResponse<LeadSourceVM> source = new();

            if (GuidExtensions.IsNullOrEmpty(leadSource.Id)) return View();
            var client = _httpClientFactory.CreateClient("ApiGatewayCall");
            var jsonleadSource = JsonConvert.SerializeObject(leadSource);
            var leadSourcecontent = new StringContent(jsonleadSource, Encoding.UTF8, "application/json");
            var responseLeadSource = await client.PutAsync("LeadSource/update-leadsource/", leadSourcecontent);
            if (responseLeadSource.IsSuccessStatusCode)
            {
                var jsonResponseLeadSource = await responseLeadSource.Content.ReadAsStringAsync();
                source = JsonConvert.DeserializeObject<ApiResultResponse<LeadSourceVM>>(jsonResponseLeadSource);
            }
            else
            {
                var errorContent = await responseLeadSource.Content.ReadAsStringAsync();
                source = new ApiResultResponse<LeadSourceVM>
                {
                    IsSuccess = false,
                    Message = responseLeadSource.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
                };
            }

            ViewBag.ApiResult = source!.Data;
            ViewBag.ApiMessage = source!.Message;
            ViewBag.ApiStatus = source.IsSuccess;

            //Server side Validation
            //List<string> serverErrorMessageList = new List<string>();
            //string serverErrorMessage = source!.Message!;
            //serverErrorMessageList.Add(serverErrorMessage);

            if (!source.IsSuccess)
                return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            else
                return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid Id)
        {
            //if (GuidExtensions.IsNullOrEmpty(Id)) return View();
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }
            ApiResultResponse<LeadSourceVM> source = new();
            var client = _httpClientFactory.CreateClient("ApiGatewayCall");
            var responseLeadSource = await client.DeleteAsync("LeadSource/delete-leadsource?Id=" + Id);
            if (responseLeadSource.IsSuccessStatusCode)
            {
                var jsonResponseLeadSource = await responseLeadSource.Content.ReadAsStringAsync();
                source = JsonConvert.DeserializeObject<ApiResultResponse<LeadSourceVM>>(jsonResponseLeadSource);
            }
            else
            {
                var errorContent = await responseLeadSource.Content.ReadAsStringAsync();
                source = new ApiResultResponse<LeadSourceVM>
                {
                    IsSuccess = false,
                    Message = responseLeadSource.StatusCode.ToString()
                };
            }

            ViewBag.ApiResult = source!.Data;
            ViewBag.ApiMessage = source!.Message;
            ViewBag.ApiStatus = source.IsSuccess;

            //Server side Validation
            //List<string> serverErrorMessageList = new List<string>();
            //string serverErrorMessage = source!.Message!;
            //serverErrorMessageList.Add(serverErrorMessage);

            if (!source.IsSuccess)
                return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            else
                return Json(new { success = true });
        }

    }
}



