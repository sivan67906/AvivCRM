using AvivCRM.UI.Areas.Environment.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AvivCRM.UI.Areas.Environment.Controllers;
public class ProjectStatusController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ProjectStatusController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    public IActionResult Index()
    {
        return View();
    }

    // Project Status
    [HttpPost]
    public async Task<IActionResult> DefaultStatusUpdate(ProjectStatusVM projStatus)
    {
        ApiResultResponse<ProjectStatusVM> pStatus = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        HttpResponseMessage? responseProjStatus =
            await client.PutAsJsonAsync("ProjectStatus/update-projectstatusdefaultstatus/", projStatus);

        if (responseProjStatus.IsSuccessStatusCode)
        {
            string? jsonResponseLeadSource = await responseProjStatus.Content.ReadAsStringAsync();
            pStatus = JsonConvert.DeserializeObject<ApiResultResponse<ProjectStatusVM>>(jsonResponseLeadSource);
        }
        else
        {
            string? errorContent = await responseProjStatus.Content.ReadAsStringAsync();
            pStatus = new ApiResultResponse<ProjectStatusVM>
            {
                IsSuccess = false,
                Message = responseProjStatus.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        // Server side Validation
        List<string> serverErrorMessageList = [];
        string serverErrorMessage = pStatus!.Message!;
        serverErrorMessageList.Add(serverErrorMessage);

        if (!pStatus!.IsSuccess)
        {
            return Json(new { success = false, errors = serverErrorMessageList });
        }

        return Json(new { success = true });
    }

    [HttpGet]
    public async Task<IActionResult> EditProjectStatus(Guid Id)
    {
        ProjectStatusVM projStatus = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        ApiResultResponse<ProjectStatusVM>? projectStatus =
            await client.GetFromJsonAsync<ApiResultResponse<ProjectStatusVM>>("ProjectStatus/byid-projectstatus/?Id=" +
                                                                              Id);
        projStatus = projectStatus!.Data!;
        return PartialView("~/Areas/Environment/Views/Project/ProjectStatuses/_Edit.cshtml", projStatus);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateProjectStatus(ProjectStatusVM projectStatus)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        ApiResultResponse<ProjectStatusVM> pStatus = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? responseProjStatus =
            await client.PutAsJsonAsync("ProjectStatus/update-projectstatus/", projectStatus);
        if (responseProjStatus.IsSuccessStatusCode)
        {
            string? jsonResponseLeadSource = await responseProjStatus.Content.ReadAsStringAsync();
            pStatus = JsonConvert.DeserializeObject<ApiResultResponse<ProjectStatusVM>>(jsonResponseLeadSource);
        }
        else
        {
            string? errorContent = await responseProjStatus.Content.ReadAsStringAsync();
            pStatus = new ApiResultResponse<ProjectStatusVM>
            {
                IsSuccess = false,
                Message = responseProjStatus.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        // Server side Validation
        List<string> serverErrorMessageList = [];
        string serverErrorMessage = pStatus!.Message!;
        serverErrorMessageList.Add(serverErrorMessage);
        if (!pStatus!.IsSuccess)
        {
            return Json(new { success = false, errors = serverErrorMessageList });
        }

        return Json(new { success = true });
    }

    [HttpPost]
    public async Task<IActionResult> DeleteProjectStatus(Guid Id)
    {
        ApiResultResponse<ProjectStatusVM> pStatus = new();
        //if (Id == 0) return View();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? responseProjectStatus =
            await client.DeleteAsync("ProjectStatus/delete-projectstatus?Id=" + Id);
        if (responseProjectStatus.IsSuccessStatusCode)
        {
            string? jsonResponseLeadSource = await responseProjectStatus.Content.ReadAsStringAsync();
            pStatus = JsonConvert.DeserializeObject<ApiResultResponse<ProjectStatusVM>>(jsonResponseLeadSource);
        }
        else
        {
            string? errorContent = await responseProjectStatus.Content.ReadAsStringAsync();
            pStatus = new ApiResultResponse<ProjectStatusVM>
            {
                IsSuccess = false,
                Message = responseProjectStatus.StatusCode.ToString()
            };
        }

        // Server side Validation
        List<string> serverErrorMessageList = [];
        string serverErrorMessage = pStatus!.Message!;
        serverErrorMessageList.Add(serverErrorMessage);
        if (!pStatus!.IsSuccess)
        {
            return Json(new { success = false, errors = serverErrorMessageList });
        }

        return Json(new { success = true });
    }

    [HttpGet]
    public IActionResult CreateProjectStatus()
    {
        ProjectStatusVM projectStatus = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        return PartialView("~/Areas/Environment/Views/Project/ProjectStatuses/_Create.cshtml", projectStatus);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProjectStatus(ProjectStatusVM projectStatus)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        ApiResultResponse<ProjectStatusVM> pStatus = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? responseProjectStatus =
            await client.PostAsJsonAsync("ProjectStatus/create-projectstatus", projectStatus);
        if (responseProjectStatus.IsSuccessStatusCode)
        {
            string? jsonResponseLeadSource = await responseProjectStatus.Content.ReadAsStringAsync();
            pStatus = JsonConvert.DeserializeObject<ApiResultResponse<ProjectStatusVM>>(jsonResponseLeadSource);
        }
        else
        {
            string? errorContent = await responseProjectStatus.Content.ReadAsStringAsync();
            pStatus = new ApiResultResponse<ProjectStatusVM>
            {
                IsSuccess = false,
                Message = responseProjectStatus.StatusCode.ToString()
            };
        }

        // Server side Validation
        List<string> serverErrorMessageList = [];
        string serverErrorMessage = pStatus!.Message!;
        serverErrorMessageList.Add(serverErrorMessage);

        if (!pStatus!.IsSuccess)
        {
            return Json(new { success = false, errors = serverErrorMessageList });
        }

        return Json(new { success = true });
    }
}