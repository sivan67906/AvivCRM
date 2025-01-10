using System.Text;
using AvivCRM.UI.Areas.Environment.ViewModels;
using AvivCRM.UI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class TicketGroupController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public TicketGroupController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> TicketGroup()
    {
        // Page Title
        ViewData["pTitle"] = "TicketGroups Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "TicketGroup";
        ViewData["bChild"] = "TicketGroup";

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        ApiResultResponse<List<TicketGroupVM>>? ticketGroups =
            await client.GetFromJsonAsync<ApiResultResponse<List<TicketGroupVM>>>("TicketGroup/all-ticketgroup");

        return PartialView("_TicketGroup", ticketGroups!.Data!);
    }

    [HttpGet]
    public IActionResult Create()
    {
        TicketGroupVM ticketGroup = new();
        return PartialView("_Create", ticketGroup);
    }

    [HttpPost]
    public async Task<IActionResult> Create(TicketGroupVM ticketGroup)
    {
        ApiResultResponse<TicketGroupVM> source = new();

        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (ticketGroup.Name == null)
        {
            ticketGroup.Name = "";
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        string? jsonticketGroup = JsonConvert.SerializeObject(ticketGroup);
        StringContent? ticketGroupcontent = new(jsonticketGroup, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseTicketGroup =
            await client.PostAsync("TicketGroup/create-ticketgroup", ticketGroupcontent);

        if (responseTicketGroup.IsSuccessStatusCode)
        {
            string? jsonResponseTicketGroup = await responseTicketGroup.Content.ReadAsStringAsync();
            source = JsonConvert.DeserializeObject<ApiResultResponse<TicketGroupVM>>(jsonResponseTicketGroup);
        }
        else
        {
            string? errorContent = await responseTicketGroup.Content.ReadAsStringAsync();
            source = new ApiResultResponse<TicketGroupVM>
            {
                IsSuccess = false,
                Message = responseTicketGroup.StatusCode + "ErrorContent: " + errorContent
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

        ApiResultResponse<TicketGroupVM> ticketGroup = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        ticketGroup =
            await client.GetFromJsonAsync<ApiResultResponse<TicketGroupVM>>("TicketGroup/byid-ticketgroup/?Id=" + Id);

        //ViewBag.ApiResult = ticketGroup!.Data;
        //ViewBag.ApiMessage = ticketGroup!.Message;
        //ViewBag.ApiStatus = ticketGroup.IsSuccess;

        if (!ticketGroup!.IsSuccess)
        {
            return View();
        }

        return PartialView("_Edit", ticketGroup.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(TicketGroupVM ticketGroup)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (ticketGroup.Name == null)
        {
            ticketGroup.Name = "";
        }

        ApiResultResponse<TicketGroupVM> source = new();

        if (GuidExtensions.IsNullOrEmpty(ticketGroup.Id))
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        string? jsonticketGroup = JsonConvert.SerializeObject(ticketGroup);
        StringContent? ticketGroupcontent = new(jsonticketGroup, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseTicketGroup =
            await client.PutAsync("TicketGroup/update-ticketgroup/", ticketGroupcontent);
        if (responseTicketGroup.IsSuccessStatusCode)
        {
            string? jsonResponseTicketGroup = await responseTicketGroup.Content.ReadAsStringAsync();
            source = JsonConvert.DeserializeObject<ApiResultResponse<TicketGroupVM>>(jsonResponseTicketGroup);
        }
        else
        {
            string? errorContent = await responseTicketGroup.Content.ReadAsStringAsync();
            source = new ApiResultResponse<TicketGroupVM>
            {
                IsSuccess = false,
                Message =
                    responseTicketGroup.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
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

        ApiResultResponse<TicketGroupVM> source = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? responseTicketGroup = await client.DeleteAsync("TicketGroup/delete-ticketgroup?Id=" + Id);
        if (responseTicketGroup.IsSuccessStatusCode)
        {
            string? jsonResponseTicketGroup = await responseTicketGroup.Content.ReadAsStringAsync();
            source = JsonConvert.DeserializeObject<ApiResultResponse<TicketGroupVM>>(jsonResponseTicketGroup);
        }
        else
        {
            string? errorContent = await responseTicketGroup.Content.ReadAsStringAsync();
            source = new ApiResultResponse<TicketGroupVM>
            {
                IsSuccess = false,
                Message = responseTicketGroup.StatusCode.ToString()
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