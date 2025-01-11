using System.Text;
using AvivCRM.UI.Areas.Environment.ViewModels;
using AvivCRM.UI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class LeadAgentController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public LeadAgentController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> LeadAgent(string searchQuery = null!)
    {
        ViewData["pTitle"] = "Lead Agents Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "Lead Agent";
        ViewData["bChild"] = "Lead Agent View";

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        //var productList = await client.GetFromJsonAsync<List<ProductVM>>("Product/GetAll");

        ApiResultResponse<List<LeadAgentVM>> leadAgentList = new();

        if (string.IsNullOrEmpty(searchQuery))
        {
            // Fetch all products if no search query is provided
            leadAgentList = await client.GetFromJsonAsync<ApiResultResponse<List<LeadAgentVM>>>("LeadAgent/all-leadagent");
        }
        else
        {
            // Fetch products matching the search query
            leadAgentList =
                await client.GetFromJsonAsync<ApiResultResponse<List<LeadAgentVM>>>($"LeadAgent/SearchByName?name={searchQuery}");
        }

        ViewData["searchQuery"] = searchQuery; // Retain search query
        //ViewBag.ApiResult = leadSourceList!.Data;
        //ViewBag.ApiMessage = leadSourceList!.Message;
        //ViewBag.ApiStatus = leadSourceList.IsSuccess;
        return View(leadAgentList!.Data);


    }

    [HttpGet]
    public IActionResult Create()
    {
        LeadAgentVM leadAgent = new();
        return PartialView("_Create", leadAgent);
    }

    [HttpPost]
    public async Task<IActionResult> Create(LeadAgentVM leadAgent)
    {
        ApiResultResponse<LeadAgentVM> agent = new();

        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        //if (leadAgent.Name == null)
        //{
        //    leadAgent.Name = "";
        //}
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        string? jsonleadAgent = JsonConvert.SerializeObject(leadAgent);
        StringContent? leadAgentcontent = new(jsonleadAgent, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseLeadAgent = await client.PostAsync("LeadAgent/create-leadagent", leadAgentcontent);
        if (responseLeadAgent.IsSuccessStatusCode)
        {
            string? jsonResponseLeadAgent = await responseLeadAgent.Content.ReadAsStringAsync();
            agent = JsonConvert.DeserializeObject<ApiResultResponse<LeadAgentVM>>(jsonResponseLeadAgent);
        }
        else
        {
            string? errorContent = await responseLeadAgent.Content.ReadAsStringAsync();
            agent = new ApiResultResponse<LeadAgentVM>
            {
                IsSuccess = false,
                Message = responseLeadAgent.StatusCode + "ErrorContent: " + errorContent
            };
        }

        //ViewBag.ApiResult = source!.Data;
        //ViewBag.ApiMessage = source!.Message;
        //ViewBag.ApiStatus = source.IsSuccess;

        //Server side Validation
        //List<string> serverErrorMessageList = new List<string>();
        //string serverErrorMessage = source!.Message!;
        //serverErrorMessageList.Add(serverErrorMessage);

        if (!agent!.IsSuccess)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        return Json(new { success = true });
        //return RedirectToAction("LeadAgent");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid Id)
    {
        if (GuidExtensions.IsNullOrEmpty(Id))
        {
            return View();
        }

        ApiResultResponse<LeadAgentVM> leadagent = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        leadagent = await client.GetFromJsonAsync<ApiResultResponse<LeadAgentVM>>("LeadAgent/byid-leadagent/?Id=" + Id);
        //ViewBag.ApiResult = leadSource!.Data;
        //ViewBag.ApiMessage = leadSource!.Message;
        //ViewBag.ApiStatus = leadSource.IsSuccess;

        if (!leadagent!.IsSuccess)
        {
            return View();
        }
        return PartialView("_Edit", leadagent.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(LeadAgentVM leadAgent)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (leadAgent.Name == null)
        {
            leadAgent.Name = "";
        }

        ApiResultResponse<LeadAgentVM> agent = new();

        if (GuidExtensions.IsNullOrEmpty(leadAgent.Id))
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        string? jsonleadAgent = JsonConvert.SerializeObject(leadAgent);
        StringContent? leadAgentcontent = new(jsonleadAgent, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseLeadAgent =
            await client.PutAsync("LeadAgent/update-leadagent/", leadAgentcontent);
        if (responseLeadAgent.IsSuccessStatusCode)
        {
            string? jsonResponseLeadAgent = await responseLeadAgent.Content.ReadAsStringAsync();
            agent = JsonConvert.DeserializeObject<ApiResultResponse<LeadAgentVM>>(jsonResponseLeadAgent);
        }
        else
        {
            string? errorContent = await responseLeadAgent.Content.ReadAsStringAsync();
            agent = new ApiResultResponse<LeadAgentVM>
            {
                IsSuccess = false,
                Message = responseLeadAgent.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        //ViewBag.ApiResult = agent!.Data;
        //ViewBag.ApiMessage = agent!.Message;
        //ViewBag.ApiStatus = agent.IsSuccess;

        //Server side Validation
        //List<string> serverErrorMessageList = new List<string>();
        //string serverErrorMessage = agent!.Message!;
        //serverErrorMessageList.Add(serverErrorMessage);

        if (!agent!.IsSuccess)
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
        ApiResultResponse<LeadAgentVM> agent = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? responseLeadAgent = await client.DeleteAsync("LeadAgent/delete-leadagent?Id=" + Id);
        if (responseLeadAgent.IsSuccessStatusCode)
        {
            string? jsonResponseLeadSource = await responseLeadAgent.Content.ReadAsStringAsync();
            agent = JsonConvert.DeserializeObject<ApiResultResponse<LeadAgentVM>>(jsonResponseLeadSource);
        }
        else
        {
            string? errorContent = await responseLeadAgent.Content.ReadAsStringAsync();
            agent = new ApiResultResponse<LeadAgentVM>
            {
                IsSuccess = false,
                Message = responseLeadAgent.StatusCode.ToString()
            };
        }

        //ViewBag.ApiResult = source!.Data;
        //ViewBag.ApiMessage = source!.Message;
        //ViewBag.ApiStatus = source.IsSuccess;

        //Server side Validation
        //List<string> serverErrorMessageList = new List<string>();
        //string serverErrorMessage = source!.Message!;
        //serverErrorMessageList.Add(serverErrorMessage);

        if (!agent!.IsSuccess)
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