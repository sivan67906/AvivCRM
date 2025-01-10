using System.Text;
using AvivCRM.UI.Areas.Environment.ViewModels;
using AvivCRM.UI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class TaxController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public TaxController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Tax(string searchQuery = null!)
    {
        ViewData["pTitle"] = "Taxs Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "Tax";
        ViewData["bChild"] = "Tax View";
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        ApiResultResponse<List<TaxVM>> taxList = new();

        if (string.IsNullOrEmpty(searchQuery))
        {
            // Fetch all products if no search query is provided
            taxList =
                await client.GetFromJsonAsync<ApiResultResponse<List<TaxVM>>>("Tax/all-tax");
        }
        else
        {
            // Fetch products matching the search query
            taxList =
                await client.GetFromJsonAsync<ApiResultResponse<List<TaxVM>>>(
                    $"Tax/SearchByName?name={searchQuery}");
        }

        ViewData["searchQuery"] = searchQuery; // Retain search query

        //ViewBag.ApiResult = taxList!.Data;
        //ViewBag.ApiMessage = taxList!.Message;
        //ViewBag.ApiStatus = taxList.IsSuccess;
        return View(taxList!.Data);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        TaxVM tax = new();
        return PartialView("_Create", tax);
    }

    [HttpPost]
    public async Task<IActionResult> Create(TaxVM tax)
    {
        ApiResultResponse<TaxVM> taxs = new();

        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (tax.Name == null)
        {
            tax.Name = "";
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        string? jsontax = JsonConvert.SerializeObject(tax);
        StringContent? taxcontent = new(jsontax, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseTax =
            await client.PostAsync("Tax/create-tax", taxcontent);

        if (responseTax.IsSuccessStatusCode)
        {
            string? jsonResponseTax = await responseTax.Content.ReadAsStringAsync();
            taxs = JsonConvert.DeserializeObject<ApiResultResponse<TaxVM>>(jsonResponseTax);
        }
        else
        {
            string? errorContent = await responseTax.Content.ReadAsStringAsync();
            taxs = new ApiResultResponse<TaxVM>
            {
                IsSuccess = false,
                Message = responseTax.StatusCode + "ErrorContent: " + errorContent
            };
        }

        //ViewBag.ApiResult = taxs!.Data;
        //ViewBag.ApiMessage = taxs!.Message;
        //ViewBag.ApiStatus = taxs.IsSuccess;

        //Server side Validation
        //List<string> serverErrorMessageList = new List<string>();
        //string serverErrorMessage = taxs!.Message!;
        //serverErrorMessageList.Add(serverErrorMessage);

        if (!taxs!.IsSuccess)
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

        ApiResultResponse<TaxVM> tax = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        tax =
            await client.GetFromJsonAsync<ApiResultResponse<TaxVM>>("Tax/byid-tax/?Id=" + Id);

        //ViewBag.ApiResult = tax!.Data;
        //ViewBag.ApiMessage = tax!.Message;
        //ViewBag.ApiStatus = tax.IsSuccess;

        if (!tax!.IsSuccess)
        {
            return View();
        }

        return PartialView("_Edit", tax.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(TaxVM tax)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (tax.Name == null)
        {
            tax.Name = "";
        }

        ApiResultResponse<TaxVM> taxs = new();

        if (GuidExtensions.IsNullOrEmpty(tax.Id))
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        string? jsontax = JsonConvert.SerializeObject(tax);
        StringContent? taxcontent = new(jsontax, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseTax =
            await client.PutAsync("Tax/update-tax/", taxcontent);
        if (responseTax.IsSuccessStatusCode)
        {
            string? jsonResponseTax = await responseTax.Content.ReadAsStringAsync();
            taxs = JsonConvert.DeserializeObject<ApiResultResponse<TaxVM>>(jsonResponseTax);
        }
        else
        {
            string? errorContent = await responseTax.Content.ReadAsStringAsync();
            taxs = new ApiResultResponse<TaxVM>
            {
                IsSuccess = false,
                Message = responseTax.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        //ViewBag.ApiResult = taxs!.Data;
        //ViewBag.ApiMessage = taxs!.Message;
        //ViewBag.ApiStatus = taxs.IsSuccess;

        //Server side Validation
        //List<string> serverErrorMessageList = new List<string>();
        //string serverErrorMessage = taxs!.Message!;
        //serverErrorMessageList.Add(serverErrorMessage);

        if (!taxs!.IsSuccess)
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

        ApiResultResponse<TaxVM> taxs = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? responseTax = await client.DeleteAsync("Tax/delete-tax?Id=" + Id);
        if (responseTax.IsSuccessStatusCode)
        {
            string? jsonResponseTax = await responseTax.Content.ReadAsStringAsync();
            taxs = JsonConvert.DeserializeObject<ApiResultResponse<TaxVM>>(jsonResponseTax);
        }
        else
        {
            string? errorContent = await responseTax.Content.ReadAsStringAsync();
            taxs = new ApiResultResponse<TaxVM>
            {
                IsSuccess = false,
                Message = responseTax.StatusCode.ToString()
            };
        }

        //ViewBag.ApiResult = taxs!.Data;
        //ViewBag.ApiMessage = taxs!.Message;
        //ViewBag.ApiStatus = taxs.IsSuccess;

        //Server side Validation
        //List<string> serverErrorMessageList = new List<string>();
        //string serverErrorMessage = taxs!.Message!;
        //serverErrorMessageList.Add(serverErrorMessage);

        if (!taxs!.IsSuccess)
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