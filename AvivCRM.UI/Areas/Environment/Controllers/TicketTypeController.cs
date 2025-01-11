using System.Text;
using AvivCRM.UI.Areas.Environment.ViewModels;
using AvivCRM.UI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class TicketTypeController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public TicketTypeController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> TicketType()
    {
        // Page Title
        ViewData["pTitle"] = "TicketTypes Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "TicketType";
        ViewData["bChild"] = "TicketType";

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        ApiResultResponse<List<TicketTypeVM>>? ticketTypes =
            await client.GetFromJsonAsync<ApiResultResponse<List<TicketTypeVM>>>("TicketType/all-tickettype");

        return PartialView("_TicketType", ticketTypes!.Data!);
    }

    [HttpGet]
    public IActionResult Create()
    {
        TicketTypeVM ticketType = new();
        return PartialView("_Create", ticketType);
    }

    [HttpPost]
    public async Task<IActionResult> Create(TicketTypeVM ticketType)
    {
        ApiResultResponse<TicketTypeVM> source = new();

        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (ticketType.Name == null)
        {
            ticketType.Name = "";
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        string? jsonticketType = JsonConvert.SerializeObject(ticketType);
        StringContent? ticketTypecontent = new(jsonticketType, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseTicketType =
            await client.PostAsync("TicketType/create-tickettype", ticketTypecontent);

        if (responseTicketType.IsSuccessStatusCode)
        {
            string? jsonResponseTicketType = await responseTicketType.Content.ReadAsStringAsync();
            source = JsonConvert.DeserializeObject<ApiResultResponse<TicketTypeVM>>(jsonResponseTicketType);
        }
        else
        {
            string? errorContent = await responseTicketType.Content.ReadAsStringAsync();
            source = new ApiResultResponse<TicketTypeVM>
            {
                IsSuccess = false,
                Message = responseTicketType.StatusCode + "ErrorContent: " + errorContent
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

        ApiResultResponse<TicketTypeVM> ticketType = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        ticketType =
            await client.GetFromJsonAsync<ApiResultResponse<TicketTypeVM>>("TicketType/byid-tickettype/?Id=" + Id);

        //ViewBag.ApiResult = ticketType!.Data;
        //ViewBag.ApiMessage = ticketType!.Message;
        //ViewBag.ApiStatus = ticketType.IsSuccess;

        if (!ticketType!.IsSuccess)
        {
            return View();
        }

        return PartialView("_Edit", ticketType.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(TicketTypeVM ticketType)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (ticketType.Name == null)
        {
            ticketType.Name = "";
        }

        ApiResultResponse<TicketTypeVM> source = new();

        if (GuidExtensions.IsNullOrEmpty(ticketType.Id))
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        string? jsonticketType = JsonConvert.SerializeObject(ticketType);
        StringContent? ticketTypecontent = new(jsonticketType, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseTicketType =
            await client.PutAsync("TicketType/update-tickettype/", ticketTypecontent);
        if (responseTicketType.IsSuccessStatusCode)
        {
            string? jsonResponseTicketType = await responseTicketType.Content.ReadAsStringAsync();
            source = JsonConvert.DeserializeObject<ApiResultResponse<TicketTypeVM>>(jsonResponseTicketType);
        }
        else
        {
            string? errorContent = await responseTicketType.Content.ReadAsStringAsync();
            source = new ApiResultResponse<TicketTypeVM>
            {
                IsSuccess = false,
                Message = responseTicketType.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
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

        ApiResultResponse<TicketTypeVM> source = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? responseTicketType = await client.DeleteAsync("TicketType/delete-tickettype?Id=" + Id);
        if (responseTicketType.IsSuccessStatusCode)
        {
            string? jsonResponseTicketType = await responseTicketType.Content.ReadAsStringAsync();
            source = JsonConvert.DeserializeObject<ApiResultResponse<TicketTypeVM>>(jsonResponseTicketType);
        }
        else
        {
            string? errorContent = await responseTicketType.Content.ReadAsStringAsync();
            source = new ApiResultResponse<TicketTypeVM>
            {
                IsSuccess = false,
                Message = responseTicketType.StatusCode.ToString()
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