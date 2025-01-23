using System.Text;
using AvivCRM.UI.Areas.Environment.ViewModels;
using AvivCRM.UI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AvivCRM.UI.Areas.Environment.Controllers;
public class ProjectSettingController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ProjectSettingController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    public IActionResult Index()
    {
        return View();
    }
    // Project Setting
    [HttpPost]
    public async Task<IActionResult> ProjectSettingUpdate(ProjectSettingVM projSetting)
    {
        if (GuidExtensions.IsNullOrEmpty(projSetting.Id))
        {
            return View();
        }

        ApiResultResponse<ProjectSettingVM> setting = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        string? jsonprojSetting = JsonConvert.SerializeObject(projSetting);
        StringContent? projSettingContent = new(jsonprojSetting, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseProjSetting =
            await client.PutAsync("ProjectSetting/update-projectsetting/", projSettingContent);

        if (responseProjSetting.IsSuccessStatusCode)
        {
            string? jsonResponseLeadSource = await responseProjSetting.Content.ReadAsStringAsync();
            setting = JsonConvert.DeserializeObject<ApiResultResponse<ProjectSettingVM>>(jsonResponseLeadSource);
        }
        else
        {
            string? errorContent = await responseProjSetting.Content.ReadAsStringAsync();
            setting = new ApiResultResponse<ProjectSettingVM>
            {
                IsSuccess = false,
                Message =
                    responseProjSetting.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        // Server side Validation
        List<string> serverErrorMessageList = [];
        string serverErrorMessage = setting!.Message!;
        serverErrorMessageList.Add(serverErrorMessage);

        if (!setting!.IsSuccess)
        {
            return Json(new { success = false, errors = serverErrorMessageList });
        }

        return Json(new { success = true });
    }
}