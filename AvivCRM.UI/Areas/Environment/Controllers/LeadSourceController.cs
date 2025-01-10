#region Namespaces
using System.Text;
using AvivCRM.UI.Areas.Environment.ViewModels;
using AvivCRM.UI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
#endregion

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class LeadSourceController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    #region Constructor
    public LeadSourceController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    #endregion

    #region Retrieves a List of Lead Sources
    /// <summary>
    /// Retrieves a list of Lead Sources from the database.
    /// </summary>
    /// <param name=""></param>
    /// <returns>Modal popup will open to create New Lead Source</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/LeadSource/LeadSource
    /// </example>
    /// <remarks> 
    /// Created: 05-Jan-2025 by Sivan T
    /// </remarks>
    public async Task<IActionResult> LeadSource()
    {
        ViewData["pTitle"] = "Lead Sources Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "Lead Source";
        ViewData["bChild"] = "Lead Source View";
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        ApiResultResponse<List<LeadSourceVM>> leadSourceList = new();

        // fetch all the Lead Sources
        leadSourceList =
                await client.GetFromJsonAsync<ApiResultResponse<List<LeadSourceVM>>>("LeadSource/all-leadsource");

        return View(leadSourceList!.Data);
    }
    #endregion

    #region Create Lead Source functionionality
    /// <summary>
    /// Show the popup to create a new Lead Source.
    /// </summary>
    /// <param name=""></param>
    /// <returns>New Lead Source</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/LeadSource/LeadSource
    /// </example>
    /// <remarks> 
    /// Created: 05-Jan-2025 by Sivan T
    /// </remarks>
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        LeadSourceVM leadSource = new();
        return PartialView("_Create", leadSource);
    }

    /// <summary>
    /// New Lead Source will be create.
    /// </summary>
    /// <param name="leadSource">Lead Source entity that needs to be create</param>
    /// <returns>New Lead Source will be create.</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/LeadSource/LeadSource
    /// </example>
    /// <remarks> 
    /// Created: 05-Jan-2025 by Sivan T
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Create(LeadSourceVM leadSource)
    {
        ApiResultResponse<LeadSourceVM> resultLeadSource = new();

        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (leadSource.Name == null)
        {
            leadSource.Name = "";
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        string? jsonLeadSource = JsonConvert.SerializeObject(leadSource);
        StringContent? leadSourceContent = new(jsonLeadSource, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseLeadSource =
            await client.PostAsync("LeadSource/create-leadsource", leadSourceContent);

        if (responseLeadSource.IsSuccessStatusCode)
        {
            string? jsonResponseLeadSource = await responseLeadSource.Content.ReadAsStringAsync();
            resultLeadSource = JsonConvert.DeserializeObject<ApiResultResponse<LeadSourceVM>>(jsonResponseLeadSource);
        }
        else
        {
            string? errorContent = await responseLeadSource.Content.ReadAsStringAsync();
            resultLeadSource = new ApiResultResponse<LeadSourceVM>
            {
                IsSuccess = false,
                Message = responseLeadSource.StatusCode + "ErrorContent: " + errorContent
            };
        }

        if (!resultLeadSource!.IsSuccess)
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

    #region Edit Lead Source functionionality
    /// <summary>
    /// Edit the existing Lead Source.
    /// </summary>
    /// <param name="Id">Lead Source Guid that needs to be edit</param>
    /// <returns>Popup will be open to edit the lead source details</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/LeadSource/LeadSource
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

        ApiResultResponse<LeadSourceVM> leadSource = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        leadSource =
            await client.GetFromJsonAsync<ApiResultResponse<LeadSourceVM>>("LeadSource/byid-leadsource/?Id=" + Id);

        if (!leadSource!.IsSuccess)
        {
            return View();
        }

        return PartialView("_Edit", leadSource.Data);
    }

    /// <summary>
    /// Update the existing Lead Source.
    /// </summary>
    /// <param name="leadSource">LeadSource entity to update the existing lead source</param>
    /// <returns>Changes will be updated for the existing lead source</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/LeadSource/LeadSource
    /// </example>
    /// <remarks> 
    /// Created: 05-Jan-2025 by Sivan T
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Edit(LeadSourceVM leadSource)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (leadSource.Name == null)
        {
            leadSource.Name = "";
        }

        ApiResultResponse<LeadSourceVM> resultLeadSource = new();

        if (GuidExtensions.IsNullOrEmpty(leadSource.Id))
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        string? jsonLeadSource = JsonConvert.SerializeObject(leadSource);
        StringContent? leadSourceContent = new(jsonLeadSource, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseLeadSource =
            await client.PutAsync("LeadSource/update-leadsource/", leadSourceContent);
        if (responseLeadSource.IsSuccessStatusCode)
        {
            string? jsonResponseLeadSource = await responseLeadSource.Content.ReadAsStringAsync();
            resultLeadSource = JsonConvert.DeserializeObject<ApiResultResponse<LeadSourceVM>>(jsonResponseLeadSource);
        }
        else
        {
            string? errorContent = await responseLeadSource.Content.ReadAsStringAsync();
            resultLeadSource = new ApiResultResponse<LeadSourceVM>
            {
                IsSuccess = false,
                Message = responseLeadSource.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        if (!resultLeadSource!.IsSuccess)
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

    #region Delete Lead Source functionionality
    /// <summary>
    /// Delete the existing Lead Source.
    /// </summary>
    /// <param name="Id">Lead Source Guid that needs to be delete</param>
    /// <returns>Lead source will be deleted</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/LeadSource/LeadSource
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

        ApiResultResponse<LeadSourceVM> resultLeadSource = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? responseLeadSource = await client.DeleteAsync("LeadSource/delete-leadsource?Id=" + Id);
        if (responseLeadSource.IsSuccessStatusCode)
        {
            string? jsonResponseLeadSource = await responseLeadSource.Content.ReadAsStringAsync();
            resultLeadSource = JsonConvert.DeserializeObject<ApiResultResponse<LeadSourceVM>>(jsonResponseLeadSource);
        }
        else
        {
            string? errorContent = await responseLeadSource.Content.ReadAsStringAsync();
            resultLeadSource = new ApiResultResponse<LeadSourceVM>
            {
                IsSuccess = false,
                Message = responseLeadSource.StatusCode.ToString()
            };
        }

        if (!resultLeadSource!.IsSuccess)
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