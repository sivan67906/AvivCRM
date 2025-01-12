using System.Text;
using AvivCRM.UI.Areas.Environment.ViewModels;
using AvivCRM.UI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AvivCRM.UI.Areas.Environment.Controllers;

[Area("Environment")]
public class RecruitCustomQuestionSettingController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public RecruitCustomQuestionSettingController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    public async Task<IActionResult> RecruitCustomQuestionSetting()
    {
        // Page Title
        ViewData["pTitle"] = "RecruitCustomQuestionSettings Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "RecruitCustomQuestionSetting";
        ViewData["bChild"] = "RecruitCustomQuestionSetting";

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        ApiResultResponse<List<RecruitCustomQuestionSettingVM>>? recruitCustomQuestionSettings =
            await client.GetFromJsonAsync<ApiResultResponse<List<RecruitCustomQuestionSettingVM>>>("RecruitCustomQuestionSetting/all-recruitcustomquestionsetting");
        List<RecruitCustomQuestionSettingVM> recruitCustomQuestionSetting = recruitCustomQuestionSettings!.Data!;

        ApiResultResponse<List<CustomQuestionTypeVM>>? customQuestionTypeList = await client.GetFromJsonAsync<ApiResultResponse<List<CustomQuestionTypeVM>>>("CustomQuestionType/all-customquestiontype");
        List<CustomQuestionTypeVM> customQuestionTypes = customQuestionTypeList!.Data!;

        ApiResultResponse<List<CustomQuestionCategoryVM>>? customQuestionCategoryList = await client.GetFromJsonAsync<ApiResultResponse<List<CustomQuestionCategoryVM>>>("CustomQuestionCategory/all-customquestioncategory");
        List<CustomQuestionCategoryVM> customQuestionCategories = customQuestionCategoryList!.Data!;

        ApiResultResponse<List<ToggleValueVM>>? toggleList = await client.GetFromJsonAsync<ApiResultResponse<List<ToggleValueVM>>>("ToggleValue/all-togglevalue");
        List<ToggleValueVM> toggles = toggleList!.Data!;

        foreach (CustomQuestionTypeVM parent in customQuestionTypes)
        {
            foreach (RecruitCustomQuestionSettingVM? child in recruitCustomQuestionSetting.Where(c => c.CustomQuestionTypeId == parent.Id))
            {
                child.CustomQuestionTypeName = parent.CQTypeName;
            }
        }
        foreach (CustomQuestionCategoryVM parent in customQuestionCategories)
        {
            foreach (RecruitCustomQuestionSettingVM? child in recruitCustomQuestionSetting.Where(c => c.CustomQuestionCategoryId == parent.Id))
            {
                child.CustomQuestionCategoryName = parent.CQCategoryName;
            }
        }
        foreach (ToggleValueVM parent in toggles)
        {
            foreach (RecruitCustomQuestionSettingVM? child in recruitCustomQuestionSetting.Where(c => c.CQStatusId == parent.Id))
            {
                if (parent.Name == "Yes")
                {
                    child.CQStatusName = "Enabled";
                }
                else
                {
                    child.CQStatusName = "Disabled";
                }
            }
        }

        return PartialView("_RecruitCustomQuestionSetting", recruitCustomQuestionSetting);
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

        RecruitCustomQuestionSettingVM recruitCustomQuestionSetting = new();

        ApiResultResponse<List<CustomQuestionTypeVM>>? customQuestionTypeList = await client.GetFromJsonAsync<ApiResultResponse<List<CustomQuestionTypeVM>>>("CustomQuestionType/all-customquestiontype");
        List<CustomQuestionTypeVM> customQuestionTypes = customQuestionTypeList!.Data!;

        ApiResultResponse<List<CustomQuestionCategoryVM>>? customQuestionCategoryList = await client.GetFromJsonAsync<ApiResultResponse<List<CustomQuestionCategoryVM>>>("CustomQuestionCategory/all-customquestioncategory");
        List<CustomQuestionCategoryVM> customQuestionCategories = customQuestionCategoryList!.Data!;

        ApiResultResponse<List<ToggleValueVM>>? toggleList = await client.GetFromJsonAsync<ApiResultResponse<List<ToggleValueVM>>>("ToggleValue/all-togglevalue");
        List<ToggleValueVM> toggles = toggleList!.Data!;
        foreach (ToggleValueVM parent in toggles)
        {
            if (parent.Name == "Yes") { parent.Name = "Enable"; }
            else
            {
                parent.Name = "Disable";
            }
        }

        recruitCustomQuestionSetting.CustomQuestionCategoryDDSetting = new();
        recruitCustomQuestionSetting.CustomQuestionTypeDDSetting = new();
        recruitCustomQuestionSetting.ToggleDDSetting = new();

        recruitCustomQuestionSetting.CustomQuestionCategoryDDSetting!.CustomQuestionCategoryList = new List<CustomQuestionCategoryVM>(customQuestionCategories);
        recruitCustomQuestionSetting.CustomQuestionTypeDDSetting!.CustomQuestionTypeList = new List<CustomQuestionTypeVM>(customQuestionTypes);
        recruitCustomQuestionSetting.ToggleDDSetting!.toggleValues = new List<ToggleValueVM>(toggles);
        //recruitCustomQuestionSetting.CQIsRequiredId = true;
        return PartialView("_Create", recruitCustomQuestionSetting);
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
    public async Task<IActionResult> Create(RecruitCustomQuestionSettingVM recruitCustomQuestionSetting)
    {
        ApiResultResponse<RecruitCustomQuestionSettingVM>? resultRecruitCustomQuestionSetting = new();

        //if (!ModelState.IsValid)
        //{
        //    return Json(new
        //    {
        //        success = false,
        //        errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
        //    });
        //}

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        string? jsonRecruitCustomQuestionSetting = JsonConvert.SerializeObject(recruitCustomQuestionSetting);
        StringContent? recruitRecruitCustomQuestionSettingContent = new(jsonRecruitCustomQuestionSetting, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseRecruitCustomQuestionSetting =
            await client.PostAsync("RecruitCustomQuestionSetting/create-recruitcustomquestionsetting", recruitRecruitCustomQuestionSettingContent);

        if (responseRecruitCustomQuestionSetting.IsSuccessStatusCode)
        {
            string? jsonResponsejsonRecruitCustomQuestionSetting = await responseRecruitCustomQuestionSetting.Content.ReadAsStringAsync();
            resultRecruitCustomQuestionSetting = JsonConvert.DeserializeObject<ApiResultResponse<RecruitCustomQuestionSettingVM>>(jsonResponsejsonRecruitCustomQuestionSetting);
        }
        else
        {
            string? errorContent = await responseRecruitCustomQuestionSetting.Content.ReadAsStringAsync();
            resultRecruitCustomQuestionSetting = new ApiResultResponse<RecruitCustomQuestionSettingVM>
            {
                IsSuccess = false,
                Message = responseRecruitCustomQuestionSetting.StatusCode + "ErrorContent: " + errorContent
            };
        }

        if (!resultRecruitCustomQuestionSetting!.IsSuccess)
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
        ApiResultResponse<RecruitCustomQuestionSettingVM>? recruitCustomQuestionSettingResponse =
            await client.GetFromJsonAsync<ApiResultResponse<RecruitCustomQuestionSettingVM>>("RecruitCustomQuestionSetting/byid-recruitcustomquestionsetting/?Id=" + Id);
        RecruitCustomQuestionSettingVM recruitCustomQuestionSetting = recruitCustomQuestionSettingResponse!.Data!;

        ApiResultResponse<List<CustomQuestionTypeVM>>? customQuestionTypeList = await client.GetFromJsonAsync<ApiResultResponse<List<CustomQuestionTypeVM>>>("CustomQuestionType/all-customquestiontype");
        List<CustomQuestionTypeVM> customQuestionTypes = customQuestionTypeList!.Data!;

        ApiResultResponse<List<CustomQuestionCategoryVM>>? customQuestionCategoryList = await client.GetFromJsonAsync<ApiResultResponse<List<CustomQuestionCategoryVM>>>("CustomQuestionCategory/all-customquestioncategory");
        List<CustomQuestionCategoryVM> customQuestionCategories = customQuestionCategoryList!.Data!;

        ApiResultResponse<List<ToggleValueVM>>? toggleList = await client.GetFromJsonAsync<ApiResultResponse<List<ToggleValueVM>>>("ToggleValue/all-togglevalue");
        List<ToggleValueVM> toggles = toggleList!.Data!;
        foreach (ToggleValueVM parent in toggles)
        {
            if (parent.Name == "Yes") { parent.Name = "Enable"; }
            else
            {
                parent.Name = "Disable";
            }
        }

        CustomQuestionCategoryDDSettingVM customQuestionCategoryDDSetting = new()
        {
            CustomQuestionCategory = null,
            SelectedId = recruitCustomQuestionSetting.CustomQuestionCategoryId,
            CustomQuestionCategoryList = customQuestionCategories
        };

        CustomQuestionTypeDDSettingVM customQuestionTypeDDSetting = new()
        {
            CustomQuestionType = null,
            SelectedId = recruitCustomQuestionSetting.CustomQuestionTypeId,
            CustomQuestionTypeList = customQuestionTypes
        };
        ToggleDDSettingVM toggleDDSetting = new()
        {
            ToggleValueVM = null,
            SelectedToggleValueId = recruitCustomQuestionSetting.CQStatusId,
            toggleValues = toggles
        };
        recruitCustomQuestionSetting.CustomQuestionCategoryDDSetting = customQuestionCategoryDDSetting;
        recruitCustomQuestionSetting.CustomQuestionTypeDDSetting = customQuestionTypeDDSetting;
        recruitCustomQuestionSetting.ToggleDDSetting = toggleDDSetting;

        return PartialView("_Edit", recruitCustomQuestionSetting);
    }

    /// <summary>
    /// Update the existing Recruit JobApplicationStatus Setting.
    /// </summary>
    /// <param name="recruitCustomQuestionSetting">RecruitCustomQuestionSetting entity to update the existing recruit jobApplicationStatus setting</param>
    /// <returns>Changes will be updated for the existing recruit jobApplicationStatus setting</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/RecruitCustomQuestionSetting/RecruitCustomQuestionSetting
    /// </example>
    /// <remarks> 
    /// Created: 05-Jan-2025 by Sivan T
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Edit(RecruitCustomQuestionSettingVM recruitCustomQuestionSetting)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        ApiResultResponse<RecruitCustomQuestionSettingVM> resultRecruitCustomQuestionSetting = new();

        if (GuidExtensions.IsNullOrEmpty(recruitCustomQuestionSetting.Id))
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        string? jsonRecruitCustomQuestionSetting = JsonConvert.SerializeObject(recruitCustomQuestionSetting);
        StringContent? recruitCustomQuestionSettingContent = new(jsonRecruitCustomQuestionSetting, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseRecruitCustomQuestionSetting =
            await client.PutAsync("RecruitCustomQuestionSetting/update-recruitcustomquestionsetting/", recruitCustomQuestionSettingContent);
        if (responseRecruitCustomQuestionSetting.IsSuccessStatusCode)
        {
            string? jsonResponseRecruitCustomQuestionSetting = await responseRecruitCustomQuestionSetting.Content.ReadAsStringAsync();
            resultRecruitCustomQuestionSetting = JsonConvert.DeserializeObject<ApiResultResponse<RecruitCustomQuestionSettingVM>>(jsonResponseRecruitCustomQuestionSetting);
        }
        else
        {
            string? errorContent = await responseRecruitCustomQuestionSetting.Content.ReadAsStringAsync();
            resultRecruitCustomQuestionSetting = new ApiResultResponse<RecruitCustomQuestionSettingVM>
            {
                IsSuccess = false,
                Message = responseRecruitCustomQuestionSetting.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        if (!resultRecruitCustomQuestionSetting!.IsSuccess)
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
    /// POST /Environment/RecruitCustomQuestionSetting/RecruitCustomQuestionSetting
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

        ApiResultResponse<RecruitCustomQuestionSettingVM> resultRecruitCustomQuestionSetting = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? responseRecruitCustomQuestionSetting = await client.DeleteAsync("RecruitCustomQuestionSetting/delete-recruitcustomquestionsetting?Id=" + Id);
        if (responseRecruitCustomQuestionSetting.IsSuccessStatusCode)
        {
            string? jsonResponseRecruitCustomQuestionSetting = await responseRecruitCustomQuestionSetting.Content.ReadAsStringAsync();
            resultRecruitCustomQuestionSetting = JsonConvert.DeserializeObject<ApiResultResponse<RecruitCustomQuestionSettingVM>>(jsonResponseRecruitCustomQuestionSetting);
        }
        else
        {
            string? errorContent = await responseRecruitCustomQuestionSetting.Content.ReadAsStringAsync();
            resultRecruitCustomQuestionSetting = new ApiResultResponse<RecruitCustomQuestionSettingVM>
            {
                IsSuccess = false,
                Message = responseRecruitCustomQuestionSetting.StatusCode.ToString()
            };
        }

        if (!resultRecruitCustomQuestionSetting!.IsSuccess)
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
