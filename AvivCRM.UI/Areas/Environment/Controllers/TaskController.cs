#region Namespaces
using System.Text;
using AvivCRM.UI.Areas.Environment.ViewModels;
using AvivCRM.UI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
#endregion

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class TaskController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    #region Constructor
    public TaskController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    #endregion

    #region Retrieves a List of Tasks
    /// <summary>
    /// Retrieves a list of Tasks from the database.
    /// </summary>
    /// <param name=""></param>
    /// <returns>Modal popup will open to create New Task</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/Task/Task
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    public async Task<IActionResult> Task()
    {
        ViewData["pTitle"] = "Tasks Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "Task";
        ViewData["bChild"] = "Task View";
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        ApiResultResponse<List<TaskVM>> taskList = new();

        // fetch all the Tasks
        taskList =
                await client.GetFromJsonAsync<ApiResultResponse<List<TaskVM>>>("Task/all-task");

        return View(taskList!.Data);
    }
    #endregion

    #region Create Task functionionality
    /// <summary>
    /// Show the popup to create a new Task.
    /// </summary>
    /// <param name=""></param>
    /// <returns>New Task</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/Task/Task
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        TaskVM task = new();
        return PartialView("_Create", task);
    }

    /// <summary>
    /// New Task will be create.
    /// </summary>
    /// <param name="task">Task entity that needs to be create</param>
    /// <returns>New Task will be create.</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/Task/Task
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Create(TaskVM task)
    {
        ApiResultResponse<TaskVM> resultTask = new();

        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (task.Name == null)
        {
            task.Name = "";
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        string? jsonTask = JsonConvert.SerializeObject(task);
        StringContent? taskContent = new(jsonTask, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseTask =
            await client.PostAsync("Task/create-task", taskContent);

        if (responseTask.IsSuccessStatusCode)
        {
            string? jsonResponseTask = await responseTask.Content.ReadAsStringAsync();
            resultTask = JsonConvert.DeserializeObject<ApiResultResponse<TaskVM>>(jsonResponseTask);
        }
        else
        {
            string? errorContent = await responseTask.Content.ReadAsStringAsync();
            resultTask = new ApiResultResponse<TaskVM>
            {
                IsSuccess = false,
                Message = responseTask.StatusCode + "ErrorContent: " + errorContent
            };
        }

        if (!resultTask!.IsSuccess)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        return Json(new { success = true });
    }

    #endregion

    #region Edit Task functionionality
    /// <summary>
    /// Edit the existing Task.
    /// </summary>
    /// <param name="Id">Task Guid that needs to be edit</param>
    /// <returns>Popup will be open to edit the task details</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/Task/Task
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    [HttpGet]
    public async Task<IActionResult> Edit(Guid Id)
    {
        if (GuidExtensions.IsNullOrEmpty(Id))
        {
            return View();
        }

        ApiResultResponse<TaskVM> task = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        task =
            await client.GetFromJsonAsync<ApiResultResponse<TaskVM>>("Task/byid-task/?Id=" + Id);

        if (!task!.IsSuccess)
        {
            return View();
        }

        return PartialView("_Edit", task.Data);
    }

    /// <summary>
    /// Update the existing Task.
    /// </summary>
    /// <param name="task">Task entity to update the existing task</param>
    /// <returns>Changes will be updated for the existing task</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/Task/Task
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Edit(TaskVM task)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (task.Name == null)
        {
            task.Name = "";
        }

        ApiResultResponse<TaskVM> resultTask = new();

        if (GuidExtensions.IsNullOrEmpty(task.Id))
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        string? jsonTask = JsonConvert.SerializeObject(task);
        StringContent? taskContent = new(jsonTask, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseTask =
            await client.PutAsync("Task/update-task/", taskContent);
        if (responseTask.IsSuccessStatusCode)
        {
            string? jsonResponseTask = await responseTask.Content.ReadAsStringAsync();
            resultTask = JsonConvert.DeserializeObject<ApiResultResponse<TaskVM>>(jsonResponseTask);
        }
        else
        {
            string? errorContent = await responseTask.Content.ReadAsStringAsync();
            resultTask = new ApiResultResponse<TaskVM>
            {
                IsSuccess = false,
                Message = responseTask.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        if (!resultTask!.IsSuccess)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        return Json(new { success = true });
    }
    #endregion

    #region Delete Task functionionality
    /// <summary>
    /// Delete the existing Task.
    /// </summary>
    /// <param name="Id">Task Guid that needs to be delete</param>
    /// <returns>Task will be deleted</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/Task/Task
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Delete(Guid Id)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        ApiResultResponse<TaskVM> resultTask = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? responseTask = await client.DeleteAsync("Task/delete-task?Id=" + Id);
        if (responseTask.IsSuccessStatusCode)
        {
            string? jsonResponseTask = await responseTask.Content.ReadAsStringAsync();
            resultTask = JsonConvert.DeserializeObject<ApiResultResponse<TaskVM>>(jsonResponseTask);
        }
        else
        {
            string? errorContent = await responseTask.Content.ReadAsStringAsync();
            resultTask = new ApiResultResponse<TaskVM>
            {
                IsSuccess = false,
                Message = responseTask.StatusCode.ToString()
            };
        }

        if (!resultTask!.IsSuccess)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        return Json(new { success = true });
    }
    #endregion
}






















