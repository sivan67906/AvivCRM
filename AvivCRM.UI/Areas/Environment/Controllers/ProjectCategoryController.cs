using AvivCRM.UI.Areas.Environment.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AvivCRM.UI.Areas.Environment.Controllers;
public class ProjectCategoryController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ProjectCategoryController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    public IActionResult Index()
    {
        return View();
    }

    // Project Category
    [HttpGet]
    public async Task<IActionResult> EditProjectCategory(Guid Id)
    {
        ProjectCategoryVM projCategory = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        ApiResultResponse<ProjectCategoryVM>? projectCategory =
            await client.GetFromJsonAsync<ApiResultResponse<ProjectCategoryVM>>(
                "ProjectCategory/byid-projectcategory/?Id=" + Id);
        projCategory = projectCategory!.Data!;
        return PartialView("~/Areas/Environment/Views/Project/ProjectCategories/_Edit.cshtml", projCategory);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateProjectCategory(ProjectCategoryVM projectCategory)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        ApiResultResponse<ProjectCategoryVM> pCategory = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? responseCategory =
            await client.PutAsJsonAsync("ProjectCategory/update-projectcategory/", projectCategory);

        if (responseCategory.IsSuccessStatusCode)
        {
            string? jsonResponseLeadSource = await responseCategory.Content.ReadAsStringAsync();
            pCategory = JsonConvert.DeserializeObject<ApiResultResponse<ProjectCategoryVM>>(jsonResponseLeadSource);
        }
        else
        {
            string? errorContent = await responseCategory.Content.ReadAsStringAsync();
            pCategory = new ApiResultResponse<ProjectCategoryVM>
            {
                IsSuccess = false,
                Message = responseCategory.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        // Server side Validation
        List<string> serverErrorMessageList = [];
        string serverErrorMessage = pCategory!.Message!;
        serverErrorMessageList.Add(serverErrorMessage);

        if (!pCategory!.IsSuccess)
        {
            return Json(new { success = false, errors = serverErrorMessageList });
        }

        return Json(new { success = true });
    }

    [HttpPost]
    public async Task<IActionResult> DeleteProjectCategory(Guid Id)
    {
        ApiResultResponse<ProjectCategoryVM> pCategory = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? responseCategory =
            await client.DeleteAsync("ProjectCategory/delete-projectcategory?Id=" + Id);
        if (responseCategory.IsSuccessStatusCode)
        {
            string? jsonResponseLeadSource = await responseCategory.Content.ReadAsStringAsync();
            pCategory = JsonConvert.DeserializeObject<ApiResultResponse<ProjectCategoryVM>>(jsonResponseLeadSource);
        }
        else
        {
            string? errorContent = await responseCategory.Content.ReadAsStringAsync();
            pCategory = new ApiResultResponse<ProjectCategoryVM>
            {
                IsSuccess = false,
                Message = responseCategory.StatusCode.ToString()
            };
        }

        // Server side Validation
        List<string> serverErrorMessageList = [];
        string serverErrorMessage = pCategory!.Message!;
        serverErrorMessageList.Add(serverErrorMessage);

        if (!pCategory!.IsSuccess)
        {
            return Json(new { success = false, errors = serverErrorMessageList });
        }

        return Json(new { success = true });
    }

    [HttpGet]
    public IActionResult CreateProjectCategory()
    {
        ProjectCategoryVM projectCategory = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        return PartialView("~/Areas/Environment/Views/Project/ProjectCategories/_Create.cshtml", projectCategory);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProjectCategory(ProjectCategoryVM projectCategory)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        ApiResultResponse<ProjectCategoryVM> pCategory = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? responseCategory =
            await client.PostAsJsonAsync("ProjectCategory/create-projectcategory", projectCategory);
        if (responseCategory.IsSuccessStatusCode)
        {
            string? jsonResponseLeadSource = await responseCategory.Content.ReadAsStringAsync();
            pCategory = JsonConvert.DeserializeObject<ApiResultResponse<ProjectCategoryVM>>(jsonResponseLeadSource);
        }
        else
        {
            string? errorContent = await responseCategory.Content.ReadAsStringAsync();
            pCategory = new ApiResultResponse<ProjectCategoryVM>
            {
                IsSuccess = false,
                Message = responseCategory.StatusCode.ToString()
            };
        }

        // Server side Validation
        List<string> serverErrorMessageList = [];
        string serverErrorMessage = pCategory!.Message!;
        serverErrorMessageList.Add(serverErrorMessage);

        if (!pCategory!.IsSuccess)
        {
            return Json(new { success = false, errors = serverErrorMessageList });
        }

        return Json(new { success = true });
    }
}