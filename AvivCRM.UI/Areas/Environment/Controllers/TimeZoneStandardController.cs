#region Namespaces
using System.Text;
using AvivCRM.UI.Areas.Environment.ViewModels;
using AvivCRM.UI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
#endregion

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class TimeZoneStandardController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    #region Constructor
    public TimeZoneStandardController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    #endregion

    #region Retrieves a List of TimeZone Standards
    /// <summary>
    /// Retrieves a list of TimeZone Standards from the database.
    /// </summary>
    /// <param name=""></param>
    /// <returns>Modal popup will open to create New TimeZone Standard</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/TimeZoneStandard/TimeZoneStandard
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    public async Task<IActionResult> TimeZoneStandard()
    {
        ViewData["pTitle"] = "TimeZone Standards Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "TimeZone Standard";
        ViewData["bChild"] = "TimeZone Standard View";
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        ApiResultResponse<List<TimeZoneStandardVM>> timeZoneStandardList = new();

        // fetch all the TimeZone Standards
        timeZoneStandardList =
                await client.GetFromJsonAsync<ApiResultResponse<List<TimeZoneStandardVM>>>("TimeZoneStandard/all-timezonestandard");

        return View(timeZoneStandardList!.Data);
    }
    #endregion

    #region Create TimeZone Standard functionionality
    /// <summary>
    /// Show the popup to create a new TimeZone Standard.
    /// </summary>
    /// <param name=""></param>
    /// <returns>New TimeZone Standard</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/TimeZoneStandard/TimeZoneStandard
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        TimeZoneStandardVM timeZoneStandard = new();
        return PartialView("_Create", timeZoneStandard);
    }

    /// <summary>
    /// New TimeZone Standard will be create.
    /// </summary>
    /// <param name="timeZoneStandard">TimeZone Standard entity that needs to be create</param>
    /// <returns>New TimeZone Standard will be create.</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/TimeZoneStandard/TimeZoneStandard
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Create(TimeZoneStandardVM timeZoneStandard)
    {
        ApiResultResponse<TimeZoneStandardVM> resultTimeZoneStandard = new();

        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (timeZoneStandard.Name == null)
        {
            timeZoneStandard.Name = "";
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        string? jsonTimeZoneStandard = JsonConvert.SerializeObject(timeZoneStandard);
        StringContent? timeZoneStandardContent = new(jsonTimeZoneStandard, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseTimeZoneStandard =
            await client.PostAsync("TimeZoneStandard/create-timezonestandard", timeZoneStandardContent);

        if (responseTimeZoneStandard.IsSuccessStatusCode)
        {
            string? jsonResponseTimeZoneStandard = await responseTimeZoneStandard.Content.ReadAsStringAsync();
            resultTimeZoneStandard = JsonConvert.DeserializeObject<ApiResultResponse<TimeZoneStandardVM>>(jsonResponseTimeZoneStandard);
        }
        else
        {
            string? errorContent = await responseTimeZoneStandard.Content.ReadAsStringAsync();
            resultTimeZoneStandard = new ApiResultResponse<TimeZoneStandardVM>
            {
                IsSuccess = false,
                Message = responseTimeZoneStandard.StatusCode + "ErrorContent: " + errorContent
            };
        }

        if (!resultTimeZoneStandard!.IsSuccess)
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

    #region Edit TimeZone Standard functionionality
    /// <summary>
    /// Edit the existing TimeZone Standard.
    /// </summary>
    /// <param name="Id">TimeZone Standard Guid that needs to be edit</param>
    /// <returns>Popup will be open to edit the timezone standard details</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/TimeZoneStandard/TimeZoneStandard
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

        ApiResultResponse<TimeZoneStandardVM> timeZoneStandard = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        timeZoneStandard =
            await client.GetFromJsonAsync<ApiResultResponse<TimeZoneStandardVM>>("TimeZoneStandard/byid-timezonestandard/?Id=" + Id);

        if (!timeZoneStandard!.IsSuccess)
        {
            return View();
        }

        return PartialView("_Edit", timeZoneStandard.Data);
    }

    /// <summary>
    /// Update the existing TimeZone Standard.
    /// </summary>
    /// <param name="timeZoneStandard">TimeZoneStandard entity to update the existing timezone standard</param>
    /// <returns>Changes will be updated for the existing timezone standard</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/TimeZoneStandard/TimeZoneStandard
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Edit(TimeZoneStandardVM timeZoneStandard)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (timeZoneStandard.Name == null)
        {
            timeZoneStandard.Name = "";
        }

        ApiResultResponse<TimeZoneStandardVM> resultTimeZoneStandard = new();

        if (GuidExtensions.IsNullOrEmpty(timeZoneStandard.Id))
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        string? jsonTimeZoneStandard = JsonConvert.SerializeObject(timeZoneStandard);
        StringContent? timeZoneStandardContent = new(jsonTimeZoneStandard, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseTimeZoneStandard =
            await client.PutAsync("TimeZoneStandard/update-timezonestandard/", timeZoneStandardContent);
        if (responseTimeZoneStandard.IsSuccessStatusCode)
        {
            string? jsonResponseTimeZoneStandard = await responseTimeZoneStandard.Content.ReadAsStringAsync();
            resultTimeZoneStandard = JsonConvert.DeserializeObject<ApiResultResponse<TimeZoneStandardVM>>(jsonResponseTimeZoneStandard);
        }
        else
        {
            string? errorContent = await responseTimeZoneStandard.Content.ReadAsStringAsync();
            resultTimeZoneStandard = new ApiResultResponse<TimeZoneStandardVM>
            {
                IsSuccess = false,
                Message = responseTimeZoneStandard.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        if (!resultTimeZoneStandard!.IsSuccess)
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

    #region Delete TimeZone Standard functionionality
    /// <summary>
    /// Delete the existing TimeZone Standard.
    /// </summary>
    /// <param name="Id">TimeZone Standard Guid that needs to be delete</param>
    /// <returns>TimeZone standard will be deleted</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/TimeZoneStandard/TimeZoneStandard
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

        ApiResultResponse<TimeZoneStandardVM> resultTimeZoneStandard = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? responseTimeZoneStandard = await client.DeleteAsync("TimeZoneStandard/delete-timezonestandard?Id=" + Id);
        if (responseTimeZoneStandard.IsSuccessStatusCode)
        {
            string? jsonResponseTimeZoneStandard = await responseTimeZoneStandard.Content.ReadAsStringAsync();
            resultTimeZoneStandard = JsonConvert.DeserializeObject<ApiResultResponse<TimeZoneStandardVM>>(jsonResponseTimeZoneStandard);
        }
        else
        {
            string? errorContent = await responseTimeZoneStandard.Content.ReadAsStringAsync();
            resultTimeZoneStandard = new ApiResultResponse<TimeZoneStandardVM>
            {
                IsSuccess = false,
                Message = responseTimeZoneStandard.StatusCode.ToString()
            };
        }

        if (!resultTimeZoneStandard!.IsSuccess)
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






















