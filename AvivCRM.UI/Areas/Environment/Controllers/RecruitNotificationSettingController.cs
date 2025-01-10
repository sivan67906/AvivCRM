using System.Text;
using AvivCRM.UI.Areas.Environment.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AvivCRM.UI.Areas.Environment.Controllers;

[Area("Environment")]
public class RecruitNotificationSettingController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public RecruitNotificationSettingController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    public async Task<IActionResult> RecruitNotificationSetting()
    {
        // Page Title
        ViewData["pTitle"] = "RecruitNotificationSettings Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "RecruitNotificationSetting";
        ViewData["bChild"] = "RecruitNotificationSetting";

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        // Recruit Notification Settings
        ApiResultResponse<List<RecruitNotificationSettingVM>>? recruitNotificationSettings =
            await client.GetFromJsonAsync<ApiResultResponse<List<RecruitNotificationSettingVM>>>("RecruitNotificationSetting/all-recruitnotificationsetting");

        RecruitNotificationSettingVM recruitNotificationSetting = recruitNotificationSettings!.Data!.FirstOrDefault()!;

        List<CBEMailSettingVM>? cbEmailItems = recruitNotificationSetting != null
            ? JsonConvert.DeserializeObject<List<CBEMailSettingVM>>(recruitNotificationSetting.CBEMailJsonSettings!)
            : new List<CBEMailSettingVM>();

        List<CBEMailNotificationSettingVM>? cbEmailNotificationItems = recruitNotificationSetting != null
            ? JsonConvert.DeserializeObject<List<CBEMailNotificationSettingVM>>(recruitNotificationSetting.CBEMailNotificationJsonSettings!)
            : new List<CBEMailNotificationSettingVM>();

        recruitNotificationSetting!.CBEMailSettings = cbEmailItems;
        recruitNotificationSetting.CBEMailNotificationSettings = cbEmailNotificationItems;

        return PartialView("_RecruitNotificationSetting", recruitNotificationSetting);
    }

    [HttpPost]
    public async Task<IActionResult> RecruitNotificationSettingUpdate(string emailJsonData, string emailNotfnJsonData, Guid Id)
    {
        //if (!ModelState.IsValid)
        //{
        //    return Json(new
        //    {
        //        success = false,
        //        errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
        //    });
        //}

        RecruitNotificationSettingVM recruitNotificationSetting = new();
        recruitNotificationSetting.Id = Id; // Guid.Parse("E2F233CC-D34F-4FFD-8B55-08DD2EF06545");
        recruitNotificationSetting.CBEMailJsonSettings = emailJsonData;
        recruitNotificationSetting.CBEMailNotificationJsonSettings = emailNotfnJsonData;

        //HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        //await client.PutAsJsonAsync("RecruitNotificationSetting/Update/", recruitNotificationSetting);
        //return RedirectToAction("Recruit");



        //ApiResultResponse<LeadSourceVM> resultRecruitNotificationSetting = new();

        //HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        //string? jsonRecruitNotificationSetting = JsonConvert.SerializeObject(recruitNotificationSetting);
        //StringContent? recruitNotificationSettingcontent = new(jsonRecruitNotificationSetting, Encoding.UTF8, "application/json");
        //HttpResponseMessage? responseRecruitNotificationSetting =
        //    await client.PutAsync("RecruitNotificationSetting/update-recruitnotificationsetting/", recruitNotificationSettingcontent);
        //if (responseRecruitNotificationSetting.IsSuccessStatusCode)
        //{
        //    string? jsonResponseRecruitNotificationSetting = await responseRecruitNotificationSetting.Content.ReadAsStringAsync();
        //    resultRecruitNotificationSetting = JsonConvert.DeserializeObject<ApiResultResponse<LeadSourceVM>>(jsonResponseRecruitNotificationSetting);
        //}
        //else
        //{
        //    string? errorContent = await responseRecruitNotificationSetting.Content.ReadAsStringAsync();
        //    resultRecruitNotificationSetting = new ApiResultResponse<LeadSourceVM>
        //    {
        //        IsSuccess = false,
        //        Message = responseRecruitNotificationSetting.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
        //    };
        //}


        //if (!resultRecruitNotificationSetting!.IsSuccess)
        //{
        //    return Json(new
        //    {
        //        success = false,
        //        errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
        //    });
        //}

        //return Json(new { success = true });


        ApiResultResponse<RecruitNotificationSettingVM> resultRecruitNotificationSetting = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        string? jsonrecruitNotificationSetting = JsonConvert.SerializeObject(recruitNotificationSetting);
        StringContent? recruitNotificationSettingcontent = new(jsonrecruitNotificationSetting, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseRecruitNotificationSetting =
            await client.PutAsync("RecruitNotificationSetting/update-recruitnotificationsetting/", recruitNotificationSettingcontent);

        if (responseRecruitNotificationSetting.IsSuccessStatusCode)
        {
            string? jsonResponseRecruitNotificationSetting = await responseRecruitNotificationSetting.Content.ReadAsStringAsync();
            resultRecruitNotificationSetting = JsonConvert.DeserializeObject<ApiResultResponse<RecruitNotificationSettingVM>>(jsonResponseRecruitNotificationSetting);
        }
        else
        {
            string? errorContent = await responseRecruitNotificationSetting.Content.ReadAsStringAsync();
            resultRecruitNotificationSetting = new ApiResultResponse<RecruitNotificationSettingVM>
            {
                IsSuccess = false,
                Message = responseRecruitNotificationSetting.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        if (!resultRecruitNotificationSetting!.IsSuccess)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        return Json(new { success = true });
    }
}
