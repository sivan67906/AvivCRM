using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using AvivCRM.UI.Areas.Environment.ViewModels;

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class RecruitController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public RecruitController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    public async Task<IActionResult> Index()
    {
        return View();
    }
    public async Task<IActionResult> Recruit()
    {
        // Page Title
        ViewData["pTitle"] = "Recruits Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Configuration";
        ViewData["bParent"] = "Recruit";
        ViewData["bChild"] = "Recruit";

        var client = _httpClientFactory.CreateClient("ApiGatewayCall");

        // General Settings
        var recruitGeneralSettings = await client.GetFromJsonAsync<List<RecruitGeneralSettingVM>>("RecruitGeneralSetting/GetAll");
        var recruitGeneralSetting = recruitGeneralSettings?.FirstOrDefault();
        var cbItems = recruitGeneralSetting != null ? JsonConvert.DeserializeObject<List<GeneralCBSettingVM>>(recruitGeneralSetting.GeneralCBJsonSettings) : new List<GeneralCBSettingVM>();
        recruitGeneralSetting.GeneralCBSettings = cbItems;

        var recruitFooterSettings = await client.GetFromJsonAsync<List<RecruitFooterSettingVM>>("RecruitFooterSetting/GetAll");
        var recruiterSettings = await client.GetFromJsonAsync<List<RecruiterSettingVM>>("RecruiterSetting/GetAll");

        // Recruit Notification Settings
        var recruitNotificationSettings = await client.GetFromJsonAsync<List<RecruitNotificationSettingVM>>("RecruitNotificationSetting/GetAll");
        var recruitNotificationSetting = recruitNotificationSettings?.FirstOrDefault();
        var cbEmailItems = recruitNotificationSetting != null ? JsonConvert.DeserializeObject<List<CBEMailSettingVM>>(recruitNotificationSetting.CBEMailJsonSettings) : new List<CBEMailSettingVM>();
        var cbEmailNotificationItems = recruitNotificationSetting != null ? JsonConvert.DeserializeObject<List<CBEMailNotificationSettingVM>>(recruitNotificationSetting.CBEMailNotificationJsonSettings) : new List<CBEMailNotificationSettingVM>();
        recruitNotificationSetting.CBEMailSettings = cbEmailItems;
        recruitNotificationSetting.CBEMailNotificationSettings = cbEmailNotificationItems;

        var recruitJobApplicationStatusSettings = await client.GetFromJsonAsync<List<RecruitJobApplicationStatusSettingVM>>("RecruitJobApplicationStatusSetting/GetAll");
        var recruitCustomQuestionSettings = await client.GetFromJsonAsync<List<RecruitCustomQuestionSettingVM>>("RecruitCustomQuestionSetting/GetAll");
        var viewModel = new RecruitVM
        {
            RecruitGeneralSettingVMList = recruitGeneralSetting,
            RecruitFooterSettingVMList = recruitFooterSettings,
            RecruiterSettingVMList = recruiterSettings,
            RecruitNotificationSettingVMList = recruitNotificationSetting,
            RecruitJobApplicationStatusSettingVMList = recruitJobApplicationStatusSettings,
            RecruitCustomQuestionSettingVMList = recruitCustomQuestionSettings
        };
        return View(viewModel);
    }

    //General Settings
    [HttpPost]
    public async Task<IActionResult> RecruitGeneralSettingUpdate(RecruitGeneralSettingVM recruitGeneralSetting, string jsonData)
    {
        if (recruitGeneralSetting.Id == 0) return View();
        //if (recruitGeneralSetting.GeneralCBSettings != null && recruitGeneralSetting.GeneralCBSettings.Count > 0)
        //{
        //    string jsonCBString = JsonConvert.SerializeObject(recruitGeneralSetting.GeneralCBSettings, Formatting.Indented);
        //    recruitGeneralSetting.GeneralCBJsonSettings = jsonCBString.Replace("\n", "").Replace("\r", "");
        //}
        recruitGeneralSetting.GeneralCBJsonSettings = jsonData;

        if (recruitGeneralSetting.GeneralCompanyLogoImage != null && recruitGeneralSetting.GeneralCompanyLogoImage.Length > 0)
        {
            var fileName = Path.GetFileName(recruitGeneralSetting.GeneralCompanyLogoImage.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await recruitGeneralSetting.GeneralCompanyLogoImage.CopyToAsync(stream);
            }

            recruitGeneralSetting.GeneralCompanyLogoPath = "/images/" + fileName;
            recruitGeneralSetting.GeneralCompanyLogoImageFileName = fileName;
        }
        if (recruitGeneralSetting.GeneralBGLogoImage != null && recruitGeneralSetting.GeneralBGLogoImage.Length > 0)
        {
            var fileName = Path.GetFileName(recruitGeneralSetting.GeneralBGLogoImage.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await recruitGeneralSetting.GeneralBGLogoImage.CopyToAsync(stream);
            }

            recruitGeneralSetting.GeneralBGLogoPath = "/images/" + fileName;
            recruitGeneralSetting.GeneralBGLogoImageFileName = fileName;
        }

        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PutAsJsonAsync("RecruitGeneralSetting/Update/", recruitGeneralSetting);
        return RedirectToAction("Recruit");
    }

    //Footer Settings
    [HttpGet]
    public async Task<IActionResult> EditRecruitFooterSetting(int Id)
    {
        if (Id == 0) return View();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        var recruitFooterSetting = await client.GetFromJsonAsync<RecruitFooterSettingVM>("RecruitFooterSetting/GetById/?Id=" + Id);
        return PartialView("~/Areas/Settings/Views/Recruit/RecruitFooterSetting/_Edit.cshtml", recruitFooterSetting);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateRecruitFooterSetting(RecruitFooterSettingVM recruitFooterSetting)
    {
        if (recruitFooterSetting.Id == 0) return View();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PutAsJsonAsync("RecruitFooterSetting/Update/", recruitFooterSetting);
        return RedirectToAction("Recruit");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteRecruitFooterSetting(int Id)
    {
        if (Id == 0) return View();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.DeleteAsync("RecruitFooterSetting/Delete?Id=" + Id);
        return RedirectToAction("Recruit");
    }

    [HttpGet]
    public async Task<IActionResult> CreateRecruitFooterSetting()
    {
        RecruitFooterSettingVM recruitFooterSetting = new();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        return PartialView("~/Areas/Settings/Views/Recruit/RecruitFooterSetting/_Create.cshtml", recruitFooterSetting);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRecruitStatus(RecruitFooterSettingVM recruitFooterSetting)
    {
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PostAsJsonAsync("RecruitFooterSetting/Create", recruitFooterSetting);
        return RedirectToAction("Recruit");
    }

    //Recruiter Settings
    [HttpGet]
    public async Task<IActionResult> EditRecruiterSetting(int Id)
    {
        if (Id == 0) return View();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        var recruiterSetting = await client.GetFromJsonAsync<RecruiterSettingVM>("RecruiterSetting/GetById/?Id=" + Id);
        return PartialView("~/Areas/Settings/Views/Recruit/RecruiterSetting/_Edit.cshtml", recruiterSetting);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateRecruiterSetting(RecruiterSettingVM recruiterSetting)
    {
        if (recruiterSetting.Id == 0) return View();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PutAsJsonAsync("RecruiterSetting/Update/", recruiterSetting);
        return RedirectToAction("Recruit");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteRecruiterSetting(int Id)
    {
        if (Id == 0) return View();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.DeleteAsync("RecruiterSetting/Delete?Id=" + Id);
        return RedirectToAction("Recruit");
    }

    [HttpGet]
    public async Task<IActionResult> CreateRecruiterSetting()
    {
        RecruiterSettingVM recruiterSetting = new();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        return PartialView("~/Areas/Settings/Views/Recruit/RecruiterSetting/_Create.cshtml", recruiterSetting);
    }
    [HttpPost]
    public async Task<IActionResult> CreateRecruitStatus(RecruiterSettingVM recruiterSetting)
    {
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PostAsJsonAsync("RecruiterSetting/Create", recruiterSetting);
        return RedirectToAction("Recruit");
    }

    //General Settings
    [HttpPost]
    public async Task<IActionResult> RecruitNotificationSettingUpdate(string emailJsonData, string emailNotfnJsonData)
    {
        //if (Id == 0) return View();
        RecruitNotificationSettingVM recruitNotificationSetting = new();
        recruitNotificationSetting.Id = 1;
        recruitNotificationSetting.CBEMailJsonSettings = emailJsonData;
        recruitNotificationSetting.CBEMailNotificationJsonSettings = emailNotfnJsonData;

        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PutAsJsonAsync("RecruitNotificationSetting/Update/", recruitNotificationSetting);
        return RedirectToAction("Recruit");
    }

    // Recruit Job Application Status Settings
    [HttpGet]
    public async Task<IActionResult> EditRecruitJobApplicationStatusSetting(int Id)
    {
        if (Id == 0) return View();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        var recruitJobApplicationStatusSetting = await client.GetFromJsonAsync<RecruitJobApplicationStatusSettingVM>("RecruitJobApplicationStatusSetting/GetById/?Id=" + Id);
        return PartialView("~/Areas/Settings/Views/Recruit/RecruitJobApplicationStatusSetting/_Edit.cshtml", recruitJobApplicationStatusSetting);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateRecruitJobApplicationStatusSetting(RecruitJobApplicationStatusSettingVM recruitJobApplicationStatusSetting)
    {
        if (recruitJobApplicationStatusSetting.Id == 0) return View();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PutAsJsonAsync("RecruitJobApplicationStatusSetting/Update/", recruitJobApplicationStatusSetting);
        return RedirectToAction("Recruit");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteRecruitJobApplicationStatusSetting(int Id)
    {
        if (Id == 0) return View();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.DeleteAsync("RecruitJobApplicationStatusSetting/Delete?Id=" + Id);
        return RedirectToAction("Recruit");
    }

    [HttpGet]
    public async Task<IActionResult> CreateRecruitJobApplicationStatusSetting()
    {
        RecruitJobApplicationStatusSettingVM recruitJobApplicationStatusSetting = new();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        return PartialView("~/Areas/Settings/Views/Recruit/RecruitJobApplicationStatusSetting/_Create.cshtml", recruitJobApplicationStatusSetting);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRecruitStatus(RecruitJobApplicationStatusSettingVM recruitJobApplicationStatusSetting)
    {
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PostAsJsonAsync("RecruitJobApplicationStatusSetting/Create", recruitJobApplicationStatusSetting);
        return RedirectToAction("Recruit");
    }

    // Recruit Custom Question Settings
    [HttpGet]
    public async Task<IActionResult> EditRecruitCustomQuestionSetting(int Id)
    {
        if (Id == 0) return View();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        var recruitCustomQuestionSetting = await client.GetFromJsonAsync<RecruitCustomQuestionSettingVM>("RecruitCustomQuestionSetting/GetById/?Id=" + Id);
        return PartialView("~/Areas/Settings/Views/Recruit/RecruitCustomQuestionSetting/_Edit.cshtml", recruitCustomQuestionSetting);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateRecruitCustomQuestionSetting(RecruitCustomQuestionSettingVM recruitCustomQuestionSetting)
    {
        if (recruitCustomQuestionSetting.Id == 0) return View();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PutAsJsonAsync("RecruitCustomQuestionSetting/Update/", recruitCustomQuestionSetting);
        return RedirectToAction("Recruit");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteRecruitCustomQuestionSetting(int Id)
    {
        if (Id == 0) return View();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.DeleteAsync("RecruitCustomQuestionSetting/Delete?Id=" + Id);
        return RedirectToAction("Recruit");
    }

    [HttpGet]
    public async Task<IActionResult> CreateRecruitCustomQuestionSetting()
    {
        RecruitCustomQuestionSettingVM recruitCustomQuestionSetting = new();
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        return PartialView("~/Areas/Settings/Views/Recruit/RecruitCustomQuestionSetting/_Create.cshtml", recruitCustomQuestionSetting);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRecruitStatus(RecruitCustomQuestionSettingVM recruitCustomQuestionSetting)
    {
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PostAsJsonAsync("RecruitCustomQuestionSetting/Create", recruitCustomQuestionSetting);
        return RedirectToAction("Recruit");
    }
}



