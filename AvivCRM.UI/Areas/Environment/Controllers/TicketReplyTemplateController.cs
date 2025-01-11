using System.Text;
using AvivCRM.UI.Areas.Environment.ViewModels;
using AvivCRM.UI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class TicketReplyTemplateController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public TicketReplyTemplateController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> TicketReplyTemplate()
    {
        // Page Title
        ViewData["pTitle"] = "TicketReplyTemplates Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "TicketReplyTemplate";
        ViewData["bChild"] = "TicketReplyTemplate";

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        ApiResultResponse<List<TicketReplyTemplateVM>>? ticketReplyTemplates =
            await client.GetFromJsonAsync<ApiResultResponse<List<TicketReplyTemplateVM>>>(
                "TicketReplyTemplate/all-ticketreplytemplate");

        return PartialView("_TicketReplyTemplate", ticketReplyTemplates!.Data!);
    }

    [HttpGet]
    public IActionResult Create()
    {
        TicketReplyTemplateVM ticketReplyTemplate = new();
        return PartialView("_Create", ticketReplyTemplate);
    }

    [HttpPost]
    public async Task<IActionResult> Create(TicketReplyTemplateVM ticketReplyTemplate)
    {
        ApiResultResponse<TicketReplyTemplateVM> source = new();

        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (ticketReplyTemplate.Name == null)
        {
            ticketReplyTemplate.Name = "";
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        string? jsonticketReplyTemplate = JsonConvert.SerializeObject(ticketReplyTemplate);
        StringContent? ticketReplyTemplatecontent = new(jsonticketReplyTemplate, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseTicketReplyTemplate =
            await client.PostAsync("TicketReplyTemplate/create-ticketreplytemplate", ticketReplyTemplatecontent);

        if (responseTicketReplyTemplate.IsSuccessStatusCode)
        {
            string? jsonResponseTicketReplyTemplate = await responseTicketReplyTemplate.Content.ReadAsStringAsync();
            source = JsonConvert.DeserializeObject<ApiResultResponse<TicketReplyTemplateVM>>(
                jsonResponseTicketReplyTemplate);
        }
        else
        {
            string? errorContent = await responseTicketReplyTemplate.Content.ReadAsStringAsync();
            source = new ApiResultResponse<TicketReplyTemplateVM>
            {
                IsSuccess = false,
                Message = responseTicketReplyTemplate.StatusCode + "ErrorContent: " + errorContent
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

        ApiResultResponse<TicketReplyTemplateVM> ticketReplyTemplate = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        ticketReplyTemplate =
            await client.GetFromJsonAsync<ApiResultResponse<TicketReplyTemplateVM>>(
                "TicketReplyTemplate/byid-ticketreplytemplate/?Id=" + Id);

        //ViewBag.ApiResult = ticketReplyTemplate!.Data;
        //ViewBag.ApiMessage = ticketReplyTemplate!.Message;
        //ViewBag.ApiStatus = ticketReplyTemplate.IsSuccess;

        if (!ticketReplyTemplate!.IsSuccess)
        {
            return View();
        }

        return PartialView("_Edit", ticketReplyTemplate.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(TicketReplyTemplateVM ticketReplyTemplate)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (ticketReplyTemplate.Name == null)
        {
            ticketReplyTemplate.Name = "";
        }

        ApiResultResponse<TicketReplyTemplateVM> source = new();

        if (GuidExtensions.IsNullOrEmpty(ticketReplyTemplate.Id))
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        string? jsonticketReplyTemplate = JsonConvert.SerializeObject(ticketReplyTemplate);
        StringContent? ticketReplyTemplatecontent = new(jsonticketReplyTemplate, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseTicketReplyTemplate =
            await client.PutAsync("TicketReplyTemplate/update-ticketreplytemplate/", ticketReplyTemplatecontent);
        if (responseTicketReplyTemplate.IsSuccessStatusCode)
        {
            string? jsonResponseTicketReplyTemplate = await responseTicketReplyTemplate.Content.ReadAsStringAsync();
            source = JsonConvert.DeserializeObject<ApiResultResponse<TicketReplyTemplateVM>>(
                jsonResponseTicketReplyTemplate);
        }
        else
        {
            string? errorContent = await responseTicketReplyTemplate.Content.ReadAsStringAsync();
            source = new ApiResultResponse<TicketReplyTemplateVM>
            {
                IsSuccess = false,
                Message =
                    responseTicketReplyTemplate.StatusCode
                        .ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
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

        ApiResultResponse<TicketReplyTemplateVM> source = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? responseTicketReplyTemplate =
            await client.DeleteAsync("TicketReplyTemplate/delete-ticketreplytemplate?Id=" + Id);
        if (responseTicketReplyTemplate.IsSuccessStatusCode)
        {
            string? jsonResponseTicketReplyTemplate = await responseTicketReplyTemplate.Content.ReadAsStringAsync();
            source = JsonConvert.DeserializeObject<ApiResultResponse<TicketReplyTemplateVM>>(
                jsonResponseTicketReplyTemplate);
        }
        else
        {
            string? errorContent = await responseTicketReplyTemplate.Content.ReadAsStringAsync();
            source = new ApiResultResponse<TicketReplyTemplateVM>
            {
                IsSuccess = false,
                Message = responseTicketReplyTemplate.StatusCode.ToString()
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