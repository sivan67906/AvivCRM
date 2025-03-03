using AvivCRM.UI.Areas.Environment.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class TimesheetController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public TimesheetController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Timesheet()
    {
        // Page Title
        ViewData["pTitle"] = "Timesheets Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "Timesheet";
        ViewData["bChild"] = "Timesheet";

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        List<TimesheetSettingVM>? timesheetSettings =
            await client.GetFromJsonAsync<List<TimesheetSettingVM>>("TimesheetSetting/GetAll");

        ApiResultResponse<List<ProjectVM>>? projectList = await client.GetFromJsonAsync<ApiResultResponse<List<ProjectVM>>>("Project/all-project");
        List<ProjectVM> projects = projectList!.Data!;

        ApiResultResponse<List<TaskingVM>>? taskingList = await client.GetFromJsonAsync<ApiResultResponse<List<Tasking>>>("Tasking/all-tasking");
        List<TaskingVM> taskings = taskingList!.Data!;

        ApiResultResponse<List<EmployeeVM>>? employeeList = await client.GetFromJsonAsync<ApiResultResponse<List<EmployeeVM>>>("Employee/all-employee");
        List<EmployeeVM> employees = employeeList!.Data!;


        foreach (ProjectVM parent in projects)
        {
            foreach (TimesheetSettingVM? child in timesheetSettings!.Where(c => c.ProjectId == parent.Id))
            {
                child.ProjectName = parent.ProjectName;
            }
        }
        foreach (TaskingVM parent in taskings)
        {
            foreach (TimesheetSettingVM? child in timesheetSettings!.Where(c => c.TaskId == parent.Id))
            {
                child.TaskName = parent.TaskName;
            }
        }
        foreach (EmployeeVM parent in employees)
        {
            foreach (TimesheetSettingVM? child in timesheetSettings!.Where(c => c.EmployeeId == parent.Id))
            {
                child.EmployeeName = parent.EmployeeName;
            }
        }

        return View(timesheetSettings);
    }


    [HttpGet]
    public IActionResult Create()
    {
        TimesheetSettingVM timesheet = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        //ViewBag.ProjectSettings = await client.GetFromJsonAsync<List<ProjectSettingVM>>("ProjectSetting/GetAll");
        //ViewBag.Tasks = await client.GetFromJsonAsync<List<TaskVM>>("Task/GetAll");
        //ViewBag.Employees = await client.GetFromJsonAsync<List<EmployeeVM>>("Employee/GetAll");

        ViewBag.ProjectSettings = new List<ProjectSettingVM>();
        ViewBag.Tasks = new List<TaskVM>();
        ViewBag.Employees = new List<EmployeeVM>();
        return PartialView("_Create", timesheet);
    }

    //[HttpPost]
    //public async Task<IActionResult> Create(CompanyVM company)
    //{
    //    var client = _httpClientFactory.CreateClient("ApiGatewayCall");
    //    await client.PostAsJsonAsync<CompanyVM>("Company/Create", company);
    //    return RedirectToAction("Company");
    //}

    //[HttpGet]
    //public async Task<IActionResult> Edit(int Id)
    //{
    //    if (Id == 0) return View();
    //    var client = _httpClientFactory.CreateClient("ApiGatewayCall");
    //    ViewBag.BusinessTypes = await client.GetFromJsonAsync<List<BusinessTypeVM>>("BusinessType/GetAll");
    //    ViewBag.CategoryTypes = await client.GetFromJsonAsync<List<BusinessCategoryVM>>("Category/GetAll");
    //    var company = await client.GetFromJsonAsync<CompanyVM>("Company/GetById/?Id=" + Id);
    //    return PartialView("_Edit", company);
    //}

    //[HttpPost]
    //public async Task<IActionResult> Update(CompanyVM company)
    //{
    //    if (company.Id == 0) return View();
    //    var client = _httpClientFactory.CreateClient("ApiGatewayCall");
    //    await client.PutAsJsonAsync<CompanyVM>("Company/Update/", company);
    //    return RedirectToAction("Company");
    //}

    //[HttpGet]
    //public async Task<IActionResult> Delete(int Id)
    //{
    //    if (Id == 0) return View();
    //    var client = _httpClientFactory.CreateClient("ApiGatewayCall");
    //    ViewBag.BusinessTypes = await client.GetFromJsonAsync<List<BusinessTypeVM>>("BusinessType/GetAll");
    //    ViewBag.CategoryTypes = await client.GetFromJsonAsync<List<BusinessCategoryVM>>("Category/GetAll");
    //    var company = await client.GetFromJsonAsync<CompanyVM>("Company/GetById/?{Id}=" + Id);
    //    return PartialView("_Delete", company);
    //}

    //[HttpPost]
    //public async Task<IActionResult> Delete(CompanyVM company)
    //{
    //    if (company.Id == 0) return View();
    //    var client = _httpClientFactory.CreateClient("ApiGatewayCall");
    //    await client.DeleteAsync("Company/Delete?Id=" + company.Id);
    //    return RedirectToAction("Company");
    //}
}