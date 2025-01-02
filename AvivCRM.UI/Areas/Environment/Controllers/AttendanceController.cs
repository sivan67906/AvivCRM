using Microsoft.AspNetCore.Mvc;
using AvivCRM.UI.Areas.Environment.ViewModels;

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class AttendanceController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AttendanceController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    public async Task<IActionResult> Index()
    {
        return View();
    }
    public async Task<IActionResult> Attendance()
    {
        // Page Title
        ViewData["pTitle"] = "Attendances Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "Attendance";
        ViewData["bChild"] = "Attendance";

        var client = _httpClientFactory.CreateClient("ApiGatewayCall");

        var attendanceSettings = await client.GetFromJsonAsync<List<AttendanceSettingVM>>("AttendanceSetting/GetAll");
        var attendanceSetting = attendanceSettings?.FirstOrDefault();
        var employeeShiftSettings = await client.GetFromJsonAsync<List<EmployeeShiftSettingVM>>("EmployeeShiftSetting/GetAll");
        var employeeShift = employeeShiftSettings?.FirstOrDefault();

        //AttendanceSettingVM attendanceSetting = new();
        //List<EmployeeShiftSettingVM> employeeShiftSettings = new();
        var viewModel = new AttendanceVM
        {
            AttendanceSetting = attendanceSetting,
            EmployeeShiftSettings = employeeShiftSettings
        };
        return View(viewModel);
    }
}



