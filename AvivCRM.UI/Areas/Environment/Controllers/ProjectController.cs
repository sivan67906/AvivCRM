using System.Text;
using AvivCRM.UI.Areas.Environment.ViewModels;
using AvivCRM.UI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class ProjectController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ProjectController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    public async Task<IActionResult> Index()
    {
        return View();
    }
    public async Task<IActionResult> Project()
    {
        // Page Title
        ViewData["pTitle"] = "Projects Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "Project";
        ViewData["bChild"] = "Project";

        var client = _httpClientFactory.CreateClient("ApiGatewayCall");

        ProjectSettingVM projectSetting = new();
        List<ProjectSettingVM> projectSettingList = new();
        List<ProjectStatusVM> projectStatusSettingList = new();
        List<ProjectCategoryVM> projectCategoriesList = new();

        // Get Project Setting List
        var projectReminderPersonSettings = await client.GetFromJsonAsync<ApiResultResponse<List<ProjectReminderPersonVM>>>("ProjectReminderPerson/all-projectreminderperson");
        var projectSettings = await client.GetFromJsonAsync<ApiResultResponse<List<ProjectSettingVM>>>("ProjectSetting/all-projectsetting");
        if (projectSettings!.Data!.Count() > 0)
        {
            projectSettingList = projectSettings!.Data!;
            projectSetting = projectSettingList.FirstOrDefault();
        }
        projectSetting!.projectReminderPersons = projectReminderPersonSettings!.Data;

        // Get Project Status List
        var projectStatusSettings = await client.GetFromJsonAsync<ApiResultResponse<List<ProjectStatusVM>>>("ProjectStatus/all-projectstatus");
        if (projectStatusSettings!.Data!.Count() > 0)
        {
            projectStatusSettingList = projectStatusSettings!.Data!;
        }
        // Get Project Category List
        var projectCategories = await client.GetFromJsonAsync<ApiResultResponse<List<ProjectCategoryVM>>>("ProjectCategory/all-projectcategory");
        if (projectCategories!.Data!.Count() > 0)
        {
            projectCategoriesList = projectCategories!.Data!;
        }

        // fill ProjectViewModel
        var vmProject = new ProjectVM
        {
            ProjectSetting = projectSetting,
            ProjectStatuses = projectStatusSettingList,
            ProjectCategories = projectCategoriesList
        };
        return View(vmProject);
    }

    // Project Setting
    [HttpPost]
    public async Task<IActionResult> ProjectSettingUpdate(ProjectSettingVM projSetting)
    {
        if (GuidExtensions.IsNullOrEmpty(projSetting.Id)) return View();
        ApiResultResponse<ProjectSettingVM> setting = new();

        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        var jsonprojSetting = JsonConvert.SerializeObject(projSetting);
        var projSettingContent = new StringContent(jsonprojSetting, Encoding.UTF8, "application/json");
        var responseProjSetting = await client.PutAsync("ProjectSetting/update-projectsetting/", projSettingContent);

        if (responseProjSetting.IsSuccessStatusCode)
        {
            var jsonResponseLeadSource = await responseProjSetting.Content.ReadAsStringAsync();
            setting = JsonConvert.DeserializeObject<ApiResultResponse<ProjectSettingVM>>(jsonResponseLeadSource);
        }
        else
        {
            var errorContent = await responseProjSetting.Content.ReadAsStringAsync();
            setting = new ApiResultResponse<ProjectSettingVM>
            {
                IsSuccess = false,
                Message = responseProjSetting.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        // Server side Validation
        List<string> serverErrorMessageList = new List<string>();
        string serverErrorMessage = setting!.Message!.ToString();
        serverErrorMessageList.Add(serverErrorMessage);

        if (!setting!.IsSuccess)
            return Json(new { success = false, errors = serverErrorMessageList });
        else
            return Json(new { success = true });
    }

    // Project Status
    [HttpPost]
    public async Task<IActionResult> DefaultStatusUpdate(ProjectStatusVM projStatus)
    {
        ApiResultResponse<ProjectStatusVM> pStatus = new();

        var client = _httpClientFactory.CreateClient("ApiGatewayCall");

        var responseProjStatus = await client.PutAsJsonAsync("ProjectStatus/update-projectstatusdefaultstatus/", projStatus);

        if (responseProjStatus.IsSuccessStatusCode)
        {
            var jsonResponseLeadSource = await responseProjStatus.Content.ReadAsStringAsync();
            pStatus = JsonConvert.DeserializeObject<ApiResultResponse<ProjectStatusVM>>(jsonResponseLeadSource);
        }
        else
        {
            var errorContent = await responseProjStatus.Content.ReadAsStringAsync();
            pStatus = new ApiResultResponse<ProjectStatusVM>
            {
                IsSuccess = false,
                Message = responseProjStatus.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        // Server side Validation
        List<string> serverErrorMessageList = new List<string>();
        string serverErrorMessage = pStatus!.Message!.ToString();
        serverErrorMessageList.Add(serverErrorMessage);

        if (!pStatus!.IsSuccess)
            return Json(new { success = false, errors = serverErrorMessageList });
        else
            return Json(new { success = true });
    }

    [HttpGet]
    public async Task<IActionResult> EditProjectStatus(Guid Id)
    {
        ProjectStatusVM projStatus = new();

        //if (Id == 0) return View();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        var projectStatus = await client.GetFromJsonAsync<ApiResultResponse<ProjectStatusVM>>("ProjectStatus/byid-projectstatus/?Id=" + Id);
        projStatus = projectStatus!.Data!;
        return PartialView("~/Areas/Environment/Views/Project/ProjectStatuses/_Edit.cshtml", projStatus);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateProjectStatus(ProjectStatusVM projectStatus)
    {
        if (!ModelState.IsValid)
        {
            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }
        ApiResultResponse<ProjectStatusVM> pStatus = new();

        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        var responseProjStatus = await client.PutAsJsonAsync("ProjectStatus/update-projectstatus/", projectStatus);
        if (responseProjStatus.IsSuccessStatusCode)
        {
            var jsonResponseLeadSource = await responseProjStatus.Content.ReadAsStringAsync();
            pStatus = JsonConvert.DeserializeObject<ApiResultResponse<ProjectStatusVM>>(jsonResponseLeadSource);
        }
        else
        {
            var errorContent = await responseProjStatus.Content.ReadAsStringAsync();
            pStatus = new ApiResultResponse<ProjectStatusVM>
            {
                IsSuccess = false,
                Message = responseProjStatus.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        // Server side Validation
        List<string> serverErrorMessageList = new List<string>();
        string serverErrorMessage = pStatus!.Message!.ToString();
        serverErrorMessageList.Add(serverErrorMessage);
        if (!pStatus!.IsSuccess)
            return Json(new { success = false, errors = serverErrorMessageList });
        else
            return Json(new { success = true });
    }

    [HttpPost]
    public async Task<IActionResult> DeleteProjectStatus(Guid Id)
    {
        ApiResultResponse<ProjectStatusVM> pStatus = new();
        //if (Id == 0) return View();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        var responseProjectStatus = await client.DeleteAsync("ProjectStatus/delete-projectstatus?Id=" + Id);
        if (responseProjectStatus.IsSuccessStatusCode)
        {
            var jsonResponseLeadSource = await responseProjectStatus.Content.ReadAsStringAsync();
            pStatus = JsonConvert.DeserializeObject<ApiResultResponse<ProjectStatusVM>>(jsonResponseLeadSource);
        }
        else
        {
            var errorContent = await responseProjectStatus.Content.ReadAsStringAsync();
            pStatus = new ApiResultResponse<ProjectStatusVM>
            {
                IsSuccess = false,
                Message = responseProjectStatus.StatusCode.ToString()
            };
        }
        // Server side Validation
        List<string> serverErrorMessageList = new List<string>();
        string serverErrorMessage = pStatus!.Message!;
        serverErrorMessageList.Add(serverErrorMessage);
        if (!pStatus!.IsSuccess)
            return Json(new { success = false, errors = serverErrorMessageList });
        else
            return Json(new { success = true });
    }

    [HttpGet]
    public async Task<IActionResult> CreateProjectStatus()
    {
        ProjectStatusVM projectStatus = new();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        return PartialView("~/Areas/Environment/Views/Project/ProjectStatuses/_Create.cshtml", projectStatus);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProjectStatus(ProjectStatusVM projectStatus)
    {
        if (!ModelState.IsValid)
        {
            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }
        ApiResultResponse<ProjectStatusVM> pStatus = new();

        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        var responseProjectStatus = await client.PostAsJsonAsync("ProjectStatus/create-projectstatus", projectStatus);
        if (responseProjectStatus.IsSuccessStatusCode)
        {
            var jsonResponseLeadSource = await responseProjectStatus.Content.ReadAsStringAsync();
            pStatus = JsonConvert.DeserializeObject<ApiResultResponse<ProjectStatusVM>>(jsonResponseLeadSource);
        }
        else
        {
            var errorContent = await responseProjectStatus.Content.ReadAsStringAsync();
            pStatus = new ApiResultResponse<ProjectStatusVM>
            {
                IsSuccess = false,
                Message = responseProjectStatus.StatusCode.ToString()
            };
        }

        // Server side Validation
        List<string> serverErrorMessageList = new List<string>();
        string serverErrorMessage = pStatus!.Message!;
        serverErrorMessageList.Add(serverErrorMessage);

        if (!pStatus!.IsSuccess)
            return Json(new { success = false, errors = serverErrorMessageList });
        else
            return Json(new { success = true });
    }

    // Project Category
    [HttpGet]
    public async Task<IActionResult> EditProjectCategory(Guid Id)
    {
        ProjectCategoryVM projCategory = new();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        var projectCategory = await client.GetFromJsonAsync<ApiResultResponse<ProjectCategoryVM>>("ProjectCategory/byid-projectcategory/?Id=" + Id);
        projCategory = projectCategory!.Data!;
        return PartialView("~/Areas/Environment/Views/Project/ProjectCategories/_Edit.cshtml", projCategory);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateProjectCategory(ProjectCategoryVM projectCategory)
    {
        if (!ModelState.IsValid)
        {
            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }
        ApiResultResponse<ProjectCategoryVM> pCategory = new();

        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        var responseCategory = await client.PutAsJsonAsync("ProjectCategory/update-projectcategory/", projectCategory);

        if (responseCategory.IsSuccessStatusCode)
        {
            var jsonResponseLeadSource = await responseCategory.Content.ReadAsStringAsync();
            pCategory = JsonConvert.DeserializeObject<ApiResultResponse<ProjectCategoryVM>>(jsonResponseLeadSource);
        }
        else
        {
            var errorContent = await responseCategory.Content.ReadAsStringAsync();
            pCategory = new ApiResultResponse<ProjectCategoryVM>
            {
                IsSuccess = false,
                Message = responseCategory.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        // Server side Validation
        List<string> serverErrorMessageList = new List<string>();
        string serverErrorMessage = pCategory!.Message!.ToString();
        serverErrorMessageList.Add(serverErrorMessage);

        if (!pCategory!.IsSuccess)
            return Json(new { success = false, errors = serverErrorMessageList });
        else
            return Json(new { success = true });
    }

    [HttpPost]
    public async Task<IActionResult> DeleteProjectCategory(Guid Id)
    {
        ApiResultResponse<ProjectCategoryVM> pCategory = new();

        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        var responseCategory = await client.DeleteAsync("ProjectCategory/delete-projectcategory?Id=" + Id);
        if (responseCategory.IsSuccessStatusCode)
        {
            var jsonResponseLeadSource = await responseCategory.Content.ReadAsStringAsync();
            pCategory = JsonConvert.DeserializeObject<ApiResultResponse<ProjectCategoryVM>>(jsonResponseLeadSource);
        }
        else
        {
            var errorContent = await responseCategory.Content.ReadAsStringAsync();
            pCategory = new ApiResultResponse<ProjectCategoryVM>
            {
                IsSuccess = false,
                Message = responseCategory.StatusCode.ToString()
            };
        }

        // Server side Validation
        List<string> serverErrorMessageList = new List<string>();
        string serverErrorMessage = pCategory!.Message!;
        serverErrorMessageList.Add(serverErrorMessage);

        if (!pCategory!.IsSuccess)
            return Json(new { success = false, errors = serverErrorMessageList });
        else
            return Json(new { success = true });
    }

    [HttpGet]
    public async Task<IActionResult> CreateProjectCategory()
    {
        ProjectCategoryVM projectCategory = new();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        return PartialView("~/Areas/Environment/Views/Project/ProjectCategories/_Create.cshtml", projectCategory);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProjectCategory(ProjectCategoryVM projectCategory)
    {
        if (!ModelState.IsValid)
        {
            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }
        ApiResultResponse<ProjectCategoryVM> pCategory = new();

        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        var responseCategory = await client.PostAsJsonAsync("ProjectCategory/create-projectcategory", projectCategory);
        if (responseCategory.IsSuccessStatusCode)
        {
            var jsonResponseLeadSource = await responseCategory.Content.ReadAsStringAsync();
            pCategory = JsonConvert.DeserializeObject<ApiResultResponse<ProjectCategoryVM>>(jsonResponseLeadSource);
        }
        else
        {
            var errorContent = await responseCategory.Content.ReadAsStringAsync();
            pCategory = new ApiResultResponse<ProjectCategoryVM>
            {
                IsSuccess = false,
                Message = responseCategory.StatusCode.ToString()
            };
        }

        // Server side Validation
        List<string> serverErrorMessageList = new List<string>();
        string serverErrorMessage = pCategory!.Message!;
        serverErrorMessageList.Add(serverErrorMessage);

        if (!pCategory!.IsSuccess)
            return Json(new { success = false, errors = serverErrorMessageList });
        else
            return Json(new { success = true });
    }
}