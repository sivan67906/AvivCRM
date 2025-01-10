using AvivCRM.UI.Areas.Configuration.ViewModels;
using AvivCRM.UI.Areas.Environment.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class TimeLogController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public TimeLogController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> TimeLog()
    {
        // Page Title
        ViewData["pTitle"] = "TimeLogs Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "TimeLog";
        ViewData["bChild"] = "TimeLog";

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        ApiResultResponse<List<TimeLogVM>>? timeLogs =
            await client.GetFromJsonAsync<ApiResultResponse<List<TimeLogVM>>>("TimeLog/all-timelog");
        TimeLogVM? timeLog = timeLogs!.Data!.FirstOrDefault();

        List<CBTimeLogSettingVM>? cbTimeLogItems = timeLog != null
            ? JsonConvert.DeserializeObject<List<CBTimeLogSettingVM>>(timeLog.CBTimeLogJsonSettings!)
            : [];
        timeLog!.CBTimeLogSettings = cbTimeLogItems;

        //var roleList = await client.GetFromJsonAsync<List<RoleVM>>("Role/all-role");
        //var role = await client.GetFromJsonAsync<RoleVM>("Role/byid-role/?Id=" + timeLog?.RoleId);

        List<RoleVM>? roleList =
        [
            new RoleVM
            {
                Id = new Guid("EDC3C550-82A3-4DC6-8842-F29351BB4BD8"), Code = "ADM", Name = "App Administrator"
            },
            new RoleVM { Id = Guid.Parse("E0BB7E72-CA1A-4C2B-B531-89E720D6ABCD"), Code = "EMP", Name = "Employee" },
            new RoleVM { Id = new Guid("0BBE1696-596A-433B-ABB7-AFD60DCD826A"), Code = "MEM", Name = "Membership" }
        ];

        RoleVM? role = new()
        {
            Id = new Guid("E0BB7E72-CA1A-4C2B-B531-89E720D6ABCD"),
            Code = "ADM",
            Name = "App Administrator"
        };

        RoleDDSetting? roleDDValue = new()
        {
            role = role,
            SelectedRoleId = role!.Id,
            roleItems = roleList?.Select(i => new RoleVM { Id = i.Id, Name = i.Name }).ToList()
        };

        timeLog!.RoleDDSettings = roleDDValue;
        return View(timeLog);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateTimeLog(TimeLogVM timeLog, string jsonData)
    {
        //if (timeLog.Id == 0) return View();
        ApiResultResponse<ProjectSettingVM> result = new();

        timeLog.CBTimeLogJsonSettings = jsonData;
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        //await client.PutAsJsonAsync("TimeLog/update-timelog/", timeLog);
        //return Redirect("TimeLog");
        HttpResponseMessage? responseTimeLog = await client.PutAsJsonAsync("TimeLog/update-timelog/", timeLog);
        if (responseTimeLog.IsSuccessStatusCode)
        {
            string? jsonResponseLeadSource = await responseTimeLog.Content.ReadAsStringAsync();
            result = JsonConvert.DeserializeObject<ApiResultResponse<ProjectSettingVM>>(jsonResponseLeadSource);
        }
        else
        {
            string? errorContent = await responseTimeLog.Content.ReadAsStringAsync();
            result = new ApiResultResponse<ProjectSettingVM>
            {
                IsSuccess = false,
                Message = responseTimeLog.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        // Server side Validation
        List<string> serverErrorMessageList = [];
        string serverErrorMessage = result!.Message!;
        serverErrorMessageList.Add(serverErrorMessage);

        if (!result!.IsSuccess)
        {
            return Json(new { success = false, errors = serverErrorMessageList });
        }

        return Json(new { success = true });
    }
}