using System.Text;
using AvivCRM.UI.Areas.Environment.ViewModels;
using AvivCRM.UI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class PlanningController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public PlanningController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Planning(string searchQuery = null!)
    {
        ViewData["pTitle"] = "Plannings Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "Planning";
        ViewData["bChild"] = "Planning View";
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        ApiResultResponse<List<PlanningVM>> planningList = new();

        if (string.IsNullOrEmpty(searchQuery))
        {
            // Fetch all products if no search query is provided
            planningList =
                await client.GetFromJsonAsync<ApiResultResponse<List<PlanningVM>>>("Planning/all-planning");
        }
        else
        {
            // Fetch products matching the search query
            planningList =
                await client.GetFromJsonAsync<ApiResultResponse<List<PlanningVM>>>(
                    $"Planning/SearchByName?name={searchQuery}");
        }

        ViewData["searchQuery"] = searchQuery; // Retain search query

        //ViewBag.ApiResult = planningList!.Data;
        //ViewBag.ApiMessage = planningList!.Message;
        //ViewBag.ApiStatus = planningList.IsSuccess;
        return View(planningList!.Data);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        PlanningVM planning = new();
        return PartialView("_Create", planning);
    }

    [HttpPost]
    public async Task<IActionResult> Create(PlanningVM planning)
    {
        ApiResultResponse<PlanningVM> plannings = new();

        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (planning.Name == null)
        {
            planning.Name = "";
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        string? jsonplanning = JsonConvert.SerializeObject(planning);
        StringContent? planningcontent = new(jsonplanning, Encoding.UTF8, "application/json");
        HttpResponseMessage? responsePlanning =
            await client.PostAsync("Planning/create-planning", planningcontent);

        if (responsePlanning.IsSuccessStatusCode)
        {
            string? jsonResponsePlanning = await responsePlanning.Content.ReadAsStringAsync();
            plannings = JsonConvert.DeserializeObject<ApiResultResponse<PlanningVM>>(jsonResponsePlanning);
        }
        else
        {
            string? errorContent = await responsePlanning.Content.ReadAsStringAsync();
            plannings = new ApiResultResponse<PlanningVM>
            {
                IsSuccess = false,
                Message = responsePlanning.StatusCode + "ErrorContent: " + errorContent
            };
        }

        //ViewBag.ApiResult = planning!.Data;
        //ViewBag.ApiMessage = planning!.Message;
        //ViewBag.ApiStatus = planning.IsSuccess;

        //Server side Validation
        //List<string> serverErrorMessageList = new List<string>();
        //string serverErrorMessage = planning!.Message!;
        //serverErrorMessageList.Add(serverErrorMessage);

        if (!plannings!.IsSuccess)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        return Json(new { success = true });
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid Id)
    {
        if (GuidExtensions.IsNullOrEmpty(Id))
        {
            return View();
        }

        ApiResultResponse<PlanningVM> planning = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        planning =
            await client.GetFromJsonAsync<ApiResultResponse<PlanningVM>>("Planning/byid-planning/?Id=" + Id);

        //ViewBag.ApiResult = planning!.Data;
        //ViewBag.ApiMessage = planning!.Message;
        //ViewBag.ApiStatus = planning.IsSuccess;

        if (!planning!.IsSuccess)
        {
            return View();
        }

        return PartialView("_Edit", planning.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(PlanningVM planning)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        //if (planning.Name == null)
        //{
        //    planning.Name = "";
        //}

        ApiResultResponse<PlanningVM> plannings = new();

        if (GuidExtensions.IsNullOrEmpty(planning.Id))
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        string? jsonplanning = JsonConvert.SerializeObject(planning);
        StringContent? planningcontent = new(jsonplanning, Encoding.UTF8, "application/json");
        HttpResponseMessage? responsePlanning =
            await client.PutAsync("Planning/update-planning/", planningcontent);
        if (responsePlanning.IsSuccessStatusCode)
        {
            string? jsonResponsePlanning = await responsePlanning.Content.ReadAsStringAsync();
            plannings = JsonConvert.DeserializeObject<ApiResultResponse<PlanningVM>>(jsonResponsePlanning);
        }
        else
        {
            string? errorContent = await responsePlanning.Content.ReadAsStringAsync();
            plannings = new ApiResultResponse<PlanningVM>
            {
                IsSuccess = false,
                Message = responsePlanning.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        //ViewBag.ApiResult = planning!.Data;
        //ViewBag.ApiMessage = planning!.Message;
        //ViewBag.ApiStatus = planning.IsSuccess;

        //Server side Validation
        //List<string> serverErrorMessageList = new List<string>();
        //string serverErrorMessage = planning!.Message!;
        //serverErrorMessageList.Add(serverErrorMessage);

        if (!plannings!.IsSuccess)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        return Json(new { success = true });
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid Id)
    {
        //if (GuidExtensions.IsNullOrEmpty(Id)) return View();
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        ApiResultResponse<PlanningVM> planning = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? responsePlanning = await client.DeleteAsync("Planning/delete-planning?Id=" + Id);
        if (responsePlanning.IsSuccessStatusCode)
        {
            string? jsonResponsePlanning = await responsePlanning.Content.ReadAsStringAsync();
            planning = JsonConvert.DeserializeObject<ApiResultResponse<PlanningVM>>(jsonResponsePlanning);
        }
        else
        {
            string? errorContent = await responsePlanning.Content.ReadAsStringAsync();
            planning = new ApiResultResponse<PlanningVM>
            {
                IsSuccess = false,
                Message = responsePlanning.StatusCode.ToString()
            };
        }

        //ViewBag.ApiResult = planning!.Data;
        //ViewBag.ApiMessage = planning!.Message;
        //ViewBag.ApiStatus = planning.IsSuccess;

        //Server side Validation
        //List<string> serverErrorMessageList = new List<string>();
        //string serverErrorMessage = planning!.Message!;
        //serverErrorMessageList.Add(serverErrorMessage);

        if (!planning!.IsSuccess)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        return Json(new { success = true });
    }
}