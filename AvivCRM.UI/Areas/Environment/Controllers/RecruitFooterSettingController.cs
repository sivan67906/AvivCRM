using System.Text;
using AvivCRM.UI.Areas.Environment.ViewModels;
using AvivCRM.UI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AvivCRM.UI.Areas.Environment.Controllers;

[Area("Environment")]
public class RecruitFooterSettingController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public RecruitFooterSettingController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    public async Task<IActionResult> RecruitFooterSetting()
    {
        // Page Title
        ViewData["pTitle"] = "RecruitFooterSettings Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "RecruitFooterSetting";
        ViewData["bChild"] = "RecruitFooterSetting";

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        ApiResultResponse<List<RecruitFooterSettingVM>>? RecruitFooterSettings =
            await client.GetFromJsonAsync<ApiResultResponse<List<RecruitFooterSettingVM>>>("RecruitFooterSetting/all-recruitfootersetting");

        List<RecruitFooterSettingVM> recruitFooterSetting = RecruitFooterSettings!.Data!;

        //List<ToggleValueVM>? toggleList = new()
        //{
        //    new ToggleValueVM
        //    {
        //        Id = new Guid("E0BB7E72-CA1A-4C2B-B531-89E720D6ABCD"), Code = "YES", Value = true
        //    },
        //    new ToggleValueVM { Id = new Guid("0BBE1696-596A-433B-ABB7-AFD60DCD826A"), Code = "NO", Value = false }
        //};

        //ToggleValueVM? toggle = new()
        //{
        //    Id = new Guid("E0BB7E72-CA1A-4C2B-B531-89E720D6ABCD"),
        //    Code = "YES",
        //    Value = true
        //};

        //foreach (var recruitFooter in recruitFooterSetting)
        //{
        //    ToggleDDSettingVM? toggleDDSetting = new()
        //    {
        //        ToggleValueVM = toggle,
        //        SelectedToggleValueId = toggle!.Id,
        //        toggleValues = toggleList?.Select(i => new ToggleValueVM { Id = i.Id, Code = i.Code, Value = i.Value }).ToList()
        //    };
        //    recruitFooter.ToggleDDSettings = toggleDDSetting;
        //}

        return PartialView("_RecruitFooterSetting", recruitFooterSetting);
    }

    #region Create Recruit Footer Setting functionionality
    /// <summary>
    /// Show the popup to create a new Recruit Footer Setting.
    /// </summary>
    /// <param name=""></param>
    /// <returns>New Recruit Footer Setting</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/RecruitFooterSetting/RecruitFooterSetting
    /// </example>
    /// <remarks> 
    /// Created: 05-Jan-2025 by Sivan T
    /// </remarks>
    //[HttpGet]
    public IActionResult Create()
    {
        RecruitFooterSettingVM recruitFooterSetting = new();

        //List<ToggleValueVM>? toggleList = new()
        //{
        //    new ToggleValueVM
        //    {
        //        Id = new Guid("E0BB7E72-CA1A-4C2B-B531-89E720D6ABCD"), TCode = "YES", TValue = true
        //    },
        //    new ToggleValueVM { Id = new Guid("0BBE1696-596A-433B-ABB7-AFD60DCD826A"), TCode = "NO", TValue = false }
        //};

        //foreach (ToggleValueVM toggleValueVM in toggleList)
        //{
        //    if (toggleValueVM.TValue) toggleValueVM.TCode = "Active";
        //    else toggleValueVM.TCode = "Inactive";
        //}

        //ToggleDDSettingVM? toggleDDSetting = new()
        //{
        //    ToggleValueVM = null,
        //    SelectedToggleValueId = Guid.Empty,
        //    toggleValues = toggleList?.Select(i => new ToggleValueVM { Id = i.Id, TCode = i.TCode, TValue = i.TValue }).ToList()
        //};

        //recruitFooterSetting.ToggleDDSettings = toggleDDSetting;
        return PartialView("_Create", recruitFooterSetting);
    }

    /// <summary>
    /// New Recruit Footer Setting will be create.
    /// </summary>
    /// <param name="recruitFooterSetting">Recruit Footer Setting entity that needs to be create</param>
    /// <returns>New Recruit Footer Setting will be create.</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/RecruitFooterSetting/RecruitFooterSetting
    /// </example>
    /// <remarks> 
    /// Created: 05-Jan-2025 by Sivan T
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Create(RecruitFooterSettingVM recruitFooterSetting)
    {
        ApiResultResponse<RecruitFooterSettingVM> resultRecruitFooterSetting = new();

        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        string? jsonRecruitFooterSetting = JsonConvert.SerializeObject(recruitFooterSetting);
        StringContent? recruitFooterSettingContent = new(jsonRecruitFooterSetting, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseRecruitFooterSetting =
            await client.PostAsync("RecruitFooterSetting/create-recruitfootersetting", recruitFooterSettingContent);

        if (responseRecruitFooterSetting.IsSuccessStatusCode)
        {
            string? jsonResponseRecruitFooterSetting = await responseRecruitFooterSetting.Content.ReadAsStringAsync();
            resultRecruitFooterSetting = JsonConvert.DeserializeObject<ApiResultResponse<RecruitFooterSettingVM>>(jsonResponseRecruitFooterSetting);
        }
        else
        {
            string? errorContent = await responseRecruitFooterSetting.Content.ReadAsStringAsync();
            resultRecruitFooterSetting = new ApiResultResponse<RecruitFooterSettingVM>
            {
                IsSuccess = false,
                Message = responseRecruitFooterSetting.StatusCode + "ErrorContent: " + errorContent
            };
        }

        if (!resultRecruitFooterSetting!.IsSuccess)
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

    #region Edit Recruit Footer Setting functionionality
    /// <summary>
    /// Edit the existing Recruit Footer Setting.
    /// </summary>
    /// <param name="Id">Recruit Footer Setting Guid that needs to be edit</param>
    /// <returns>Popup will be open to edit the recruit footer setting details</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/RecruitFooterSetting/RecruitFooterSetting
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



        //List<ToggleValueVM>? toggleList = new()
        //{
        //    new ToggleValueVM
        //    {
        //        Id = new Guid("E0BB7E72-CA1A-4C2B-B531-89E720D6ABCD"), TCode = "YES", TValue = true
        //    },
        //    new ToggleValueVM { Id = new Guid("0BBE1696-596A-433B-ABB7-AFD60DCD826A"), TCode = "NO", TValue = false }
        //};



        //foreach (ToggleValueVM toggleValueVM in toggleList)
        //{
        //    if (toggleValueVM.TValue) toggleValueVM.TCode = "Active";
        //    else toggleValueVM.TCode = "Inactive";
        //}
        //ToggleValueVM? toggle = new()
        //{
        //    Id = new Guid("E0BB7E72-CA1A-4C2B-B531-89E720D6ABCD"),
        //    TCode = "Active",
        //    TValue = true
        //};

        //ToggleDDSettingVM? toggleDDSetting = new()
        //{
        //    ToggleValueVM = toggle,
        //    SelectedToggleValueId = toggle.Id,
        //    toggleValues = toggleList?.Select(i => new ToggleValueVM { Id = i.Id, TCode = i.TCode, TValue = i.TValue }).ToList()
        //};


        ApiResultResponse<RecruitFooterSettingVM> recruitFooterSetting = new();


        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        recruitFooterSetting =
            await client.GetFromJsonAsync<ApiResultResponse<RecruitFooterSettingVM>>("RecruitFooterSetting/byid-recruitfootersetting/?Id=" + Id);

        //recruitFooterSetting!.Data!.ToggleDDSettings = toggleDDSetting;

        if (!recruitFooterSetting!.IsSuccess)
        {
            return View();
        }

        return PartialView("_Edit", recruitFooterSetting.Data);
    }

    /// <summary>
    /// Update the existing Recruit Footer Setting.
    /// </summary>
    /// <param name="recruitFooterSetting">RecruitFooterSetting entity to update the existing recruit footer setting</param>
    /// <returns>Changes will be updated for the existing recruit footer setting</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/RecruitFooterSetting/RecruitFooterSetting
    /// </example>
    /// <remarks> 
    /// Created: 05-Jan-2025 by Sivan T
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Edit(RecruitFooterSettingVM recruitFooterSetting)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        ApiResultResponse<RecruitFooterSettingVM> resultRecruitFooterSetting = new();

        if (GuidExtensions.IsNullOrEmpty(recruitFooterSetting.Id))
        {
            return View();
        }



        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        string? jsonRecruitFooterSetting = JsonConvert.SerializeObject(recruitFooterSetting);
        StringContent? recruitFooterSettingContent = new(jsonRecruitFooterSetting, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseRecruitFooterSetting =
            await client.PutAsync("RecruitFooterSetting/update-recruitfootersetting/", recruitFooterSettingContent);
        if (responseRecruitFooterSetting.IsSuccessStatusCode)
        {
            string? jsonResponseRecruitFooterSetting = await responseRecruitFooterSetting.Content.ReadAsStringAsync();
            resultRecruitFooterSetting = JsonConvert.DeserializeObject<ApiResultResponse<RecruitFooterSettingVM>>(jsonResponseRecruitFooterSetting);
        }
        else
        {
            string? errorContent = await responseRecruitFooterSetting.Content.ReadAsStringAsync();
            resultRecruitFooterSetting = new ApiResultResponse<RecruitFooterSettingVM>
            {
                IsSuccess = false,
                Message = responseRecruitFooterSetting.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        if (!resultRecruitFooterSetting!.IsSuccess)
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

    #region Delete Recruit Footer Setting functionionality
    /// <summary>
    /// Delete the existing Recruit Footer Setting.
    /// </summary>
    /// <param name="Id">Recruit Footer Setting Guid that needs to be delete</param>
    /// <returns>Recruit footer setting will be deleted</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/RecruitFooterSetting/RecruitFooterSetting
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

        ApiResultResponse<RecruitFooterSettingVM> resultRecruitFooterSetting = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? responseRecruitFooterSetting = await client.DeleteAsync("RecruitFooterSetting/delete-recruitfootersetting?Id=" + Id);
        if (responseRecruitFooterSetting.IsSuccessStatusCode)
        {
            string? jsonResponseRecruitFooterSetting = await responseRecruitFooterSetting.Content.ReadAsStringAsync();
            resultRecruitFooterSetting = JsonConvert.DeserializeObject<ApiResultResponse<RecruitFooterSettingVM>>(jsonResponseRecruitFooterSetting);
        }
        else
        {
            string? errorContent = await responseRecruitFooterSetting.Content.ReadAsStringAsync();
            resultRecruitFooterSetting = new ApiResultResponse<RecruitFooterSettingVM>
            {
                IsSuccess = false,
                Message = responseRecruitFooterSetting.StatusCode.ToString()
            };
        }

        if (!resultRecruitFooterSetting!.IsSuccess)
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