using System.Text;
using AvivCRM.UI.Areas.Environment.ViewModels;
using AvivCRM.UI.Utilities;
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

        List<RecruiterSettingVM> recruiterSetting = RecruiterSettings!.Data!;

        ApiResultResponse<List<ToggleValueVM>>? ToggleValues =
            await client.GetFromJsonAsync<ApiResultResponse<List<ToggleValueVM>>>("ToggleValue/all-togglevalue");

        List<ToggleValueVM> toggleList = ToggleValues!.Data!;

        foreach (ToggleValueVM parent in toggleList!)
        {
            foreach (RecruiterSettingVM? child in recruiterSetting.Where(c => c.RecruiterStatusId == parent.Id))
            {
                ToggleDDSettingVM? toggleDDSetting = new()
                {
                    ToggleValueVM = parent,
                    SelectedToggleValueId = child!.RecruiterStatusId,
                    toggleValues = toggleList.ToList()
                };
                child.ToggleDDSettings = toggleDDSetting;
                child.RecruiterStatusName = parent.Name;
            }
        }

        return PartialView("_RecruiterSetting", recruiterSetting);
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
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        ApiResultResponse<List<ToggleValueVM>>? ToggleValues =
            await client.GetFromJsonAsync<ApiResultResponse<List<ToggleValueVM>>>("ToggleValue/all-togglevalue");

        List<ToggleValueVM> toggleList = ToggleValues!.Data!;

        foreach (ToggleValueVM entity in toggleList)
        {
            if (entity.Name == "Yes")
            {
                entity.Name = "Enabled";
            }
            else if (entity.Name == "No")
            {
                entity.Name = "Disabled";
            }
        }
        ToggleDDSettingVM? toggleDDSetting = new()
        {
            ToggleValueVM = null,
            SelectedToggleValueId = Guid.Empty,
            toggleValues = toggleList.ToList()
        };
        recruiterSetting.ToggleDDSettings = toggleDDSetting;
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

        if (GuidExtensions.IsNullOrEmpty(recruiterSetting.RecruiterStatusId))
        {
            // Remove the default error for the "FooterStatusId" property
            ModelState.Remove(nameof(recruiterSetting.RecruiterStatusId));
            // Add a custom error message for the "FooterStatusId" property
            ModelState.AddModelError("RecruiterStatusId", "Please select a status");
        }
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