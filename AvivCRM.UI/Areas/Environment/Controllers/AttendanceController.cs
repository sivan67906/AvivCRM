using AvivCRM.UI.Areas.Environment.ViewModels;
using Microsoft.AspNetCore.Mvc;

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

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        List<AttendanceSettingVM>? attendanceSettings =
            await client.GetFromJsonAsync<List<AttendanceSettingVM>>("AttendanceSetting/GetAll");
        AttendanceSettingVM? attendanceSetting = attendanceSettings?.FirstOrDefault();
        List<EmployeeShiftSettingVM>? employeeShiftSettings =
            await client.GetFromJsonAsync<List<EmployeeShiftSettingVM>>("EmployeeShiftSetting/GetAll");
        EmployeeShiftSettingVM? employeeShift = employeeShiftSettings?.FirstOrDefault();

        //AttendanceSettingVM attendanceSetting = new();
        //List<EmployeeShiftSettingVM> employeeShiftSettings = new();
        AttendanceVM? viewModel = new()
        {
            AttendanceSetting = attendanceSetting, EmployeeShiftSettings = employeeShiftSettings
        };
        return View(viewModel);
    }
}