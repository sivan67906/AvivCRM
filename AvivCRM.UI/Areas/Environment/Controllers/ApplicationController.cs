#region Namespaces
using System.Text;
using AvivCRM.UI.Areas.Environment.ViewModels;
using AvivCRM.UI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
#endregion

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class ApplicationController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    #region Constructor
    public ApplicationController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    #endregion

    #region Retrieves a List of Applications
    /// <summary>
    /// Retrieves a list of Applications from the database.
    /// </summary>
    /// <param name=""></param>
    /// <returns>Modal popup will open to create New Application</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/Application/Application
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    public async Task<IActionResult> Application()
    {
        ViewData["pTitle"] = "Applications Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "Application";
        ViewData["bChild"] = "Application View";
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        ApiResultResponse<List<ApplicationVM>> applicationList = new();

        // fetch all the Applications
        applicationList =
                await client.GetFromJsonAsync<ApiResultResponse<List<ApplicationVM>>>("Application/all-application");

        return View(applicationList!.Data);
    }
    #endregion

    #region Create Application functionionality
    /// <summary>
    /// Show the popup to create a new Application.
    /// </summary>
    /// <param name=""></param>
    /// <returns>New Application</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/Application/Application
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ApplicationVM application = new();
        return PartialView("_Create", application);
    }

    /// <summary>
    /// New Application will be create.
    /// </summary>
    /// <param name="application">Application entity that needs to be create</param>
    /// <returns>New Application will be create.</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/Application/Application
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Create(ApplicationVM application)
    {
        ApiResultResponse<ApplicationVM> resultApplication = new();

        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (application.Name == null)
        {
            application.Name = "";
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        string? jsonApplication = JsonConvert.SerializeObject(application);
        StringContent? applicationContent = new(jsonApplication, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseApplication =
            await client.PostAsync("Application/create-application", applicationContent);

        if (responseApplication.IsSuccessStatusCode)
        {
            string? jsonResponseApplication = await responseApplication.Content.ReadAsStringAsync();
            resultApplication = JsonConvert.DeserializeObject<ApiResultResponse<ApplicationVM>>(jsonResponseApplication);
        }
        else
        {
            string? errorContent = await responseApplication.Content.ReadAsStringAsync();
            resultApplication = new ApiResultResponse<ApplicationVM>
            {
                IsSuccess = false,
                Message = responseApplication.StatusCode + "ErrorContent: " + errorContent
            };
        }

        if (!resultApplication!.IsSuccess)
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

    #region Edit Application functionionality
    /// <summary>
    /// Edit the existing Application.
    /// </summary>
    /// <param name="Id">Application Guid that needs to be edit</param>
    /// <returns>Popup will be open to edit the application details</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/Application/Application
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

        ApiResultResponse<ApplicationVM> application = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        application =
            await client.GetFromJsonAsync<ApiResultResponse<ApplicationVM>>("Application/byid-application/?Id=" + Id);

        if (!application!.IsSuccess)
        {
            return View();
        }

        return PartialView("_Edit", application.Data);
    }

    /// <summary>
    /// Update the existing Application.
    /// </summary>
    /// <param name="application">Application entity to update the existing application</param>
    /// <returns>Changes will be updated for the existing application</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/Application/Application
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Edit(ApplicationVM application)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (application.Name == null)
        {
            application.Name = "";
        }

        ApiResultResponse<ApplicationVM> resultApplication = new();

        if (GuidExtensions.IsNullOrEmpty(application.Id))
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        string? jsonApplication = JsonConvert.SerializeObject(application);
        StringContent? applicationContent = new(jsonApplication, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseApplication =
            await client.PutAsync("Application/update-application/", applicationContent);
        if (responseApplication.IsSuccessStatusCode)
        {
            string? jsonResponseApplication = await responseApplication.Content.ReadAsStringAsync();
            resultApplication = JsonConvert.DeserializeObject<ApiResultResponse<ApplicationVM>>(jsonResponseApplication);
        }
        else
        {
            string? errorContent = await responseApplication.Content.ReadAsStringAsync();
            resultApplication = new ApiResultResponse<ApplicationVM>
            {
                IsSuccess = false,
                Message = responseApplication.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        if (!resultApplication!.IsSuccess)
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

    #region Delete Application functionionality
    /// <summary>
    /// Delete the existing Application.
    /// </summary>
    /// <param name="Id">Application Guid that needs to be delete</param>
    /// <returns>Application will be deleted</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/Application/Application
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

        ApiResultResponse<ApplicationVM> resultApplication = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? responseApplication = await client.DeleteAsync("Application/delete-application?Id=" + Id);
        if (responseApplication.IsSuccessStatusCode)
        {
            string? jsonResponseApplication = await responseApplication.Content.ReadAsStringAsync();
            resultApplication = JsonConvert.DeserializeObject<ApiResultResponse<ApplicationVM>>(jsonResponseApplication);
        }
        else
        {
            string? errorContent = await responseApplication.Content.ReadAsStringAsync();
            resultApplication = new ApiResultResponse<ApplicationVM>
            {
                IsSuccess = false,
                Message = responseApplication.StatusCode.ToString()
            };
        }

        if (!resultApplication!.IsSuccess)
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






















