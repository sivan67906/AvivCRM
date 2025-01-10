using System.Text;
using AvivCRM.UI.Areas.Environment.Models;
using AvivCRM.UI.Areas.Environment.ViewModels;
using AvivCRM.UI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class LeadStatusController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public LeadStatusController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> LeadStatus(string searchQuery = null!)
    {
        ViewData["pTitle"] = "Lead Sources Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "Lead Status";
        ViewData["bChild"] = "Lead Status View";
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        ApiResultResponse<List<LeadStatusVM>> leadStatusList = new();

        if (string.IsNullOrEmpty(searchQuery))
        {
            // Fetch all products if no search query is provided
            leadStatusList =
                await client.GetFromJsonAsync<ApiResultResponse<List<LeadStatusVM>>>("LeadStatus/all-leadstatus");
        }
        else
        {
            // Fetch products matching the search query
            leadStatusList =
                await client.GetFromJsonAsync<ApiResultResponse<List<LeadStatusVM>>>(
                    $"LeadStatus/SearchByName?name={searchQuery}");
        }

        ViewData["searchQuery"] = searchQuery; // Retain search query

        //ViewBag.ApiResult = leadStatusList!.Data;
        //ViewBag.ApiMessage = leadStatusList!.Message;
        //ViewBag.ApiStatus = leadStatusList.IsSuccess;
        return View(leadStatusList!.Data);
    }

    [HttpGet]
    public IActionResult Create()
    {
        LeadStatusVM leadStatus = new();
        return PartialView("_Create", leadStatus);
    }

    [HttpPost]
    public async Task<IActionResult> Create(LeadStatusVM leadStatus)
    {
        ApiResultResponse<LeadStatusVM> status = new();

        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (leadStatus.Name == null)
        {
            leadStatus.Name = "";
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        string? jsonleadStatus = JsonConvert.SerializeObject(leadStatus);
        StringContent? leadStatuscontent = new(jsonleadStatus, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseLeadStatus =
            await client.PostAsync("LeadStatus/create-leadstatus", leadStatuscontent);

        if (responseLeadStatus.IsSuccessStatusCode)
        {
            string? jsonResponseLeadStatus = await responseLeadStatus.Content.ReadAsStringAsync();
            status = JsonConvert.DeserializeObject<ApiResultResponse<LeadStatusVM>>(jsonResponseLeadStatus);
        }
        else
        {
            string? errorContent = await responseLeadStatus.Content.ReadAsStringAsync();
            status = new ApiResultResponse<LeadStatusVM>
            {
                IsSuccess = false,
                Message = responseLeadStatus.StatusCode + "ErrorContent: " + errorContent
            };
        }

        //ViewBag.ApiResult = status!.Data;
        //ViewBag.ApiMessage = status!.Message;
        //ViewBag.ApiStatus = status.IsSuccess;

        //Server side Validation
        //List<string> serverErrorMessageList = new List<string>();
        //string serverErrorMessage = status!.Message!;
        //serverErrorMessageList.Add(serverErrorMessage);

        if (!status!.IsSuccess)
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

        ApiResultResponse<LeadStatusVM> leadStatus = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        leadStatus =
            await client.GetFromJsonAsync<ApiResultResponse<LeadStatusVM>>("LeadStatus/byid-leadstatus/?Id=" + Id);

        //ViewBag.ApiResult = leadStatus!.Data;
        //ViewBag.ApiMessage = leadStatus!.Message;
        //ViewBag.ApiStatus = leadStatus.IsSuccess;

        if (!leadStatus!.IsSuccess)
        {
            return View();
        }

        return PartialView("_Edit", leadStatus.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(LeadStatusVM leadStatus)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (leadStatus.Name == null)
        {
            leadStatus.Name = "";
        }

        ApiResultResponse<LeadStatusVM> status = new();

        if (GuidExtensions.IsNullOrEmpty(leadStatus.Id))
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        string? jsonleadStatus = JsonConvert.SerializeObject(leadStatus);
        StringContent? leadStatuscontent = new(jsonleadStatus, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseLeadStatus =
            await client.PutAsync("LeadStatus/update-leadstatus/", leadStatuscontent);
        if (responseLeadStatus.IsSuccessStatusCode)
        {
            string? jsonResponseLeadStatus = await responseLeadStatus.Content.ReadAsStringAsync();
            status = JsonConvert.DeserializeObject<ApiResultResponse<LeadStatusVM>>(jsonResponseLeadStatus);
        }
        else
        {
            string? errorContent = await responseLeadStatus.Content.ReadAsStringAsync();
            status = new ApiResultResponse<LeadStatusVM>
            {
                IsSuccess = false,
                Message = responseLeadStatus.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        //ViewBag.ApiResult = status!.Data;
        //ViewBag.ApiMessage = status!.Message;
        //ViewBag.ApiStatus = status.IsSuccess;

        //Server side Validation
        //List<string> serverErrorMessageList = new List<string>();
        //string serverErrorMessage = status!.Message!;
        //serverErrorMessageList.Add(serverErrorMessage);

        if (!status!.IsSuccess)
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

        ApiResultResponse<LeadStatusVM> status = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? responseLeadStatus = await client.DeleteAsync("LeadStatus/delete-leadstatus?Id=" + Id);
        if (responseLeadStatus.IsSuccessStatusCode)
        {
            string? jsonResponseLeadStatus = await responseLeadStatus.Content.ReadAsStringAsync();
            status = JsonConvert.DeserializeObject<ApiResultResponse<LeadStatusVM>>(jsonResponseLeadStatus);
        }
        else
        {
            string? errorContent = await responseLeadStatus.Content.ReadAsStringAsync();
            status = new ApiResultResponse<LeadStatusVM>
            {
                IsSuccess = false,
                Message = responseLeadStatus.StatusCode.ToString()
            };
        }

        //ViewBag.ApiResult = status!.Data;
        //ViewBag.ApiMessage = status!.Message;
        //ViewBag.ApiStatus = status.IsSuccess;

        //Server side Validation
        //List<string> serverErrorMessageList = new List<string>();
        //string serverErrorMessage = status!.Message!;
        //serverErrorMessageList.Add(serverErrorMessage);

        if (!status!.IsSuccess)
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