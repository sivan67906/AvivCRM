using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using AvivCRM.UI.Areas.Configuration.ViewModels;
using AvivCRM.UI.Areas.Environment.ViewModels;

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class TimeLogController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public TimeLogController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    public async Task<IActionResult> Index()
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

        var client = _httpClientFactory.CreateClient("ApiGatewayCall");

        var timeLogs = await client.GetFromJsonAsync<List<TimeLogVM>>("TimeLog/GetAll");
        var timeLog = timeLogs?.FirstOrDefault();

        var cbTimeLogItems = timeLog != null ? JsonConvert.DeserializeObject<List<CBTimeLogSettingVM>>(timeLog.CBTimeLogJsonSettings) : new List<CBTimeLogSettingVM>();
        timeLog!.CBTimeLogSettings = cbTimeLogItems;

        var roleList = await client.GetFromJsonAsync<List<RoleVM>>("Role/GetAll");
        var role = await client.GetFromJsonAsync<RoleVM>("Role/GetById/?Id=" + timeLog?.RoleId);

        var roleDDValue = new RoleDDSetting
        {
            role = role,
            SelectedRoleId = role!.Id,
            roleItems = roleList?.Select(i => new RoleVM
            {
                Id = i.Id,
                Name = i.Name
            }).ToList()
        };
        timeLog!.RoleDDSettings = roleDDValue;
        return View(timeLog);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateTimeLog(TimeLogVM timeLog, string jsonData)
    {
        if (timeLog.Id == 0) return View();
        timeLog.CBTimeLogJsonSettings = jsonData;
        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PutAsJsonAsync("TimeLog/Update/", timeLog);
        return Redirect("TimeLog");
    }
}



