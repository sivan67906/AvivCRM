#region Namespaces
using System.Text;
using AvivCRM.UI.Areas.Environment.ViewModels;
using AvivCRM.UI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
#endregion

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class CustomQuestionCategoryController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    #region Constructor
    public CustomQuestionCategoryController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    #endregion

    #region Retrieves a List of Custom Question Categories
    /// <summary>
    /// Retrieves a list of Custom Question Categories from the database.
    /// </summary>
    /// <param name=""></param>
    /// <returns>Modal popup will open to create New Custom Question Category</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/CustomQuestionCategory/CustomQuestionCategory
    /// </example>
    /// <remarks> 
    /// Created: 10-Jan-2025 by Sivan T
    /// </remarks>
    public async Task<IActionResult> CustomQuestionCategory()
    {
        ViewData["pTitle"] = "Custom Question Categories Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "Custom Question Category";
        ViewData["bChild"] = "Custom Question Category View";
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        ApiResultResponse<List<CustomQuestionCategoryVM>> customQuestionCategoryList = new();

        // fetch all the Custom Question Categories
        customQuestionCategoryList =
                await client.GetFromJsonAsync<ApiResultResponse<List<CustomQuestionCategoryVM>>>("CustomQuestionCategory/all-customquestioncategory");

        return View(customQuestionCategoryList!.Data);
    }
    #endregion

    #region Create Custom Question Category functionionality
    /// <summary>
    /// Show the popup to create a new Custom Question Category.
    /// </summary>
    /// <param name=""></param>
    /// <returns>New Custom Question Category</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/CustomQuestionCategory/CustomQuestionCategory
    /// </example>
    /// <remarks> 
    /// Created: 10-Jan-2025 by Sivan T
    /// </remarks>
    [HttpGet]
    public IActionResult Create()
    {
        CustomQuestionCategoryVM customQuestionCategory = new();
        return PartialView("_Create", customQuestionCategory);
    }

    /// <summary>
    /// New Custom Question Category will be create.
    /// </summary>
    /// <param name="customQuestionCategory">Custom Question Category entity that needs to be create</param>
    /// <returns>New Custom Question Category will be create.</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/CustomQuestionCategory/CustomQuestionCategory
    /// </example>
    /// <remarks> 
    /// Created: 10-Jan-2025 by Sivan T
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Create(CustomQuestionCategoryVM customQuestionCategory)
    {
        ApiResultResponse<CustomQuestionCategoryVM> resultCustomQuestionCategory = new();

        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        string? jsonCustomQuestionCategory = JsonConvert.SerializeObject(customQuestionCategory);
        StringContent? customQuestionCategoryContent = new(jsonCustomQuestionCategory, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseCustomQuestionCategory =
            await client.PostAsync("CustomQuestionCategory/create-customquestioncategory", customQuestionCategoryContent);

        if (responseCustomQuestionCategory.IsSuccessStatusCode)
        {
            string? jsonResponseCustomQuestionCategory = await responseCustomQuestionCategory.Content.ReadAsStringAsync();
            resultCustomQuestionCategory = JsonConvert.DeserializeObject<ApiResultResponse<CustomQuestionCategoryVM>>(jsonResponseCustomQuestionCategory);
        }
        else
        {
            string? errorContent = await responseCustomQuestionCategory.Content.ReadAsStringAsync();
            resultCustomQuestionCategory = new ApiResultResponse<CustomQuestionCategoryVM>
            {
                IsSuccess = false,
                Message = responseCustomQuestionCategory.StatusCode + "ErrorContent: " + errorContent
            };
        }

        if (!resultCustomQuestionCategory!.IsSuccess)
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

    #region Edit Custom Question Category functionionality
    /// <summary>
    /// Edit the existing Custom Question Category.
    /// </summary>
    /// <param name="Id">Custom Question Category Guid that needs to be edit</param>
    /// <returns>Popup will be open to edit the custom questioncategory details</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/CustomQuestionCategory/CustomQuestionCategory
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

        ApiResultResponse<CustomQuestionCategoryVM> customQuestionCategory = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        customQuestionCategory =
            await client.GetFromJsonAsync<ApiResultResponse<CustomQuestionCategoryVM>>("CustomQuestionCategory/byid-customquestioncategory/?Id=" + Id);

        if (!customQuestionCategory!.IsSuccess)
        {
            return View();
        }

        return PartialView("_Edit", customQuestionCategory.Data);
    }

    /// <summary>
    /// Update the existing Custom Question Category.
    /// </summary>
    /// <param name="customQuestionCategory">CustomQuestionCategory entity to update the existing custom questioncategory</param>
    /// <returns>Changes will be updated for the existing custom questioncategory</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/CustomQuestionCategory/CustomQuestionCategory
    /// </example>
    /// <remarks> 
    /// Created: 10-Jan-2025 by Sivan T
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Edit(CustomQuestionCategoryVM customQuestionCategory)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        ApiResultResponse<CustomQuestionCategoryVM> resultCustomQuestionCategory = new();

        if (GuidExtensions.IsNullOrEmpty(customQuestionCategory.Id))
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        string? jsonCustomQuestionCategory = JsonConvert.SerializeObject(customQuestionCategory);
        StringContent? customQuestionCategoryContent = new(jsonCustomQuestionCategory, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseCustomQuestionCategory =
            await client.PutAsync("CustomQuestionCategory/update-customquestioncategory/", customQuestionCategoryContent);
        if (responseCustomQuestionCategory.IsSuccessStatusCode)
        {
            string? jsonResponseCustomQuestionCategory = await responseCustomQuestionCategory.Content.ReadAsStringAsync();
            resultCustomQuestionCategory = JsonConvert.DeserializeObject<ApiResultResponse<CustomQuestionCategoryVM>>(jsonResponseCustomQuestionCategory);
        }
        else
        {
            string? errorContent = await responseCustomQuestionCategory.Content.ReadAsStringAsync();
            resultCustomQuestionCategory = new ApiResultResponse<CustomQuestionCategoryVM>
            {
                IsSuccess = false,
                Message = responseCustomQuestionCategory.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        if (!resultCustomQuestionCategory!.IsSuccess)
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

    #region Delete Custom Question Category functionionality
    /// <summary>
    /// Delete the existing Custom Question Category.
    /// </summary>
    /// <param name="Id">Custom Question Category Guid that needs to be delete</param>
    /// <returns>Custom question category will be deleted</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/CustomQuestionCategory/CustomQuestionCategory
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

        ApiResultResponse<CustomQuestionCategoryVM> resultCustomQuestionCategory = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? responseCustomQuestionCategory = await client.DeleteAsync("CustomQuestionCategory/delete-customquestioncategory?Id=" + Id);
        if (responseCustomQuestionCategory.IsSuccessStatusCode)
        {
            string? jsonResponseCustomQuestionCategory = await responseCustomQuestionCategory.Content.ReadAsStringAsync();
            resultCustomQuestionCategory = JsonConvert.DeserializeObject<ApiResultResponse<CustomQuestionCategoryVM>>(jsonResponseCustomQuestionCategory);
        }
        else
        {
            string? errorContent = await responseCustomQuestionCategory.Content.ReadAsStringAsync();
            resultCustomQuestionCategory = new ApiResultResponse<CustomQuestionCategoryVM>
            {
                IsSuccess = false,
                Message = responseCustomQuestionCategory.StatusCode.ToString()
            };
        }

        if (!resultCustomQuestionCategory!.IsSuccess)
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






















