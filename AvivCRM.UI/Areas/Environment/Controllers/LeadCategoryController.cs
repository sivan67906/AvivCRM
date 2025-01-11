using System.Text;
using AvivCRM.UI.Areas.Environment.ViewModels;
using AvivCRM.UI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class LeadCategoryController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public LeadCategoryController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> LeadCategory(string searchQuery = null!)
    {
        ViewData["pTitle"] = "Lead Categories Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "Lead Category";
        ViewData["bChild"] = "Lead Category View";
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        //var leadcategoryList = await client.GetFromJsonAsync<List<ProductVM>>("Product/GetAll");
        ApiResultResponse<List<LeadCategoryVM>> leadcategoryList = new();


        if (string.IsNullOrEmpty(searchQuery))
        {
            // Fetch all leadcategorys if no search query is provided
            leadcategoryList = await client.GetFromJsonAsync<ApiResultResponse<List<LeadCategoryVM>>>("LeadCategory/all-leadcategory");
        }
        else
        {
            // Fetch leadcategorys matching the search query
            leadcategoryList =
                await client.GetFromJsonAsync<ApiResultResponse<List<LeadCategoryVM>>>($"LeadCategory/SearchByName?name={searchQuery}");
        }

        ViewData["searchQuery"] = searchQuery; // Retain search query
        //ViewBag.ApiResult = leadSourceList!.Data;
        //ViewBag.ApiMessage = leadSourceList!.Message;
        //ViewBag.ApiStatus = leadSourceList.IsSuccess;
        return View(leadcategoryList!.Data);
    }

    [HttpGet]
    public IActionResult Create()
    {
        LeadCategoryVM leadcategory = new();
        return PartialView("_Create", leadcategory);
    }

    [HttpPost]
    public async Task<IActionResult> Create(LeadCategoryVM leadcategory)
    {
        ApiResultResponse<LeadCategoryVM> category = new();

        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        //if (leadcategory.Name == null)
        //{
        //    leadcategory.Name = "";
        //}
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        string? jsonleadcategory = JsonConvert.SerializeObject(leadcategory);
        StringContent? leadcategorycontent = new(jsonleadcategory, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseleadcategory =
        await client.PostAsJsonAsync<LeadCategoryVM>("LeadCategory/create-leadcategory", leadcategory);
        if (responseleadcategory.IsSuccessStatusCode)
        {
            string? jsonResponseLeadCategory = await responseleadcategory.Content.ReadAsStringAsync();
            category = JsonConvert.DeserializeObject<ApiResultResponse<LeadCategoryVM>>(jsonResponseLeadCategory);
        }
        else
        {
            string? errorContent = await responseleadcategory.Content.ReadAsStringAsync();
            category = new ApiResultResponse<LeadCategoryVM>
            {
                IsSuccess = false,
                Message = responseleadcategory.StatusCode + "ErrorContent: " + errorContent
            };
        }

        //ViewBag.ApiResult = source!.Data;
        //ViewBag.ApiMessage = source!.Message;
        //ViewBag.ApiStatus = source.IsSuccess;

        //Server side Validation
        //List<string> serverErrorMessageList = new List<string>();
        //string serverErrorMessage = source!.Message!;
        //serverErrorMessageList.Add(serverErrorMessage);

        if (!category!.IsSuccess)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        return Json(new { success = true });
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid Id)
    {
        if (GuidExtensions.IsNullOrEmpty(Id))
        {
            return View();
        }

        ApiResultResponse<LeadCategoryVM> leadcategory = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        leadcategory = await client.GetFromJsonAsync<ApiResultResponse<LeadCategoryVM>>("LeadCategory/byid-leadcategory/?Id=" + Id);
        //ViewBag.ApiResult = leadSource!.Data;
        //ViewBag.ApiMessage = leadSource!.Message;
        //ViewBag.ApiStatus = leadSource.IsSuccess;

        if (!leadcategory!.IsSuccess)
        {
            return View();
        }

        return PartialView("_Edit", leadcategory.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(LeadCategoryVM leadCategory)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (leadCategory.Name == null)
        {
            leadCategory.Name = "";
        }

        ApiResultResponse<LeadCategoryVM> category = new();

        if (GuidExtensions.IsNullOrEmpty(leadCategory.Id))
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        string? jsonleadCategory = JsonConvert.SerializeObject(leadCategory);
        StringContent? leadCategorycontent = new(jsonleadCategory, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseLeadCategory =
            await client.PutAsync("LeadCategory/update-leadcategory/", leadCategorycontent);
        if (responseLeadCategory.IsSuccessStatusCode)
        {
            string? jsonResponseLeadCategory = await responseLeadCategory.Content.ReadAsStringAsync();
            category = JsonConvert.DeserializeObject<ApiResultResponse<LeadCategoryVM>>(jsonResponseLeadCategory);
        }
        else
        {
            string? errorContent = await responseLeadCategory.Content.ReadAsStringAsync();
            category = new ApiResultResponse<LeadCategoryVM>
            {
                IsSuccess = false,
                Message = responseLeadCategory.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        //ViewBag.ApiResult = category!.Data;
        //ViewBag.ApiMessage = category!.Message;
        //ViewBag.ApiStatus = category.IsSuccess;

        //Server side Validation
        //List<string> serverErrorMessageList = new List<string>();
        //string serverErrorMessage = category!.Message!;
        //serverErrorMessageList.Add(serverErrorMessage);

        if (!category!.IsSuccess)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        return Json(new { success = true });
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid Id)
    {
        //if (GuidExtensions.IsNullOrEmpty(Id)) return View();
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        ApiResultResponse<LeadCategoryVM> category = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? responseLeadCategory = await client.DeleteAsync("LeadCategory/delete-leadcategory?Id=" + Id);
        if (responseLeadCategory.IsSuccessStatusCode)
        {
            string? jsonResponseLeadSource = await responseLeadCategory.Content.ReadAsStringAsync();
            category = JsonConvert.DeserializeObject<ApiResultResponse<LeadCategoryVM>>(jsonResponseLeadSource);
        }
        else
        {
            string? errorContent = await responseLeadCategory.Content.ReadAsStringAsync();
            category = new ApiResultResponse<LeadCategoryVM>
            {
                IsSuccess = false,
                Message = responseLeadCategory.StatusCode.ToString()
            };
        }

        //ViewBag.ApiResult = source!.Data;
        //ViewBag.ApiMessage = source!.Message;
        //ViewBag.ApiStatus = source.IsSuccess;

        //Server side Validation
        //List<string> serverErrorMessageList = new List<string>();
        //string serverErrorMessage = source!.Message!;
        //serverErrorMessageList.Add(serverErrorMessage);

        if (!category!.IsSuccess)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        return Json(new { success = true });
    }
}