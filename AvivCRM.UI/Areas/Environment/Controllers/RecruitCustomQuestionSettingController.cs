using AvivCRM.UI.Areas.Environment.ViewModels;
using Microsoft.AspNetCore.Mvc;

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

        ApiResultResponse<List<RecruitCustomQuestionSettingVM>>? RecruitCustomQuestionSettings =
            await client.GetFromJsonAsync<ApiResultResponse<List<RecruitCustomQuestionSettingVM>>>("RecruitCustomQuestionSetting/all-recruitcustomquestionsetting");

        return PartialView("_RecruitCustomQuestionSetting", RecruitCustomQuestionSettings!.Data!);
    }
}
