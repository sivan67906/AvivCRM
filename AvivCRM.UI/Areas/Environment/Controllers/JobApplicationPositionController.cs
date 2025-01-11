#region Namespaces
using System.Text;
using AvivCRM.UI.Areas.Environment.ViewModels;
using AvivCRM.UI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
#endregion

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class JobApplicationPositionController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    #region Constructor
    public JobApplicationPositionController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    #endregion

    #region Retrieves a List of Job Application Positions
    /// <summary>
    /// Retrieves a list of Job Application Positions from the database.
    /// </summary>
    /// <param name=""></param>
    /// <returns>Modal popup will open to create New Job Application Position</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/JobApplicationPosition/JobApplicationPosition
    /// </example>
    /// <remarks> 
    /// Created: 10-Jan-2025 by Sivan T
    /// </remarks>
    public async Task<IActionResult> JobApplicationPosition()
    {
        ViewData["pTitle"] = "Job Application Positions Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "Job Application Position";
        ViewData["bChild"] = "Job Application Position View";
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        ApiResultResponse<List<JobApplicationPositionVM>> jobApplicationPositionList = new();

        // fetch all the Job Application Positions
        jobApplicationPositionList =
                await client.GetFromJsonAsync<ApiResultResponse<List<JobApplicationPositionVM>>>("JobApplicationPosition/all-jobapplicationposition");

        return View(jobApplicationPositionList!.Data);
    }
    #endregion

    #region Create Job Application Position functionionality
    /// <summary>
    /// Show the popup to create a new Job Application Position.
    /// </summary>
    /// <param name=""></param>
    /// <returns>New Job Application Position</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/JobApplicationPosition/JobApplicationPosition
    /// </example>
    /// <remarks> 
    /// Created: 10-Jan-2025 by Sivan T
    /// </remarks>
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        JobApplicationPositionVM jobApplicationPosition = new();
        return PartialView("_Create", jobApplicationPosition);
    }

    /// <summary>
    /// New Job Application Position will be create.
    /// </summary>
    /// <param name="jobApplicationPosition">Job Application Position entity that needs to be create</param>
    /// <returns>New Job Application Position will be create.</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/JobApplicationPosition/JobApplicationPosition
    /// </example>
    /// <remarks> 
    /// Created: 10-Jan-2025 by Sivan T
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Create(JobApplicationPositionVM jobApplicationPosition)
    {
        ApiResultResponse<JobApplicationPositionVM> resultJobApplicationPosition = new();

        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        string? jsonJobApplicationPosition = JsonConvert.SerializeObject(jobApplicationPosition);
        StringContent? jobApplicationPositionContent = new(jsonJobApplicationPosition, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseJobApplicationPosition =
            await client.PostAsync("JobApplicationPosition/create-jobapplicationposition", jobApplicationPositionContent);

        if (responseJobApplicationPosition.IsSuccessStatusCode)
        {
            string? jsonResponseJobApplicationPosition = await responseJobApplicationPosition.Content.ReadAsStringAsync();
            resultJobApplicationPosition = JsonConvert.DeserializeObject<ApiResultResponse<JobApplicationPositionVM>>(jsonResponseJobApplicationPosition);
        }
        else
        {
            string? errorContent = await responseJobApplicationPosition.Content.ReadAsStringAsync();
            resultJobApplicationPosition = new ApiResultResponse<JobApplicationPositionVM>
            {
                IsSuccess = false,
                Message = responseJobApplicationPosition.StatusCode + "ErrorContent: " + errorContent
            };
        }

        if (!resultJobApplicationPosition!.IsSuccess)
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

    #region Edit Job Application Position functionionality
    /// <summary>
    /// Edit the existing Job Application Position.
    /// </summary>
    /// <param name="Id">Job Application Position Guid that needs to be edit</param>
    /// <returns>Popup will be open to edit the job application position details</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/JobApplicationPosition/JobApplicationPosition
    /// </example>
    /// <remarks> 
    /// Created: 10-Jan-2025 by Sivan T
    /// </remarks>
    [HttpGet]
    public async Task<IActionResult> Edit(Guid Id)
    {
        if (GuidExtensions.IsNullOrEmpty(Id))
        {
            return View();
        }

        ApiResultResponse<JobApplicationPositionVM> jobApplicationPosition = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        jobApplicationPosition =
            await client.GetFromJsonAsync<ApiResultResponse<JobApplicationPositionVM>>("JobApplicationPosition/byid-jobapplicationposition/?Id=" + Id);

        if (!jobApplicationPosition!.IsSuccess)
        {
            return View();
        }

        return PartialView("_Edit", jobApplicationPosition.Data);
    }

    /// <summary>
    /// Update the existing Job Application Position.
    /// </summary>
    /// <param name="jobApplicationPosition">JobApplicationPosition entity to update the existing job application position</param>
    /// <returns>Changes will be updated for the existing job application position</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/JobApplicationPosition/JobApplicationPosition
    /// </example>
    /// <remarks> 
    /// Created: 10-Jan-2025 by Sivan T
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Edit(JobApplicationPositionVM jobApplicationPosition)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }


        ApiResultResponse<JobApplicationPositionVM> resultJobApplicationPosition = new();

        if (GuidExtensions.IsNullOrEmpty(jobApplicationPosition.Id))
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        string? jsonJobApplicationPosition = JsonConvert.SerializeObject(jobApplicationPosition);
        StringContent? jobApplicationPositionContent = new(jsonJobApplicationPosition, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseJobApplicationPosition =
            await client.PutAsync("JobApplicationPosition/update-jobapplicationposition/", jobApplicationPositionContent);
        if (responseJobApplicationPosition.IsSuccessStatusCode)
        {
            string? jsonResponseJobApplicationPosition = await responseJobApplicationPosition.Content.ReadAsStringAsync();
            resultJobApplicationPosition = JsonConvert.DeserializeObject<ApiResultResponse<JobApplicationPositionVM>>(jsonResponseJobApplicationPosition);
        }
        else
        {
            string? errorContent = await responseJobApplicationPosition.Content.ReadAsStringAsync();
            resultJobApplicationPosition = new ApiResultResponse<JobApplicationPositionVM>
            {
                IsSuccess = false,
                Message = responseJobApplicationPosition.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        if (!resultJobApplicationPosition!.IsSuccess)
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

    #region Delete Job Application Position functionionality
    /// <summary>
    /// Delete the existing Job Application Position.
    /// </summary>
    /// <param name="Id">Job Application Position Guid that needs to be delete</param>
    /// <returns>Job application position will be deleted</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/JobApplicationPosition/JobApplicationPosition
    /// </example>
    /// <remarks> 
    /// Created: 10-Jan-2025 by Sivan T
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

        ApiResultResponse<JobApplicationPositionVM> resultJobApplicationPosition = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? responseJobApplicationPosition = await client.DeleteAsync("JobApplicationPosition/delete-jobapplicationposition?Id=" + Id);
        if (responseJobApplicationPosition.IsSuccessStatusCode)
        {
            string? jsonResponseJobApplicationPosition = await responseJobApplicationPosition.Content.ReadAsStringAsync();
            resultJobApplicationPosition = JsonConvert.DeserializeObject<ApiResultResponse<JobApplicationPositionVM>>(jsonResponseJobApplicationPosition);
        }
        else
        {
            string? errorContent = await responseJobApplicationPosition.Content.ReadAsStringAsync();
            resultJobApplicationPosition = new ApiResultResponse<JobApplicationPositionVM>
            {
                IsSuccess = false,
                Message = responseJobApplicationPosition.StatusCode.ToString()
            };
        }

        if (!resultJobApplicationPosition!.IsSuccess)
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






















