using AvivCRM.UI.Areas.Environment.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class TaskController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public TaskController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Task()
    {
        // Page Title
        ViewData["pTitle"] = "Currencies Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "Task";
        ViewData["bChild"] = "Task View";

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        List<TaskVM>? TaskList = await client.GetFromJsonAsync<List<TaskVM>>("Task/GetAll");

        return View(TaskList);
    }

    [HttpGet]
    public IActionResult Create()
    {
        TaskVM company = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        return PartialView("_Create", company);
    }

    [HttpPost]
    public async Task<IActionResult> Create(TaskVM Task)
    {
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PostAsJsonAsync<TaskVM>("Task/Create", Task);
        return RedirectToAction("Task");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int Id)
    {
        if (Id == 0)
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        TaskVM? Task = await client.GetFromJsonAsync<TaskVM>("Task/GetById/?Id=" + Id);
        return PartialView("_Edit", Task);
    }

    [HttpPost]
    public async Task<IActionResult> Update(TaskVM Task)
    {
        if (Task.Id == 0)
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.PutAsJsonAsync<TaskVM>("Task/Update/", Task);
        return RedirectToAction("Task");
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int Id)
    {
        if (Id == 0)
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        TaskVM? Task = await client.GetFromJsonAsync<TaskVM>("Task/GetById/?Id=" + Id);
        return PartialView("_Delete", Task);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(TaskVM Task)
    {
        if (Task.Id == 0)
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        await client.DeleteAsync("Task/Delete?Id=" + Task.Id);
        return RedirectToAction("Task");
    }
}