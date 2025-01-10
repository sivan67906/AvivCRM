using AvivCRM.UI.Areas.Environment.ViewModels;
using Microsoft.AspNetCore.Mvc;

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

        //HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        //// General Settings
        //List<RecruitGeneralSettingVM>? recruitGeneralSettings =
        //    await client.GetFromJsonAsync<List<RecruitGeneralSettingVM>>("RecruitGeneralSetting/GetAll");
        //RecruitGeneralSettingVM? recruitGeneralSetting = recruitGeneralSettings?.FirstOrDefault();
        //List<GeneralCBSettingVM>? cbItems = recruitGeneralSetting != null
        //    ? JsonConvert.DeserializeObject<List<GeneralCBSettingVM>>(recruitGeneralSetting.GeneralCBJsonSettings)
        //    : new List<GeneralCBSettingVM>();
        //recruitGeneralSetting.GeneralCBSettings = cbItems;

        //List<RecruitFooterSettingVM>? recruitFooterSettings =
        //    await client.GetFromJsonAsync<List<RecruitFooterSettingVM>>("RecruitFooterSetting/GetAll");
        //List<RecruiterSettingVM>? recruiterSettings =
        //    await client.GetFromJsonAsync<List<RecruiterSettingVM>>("RecruiterSetting/GetAll");

        //// Recruit Notification Settings
        //List<RecruitNotificationSettingVM>? recruitNotificationSettings =
        //    await client.GetFromJsonAsync<List<RecruitNotificationSettingVM>>("RecruitNotificationSetting/GetAll");
        //RecruitNotificationSettingVM? recruitNotificationSetting = recruitNotificationSettings?.FirstOrDefault();
        //List<CBEMailSettingVM>? cbEmailItems = recruitNotificationSetting != null
        //    ? JsonConvert.DeserializeObject<List<CBEMailSettingVM>>(recruitNotificationSetting.CBEMailJsonSettings)
        //    : new List<CBEMailSettingVM>();
        //List<CBEMailNotificationSettingVM>? cbEmailNotificationItems = recruitNotificationSetting != null
        //    ? JsonConvert.DeserializeObject<List<CBEMailNotificationSettingVM>>(recruitNotificationSetting
        //        .CBEMailNotificationJsonSettings)
        //    : new List<CBEMailNotificationSettingVM>();
        //recruitNotificationSetting.CBEMailSettings = cbEmailItems;
        //recruitNotificationSetting.CBEMailNotificationSettings = cbEmailNotificationItems;

        //List<RecruitJobApplicationStatusSettingVM>? recruitJobApplicationStatusSettings =
        //    await client.GetFromJsonAsync<List<RecruitJobApplicationStatusSettingVM>>(
        //        "RecruitJobApplicationStatusSetting/GetAll");
        //List<RecruitCustomQuestionSettingVM>? recruitCustomQuestionSettings =
        //    await client.GetFromJsonAsync<List<RecruitCustomQuestionSettingVM>>("RecruitCustomQuestionSetting/GetAll");
        //RecruitVM? viewModel = new()
        //{
        //    RecruitGeneralSettingVMList = recruitGeneralSetting,
        //    RecruitFooterSettingVMList = recruitFooterSettings,
        //    RecruiterSettingVMList = recruiterSettings,
        //    RecruitNotificationSettingVMList = recruitNotificationSetting,
        //    RecruitJobApplicationStatusSettingVMList = recruitJobApplicationStatusSettings,
        //    RecruitCustomQuestionSettingVMList = recruitCustomQuestionSettings
        //};
        //return View(viewModel);
        return View();
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

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PutAsJsonAsync("RecruitGeneralSetting/Update/", recruitGeneralSetting);
        return RedirectToAction("Recruit");
    }

    //Footer Settings
    [HttpGet]
    public async Task<IActionResult> EditRecruitFooterSetting(int Id)
    {
        if (Id == 0)
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        RecruitFooterSettingVM? recruitFooterSetting =
            await client.GetFromJsonAsync<RecruitFooterSettingVM>("RecruitFooterSetting/GetById/?Id=" + Id);
        return PartialView("~/Areas/Environment/Views/Recruit/RecruitFooterSetting/_Edit.cshtml", recruitFooterSetting);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateRecruitFooterSetting(RecruitFooterSettingVM recruitFooterSetting)
    {
        //if (recruitFooterSetting.Id == 0)
        //{
        //    return View();
        //}

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PutAsJsonAsync("RecruitFooterSetting/Update/", recruitFooterSetting);
        return RedirectToAction("Recruit");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteRecruitFooterSetting(int Id)
    {
        if (Id == 0)
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.DeleteAsync("RecruitFooterSetting/Delete?Id=" + Id);
        return RedirectToAction("Recruit");
    }

    [HttpGet]
    public async Task<IActionResult> CreateRecruitFooterSetting()
    {
        RecruitFooterSettingVM recruitFooterSetting = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        return PartialView("~/Areas/Environment/Views/Recruit/RecruitFooterSetting/_Create.cshtml",
            recruitFooterSetting);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRecruitStatus(RecruitFooterSettingVM recruitFooterSetting)
    {
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PostAsJsonAsync("RecruitFooterSetting/Create", recruitFooterSetting);
        return RedirectToAction("Recruit");
    }

    //Recruiter Settings
    [HttpGet]
    public async Task<IActionResult> EditRecruiterSetting(int Id)
    {
        if (Id == 0)
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        RecruiterSettingVM? recruiterSetting =
            await client.GetFromJsonAsync<RecruiterSettingVM>("RecruiterSetting/GetById/?Id=" + Id);
        return PartialView("~/Areas/Environment/Views/Recruit/RecruiterSetting/_Edit.cshtml", recruiterSetting);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateRecruiterSetting(RecruiterSettingVM recruiterSetting)
    {
        //if (recruiterSetting.Id == 0)
        //{
        //    return View();
        //}

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PutAsJsonAsync("RecruiterSetting/Update/", recruiterSetting);
        return RedirectToAction("Recruit");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteRecruiterSetting(int Id)
    {
        if (Id == 0)
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.DeleteAsync("RecruiterSetting/Delete?Id=" + Id);
        return RedirectToAction("Recruit");
    }

    [HttpGet]
    public async Task<IActionResult> CreateRecruiterSetting()
    {
        RecruiterSettingVM recruiterSetting = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        return PartialView("~/Areas/Environment/Views/Recruit/RecruiterSetting/_Create.cshtml", recruiterSetting);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRecruitStatus(RecruiterSettingVM recruiterSetting)
    {
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PostAsJsonAsync("RecruiterSetting/Create", recruiterSetting);
        return RedirectToAction("Recruit");
    }

    //General Settings
    [HttpPost]
    public async Task<IActionResult> RecruitNotificationSettingUpdate(string emailJsonData, string emailNotfnJsonData)
    {
        //if (Id == 0) return View();
        RecruitNotificationSettingVM recruitNotificationSetting = new();
        recruitNotificationSetting.Id = Guid.Parse("E2F233CC-D34F-4FFD-8B55-08DD2EF06545");
        recruitNotificationSetting.CBEMailJsonSettings = emailJsonData;
        recruitNotificationSetting.CBEMailNotificationJsonSettings = emailNotfnJsonData;

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PutAsJsonAsync("RecruitNotificationSetting/Update/", recruitNotificationSetting);
        return RedirectToAction("Recruit");
    }

    // Recruit Job Application Status Settings
    [HttpGet]
    public async Task<IActionResult> EditRecruitJobApplicationStatusSetting(int Id)
    {
        if (Id == 0)
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        RecruitJobApplicationStatusSettingVM? recruitJobApplicationStatusSetting =
            await client.GetFromJsonAsync<RecruitJobApplicationStatusSettingVM>(
                "RecruitJobApplicationStatusSetting/GetById/?Id=" + Id);
        return PartialView("~/Areas/Environment/Views/Recruit/RecruitJobApplicationStatusSetting/_Edit.cshtml",
            recruitJobApplicationStatusSetting);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateRecruitJobApplicationStatusSetting(
        RecruitJobApplicationStatusSettingVM recruitJobApplicationStatusSetting)
    {
        //if (recruitJobApplicationStatusSetting.Id == 0)
        //{
        //    return View();
        //}

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PutAsJsonAsync("RecruitJobApplicationStatusSetting/Update/", recruitJobApplicationStatusSetting);
        return RedirectToAction("Recruit");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteRecruitJobApplicationStatusSetting(int Id)
    {
        if (Id == 0)
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.DeleteAsync("RecruitJobApplicationStatusSetting/Delete?Id=" + Id);
        return RedirectToAction("Recruit");
    }

    [HttpGet]
    public async Task<IActionResult> CreateRecruitJobApplicationStatusSetting()
    {
        RecruitJobApplicationStatusSettingVM recruitJobApplicationStatusSetting = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        return PartialView("~/Areas/Environment/Views/Recruit/RecruitJobApplicationStatusSetting/_Create.cshtml",
            recruitJobApplicationStatusSetting);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRecruitStatus(
        RecruitJobApplicationStatusSettingVM recruitJobApplicationStatusSetting)
    {
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PostAsJsonAsync("RecruitJobApplicationStatusSetting/Create", recruitJobApplicationStatusSetting);
        return RedirectToAction("Recruit");
    }

    // Recruit Custom Question Settings
    [HttpGet]
    public async Task<IActionResult> EditRecruitCustomQuestionSetting(int Id)
    {
        if (Id == 0)
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        RecruitCustomQuestionSettingVM? recruitCustomQuestionSetting =
            await client.GetFromJsonAsync<RecruitCustomQuestionSettingVM>("RecruitCustomQuestionSetting/GetById/?Id=" +
                                                                          Id);
        return PartialView("~/Areas/Environment/Views/Recruit/RecruitCustomQuestionSetting/_Edit.cshtml",
            recruitCustomQuestionSetting);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateRecruitCustomQuestionSetting(
        RecruitCustomQuestionSettingVM recruitCustomQuestionSetting)
    {
        //if (recruitCustomQuestionSetting.Id == 0)
        //{
        //    return View();
        //}

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PutAsJsonAsync("RecruitCustomQuestionSetting/Update/", recruitCustomQuestionSetting);
        return RedirectToAction("Recruit");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteRecruitCustomQuestionSetting(int Id)
    {
        if (Id == 0)
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.DeleteAsync("RecruitCustomQuestionSetting/Delete?Id=" + Id);
        return RedirectToAction("Recruit");
    }

    [HttpGet]
    public async Task<IActionResult> CreateRecruitCustomQuestionSetting()
    {
        RecruitCustomQuestionSettingVM recruitCustomQuestionSetting = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        return PartialView("~/Areas/Environment/Views/Recruit/RecruitCustomQuestionSetting/_Create.cshtml",
            recruitCustomQuestionSetting);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRecruitStatus(RecruitCustomQuestionSettingVM recruitCustomQuestionSetting)
    {
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PostAsJsonAsync("RecruitCustomQuestionSetting/Create", recruitCustomQuestionSetting);
        return RedirectToAction("Recruit");
    }
}