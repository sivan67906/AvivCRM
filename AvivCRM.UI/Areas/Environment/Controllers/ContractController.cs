using System.Text;
using AvivCRM.UI.Areas.Environment.ViewModels;
using AvivCRM.UI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class ContractController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ContractController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Contract(string searchQuery = null!)
    {
        ViewData["pTitle"] = "contracts Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "contract";
        ViewData["bChild"] = "contract View";
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        ApiResultResponse<List<ContractVM>> contractList = new();

        if (string.IsNullOrEmpty(searchQuery))
        {
            // Fetch all products if no search query is provided
            contractList =
                await client.GetFromJsonAsync<ApiResultResponse<List<ContractVM>>>("Contract/all-contract");
        }
        else
        {
            // Fetch products matching the search query
            contractList =
                await client.GetFromJsonAsync<ApiResultResponse<List<ContractVM>>>(
                    $"Contract/SearchByName?name={searchQuery}");
        }

        ViewData["searchQuery"] = searchQuery; // Retain search query

        //ViewBag.ApiResult = contractList!.Data;
        //ViewBag.ApiMessage = contractList!.Message;
        //ViewBag.ApiStatus = contractList.IsSuccess;
        return View(contractList!.Data);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ContractVM contract = new();
        return PartialView("_Create", contract);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ContractVM contract)
    {
        ApiResultResponse<ContractVM> source = new();

        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        //if (contract.Name == null)
        //{
        //    contract.Name = "";
        //}

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        string? jsoncontract = JsonConvert.SerializeObject(contract);
        StringContent? contractcontent = new(jsoncontract, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseContract =
            await client.PostAsync("Contract/create-contract", contractcontent);

        if (responseContract.IsSuccessStatusCode)
        {
            string? jsonResponseContract = await responseContract.Content.ReadAsStringAsync();
            source = JsonConvert.DeserializeObject<ApiResultResponse<ContractVM>>(jsonResponseContract);
        }
        else
        {
            string? errorContent = await responseContract.Content.ReadAsStringAsync();
            source = new ApiResultResponse<ContractVM>
            {
                IsSuccess = false,
                Message = responseContract.StatusCode + "ErrorContent: " + errorContent
            };
        }

        //ViewBag.ApiResult = source!.Data;
        //ViewBag.ApiMessage = source!.Message;
        //ViewBag.ApiStatus = source.IsSuccess;

        //Server side Validation
        //List<string> serverErrorMessageList = new List<string>();
        //string serverErrorMessage = source!.Message!;
        //serverErrorMessageList.Add(serverErrorMessage);

        if (!source!.IsSuccess)
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

        ApiResultResponse<ContractVM> contract = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        contract =
            await client.GetFromJsonAsync<ApiResultResponse<ContractVM>>("Contract/byid-contract/?Id=" + Id);

        //ViewBag.ApiResult = contract!.Data;
        //ViewBag.ApiMessage = contract!.Message;
        //ViewBag.ApiStatus = contract.IsSuccess;

        if (!contract!.IsSuccess)
        {
            return View();
        }

        return PartialView("_Edit", contract.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ContractVM contract)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        //if (contract.Name == null)
        //{
        //    contract.Name = "";
        //}

        ApiResultResponse<ContractVM> source = new();

        if (GuidExtensions.IsNullOrEmpty(contract.Id))
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        string? jsoncontract = JsonConvert.SerializeObject(contract);
        StringContent? contractcontent = new(jsoncontract, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseContract =
            await client.PutAsync("Contract/update-contract/", contractcontent);
        if (responseContract.IsSuccessStatusCode)
        {
            string? jsonResponseContract = await responseContract.Content.ReadAsStringAsync();
            source = JsonConvert.DeserializeObject<ApiResultResponse<ContractVM>>(jsonResponseContract);
        }
        else
        {
            string? errorContent = await responseContract.Content.ReadAsStringAsync();
            source = new ApiResultResponse<ContractVM>
            {
                IsSuccess = false,
                Message = responseContract.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        //ViewBag.ApiResult = source!.Data;
        //ViewBag.ApiMessage = source!.Message;
        //ViewBag.ApiStatus = source.IsSuccess;

        //Server side Validation
        //List<string> serverErrorMessageList = new List<string>();
        //string serverErrorMessage = source!.Message!;
        //serverErrorMessageList.Add(serverErrorMessage);

        if (!source!.IsSuccess)
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

        ApiResultResponse<ContractVM> source = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? responseContract = await client.DeleteAsync("Contract/delete-contract?Id=" + Id);
        if (responseContract.IsSuccessStatusCode)
        {
            string? jsonResponseContract = await responseContract.Content.ReadAsStringAsync();
            source = JsonConvert.DeserializeObject<ApiResultResponse<ContractVM>>(jsonResponseContract);
        }
        else
        {
            string? errorContent = await responseContract.Content.ReadAsStringAsync();
            source = new ApiResultResponse<ContractVM>
            {
                IsSuccess = false,
                Message = responseContract.StatusCode.ToString()
            };
        }

        //ViewBag.ApiResult = source!.Data;
        //ViewBag.ApiMessage = source!.Message;
        //ViewBag.ApiStatus = source.IsSuccess;

        //Server side Validation
        //List<string> serverErrorMessageList = new List<string>();
        //string serverErrorMessage = source!.Message!;
        //serverErrorMessageList.Add(serverErrorMessage);

        if (!source!.IsSuccess)
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