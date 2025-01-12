#region Namespaces
using System.Text;
using AvivCRM.UI.Areas.Environment.ViewModels;
using AvivCRM.UI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
#endregion

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class LanguageController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    #region Constructor
    public LanguageController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    #endregion

    #region Retrieves a List of Languages
    /// <summary>
    /// Retrieves a list of Languages from the database.
    /// </summary>
    /// <param name=""></param>
    /// <returns>Modal popup will open to create New Language</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/Language/Language
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    public async Task<IActionResult> Language()
    {
        ViewData["pTitle"] = "Languages Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "Language";
        ViewData["bChild"] = "Language View";
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        ApiResultResponse<List<LanguageVM>> languageList = new();

        // fetch all the Languages
        languageList =
                await client.GetFromJsonAsync<ApiResultResponse<List<LanguageVM>>>("Language/all-language");

        return View(languageList!.Data);
    }
    #endregion

    #region Create Language functionionality
    /// <summary>
    /// Show the popup to create a new Language.
    /// </summary>
    /// <param name=""></param>
    /// <returns>New Language</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/Language/Language
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        LanguageVM language = new();
        return PartialView("_Create", language);
    }

    /// <summary>
    /// New Language will be create.
    /// </summary>
    /// <param name="language">Language entity that needs to be create</param>
    /// <returns>New Language will be create.</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/Language/Language
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Create(LanguageVM language)
    {
        ApiResultResponse<LanguageVM> resultLanguage = new();

        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        //if (language.Name == null)
        //{
        //    language.Name = "";
        //}

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        string? jsonLanguage = JsonConvert.SerializeObject(language);
        StringContent? languageContent = new(jsonLanguage, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseLanguage =
            await client.PostAsync("Language/create-language", languageContent);

        if (responseLanguage.IsSuccessStatusCode)
        {
            string? jsonResponseLanguage = await responseLanguage.Content.ReadAsStringAsync();
            resultLanguage = JsonConvert.DeserializeObject<ApiResultResponse<LanguageVM>>(jsonResponseLanguage);
        }
        else
        {
            string? errorContent = await responseLanguage.Content.ReadAsStringAsync();
            resultLanguage = new ApiResultResponse<LanguageVM>
            {
                IsSuccess = false,
                Message = responseLanguage.StatusCode + "ErrorContent: " + errorContent
            };
        }

        if (!resultLanguage!.IsSuccess)
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

    #region Edit Language functionionality
    /// <summary>
    /// Edit the existing Language.
    /// </summary>
    /// <param name="Id">Language Guid that needs to be edit</param>
    /// <returns>Popup will be open to edit the language details</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/Language/Language
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

        ApiResultResponse<LanguageVM> language = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        language =
            await client.GetFromJsonAsync<ApiResultResponse<LanguageVM>>("Language/byid-language/?Id=" + Id);

        if (!language!.IsSuccess)
        {
            return View();
        }

        return PartialView("_Edit", language.Data);
    }

    /// <summary>
    /// Update the existing Language.
    /// </summary>
    /// <param name="language">Language entity to update the existing language</param>
    /// <returns>Changes will be updated for the existing language</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/Language/Language
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Edit(LanguageVM language)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        //if (language.Name == null)
        //{
        //    language.Name = "";
        //}

        ApiResultResponse<LanguageVM> resultLanguage = new();

        if (GuidExtensions.IsNullOrEmpty(language.Id))
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        string? jsonLanguage = JsonConvert.SerializeObject(language);
        StringContent? languageContent = new(jsonLanguage, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseLanguage =
            await client.PutAsync("Language/update-language/", languageContent);
        if (responseLanguage.IsSuccessStatusCode)
        {
            string? jsonResponseLanguage = await responseLanguage.Content.ReadAsStringAsync();
            resultLanguage = JsonConvert.DeserializeObject<ApiResultResponse<LanguageVM>>(jsonResponseLanguage);
        }
        else
        {
            string? errorContent = await responseLanguage.Content.ReadAsStringAsync();
            resultLanguage = new ApiResultResponse<LanguageVM>
            {
                IsSuccess = false,
                Message = responseLanguage.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        if (!resultLanguage!.IsSuccess)
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

    #region Delete Language functionionality
    /// <summary>
    /// Delete the existing Language.
    /// </summary>
    /// <param name="Id">Language Guid that needs to be delete</param>
    /// <returns>Language will be deleted</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/Language/Language
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

        ApiResultResponse<LanguageVM> resultLanguage = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? responseLanguage = await client.DeleteAsync("Language/delete-language?Id=" + Id);
        if (responseLanguage.IsSuccessStatusCode)
        {
            string? jsonResponseLanguage = await responseLanguage.Content.ReadAsStringAsync();
            resultLanguage = JsonConvert.DeserializeObject<ApiResultResponse<LanguageVM>>(jsonResponseLanguage);
        }
        else
        {
            string? errorContent = await responseLanguage.Content.ReadAsStringAsync();
            resultLanguage = new ApiResultResponse<LanguageVM>
            {
                IsSuccess = false,
                Message = responseLanguage.StatusCode.ToString()
            };
        }

        if (!resultLanguage!.IsSuccess)
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






















