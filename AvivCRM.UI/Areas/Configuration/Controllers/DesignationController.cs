#region Namespaces
using System.Text;
using AvivCRM.UI.Areas.Configuration.ViewModels;
using AvivCRM.UI.Areas.Configuraton.ViewModels;
using AvivCRM.UI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
#endregion

namespace AvivCRM.UI.Areas.Configuraton.Controllers;
[Area("Environment")]
public class DesignationController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    #region Constructor
    public DesignationController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    #endregion

    #region Retrieves a List of Designations
    /// <summary>
    /// Retrieves a list of Designations from the database.
    /// </summary>
    /// <param name=""></param>
    /// <returns>Modal popup will open to create New Designation</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/Designation/Designation
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    public async Task<IActionResult> Designation()
    {
        ViewData["pTitle"] = "Designations Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "Designation";
        ViewData["bChild"] = "Designation View";
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        ApiResultResponseConfigVM<List<DesignationVM>> designationList = new();

        // fetch all the Designations
        designationList =
                await client.GetFromJsonAsync<ApiResultResponseConfigVM<List<DesignationVM>>>("Designation/all-designation");

        return View(designationList!.Data);
    }
    #endregion

    #region Create Designation functionionality
    /// <summary>
    /// Show the popup to create a new Designation.
    /// </summary>
    /// <param name=""></param>
    /// <returns>New Designation</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/Designation/Designation
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    [HttpGet]
    public IActionResult Create()
    {
        DesignationVM designation = new();
        return PartialView("_Create", designation);
    }

    /// <summary>
    /// New Designation will be create.
    /// </summary>
    /// <param name="designation">Designation entity that needs to be create</param>
    /// <returns>New Designation will be create.</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/Designation/Designation
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Create(DesignationVM designation)
    {
        ApiResultResponseConfigVM<DesignationVM> resultDesignation = new();

        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (designation.Name == null)
        {
            designation.Name = "";
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        string? jsonDesignation = JsonConvert.SerializeObject(designation);
        StringContent? designationContent = new(jsonDesignation, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseDesignation =
            await client.PostAsync("Designation/create-designation", designationContent);

        if (responseDesignation.IsSuccessStatusCode)
        {
            string? jsonResponseDesignation = await responseDesignation.Content.ReadAsStringAsync();
            resultDesignation = JsonConvert.DeserializeObject<ApiResultResponseConfigVM<DesignationVM>>(jsonResponseDesignation);
        }
        else
        {
            string? errorContent = await responseDesignation.Content.ReadAsStringAsync();
            resultDesignation = new ApiResultResponseConfigVM<DesignationVM>
            {
                IsSuccess = false,
                Message = responseDesignation.StatusCode + "ErrorContent: " + errorContent
            };
        }

        if (!resultDesignation!.IsSuccess)
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

    #region Edit Designation functionionality
    /// <summary>
    /// Edit the existing Designation.
    /// </summary>
    /// <param name="Id">Designation Guid that needs to be edit</param>
    /// <returns>Popup will be open to edit the designation details</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/Designation/Designation
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

        ApiResultResponseConfigVM<DesignationVM> designation = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        designation =
            await client.GetFromJsonAsync<ApiResultResponseConfigVM<DesignationVM>>("Designation/byid-designation/?Id=" + Id);

        if (!designation!.IsSuccess)
        {
            return View();
        }

        return PartialView("_Edit", designation.Data);
    }

    /// <summary>
    /// Update the existing Designation.
    /// </summary>
    /// <param name="designation">Designation entity to update the existing designation</param>
    /// <returns>Changes will be updated for the existing designation</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/Designation/Designation
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Edit(DesignationVM designation)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (designation.Name == null)
        {
            designation.Name = "";
        }

        ApiResultResponseConfigVM<DesignationVM> resultDesignation = new();

        if (GuidExtensions.IsNullOrEmpty(designation.Id))
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        string? jsonDesignation = JsonConvert.SerializeObject(designation);
        StringContent? designationContent = new(jsonDesignation, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseDesignation =
            await client.PutAsync("Designation/update-designation/", designationContent);
        if (responseDesignation.IsSuccessStatusCode)
        {
            string? jsonResponseDesignation = await responseDesignation.Content.ReadAsStringAsync();
            resultDesignation = JsonConvert.DeserializeObject<ApiResultResponseConfigVM<DesignationVM>>(jsonResponseDesignation);
        }
        else
        {
            string? errorContent = await responseDesignation.Content.ReadAsStringAsync();
            resultDesignation = new ApiResultResponseConfigVM<DesignationVM>
            {
                IsSuccess = false,
                Message = responseDesignation.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        if (!resultDesignation!.IsSuccess)
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

    #region Delete Designation functionionality
    /// <summary>
    /// Delete the existing Designation.
    /// </summary>
    /// <param name="Id">Designation Guid that needs to be delete</param>
    /// <returns>Designation will be deleted</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/Designation/Designation
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

        ApiResultResponseConfigVM<DesignationVM> resultDesignation = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? responseDesignation = await client.DeleteAsync("Designation/delete-designation?Id=" + Id);
        if (responseDesignation.IsSuccessStatusCode)
        {
            string? jsonResponseDesignation = await responseDesignation.Content.ReadAsStringAsync();
            resultDesignation = JsonConvert.DeserializeObject<ApiResultResponseConfigVM<DesignationVM>>(jsonResponseDesignation);
        }
        else
        {
            string? errorContent = await responseDesignation.Content.ReadAsStringAsync();
            resultDesignation = new ApiResultResponseConfigVM<DesignationVM>
            {
                IsSuccess = false,
                Message = responseDesignation.StatusCode.ToString()
            };
        }

        if (!resultDesignation!.IsSuccess)
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






















