#region Namespaces
using System.Text;
using AvivCRM.UI.Areas.Environment.ViewModels;
using AvivCRM.UI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
#endregion

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class ToggleValueController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    #region Constructor
    public ToggleValueController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    #endregion

    #region Retrieves a List of Toggle Values
    /// <summary>
    /// Retrieves a list of Toggle Values from the database.
    /// </summary>
    /// <param name=""></param>
    /// <returns>Modal popup will open to create New Toggle Value</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/ToggleValue/ToggleValue
    /// </example>
    /// <remarks> 
    /// Created: 05-Jan-2025 by Sivan T
    /// </remarks>
    public async Task<IActionResult> ToggleValue()
    {
        ViewData["pTitle"] = "Toggle Values Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "Toggle Value";
        ViewData["bChild"] = "Toggle Value View";
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        ApiResultResponse<List<ToggleValueVM>> toggleValueList = new();

        // fetch all the Toggle Values
        toggleValueList =
                await client.GetFromJsonAsync<ApiResultResponse<List<ToggleValueVM>>>("ToggleValue/all-togglevalue");

        return View(toggleValueList!.Data);
    }
    #endregion

    #region Create Toggle Value functionionality
    /// <summary>
    /// Show the popup to create a new Toggle Value.
    /// </summary>
    /// <param name=""></param>
    /// <returns>New Toggle Value</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/ToggleValue/ToggleValue
    /// </example>
    /// <remarks> 
    /// Created: 05-Jan-2025 by Sivan T
    /// </remarks>
    [HttpGet]
    public IActionResult Create()
    {
        ToggleValueVM toggleValue = new();
        return PartialView("_Create", toggleValue);
    }

    /// <summary>
    /// New Toggle Value will be create.
    /// </summary>
    /// <param name="toggleValue">Toggle Value entity that needs to be create</param>
    /// <returns>New Toggle Value will be create.</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/ToggleValue/ToggleValue
    /// </example>
    /// <remarks> 
    /// Created: 05-Jan-2025 by Sivan T
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Create(ToggleValueVM toggleValue)
    {
        ApiResultResponse<ToggleValueVM> resultToggleValue = new();

        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (toggleValue.Name == null)
        {
            toggleValue.Name = "";
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        string? jsonToggleValue = JsonConvert.SerializeObject(toggleValue);
        StringContent? toggleValueContent = new(jsonToggleValue, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseToggleValue =
            await client.PostAsync("ToggleValue/create-togglevalue", toggleValueContent);

        if (responseToggleValue.IsSuccessStatusCode)
        {
            string? jsonResponseToggleValue = await responseToggleValue.Content.ReadAsStringAsync();
            resultToggleValue = JsonConvert.DeserializeObject<ApiResultResponse<ToggleValueVM>>(jsonResponseToggleValue);
        }
        else
        {
            string? errorContent = await responseToggleValue.Content.ReadAsStringAsync();
            resultToggleValue = new ApiResultResponse<ToggleValueVM>
            {
                IsSuccess = false,
                Message = responseToggleValue.StatusCode + "ErrorContent: " + errorContent
            };
        }

        if (!resultToggleValue!.IsSuccess)
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

    #region Edit Toggle Value functionionality
    /// <summary>
    /// Edit the existing Toggle Value.
    /// </summary>
    /// <param name="Id">Toggle Value Guid that needs to be edit</param>
    /// <returns>Popup will be open to edit the toggle value details</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/ToggleValue/ToggleValue
    /// </example>
    /// <remarks> 
    /// Created: 05-Jan-2025 by Sivan T
    /// </remarks>
    [HttpGet]
    public async Task<IActionResult> Edit(Guid Id)
    {
        if (GuidExtensions.IsNullOrEmpty(Id))
        {
            return View();
        }

        ApiResultResponse<ToggleValueVM> toggleValue = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        toggleValue =
            await client.GetFromJsonAsync<ApiResultResponse<ToggleValueVM>>("ToggleValue/byid-togglevalue/?Id=" + Id);

        if (!toggleValue!.IsSuccess)
        {
            return View();
        }

        return PartialView("_Edit", toggleValue.Data);
    }

    /// <summary>
    /// Update the existing Toggle Value.
    /// </summary>
    /// <param name="toggleValue">ToggleValue entity to update the existing toggle value</param>
    /// <returns>Changes will be updated for the existing toggle value</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/ToggleValue/ToggleValue
    /// </example>
    /// <remarks> 
    /// Created: 05-Jan-2025 by Sivan T
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Edit(ToggleValueVM toggleValue)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (toggleValue.Name == null)
        {
            toggleValue.Name = "";
        }

        ApiResultResponse<ToggleValueVM> resultToggleValue = new();

        if (GuidExtensions.IsNullOrEmpty(toggleValue.Id))
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        string? jsonToggleValue = JsonConvert.SerializeObject(toggleValue);
        StringContent? toggleValueContent = new(jsonToggleValue, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseToggleValue =
            await client.PutAsync("ToggleValue/update-togglevalue/", toggleValueContent);
        if (responseToggleValue.IsSuccessStatusCode)
        {
            string? jsonResponseToggleValue = await responseToggleValue.Content.ReadAsStringAsync();
            resultToggleValue = JsonConvert.DeserializeObject<ApiResultResponse<ToggleValueVM>>(jsonResponseToggleValue);
        }
        else
        {
            string? errorContent = await responseToggleValue.Content.ReadAsStringAsync();
            resultToggleValue = new ApiResultResponse<ToggleValueVM>
            {
                IsSuccess = false,
                Message = responseToggleValue.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        if (!resultToggleValue!.IsSuccess)
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

    #region Delete Toggle Value functionionality
    /// <summary>
    /// Delete the existing Toggle Value.
    /// </summary>
    /// <param name="Id">Toggle Value Guid that needs to be delete</param>
    /// <returns>Toggle value will be deleted</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/ToggleValue/ToggleValue
    /// </example>
    /// <remarks> 
    /// Created: 05-Jan-2025 by Sivan T
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

        ApiResultResponse<ToggleValueVM> resultToggleValue = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? responseToggleValue = await client.DeleteAsync("ToggleValue/delete-togglevalue?Id=" + Id);
        if (responseToggleValue.IsSuccessStatusCode)
        {
            string? jsonResponseToggleValue = await responseToggleValue.Content.ReadAsStringAsync();
            resultToggleValue = JsonConvert.DeserializeObject<ApiResultResponse<ToggleValueVM>>(jsonResponseToggleValue);
        }
        else
        {
            string? errorContent = await responseToggleValue.Content.ReadAsStringAsync();
            resultToggleValue = new ApiResultResponse<ToggleValueVM>
            {
                IsSuccess = false,
                Message = responseToggleValue.StatusCode.ToString()
            };
        }

        if (!resultToggleValue!.IsSuccess)
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








































