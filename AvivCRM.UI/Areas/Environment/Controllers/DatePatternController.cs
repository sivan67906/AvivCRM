#region Namespaces
using System.Text;
using AvivCRM.UI.Areas.Environment.ViewModels;
using AvivCRM.UI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
#endregion

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class DatePatternController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    #region Constructor
    public DatePatternController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    #endregion

    #region Retrieves a List of DatePatterns
    /// <summary>
    /// Retrieves a list of DatePatterns from the database.
    /// </summary>
    /// <param name=""></param>
    /// <returns>Modal popup will open to create New DatePattern</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/DatePattern/DatePattern
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    public async Task<IActionResult> DatePattern()
    {
        ViewData["pTitle"] = "DatePatterns Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "DatePattern";
        ViewData["bChild"] = "DatePattern View";
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        ApiResultResponse<List<DatePatternVM>> datePatternList = new();

        // fetch all the DatePatterns
        datePatternList =
                await client.GetFromJsonAsync<ApiResultResponse<List<DatePatternVM>>>("DatePattern/all-datepattern");

        return View(datePatternList!.Data);
    }
    #endregion

    #region Create DatePattern functionionality
    /// <summary>
    /// Show the popup to create a new DatePattern.
    /// </summary>
    /// <param name=""></param>
    /// <returns>New DatePattern</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/DatePattern/DatePattern
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        DatePatternVM datePattern = new();
        return PartialView("_Create", datePattern);
    }

    /// <summary>
    /// New DatePattern will be create.
    /// </summary>
    /// <param name="datePattern">DatePattern entity that needs to be create</param>
    /// <returns>New DatePattern will be create.</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/DatePattern/DatePattern
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Create(DatePatternVM datePattern)
    {
        ApiResultResponse<DatePatternVM> resultDatePattern = new();

        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (datePattern.Name == null)
        {
            datePattern.Name = "";
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        string? jsonDatePattern = JsonConvert.SerializeObject(datePattern);
        StringContent? datePatternContent = new(jsonDatePattern, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseDatePattern =
            await client.PostAsync("DatePattern/create-datepattern", datePatternContent);

        if (responseDatePattern.IsSuccessStatusCode)
        {
            string? jsonResponseDatePattern = await responseDatePattern.Content.ReadAsStringAsync();
            resultDatePattern = JsonConvert.DeserializeObject<ApiResultResponse<DatePatternVM>>(jsonResponseDatePattern);
        }
        else
        {
            string? errorContent = await responseDatePattern.Content.ReadAsStringAsync();
            resultDatePattern = new ApiResultResponse<DatePatternVM>
            {
                IsSuccess = false,
                Message = responseDatePattern.StatusCode + "ErrorContent: " + errorContent
            };
        }

        if (!resultDatePattern!.IsSuccess)
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

    #region Edit DatePattern functionionality
    /// <summary>
    /// Edit the existing DatePattern.
    /// </summary>
    /// <param name="Id">DatePattern Guid that needs to be edit</param>
    /// <returns>Popup will be open to edit the datepattern details</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/DatePattern/DatePattern
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

        ApiResultResponse<DatePatternVM> datePattern = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        datePattern =
            await client.GetFromJsonAsync<ApiResultResponse<DatePatternVM>>("DatePattern/byid-datepattern/?Id=" + Id);

        if (!datePattern!.IsSuccess)
        {
            return View();
        }

        return PartialView("_Edit", datePattern.Data);
    }

    /// <summary>
    /// Update the existing DatePattern.
    /// </summary>
    /// <param name="datePattern">DatePattern entity to update the existing datepattern</param>
    /// <returns>Changes will be updated for the existing datepattern</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/DatePattern/DatePattern
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Edit(DatePatternVM datePattern)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (datePattern.Name == null)
        {
            datePattern.Name = "";
        }

        ApiResultResponse<DatePatternVM> resultDatePattern = new();

        if (GuidExtensions.IsNullOrEmpty(datePattern.Id))
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        string? jsonDatePattern = JsonConvert.SerializeObject(datePattern);
        StringContent? datePatternContent = new(jsonDatePattern, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseDatePattern =
            await client.PutAsync("DatePattern/update-datepattern/", datePatternContent);
        if (responseDatePattern.IsSuccessStatusCode)
        {
            string? jsonResponseDatePattern = await responseDatePattern.Content.ReadAsStringAsync();
            resultDatePattern = JsonConvert.DeserializeObject<ApiResultResponse<DatePatternVM>>(jsonResponseDatePattern);
        }
        else
        {
            string? errorContent = await responseDatePattern.Content.ReadAsStringAsync();
            resultDatePattern = new ApiResultResponse<DatePatternVM>
            {
                IsSuccess = false,
                Message = responseDatePattern.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        if (!resultDatePattern!.IsSuccess)
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

    #region Delete DatePattern functionionality
    /// <summary>
    /// Delete the existing DatePattern.
    /// </summary>
    /// <param name="Id">DatePattern Guid that needs to be delete</param>
    /// <returns>Datepattern will be deleted</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/DatePattern/DatePattern
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

        ApiResultResponse<DatePatternVM> resultDatePattern = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? responseDatePattern = await client.DeleteAsync("DatePattern/delete-datepattern?Id=" + Id);
        if (responseDatePattern.IsSuccessStatusCode)
        {
            string? jsonResponseDatePattern = await responseDatePattern.Content.ReadAsStringAsync();
            resultDatePattern = JsonConvert.DeserializeObject<ApiResultResponse<DatePatternVM>>(jsonResponseDatePattern);
        }
        else
        {
            string? errorContent = await responseDatePattern.Content.ReadAsStringAsync();
            resultDatePattern = new ApiResultResponse<DatePatternVM>
            {
                IsSuccess = false,
                Message = responseDatePattern.StatusCode.ToString()
            };
        }

        if (!resultDatePattern!.IsSuccess)
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






















