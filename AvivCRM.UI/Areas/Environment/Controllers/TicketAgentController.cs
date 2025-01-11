using System.Text;
using AvivCRM.UI.Areas.Environment.ViewModels;
using AvivCRM.UI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class TicketAgentController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public TicketAgentController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> TicketAgent()
    {
        // Page Title
        ViewData["pTitle"] = "TicketAgents Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "TicketAgent";
        ViewData["bChild"] = "TicketAgent";

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        ApiResultResponse<List<TicketAgentVM>>? ticketAgents =
            await client.GetFromJsonAsync<ApiResultResponse<List<TicketAgentVM>>>("TicketAgent/all-ticketagent");

        return PartialView("_TicketAgent", ticketAgents!.Data!);
    }

    [HttpGet]
    public IActionResult Create()
    {
        TicketAgentVM ticketAgent = new();
        return PartialView("_Create", ticketAgent);
    }

    [HttpPost]
    public async Task<IActionResult> Create(TicketAgentVM ticketAgent)
    {
        ApiResultResponse<TicketAgentVM> source = new();

        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (ticketAgent.Name == null)
        {
            ticketAgent.Name = "";
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        string? jsonticketAgent = JsonConvert.SerializeObject(ticketAgent);
        StringContent? ticketAgentcontent = new(jsonticketAgent, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseTicketAgent =
            await client.PostAsync("TicketAgent/create-ticketagent", ticketAgentcontent);

        if (responseTicketAgent.IsSuccessStatusCode)
        {
            string? jsonResponseTicketAgent = await responseTicketAgent.Content.ReadAsStringAsync();
            source = JsonConvert.DeserializeObject<ApiResultResponse<TicketAgentVM>>(jsonResponseTicketAgent);
        }
        else
        {
            string? errorContent = await responseTicketAgent.Content.ReadAsStringAsync();
            source = new ApiResultResponse<TicketAgentVM>
            {
                IsSuccess = false,
                Message = responseTicketAgent.StatusCode + "ErrorContent: " + errorContent
            };
        }

        //ViewBag.ApiResult = source!.Data;
        //ViewBag.ApiMessage = source!.Message;
        //ViewBag.ApiStatus = source.IsSuccess;

        //Server side Validation
        //List<string> serverErrorMessageList = new List<string>();
        //string serverErrorMessage = source!.Message!;
        //serverErrorMessageList.Add(serverErrorMessage);

        if (!source!.IsSuccess)
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

        ApiResultResponse<TicketAgentVM> ticketAgent = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        ticketAgent =
            await client.GetFromJsonAsync<ApiResultResponse<TicketAgentVM>>("TicketAgent/byid-ticketagent/?Id=" + Id);

        //ViewBag.ApiResult = ticketAgent!.Data;
        //ViewBag.ApiMessage = ticketAgent!.Message;
        //ViewBag.ApiStatus = ticketAgent.IsSuccess;

        if (!ticketAgent!.IsSuccess)
        {
            return View();
        }

        return PartialView("_Edit", ticketAgent.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(TicketAgentVM ticketAgent)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (ticketAgent.Name == null)
        {
            ticketAgent.Name = "";
        }

        ApiResultResponse<TicketAgentVM> source = new();

        if (GuidExtensions.IsNullOrEmpty(ticketAgent.Id))
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        string? jsonticketAgent = JsonConvert.SerializeObject(ticketAgent);
        StringContent? ticketAgentcontent = new(jsonticketAgent, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseTicketAgent =
            await client.PutAsync("TicketAgent/update-ticketagent/", ticketAgentcontent);
        if (responseTicketAgent.IsSuccessStatusCode)
        {
            string? jsonResponseTicketAgent = await responseTicketAgent.Content.ReadAsStringAsync();
            source = JsonConvert.DeserializeObject<ApiResultResponse<TicketAgentVM>>(jsonResponseTicketAgent);
        }
        else
        {
            string? errorContent = await responseTicketAgent.Content.ReadAsStringAsync();
            source = new ApiResultResponse<TicketAgentVM>
            {
                IsSuccess = false,
                Message =
                    responseTicketAgent.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        //ViewBag.ApiResult = source!.Data;
        //ViewBag.ApiMessage = source!.Message;
        //ViewBag.ApiStatus = source.IsSuccess;

        //Server side Validation
        //List<string> serverErrorMessageList = new List<string>();
        //string serverErrorMessage = source!.Message!;
        //serverErrorMessageList.Add(serverErrorMessage);

        if (!source!.IsSuccess)
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

        ApiResultResponse<TicketAgentVM> source = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? responseTicketAgent = await client.DeleteAsync("TicketAgent/delete-ticketagent?Id=" + Id);
        if (responseTicketAgent.IsSuccessStatusCode)
        {
            string? jsonResponseTicketAgent = await responseTicketAgent.Content.ReadAsStringAsync();
            source = JsonConvert.DeserializeObject<ApiResultResponse<TicketAgentVM>>(jsonResponseTicketAgent);
        }
        else
        {
            string? errorContent = await responseTicketAgent.Content.ReadAsStringAsync();
            source = new ApiResultResponse<TicketAgentVM>
            {
                IsSuccess = false,
                Message = responseTicketAgent.StatusCode.ToString()
            };
        }

        //ViewBag.ApiResult = source!.Data;
        //ViewBag.ApiMessage = source!.Message;
        //ViewBag.ApiStatus = source.IsSuccess;

        //Server side Validation
        //List<string> serverErrorMessageList = new List<string>();
        //string serverErrorMessage = source!.Message!;
        //serverErrorMessageList.Add(serverErrorMessage);

        if (!source!.IsSuccess)
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