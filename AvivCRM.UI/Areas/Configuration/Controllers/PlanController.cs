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
public class PlanController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    #region Constructor
    public PlanController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    #endregion

    #region Retrieves a List of Plans
    /// <summary>
    /// Retrieves a list of Plans from the database.
    /// </summary>
    /// <param name=""></param>
    /// <returns>Modal popup will open to create New Plan</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/Plan/Plan
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    public async Task<IActionResult> Plan()
    {
        ViewData["pTitle"] = "Plans Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "Plan";
        ViewData["bChild"] = "Plan View";
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        ApiResultResponseConfigVM<List<PlanVM>> planList = new();

        // fetch all the Plans
        planList =
                await client.GetFromJsonAsync<ApiResultResponseConfigVM<List<PlanVM>>>("Plan/all-plan");

        return View(planList!.Data);
    }
    #endregion

    #region Create Plan functionionality
    /// <summary>
    /// Show the popup to create a new Plan.
    /// </summary>
    /// <param name=""></param>
    /// <returns>New Plan</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/Plan/Plan
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    [HttpGet]
    public IActionResult Create()
    {
        PlanVM plan = new();
        return PartialView("_Create", plan);
    }

    /// <summary>
    /// New Plan will be create.
    /// </summary>
    /// <param name="plan">Plan entity that needs to be create</param>
    /// <returns>New Plan will be create.</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/Plan/Plan
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Create(PlanVM plan)
    {
        ApiResultResponseConfigVM<PlanVM> resultPlan = new();

        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (plan.Name == null)
        {
            plan.Name = "";
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        string? jsonPlan = JsonConvert.SerializeObject(plan);
        StringContent? planContent = new(jsonPlan, Encoding.UTF8, "application/json");
        HttpResponseMessage? responsePlan =
            await client.PostAsync("Plan/create-plan", planContent);

        if (responsePlan.IsSuccessStatusCode)
        {
            string? jsonResponsePlan = await responsePlan.Content.ReadAsStringAsync();
            resultPlan = JsonConvert.DeserializeObject<ApiResultResponseConfigVM<PlanVM>>(jsonResponsePlan);
        }
        else
        {
            string? errorContent = await responsePlan.Content.ReadAsStringAsync();
            resultPlan = new ApiResultResponseConfigVM<PlanVM>
            {
                IsSuccess = false,
                Message = responsePlan.StatusCode + "ErrorContent: " + errorContent
            };
        }

        if (!resultPlan!.IsSuccess)
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

    #region Edit Plan functionionality
    /// <summary>
    /// Edit the existing Plan.
    /// </summary>
    /// <param name="Id">Plan Guid that needs to be edit</param>
    /// <returns>Popup will be open to edit the plan details</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/Plan/Plan
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

        ApiResultResponseConfigVM<PlanVM> plan = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        plan =
            await client.GetFromJsonAsync<ApiResultResponseConfigVM<PlanVM>>("Plan/byid-plan/?Id=" + Id);

        if (!plan!.IsSuccess)
        {
            return View();
        }

        return PartialView("_Edit", plan.Data);
    }

    /// <summary>
    /// Update the existing Plan.
    /// </summary>
    /// <param name="plan">Plan entity to update the existing plan</param>
    /// <returns>Changes will be updated for the existing plan</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/Plan/Plan
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Edit(PlanVM plan)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (plan.Name == null)
        {
            plan.Name = "";
        }

        ApiResultResponseConfigVM<PlanVM> resultPlan = new();

        if (GuidExtensions.IsNullOrEmpty(plan.Id))
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        string? jsonPlan = JsonConvert.SerializeObject(plan);
        StringContent? planContent = new(jsonPlan, Encoding.UTF8, "application/json");
        HttpResponseMessage? responsePlan =
            await client.PutAsync("Plan/update-plan/", planContent);
        if (responsePlan.IsSuccessStatusCode)
        {
            string? jsonResponsePlan = await responsePlan.Content.ReadAsStringAsync();
            resultPlan = JsonConvert.DeserializeObject<ApiResultResponseConfigVM<PlanVM>>(jsonResponsePlan);
        }
        else
        {
            string? errorContent = await responsePlan.Content.ReadAsStringAsync();
            resultPlan = new ApiResultResponseConfigVM<PlanVM>
            {
                IsSuccess = false,
                Message = responsePlan.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        if (!resultPlan!.IsSuccess)
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

    #region Delete Plan functionionality
    /// <summary>
    /// Delete the existing Plan.
    /// </summary>
    /// <param name="Id">Plan Guid that needs to be delete</param>
    /// <returns>Plan will be deleted</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/Plan/Plan
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

        ApiResultResponseConfigVM<PlanVM> resultPlan = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? responsePlan = await client.DeleteAsync("Plan/delete-plan?Id=" + Id);
        if (responsePlan.IsSuccessStatusCode)
        {
            string? jsonResponsePlan = await responsePlan.Content.ReadAsStringAsync();
            resultPlan = JsonConvert.DeserializeObject<ApiResultResponseConfigVM<PlanVM>>(jsonResponsePlan);
        }
        else
        {
            string? errorContent = await responsePlan.Content.ReadAsStringAsync();
            resultPlan = new ApiResultResponseConfigVM<PlanVM>
            {
                IsSuccess = false,
                Message = responsePlan.StatusCode.ToString()
            };
        }

        if (!resultPlan!.IsSuccess)
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






















