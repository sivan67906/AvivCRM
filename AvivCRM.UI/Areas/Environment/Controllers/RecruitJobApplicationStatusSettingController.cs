using System.Text;
using AvivCRM.UI.Areas.Environment.ViewModels;
using AvivCRM.UI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AvivCRM.UI.Areas.Environment.Controllers;

[Area("Environment")]
public class RecruitJobApplicationStatusSettingController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public RecruitJobApplicationStatusSettingController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    public async Task<IActionResult> RecruitJobApplicationStatusSetting()
    {
        // Page Title
        ViewData["pTitle"] = "RecruitJobApplicationStatusSettings Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "RecruitJobApplicationStatusSetting";
        ViewData["bChild"] = "RecruitJobApplicationStatusSetting";

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        ApiResultResponse<List<RecruitJobApplicationStatusSettingVM>>? recruitJobApplicationStatusSettings =
            await client.GetFromJsonAsync<ApiResultResponse<List<RecruitJobApplicationStatusSettingVM>>>("RecruitJobApplicationStatusSetting/all-recruitjobapplicationstatussetting");
        List<RecruitJobApplicationStatusSettingVM> recruitJobApplicationStatusSetting = recruitJobApplicationStatusSettings!.Data!;

        ApiResultResponse<List<JobApplicationCategoryVM>>? jobApplicationCategoryList = await client.GetFromJsonAsync<ApiResultResponse<List<JobApplicationCategoryVM>>>("JobApplicationCategory/all-jobapplicationcategory");
        List<JobApplicationCategoryVM> jobApplicationCategories = jobApplicationCategoryList!.Data!;

        ApiResultResponse<List<JobApplicationPositionVM>>? jobApplicationPositionList = await client.GetFromJsonAsync<ApiResultResponse<List<JobApplicationPositionVM>>>("JobApplicationPosition/all-jobapplicationposition");
        List<JobApplicationPositionVM> jobApplicationPositions = jobApplicationPositionList!.Data!;

        foreach (JobApplicationCategoryVM parent in jobApplicationCategories)
        {
            foreach (RecruitJobApplicationStatusSettingVM? child in recruitJobApplicationStatusSetting.Where(c => c.JobApplicationCategoryId == parent.Id))
            {
                child.JobApplicationCategoryName = parent.JACategoryName;
            }
        }
        foreach (JobApplicationPositionVM parent in jobApplicationPositions)
        {
            foreach (RecruitJobApplicationStatusSettingVM? child in recruitJobApplicationStatusSetting.Where(c => c.JobApplicationPositionId == parent.Id))
            {
                child.JobApplicationPositionName = parent.JAPositionName;
            }
        }

        return PartialView("_RecruitJobApplicationStatusSetting", recruitJobApplicationStatusSetting);
    }

    #region Create Recruit JobApplicationStatus Setting functionionality
    /// <summary>
    /// Show the popup to create a new Recruit JobApplicationStatus Setting.
    /// </summary>
    /// <param name=""></param>
    /// <returns>New Recruit JobApplicationStatus Setting</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/RecruitJobApplicationStatusSetting/RecruitJobApplicationStatusSetting
    /// </example>
    /// <remarks> 
    /// Created: 05-Jan-2025 by Sivan T
    /// </remarks>
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        RecruitJobApplicationStatusSettingVM recruitJobApplicationStatusSetting = new();
        ApiResultResponse<List<JobApplicationCategoryVM>>? jobApplicationCategoryList = await client.GetFromJsonAsync<ApiResultResponse<List<JobApplicationCategoryVM>>>("JobApplicationCategory/all-jobapplicationcategory");
        List<JobApplicationCategoryVM> jobApplicationCategories = jobApplicationCategoryList!.Data!;

        ApiResultResponse<List<JobApplicationPositionVM>>? jobApplicationPositionList = await client.GetFromJsonAsync<ApiResultResponse<List<JobApplicationPositionVM>>>("JobApplicationPosition/all-jobapplicationposition");
        List<JobApplicationPositionVM> jobApplicationPositions = jobApplicationPositionList!.Data!;

        recruitJobApplicationStatusSetting.JobApplicationCategoryDDSetting = new JobApplicationCategoryDDSettingVM
        {
            JobApplicationCategoryList = jobApplicationCategories
        };
        recruitJobApplicationStatusSetting.JobApplicationPositionDDSetting = new JobApplicationPositionDDSettingVM
        {
            JobApplicationPositionList = jobApplicationPositions
        };
        return PartialView("_Create", recruitJobApplicationStatusSetting);
    }

    /// <summary>
    /// New Recruit JobApplicationStatus Setting will be create.
    /// </summary>
    /// <param name="recruitJobApplicationStatusSetting">Recruit JobApplicationStatus Setting entity that needs to be create</param>
    /// <returns>New Recruit JobApplicationStatus Setting will be create.</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/RecruitJobApplicationStatusSetting/RecruitJobApplicationStatusSetting
    /// </example>
    /// <remarks> 
    /// Created: 05-Jan-2025 by Sivan T
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Create(RecruitJobApplicationStatusSettingVM recruitJobApplicationStatusSetting)
    {
        if (GuidExtensions.IsNullOrEmpty(recruitJobApplicationStatusSetting.JobApplicationPositionId))
        {
            ModelState.Remove(nameof(recruitJobApplicationStatusSetting.JobApplicationPositionId));
            ModelState.AddModelError("FooterStatusId", "Please select a position");
        }
        if (GuidExtensions.IsNullOrEmpty(recruitJobApplicationStatusSetting.JobApplicationCategoryId))
        {
            ModelState.Remove(nameof(recruitJobApplicationStatusSetting.JobApplicationCategoryId));
            ModelState.AddModelError("FooterStatusId", "Please select a category");
        }
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }
        ApiResultResponse<RecruitJobApplicationStatusSettingVM> resultRecruitJobApplicationStatusSetting = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        string? jsonRecruitJobApplicationStatusSetting = JsonConvert.SerializeObject(recruitJobApplicationStatusSetting);
        StringContent? recruitJobApplicationStatusSettingContent = new(jsonRecruitJobApplicationStatusSetting, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseRecruitJobApplicationStatusSetting =
            await client.PostAsync("RecruitJobApplicationStatusSetting/create-recruitjobApplicationStatussetting", recruitJobApplicationStatusSettingContent);

        if (responseRecruitJobApplicationStatusSetting.IsSuccessStatusCode)
        {
            string? jsonResponseRecruitJobApplicationStatusSetting = await responseRecruitJobApplicationStatusSetting.Content.ReadAsStringAsync();
            resultRecruitJobApplicationStatusSetting = JsonConvert.DeserializeObject<ApiResultResponse<RecruitJobApplicationStatusSettingVM>>(jsonResponseRecruitJobApplicationStatusSetting);
        }
        else
        {
            string? errorContent = await responseRecruitJobApplicationStatusSetting.Content.ReadAsStringAsync();
            resultRecruitJobApplicationStatusSetting = new ApiResultResponse<RecruitJobApplicationStatusSettingVM>
            {
                IsSuccess = false,
                Message = responseRecruitJobApplicationStatusSetting.StatusCode + "ErrorContent: " + errorContent
            };
        }

        if (!resultRecruitJobApplicationStatusSetting!.IsSuccess)
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

    #region Edit Recruit JobApplicationStatus Setting functionionality
    /// <summary>
    /// Edit the existing Recruit JobApplicationStatus Setting.
    /// </summary>
    /// <param name="Id">Recruit JobApplicationStatus Setting Guid that needs to be edit</param>
    /// <returns>Popup will be open to edit the recruit jobApplicationStatus setting details</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/RecruitJobApplicationStatusSetting/RecruitJobApplicationStatusSetting
    /// </example>
    /// <remarks> 
    /// Created: 05-Jan-2025 by Sivan T
    /// </remarks>
    [HttpGet]
    public async Task<IActionResult> Edit(Guid Id)
    {
        if (GuidExtensions.IsNullOrEmpty(Id))
        {
            return View();
        }


        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        ApiResultResponse<RecruitJobApplicationStatusSettingVM>? recruitJobApplicationStatusSettingResponse =
            await client.GetFromJsonAsync<ApiResultResponse<RecruitJobApplicationStatusSettingVM>>("RecruitJobApplicationStatusSetting/byid-recruitjobApplicationStatussetting/?Id=" + Id);
        RecruitJobApplicationStatusSettingVM recruitJobApplicationStatusSetting = recruitJobApplicationStatusSettingResponse!.Data!;

        ApiResultResponse<List<JobApplicationCategoryVM>>? jobApplicationCategoryList = await client.GetFromJsonAsync<ApiResultResponse<List<JobApplicationCategoryVM>>>("JobApplicationCategory/all-jobapplicationcategory");
        List<JobApplicationCategoryVM> jobApplicationCategories = jobApplicationCategoryList!.Data!;

        ApiResultResponse<List<JobApplicationPositionVM>>? jobApplicationPositionList = await client.GetFromJsonAsync<ApiResultResponse<List<JobApplicationPositionVM>>>("JobApplicationPosition/all-jobapplicationposition");
        List<JobApplicationPositionVM> jobApplicationPositions = jobApplicationPositionList!.Data!;

        JobApplicationCategoryDDSettingVM applicationCategoryDDSettingVM = new()
        {
            JobApplicationCategory = null,
            SelectedId = recruitJobApplicationStatusSetting.JobApplicationCategoryId,
            JobApplicationCategoryList = jobApplicationCategories
        };

        JobApplicationPositionDDSettingVM applicationPositionDDSettingVM = new()
        {
            JobApplicationPosition = null,
            SelectedId = recruitJobApplicationStatusSetting.JobApplicationPositionId,
            JobApplicationPositionList = jobApplicationPositions
        };
        recruitJobApplicationStatusSetting.JobApplicationCategoryDDSetting = applicationCategoryDDSettingVM;
        recruitJobApplicationStatusSetting.JobApplicationPositionDDSetting = applicationPositionDDSettingVM;

        return PartialView("_Edit", recruitJobApplicationStatusSetting);
    }

    /// <summary>
    /// Update the existing Recruit JobApplicationStatus Setting.
    /// </summary>
    /// <param name="recruitJobApplicationStatusSetting">RecruitJobApplicationStatusSetting entity to update the existing recruit jobApplicationStatus setting</param>
    /// <returns>Changes will be updated for the existing recruit jobApplicationStatus setting</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/RecruitJobApplicationStatusSetting/RecruitJobApplicationStatusSetting
    /// </example>
    /// <remarks> 
    /// Created: 05-Jan-2025 by Sivan T
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Edit(RecruitJobApplicationStatusSettingVM recruitJobApplicationStatusSetting)
    {
        if (GuidExtensions.IsNullOrEmpty(recruitJobApplicationStatusSetting.JobApplicationPositionId))
        {
            ModelState.Remove(nameof(recruitJobApplicationStatusSetting.JobApplicationPositionId));
            ModelState.AddModelError("FooterStatusId", "Please select a position");
        }
        if (GuidExtensions.IsNullOrEmpty(recruitJobApplicationStatusSetting.JobApplicationCategoryId))
        {
            ModelState.Remove(nameof(recruitJobApplicationStatusSetting.JobApplicationCategoryId));
            ModelState.AddModelError("FooterStatusId", "Please select a category");
        }
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        ApiResultResponse<RecruitJobApplicationStatusSettingVM> resultRecruitJobApplicationStatusSetting = new();

        if (GuidExtensions.IsNullOrEmpty(recruitJobApplicationStatusSetting.Id))
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        string? jsonRecruitJobApplicationStatusSetting = JsonConvert.SerializeObject(recruitJobApplicationStatusSetting);
        StringContent? recruitJobApplicationStatusSettingContent = new(jsonRecruitJobApplicationStatusSetting, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseRecruitJobApplicationStatusSetting =
            await client.PutAsync("RecruitJobApplicationStatusSetting/update-recruitjobApplicationStatussetting/", recruitJobApplicationStatusSettingContent);
        if (responseRecruitJobApplicationStatusSetting.IsSuccessStatusCode)
        {
            string? jsonResponseRecruitJobApplicationStatusSetting = await responseRecruitJobApplicationStatusSetting.Content.ReadAsStringAsync();
            resultRecruitJobApplicationStatusSetting = JsonConvert.DeserializeObject<ApiResultResponse<RecruitJobApplicationStatusSettingVM>>(jsonResponseRecruitJobApplicationStatusSetting);
        }
        else
        {
            string? errorContent = await responseRecruitJobApplicationStatusSetting.Content.ReadAsStringAsync();
            resultRecruitJobApplicationStatusSetting = new ApiResultResponse<RecruitJobApplicationStatusSettingVM>
            {
                IsSuccess = false,
                Message = responseRecruitJobApplicationStatusSetting.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        if (!resultRecruitJobApplicationStatusSetting!.IsSuccess)
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

    #region Delete Recruit JobApplicationStatus Setting functionionality
    /// <summary>
    /// Delete the existing Recruit JobApplicationStatus Setting.
    /// </summary>
    /// <param name="Id">Recruit JobApplicationStatus Setting Guid that needs to be delete</param>
    /// <returns>Recruit jobApplicationStatus setting will be deleted</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/RecruitJobApplicationStatusSetting/RecruitJobApplicationStatusSetting
    /// </example>
    /// <remarks> 
    /// Created: 05-Jan-2025 by Sivan T
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

        ApiResultResponse<RecruitJobApplicationStatusSettingVM> resultRecruitJobApplicationStatusSetting = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? responseRecruitJobApplicationStatusSetting = await client.DeleteAsync("RecruitJobApplicationStatusSetting/delete-recruitjobApplicationStatussetting?Id=" + Id);
        if (responseRecruitJobApplicationStatusSetting.IsSuccessStatusCode)
        {
            string? jsonResponseRecruitJobApplicationStatusSetting = await responseRecruitJobApplicationStatusSetting.Content.ReadAsStringAsync();
            resultRecruitJobApplicationStatusSetting = JsonConvert.DeserializeObject<ApiResultResponse<RecruitJobApplicationStatusSettingVM>>(jsonResponseRecruitJobApplicationStatusSetting);
        }
        else
        {
            string? errorContent = await responseRecruitJobApplicationStatusSetting.Content.ReadAsStringAsync();
            resultRecruitJobApplicationStatusSetting = new ApiResultResponse<RecruitJobApplicationStatusSettingVM>
            {
                IsSuccess = false,
                Message = responseRecruitJobApplicationStatusSetting.StatusCode.ToString()
            };
        }

        if (!resultRecruitJobApplicationStatusSetting!.IsSuccess)
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
