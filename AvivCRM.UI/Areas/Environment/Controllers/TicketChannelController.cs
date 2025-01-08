using System.Text;
using AvivCRM.UI.Areas.Environment.ViewModels;
using AvivCRM.UI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class TicketChannelController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public TicketChannelController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        return View();
    }

    public async Task<IActionResult> TicketChannel()
    {
        // Page Title
        ViewData["pTitle"] = "TicketChannels Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "TicketChannel";
        ViewData["bChild"] = "TicketChannel";

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        ApiResultResponse<List<TicketChannelVM>>? ticketChannels =
            await client.GetFromJsonAsync<ApiResultResponse<List<TicketChannelVM>>>("TicketChannel/all-ticketchannel");

        return PartialView("_TicketChannel", ticketChannels!.Data!);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        TicketChannelVM ticketChannel = new();
        return PartialView("_Create", ticketChannel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(TicketChannelVM ticketChannel)
    {
        ApiResultResponse<TicketChannelVM> source = new();

        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (ticketChannel.Name == null)
        {
            ticketChannel.Name = "";
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        string? jsonticketChannel = JsonConvert.SerializeObject(ticketChannel);
        StringContent? ticketChannelcontent = new(jsonticketChannel, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseTicketChannel =
            await client.PostAsync("TicketChannel/create-ticketchannel", ticketChannelcontent);

        if (responseTicketChannel.IsSuccessStatusCode)
        {
            string? jsonResponseTicketChannel = await responseTicketChannel.Content.ReadAsStringAsync();
            source = JsonConvert.DeserializeObject<ApiResultResponse<TicketChannelVM>>(jsonResponseTicketChannel);
        }
        else
        {
            string? errorContent = await responseTicketChannel.Content.ReadAsStringAsync();
            source = new ApiResultResponse<TicketChannelVM>
            {
                IsSuccess = false, Message = responseTicketChannel.StatusCode + "ErrorContent: " + errorContent
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
                success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
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

        ApiResultResponse<TicketChannelVM> ticketChannel = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        ticketChannel =
            await client.GetFromJsonAsync<ApiResultResponse<TicketChannelVM>>("TicketChannel/byid-ticketchannel/?Id=" +
                                                                              Id);

        //ViewBag.ApiResult = ticketChannel!.Data;
        //ViewBag.ApiMessage = ticketChannel!.Message;
        //ViewBag.ApiStatus = ticketChannel.IsSuccess;

        if (!ticketChannel!.IsSuccess)
        {
            return View();
        }

        return PartialView("_Edit", ticketChannel.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(TicketChannelVM ticketChannel)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (ticketChannel.Name == null)
        {
            ticketChannel.Name = "";
        }

        ApiResultResponse<TicketChannelVM> source = new();

        if (GuidExtensions.IsNullOrEmpty(ticketChannel.Id))
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        string? jsonticketChannel = JsonConvert.SerializeObject(ticketChannel);
        StringContent? ticketChannelcontent = new(jsonticketChannel, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseTicketChannel =
            await client.PutAsync("TicketChannel/update-ticketchannel/", ticketChannelcontent);
        if (responseTicketChannel.IsSuccessStatusCode)
        {
            string? jsonResponseTicketChannel = await responseTicketChannel.Content.ReadAsStringAsync();
            source = JsonConvert.DeserializeObject<ApiResultResponse<TicketChannelVM>>(jsonResponseTicketChannel);
        }
        else
        {
            string? errorContent = await responseTicketChannel.Content.ReadAsStringAsync();
            source = new ApiResultResponse<TicketChannelVM>
            {
                IsSuccess = false,
                Message =
                    responseTicketChannel.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
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
                success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
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
                success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        ApiResultResponse<TicketChannelVM> source = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? responseTicketChannel =
            await client.DeleteAsync("TicketChannel/delete-ticketchannel?Id=" + Id);
        if (responseTicketChannel.IsSuccessStatusCode)
        {
            string? jsonResponseTicketChannel = await responseTicketChannel.Content.ReadAsStringAsync();
            source = JsonConvert.DeserializeObject<ApiResultResponse<TicketChannelVM>>(jsonResponseTicketChannel);
        }
        else
        {
            string? errorContent = await responseTicketChannel.Content.ReadAsStringAsync();
            source = new ApiResultResponse<TicketChannelVM>
            {
                IsSuccess = false, Message = responseTicketChannel.StatusCode.ToString()
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
                success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        return Json(new { success = true });
    }
}