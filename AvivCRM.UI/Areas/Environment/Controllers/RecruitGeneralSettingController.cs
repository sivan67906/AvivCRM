using System.Text;
using AvivCRM.UI.Areas.Environment.ViewModels;
using AvivCRM.UI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AvivCRM.UI.Areas.Environment.Controllers;

[Area("Environment")]
public class RecruitGeneralSettingController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public RecruitGeneralSettingController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    public async Task<IActionResult> RecruitGeneralSetting()
    {
        // Page Title
        ViewData["pTitle"] = "RecruitGeneralSettings Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "RecruitGeneralSetting";
        ViewData["bChild"] = "RecruitGeneralSetting";

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        ApiResultResponse<List<RecruitGeneralSettingVM>>? RecruitGeneralSettings =
            await client.GetFromJsonAsync<ApiResultResponse<List<RecruitGeneralSettingVM>>>("RecruitGeneralSetting/all-recruitgeneralsetting");

        RecruitGeneralSettingVM? recruitGeneralSetting = RecruitGeneralSettings!.Data!.FirstOrDefault();
        List<GeneralCBSettingVM>? cbItems = recruitGeneralSetting != null
            ? JsonConvert.DeserializeObject<List<GeneralCBSettingVM>>(recruitGeneralSetting.GeneralCBJsonSettings!)
            : new List<GeneralCBSettingVM>();
        recruitGeneralSetting!.GeneralCBSettings = cbItems;

        return PartialView("_RecruitGeneralSetting", recruitGeneralSetting);
    }

    //General Settings
    [HttpPost]
    public async Task<IActionResult> RecruitGeneralSettingUpdate(RecruitGeneralSettingVM recruitGeneralSetting,
        string jsonData)
    {
        //if (recruitGeneralSetting.Id == 0)
        //{
        //    return View();
        //}

        //if (recruitGeneralSetting.GeneralCBSettings != null && recruitGeneralSetting.GeneralCBSettings.Count > 0)
        //{
        //    string jsonCBString = JsonConvert.SerializeObject(recruitGeneralSetting.GeneralCBSettings, Formatting.Indented);
        //    recruitGeneralSetting.GeneralCBJsonSettings = jsonCBString.Replace("\n", "").Replace("\r", "");
        //}

        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }
        ApiResultResponse<LeadSourceVM> source = new();
        if (GuidExtensions.IsNullOrEmpty(recruitGeneralSetting.Id))
        {
            return View();
        }

        recruitGeneralSetting.GeneralCBJsonSettings = jsonData;

        if (recruitGeneralSetting.GeneralCompanyLogoImage != null &&
            recruitGeneralSetting.GeneralCompanyLogoImage.Length > 0)
        {
            string? fileName = Path.GetFileName(recruitGeneralSetting.GeneralCompanyLogoImage.FileName);
            string? filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

            using (FileStream? stream = new(filePath, FileMode.Create))
            {
                await recruitGeneralSetting.GeneralCompanyLogoImage.CopyToAsync(stream);
            }

            recruitGeneralSetting.GeneralCompanyLogoPath = "/images/" + fileName;
            recruitGeneralSetting.GeneralCompanyLogoImageFileName = fileName;
        }

        if (recruitGeneralSetting.GeneralBGLogoImage != null && recruitGeneralSetting.GeneralBGLogoImage.Length > 0)
        {
            string? fileName = Path.GetFileName(recruitGeneralSetting.GeneralBGLogoImage.FileName);
            string? filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

            using (FileStream? stream = new(filePath, FileMode.Create))
            {
                await recruitGeneralSetting.GeneralBGLogoImage.CopyToAsync(stream);
            }

            recruitGeneralSetting.GeneralBGLogoPath = "/images/" + fileName;
            recruitGeneralSetting.GeneralBGLogoImageFileName = fileName;
        }

        //HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        //await client.PutAsJsonAsync("RecruitGeneralSetting/Update/", recruitGeneralSetting);
        //return RedirectToAction("Recruit");


        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        string? jsonRecruitGeneralSetting = JsonConvert.SerializeObject(recruitGeneralSetting);
        StringContent? recruitGeneralSettingContent = new(jsonRecruitGeneralSetting, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseGeneralSetting =
            await client.PutAsync("RecruitGeneralSetting/update-recruitgeneralsetting/", recruitGeneralSettingContent);
        if (responseGeneralSetting.IsSuccessStatusCode)
        {
            string? jsonResponseGeneralSetting = await responseGeneralSetting.Content.ReadAsStringAsync();
            source = JsonConvert.DeserializeObject<ApiResultResponse<LeadSourceVM>>(jsonResponseGeneralSetting);
        }
        else
        {
            string? errorContent = await responseGeneralSetting.Content.ReadAsStringAsync();
            source = new ApiResultResponse<LeadSourceVM>
            {
                IsSuccess = false,
                Message = responseGeneralSetting.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        if (!source!.IsSuccess)
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
