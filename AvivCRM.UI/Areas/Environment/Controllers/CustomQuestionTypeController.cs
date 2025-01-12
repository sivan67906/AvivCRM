#region Namespaces
using System.Text;
using AvivCRM.UI.Areas.Environment.ViewModels;
using AvivCRM.UI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
#endregion

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class CustomQuestionTypeController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    #region Constructor
    public CustomQuestionTypeController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    #endregion

    #region Retrieves a List of Custom Question Types
    /// <summary>
    /// Retrieves a list of Custom Question Types from the database.
    /// </summary>
    /// <param name=""></param>
    /// <returns>Modal popup will open to create New Custom Question Type</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/CustomQuestionType/CustomQuestionType
    /// </example>
    /// <remarks> 
    /// Created: 10-Jan-2025 by Sivan T
    /// </remarks>
    public async Task<IActionResult> CustomQuestionType()
    {
        ViewData["pTitle"] = "Custom Question Types Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "Custom Question Type";
        ViewData["bChild"] = "Custom Question Type View";
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        ApiResultResponse<List<CustomQuestionTypeVM>> customQuestionTypeList = new();

        // fetch all the Custom Question Types
        customQuestionTypeList =
                await client.GetFromJsonAsync<ApiResultResponse<List<CustomQuestionTypeVM>>>("CustomQuestionType/all-customquestiontype");

        return View(customQuestionTypeList!.Data);
    }
    #endregion

    #region Create Custom Question Type functionionality
    /// <summary>
    /// Show the popup to create a new Custom Question Type.
    /// </summary>
    /// <param name=""></param>
    /// <returns>New Custom Question Type</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/CustomQuestionType/CustomQuestionType
    /// </example>
    /// <remarks> 
    /// Created: 10-Jan-2025 by Sivan T
    /// </remarks>
    [HttpGet]
    public IActionResult Create()
    {
        CustomQuestionTypeVM customQuestionType = new();
        return PartialView("_Create", customQuestionType);
    }

    /// <summary>
    /// New Custom Question Type will be create.
    /// </summary>
    /// <param name="customQuestionType">Custom Question Type entity that needs to be create</param>
    /// <returns>New Custom Question Type will be create.</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/CustomQuestionType/CustomQuestionType
    /// </example>
    /// <remarks> 
    /// Created: 10-Jan-2025 by Sivan T
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Create(CustomQuestionTypeVM customQuestionType)
    {
        ApiResultResponse<CustomQuestionTypeVM> resultCustomQuestionType = new();

        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        string? jsonCustomQuestionType = JsonConvert.SerializeObject(customQuestionType);
        StringContent? customQuestionTypeContent = new(jsonCustomQuestionType, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseCustomQuestionType =
            await client.PostAsync("CustomQuestionType/create-customquestiontype", customQuestionTypeContent);

        if (responseCustomQuestionType.IsSuccessStatusCode)
        {
            string? jsonResponseCustomQuestionType = await responseCustomQuestionType.Content.ReadAsStringAsync();
            resultCustomQuestionType = JsonConvert.DeserializeObject<ApiResultResponse<CustomQuestionTypeVM>>(jsonResponseCustomQuestionType);
        }
        else
        {
            string? errorContent = await responseCustomQuestionType.Content.ReadAsStringAsync();
            resultCustomQuestionType = new ApiResultResponse<CustomQuestionTypeVM>
            {
                IsSuccess = false,
                Message = responseCustomQuestionType.StatusCode + "ErrorContent: " + errorContent
            };
        }

        if (!resultCustomQuestionType!.IsSuccess)
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

    #region Edit Custom Question Type functionionality
    /// <summary>
    /// Edit the existing Custom Question Type.
    /// </summary>
    /// <param name="Id">Custom Question Type Guid that needs to be edit</param>
    /// <returns>Popup will be open to edit the custom question type details</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/CustomQuestionType/CustomQuestionType
    /// </example>
    /// <remarks> 
    /// Created: 10-Jan-2025 by Sivan T
    /// </remarks>
    [HttpGet]
    public async Task<IActionResult> Edit(Guid Id)
    {
        if (GuidExtensions.IsNullOrEmpty(Id))
        {
            return View();
        }

        ApiResultResponse<CustomQuestionTypeVM> customQuestionType = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        customQuestionType =
            await client.GetFromJsonAsync<ApiResultResponse<CustomQuestionTypeVM>>("CustomQuestionType/byid-customquestiontype/?Id=" + Id);

        if (!customQuestionType!.IsSuccess)
        {
            return View();
        }

        return PartialView("_Edit", customQuestionType.Data);
    }

    /// <summary>
    /// Update the existing Custom Question Type.
    /// </summary>
    /// <param name="customQuestionType">CustomQuestionType entity to update the existing custom question type</param>
    /// <returns>Changes will be updated for the existing custom question type</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/CustomQuestionType/CustomQuestionType
    /// </example>
    /// <remarks> 
    /// Created: 10-Jan-2025 by Sivan T
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Edit(CustomQuestionTypeVM customQuestionType)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        ApiResultResponse<CustomQuestionTypeVM> resultCustomQuestionType = new();

        if (GuidExtensions.IsNullOrEmpty(customQuestionType.Id))
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        string? jsonCustomQuestionType = JsonConvert.SerializeObject(customQuestionType);
        StringContent? customQuestionTypeContent = new(jsonCustomQuestionType, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseCustomQuestionType =
            await client.PutAsync("CustomQuestionType/update-customquestiontype/", customQuestionTypeContent);
        if (responseCustomQuestionType.IsSuccessStatusCode)
        {
            string? jsonResponseCustomQuestionType = await responseCustomQuestionType.Content.ReadAsStringAsync();
            resultCustomQuestionType = JsonConvert.DeserializeObject<ApiResultResponse<CustomQuestionTypeVM>>(jsonResponseCustomQuestionType);
        }
        else
        {
            string? errorContent = await responseCustomQuestionType.Content.ReadAsStringAsync();
            resultCustomQuestionType = new ApiResultResponse<CustomQuestionTypeVM>
            {
                IsSuccess = false,
                Message = responseCustomQuestionType.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        if (!resultCustomQuestionType!.IsSuccess)
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

    #region Delete Custom Question Type functionionality
    /// <summary>
    /// Delete the existing Custom Question Type.
    /// </summary>
    /// <param name="Id">Custom Question Type Guid that needs to be delete</param>
    /// <returns>Custom question type will be deleted</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/CustomQuestionType/CustomQuestionType
    /// </example>
    /// <remarks> 
    /// Created: 10-Jan-2025 by Sivan T
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

        ApiResultResponse<CustomQuestionTypeVM> resultCustomQuestionType = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? responseCustomQuestionType = await client.DeleteAsync("CustomQuestionType/delete-customquestiontype?Id=" + Id);
        if (responseCustomQuestionType.IsSuccessStatusCode)
        {
            string? jsonResponseCustomQuestionType = await responseCustomQuestionType.Content.ReadAsStringAsync();
            resultCustomQuestionType = JsonConvert.DeserializeObject<ApiResultResponse<CustomQuestionTypeVM>>(jsonResponseCustomQuestionType);
        }
        else
        {
            string? errorContent = await responseCustomQuestionType.Content.ReadAsStringAsync();
            resultCustomQuestionType = new ApiResultResponse<CustomQuestionTypeVM>
            {
                IsSuccess = false,
                Message = responseCustomQuestionType.StatusCode.ToString()
            };
        }

        if (!resultCustomQuestionType!.IsSuccess)
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






















