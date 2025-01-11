#region Namespaces
using System.Text;
using AvivCRM.UI.Areas.Environment.ViewModels;
using AvivCRM.UI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
#endregion

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class JobApplicationCategoryController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    #region Constructor
    public JobApplicationCategoryController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    #endregion

    #region Retrieves a List of Job Application Categories
    /// <summary>
    /// Retrieves a list of Job Application Categories from the database.
    /// </summary>
    /// <param name=""></param>
    /// <returns>Modal popup will open to create New Job Application Category</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/JobApplicationCategory/JobApplicationCategory
    /// </example>
    /// <remarks> 
    /// Created: 11-Jan-2025 by Sivan T
    /// </remarks>
    public async Task<IActionResult> JobApplicationCategory()
    {
        ViewData["pTitle"] = "Job Application Categories Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "Job Application Category";
        ViewData["bChild"] = "Job Application Category View";
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        ApiResultResponse<List<JobApplicationCategoryVM>> jobApplicationCategoryList = new();

        // fetch all the Job Application Categories
        jobApplicationCategoryList =
                await client.GetFromJsonAsync<ApiResultResponse<List<JobApplicationCategoryVM>>>("JobApplicationCategory/all-jobapplicationcategory");

        return View(jobApplicationCategoryList!.Data);
    }
    #endregion

    #region Create Job Application Category functionionality
    /// <summary>
    /// Show the popup to create a new Job Application Category.
    /// </summary>
    /// <param name=""></param>
    /// <returns>New Job Application Category</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/JobApplicationCategory/JobApplicationCategory
    /// </example>
    /// <remarks> 
    /// Created: 11-Jan-2025 by Sivan T
    /// </remarks>
    [HttpGet]
    public IActionResult Create()
    {
        JobApplicationCategoryVM jobApplicationCategory = new();
        return PartialView("_Create", jobApplicationCategory);
    }

    /// <summary>
    /// New Job Application Category will be create.
    /// </summary>
    /// <param name="jobApplicationCategory">Job Application Category entity that needs to be create</param>
    /// <returns>New Job Application Category will be create.</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/JobApplicationCategory/JobApplicationCategory
    /// </example>
    /// <remarks> 
    /// Created: 11-Jan-2025 by Sivan T
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Create(JobApplicationCategoryVM jobApplicationCategory)
    {
        ApiResultResponse<JobApplicationCategoryVM> resultJobApplicationCategory = new();

        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        string? jsonJobApplicationCategory = JsonConvert.SerializeObject(jobApplicationCategory);
        StringContent? jobApplicationCategoryContent = new(jsonJobApplicationCategory, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseJobApplicationCategory =
            await client.PostAsync("JobApplicationCategory/create-jobapplicationcategory", jobApplicationCategoryContent);

        if (responseJobApplicationCategory.IsSuccessStatusCode)
        {
            string? jsonResponseJobApplicationCategory = await responseJobApplicationCategory.Content.ReadAsStringAsync();
            resultJobApplicationCategory = JsonConvert.DeserializeObject<ApiResultResponse<JobApplicationCategoryVM>>(jsonResponseJobApplicationCategory);
        }
        else
        {
            string? errorContent = await responseJobApplicationCategory.Content.ReadAsStringAsync();
            resultJobApplicationCategory = new ApiResultResponse<JobApplicationCategoryVM>
            {
                IsSuccess = false,
                Message = responseJobApplicationCategory.StatusCode + "ErrorContent: " + errorContent
            };
        }

        if (!resultJobApplicationCategory!.IsSuccess)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        return Json(new { success = true });
    }

    #endregion

    #region Edit Job Application Category functionionality
    /// <summary>
    /// Edit the existing Job Application Category.
    /// </summary>
    /// <param name="Id">Job Application Category Guid that needs to be edit</param>
    /// <returns>Popup will be open to edit the job application category details</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/JobApplicationCategory/JobApplicationCategory
    /// </example>
    /// <remarks> 
    /// Created: 11-Jan-2025 by Sivan T
    /// </remarks>
    [HttpGet]
    public async Task<IActionResult> Edit(Guid Id)
    {
        if (GuidExtensions.IsNullOrEmpty(Id))
        {
            return View();
        }

        ApiResultResponse<JobApplicationCategoryVM> jobApplicationCategory = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        jobApplicationCategory =
            await client.GetFromJsonAsync<ApiResultResponse<JobApplicationCategoryVM>>("JobApplicationCategory/byid-jobapplicationcategory/?Id=" + Id);

        if (!jobApplicationCategory!.IsSuccess)
        {
            return View();
        }

        return PartialView("_Edit", jobApplicationCategory.Data);
    }

    /// <summary>
    /// Update the existing Job Application Category.
    /// </summary>
    /// <param name="jobApplicationCategory">JobApplicationCategory entity to update the existing job application category</param>
    /// <returns>Changes will be updated for the existing job application category</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/JobApplicationCategory/JobApplicationCategory
    /// </example>
    /// <remarks> 
    /// Created: 11-Jan-2025 by Sivan T
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Edit(JobApplicationCategoryVM jobApplicationCategory)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        ApiResultResponse<JobApplicationCategoryVM> resultJobApplicationCategory = new();

        if (GuidExtensions.IsNullOrEmpty(jobApplicationCategory.Id))
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        string? jsonJobApplicationCategory = JsonConvert.SerializeObject(jobApplicationCategory);
        StringContent? jobApplicationCategoryContent = new(jsonJobApplicationCategory, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseJobApplicationCategory =
            await client.PutAsync("JobApplicationCategory/update-jobapplicationcategory/", jobApplicationCategoryContent);
        if (responseJobApplicationCategory.IsSuccessStatusCode)
        {
            string? jsonResponseJobApplicationCategory = await responseJobApplicationCategory.Content.ReadAsStringAsync();
            resultJobApplicationCategory = JsonConvert.DeserializeObject<ApiResultResponse<JobApplicationCategoryVM>>(jsonResponseJobApplicationCategory);
        }
        else
        {
            string? errorContent = await responseJobApplicationCategory.Content.ReadAsStringAsync();
            resultJobApplicationCategory = new ApiResultResponse<JobApplicationCategoryVM>
            {
                IsSuccess = false,
                Message = responseJobApplicationCategory.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        if (!resultJobApplicationCategory!.IsSuccess)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        return Json(new { success = true });
    }
    #endregion

    #region Delete Job Application Category functionionality
    /// <summary>
    /// Delete the existing Job Application Category.
    /// </summary>
    /// <param name="Id">Job Application Category Guid that needs to be delete</param>
    /// <returns>Job application category will be deleted</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/JobApplicationCategory/JobApplicationCategory
    /// </example>
    /// <remarks> 
    /// Created: 11-Jan-2025 by Sivan T
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Delete(Guid Id)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        ApiResultResponse<JobApplicationCategoryVM> resultJobApplicationCategory = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? responseJobApplicationCategory = await client.DeleteAsync("JobApplicationCategory/delete-jobapplicationcategory?Id=" + Id);
        if (responseJobApplicationCategory.IsSuccessStatusCode)
        {
            string? jsonResponseJobApplicationCategory = await responseJobApplicationCategory.Content.ReadAsStringAsync();
            resultJobApplicationCategory = JsonConvert.DeserializeObject<ApiResultResponse<JobApplicationCategoryVM>>(jsonResponseJobApplicationCategory);
        }
        else
        {
            string? errorContent = await responseJobApplicationCategory.Content.ReadAsStringAsync();
            resultJobApplicationCategory = new ApiResultResponse<JobApplicationCategoryVM>
            {
                IsSuccess = false,
                Message = responseJobApplicationCategory.StatusCode.ToString()
            };
        }

        if (!resultJobApplicationCategory!.IsSuccess)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        return Json(new { success = true });
    }
    #endregion
}






















