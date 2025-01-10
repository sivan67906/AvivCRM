using System.Text;
using AvivCRM.UI.Areas.Environment.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AvivCRM.UI.Areas.Environment.Controllers;

[Area("Environment")]
public class RecruiterSettingController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public RecruiterSettingController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    public async Task<IActionResult> RecruiterSetting()
    {
        // Page Title
        ViewData["pTitle"] = "RecruiterSettings Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "RecruiterSetting";
        ViewData["bChild"] = "RecruiterSetting";

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        ApiResultResponse<List<RecruiterSettingVM>>? RecruiterSettings =
            await client.GetFromJsonAsync<ApiResultResponse<List<RecruiterSettingVM>>>("RecruiterSetting/all-recruitersetting");

        return PartialView("_RecruiterSetting", RecruiterSettings!.Data!);
    }
    #region Create Recruiter Setting functionionality
    /// <summary>
    /// Show the popup to create a new Recruiter Setting.
    /// </summary>
    /// <param name=""></param>
    /// <returns>New Recruiter Setting</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/RecruiterSetting/RecruiterSetting
    /// </example>
    /// <remarks> 
    /// Created: 05-Jan-2025 by Sivan T
    /// </remarks>
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        RecruiterSettingVM recruiterSetting = new();
        return PartialView("_Create", recruiterSetting);
    }

    /// <summary>
    /// New Recruiter Setting will be create.
    /// </summary>
    /// <param name="recruiterSetting">Recruiter Setting entity that needs to be create</param>
    /// <returns>New Recruiter Setting will be create.</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/RecruiterSetting/RecruiterSetting
    /// </example>
    /// <remarks> 
    /// Created: 05-Jan-2025 by Sivan T
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Create(RecruiterSettingVM recruiterSetting)
    {
        ApiResultResponse<RecruiterSettingVM> resultRecruiterSetting = new();

        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        string? jsonRecruiterSetting = JsonConvert.SerializeObject(recruiterSetting);
        StringContent? recruiterSettingContent = new(jsonRecruiterSetting, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseRecruiterSetting =
            await client.PostAsync("RecruiterSetting/create-recruitersetting", recruiterSettingContent);

        if (responseRecruiterSetting.IsSuccessStatusCode)
        {
            string? jsonResponseRecruiterSetting = await responseRecruiterSetting.Content.ReadAsStringAsync();
            resultRecruiterSetting = JsonConvert.DeserializeObject<ApiResultResponse<RecruiterSettingVM>>(jsonResponseRecruiterSetting);
        }
        else
        {
            string? errorContent = await responseRecruiterSetting.Content.ReadAsStringAsync();
            resultRecruiterSetting = new ApiResultResponse<RecruiterSettingVM>
            {
                IsSuccess = false,
                Message = responseRecruiterSetting.StatusCode + "ErrorContent: " + errorContent
            };
        }

        if (!resultRecruiterSetting!.IsSuccess)
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

    #region Delete Recruiter Setting functionionality
    /// <summary>
    /// Delete the existing Recruiter Setting.
    /// </summary>
    /// <param name="Id">Recruiter Setting Guid that needs to be delete</param>
    /// <returns>Recruiter setting will be deleted</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/RecruiterSetting/RecruiterSetting
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

        ApiResultResponse<RecruiterSettingVM> resultRecruiterSetting = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? responseRecruiterSetting = await client.DeleteAsync("RecruiterSetting/delete-recruitersetting?Id=" + Id);
        if (responseRecruiterSetting.IsSuccessStatusCode)
        {
            string? jsonResponseRecruiterSetting = await responseRecruiterSetting.Content.ReadAsStringAsync();
            resultRecruiterSetting = JsonConvert.DeserializeObject<ApiResultResponse<RecruiterSettingVM>>(jsonResponseRecruiterSetting);
        }
        else
        {
            string? errorContent = await responseRecruiterSetting.Content.ReadAsStringAsync();
            resultRecruiterSetting = new ApiResultResponse<RecruiterSettingVM>
            {
                IsSuccess = false,
                Message = responseRecruiterSetting.StatusCode.ToString()
            };
        }

        if (!resultRecruiterSetting!.IsSuccess)
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