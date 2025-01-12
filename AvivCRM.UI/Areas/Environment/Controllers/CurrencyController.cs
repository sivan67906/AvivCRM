using System.Text;
using AvivCRM.UI.Areas.Environment.ViewModels;
using AvivCRM.UI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class CurrencyController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CurrencyController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Currency(string searchQuery = null!)
    {
        ViewData["pTitle"] = "Lead Sources Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "Currency";
        ViewData["bChild"] = "Currency View";
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        ApiResultResponse<List<CurrencyVM>> currencyList = new();

        if (string.IsNullOrEmpty(searchQuery))
        {
            // Fetch all products if no search query is provided
            currencyList =
                await client.GetFromJsonAsync<ApiResultResponse<List<CurrencyVM>>>("Currency/all-currency");
        }
        else
        {
            // Fetch products matching the search query
            currencyList =
                await client.GetFromJsonAsync<ApiResultResponse<List<CurrencyVM>>>(
                    $"Currency/SearchByName?name={searchQuery}");
        }

        ViewData["searchQuery"] = searchQuery; // Retain search query

        //ViewBag.ApiResult = currencyList!.Data;
        //ViewBag.ApiMessage = currencyList!.Message;
        //ViewBag.ApiStatus = currencyList.IsSuccess;
        return View(currencyList!.Data);
    }

    [HttpGet]
    public IActionResult Create()
    {
        CurrencyVM currency = new();
        return PartialView("_Create", currency);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CurrencyVM currency)
    {
        ApiResultResponse<CurrencyVM> currencies = new();

        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (currency.CurrencyName == null)
        {
            currency.CurrencyName = "";
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        string? jsoncurrency = JsonConvert.SerializeObject(currency);
        StringContent? currencycontent = new(jsoncurrency, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseCurrency =
            await client.PostAsync("Currency/create-currency", currencycontent);

        if (responseCurrency.IsSuccessStatusCode)
        {
            string? jsonResponseCurrency = await responseCurrency.Content.ReadAsStringAsync();
            currencies = JsonConvert.DeserializeObject<ApiResultResponse<CurrencyVM>>(jsonResponseCurrency);
        }
        else
        {
            string? errorContent = await responseCurrency.Content.ReadAsStringAsync();
            currencies = new ApiResultResponse<CurrencyVM>
            {
                IsSuccess = false,
                Message = responseCurrency.StatusCode + "ErrorContent: " + errorContent
            };
        }

        //ViewBag.ApiResult = currency!.Data;
        //ViewBag.ApiMessage = currency!.Message;
        //ViewBag.ApiStatus = currency.IsSuccess;

        //Server side Validation
        //List<string> serverErrorMessageList = new List<string>();
        //string serverErrorMessage = currency!.Message!;
        //serverErrorMessageList.Add(serverErrorMessage);

        if (!currencies!.IsSuccess)
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

        ApiResultResponse<CurrencyVM> currency = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        currency =
            await client.GetFromJsonAsync<ApiResultResponse<CurrencyVM>>("Currency/byid-currency/?Id=" + Id);

        //ViewBag.ApiResult = currency!.Data;
        //ViewBag.ApiMessage = currency!.Message;
        //ViewBag.ApiStatus = currency.IsSuccess;

        if (!currency!.IsSuccess)
        {
            return View();
        }

        return PartialView("_Edit", currency.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(CurrencyVM currency)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (currency.CurrencyName == null)
        {
            currency.CurrencyName = "";
        }

        ApiResultResponse<CurrencyVM> currencies = new();

        if (GuidExtensions.IsNullOrEmpty(currency.Id))
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        string? jsoncurrency = JsonConvert.SerializeObject(currency);
        StringContent? currencycontent = new(jsoncurrency, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseCurrency =
            await client.PutAsync("Currency/update-currency/", currencycontent);
        if (responseCurrency.IsSuccessStatusCode)
        {
            string? jsonResponseCurrency = await responseCurrency.Content.ReadAsStringAsync();
            currencies = JsonConvert.DeserializeObject<ApiResultResponse<CurrencyVM>>(jsonResponseCurrency);
        }
        else
        {
            string? errorContent = await responseCurrency.Content.ReadAsStringAsync();
            currencies = new ApiResultResponse<CurrencyVM>
            {
                IsSuccess = false,
                Message = responseCurrency.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        //ViewBag.ApiResult = currency!.Data;
        //ViewBag.ApiMessage = currency!.Message;
        //ViewBag.ApiStatus = currency.IsSuccess;

        //Server side Validation
        //List<string> serverErrorMessageList = new List<string>();
        //string serverErrorMessage = currency!.Message!;
        //serverErrorMessageList.Add(serverErrorMessage);

        if (!currencies!.IsSuccess)
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

        ApiResultResponse<CurrencyVM> currency = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? responseCurrency = await client.DeleteAsync("Currency/delete-currency?Id=" + Id);
        if (responseCurrency.IsSuccessStatusCode)
        {
            string? jsonResponseCurrency = await responseCurrency.Content.ReadAsStringAsync();
            currency = JsonConvert.DeserializeObject<ApiResultResponse<CurrencyVM>>(jsonResponseCurrency);
        }
        else
        {
            string? errorContent = await responseCurrency.Content.ReadAsStringAsync();
            currency = new ApiResultResponse<CurrencyVM>
            {
                IsSuccess = false,
                Message = responseCurrency.StatusCode.ToString()
            };
        }

        //ViewBag.ApiResult = currency!.Data;
        //ViewBag.ApiMessage = currency!.Message;
        //ViewBag.ApiStatus = currency.IsSuccess;

        //Server side Validation
        //List<string> serverErrorMessageList = new List<string>();
        //string serverErrorMessage = currency!.Message!;
        //serverErrorMessageList.Add(serverErrorMessage);

        if (!currency!.IsSuccess)
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