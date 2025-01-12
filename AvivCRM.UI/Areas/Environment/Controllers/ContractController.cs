#region Namespaces
using System.Text;
using AvivCRM.UI.Areas.Environment.ViewModels;
using AvivCRM.UI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
#endregion

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class ContractController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    #region Constructor
    public ContractController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    #endregion

    #region Retrieves a List of Contracts
    /// <summary>
    /// Retrieves a list of Contracts from the database.
    /// </summary>
    /// <param name=""></param>
    /// <returns>Modal popup will open to create New Contract</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/Contract/Contract
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    public async Task<IActionResult> Contract()
    {
        ViewData["pTitle"] = "Contracts Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "Contract";
        ViewData["bChild"] = "Contract View";
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        ApiResultResponse<List<ContractVM>> contractList = new();

        // fetch all the Contracts
        contractList =
                await client.GetFromJsonAsync<ApiResultResponse<List<ContractVM>>>("Contract/all-contract");

        return View(contractList!.Data);
    }
    #endregion

    #region Create Contract functionionality
    /// <summary>
    /// Show the popup to create a new Contract.
    /// </summary>
    /// <param name=""></param>
    /// <returns>New Contract</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/Contract/Contract
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ContractVM contract = new();
        return PartialView("_Create", contract);
    }

    /// <summary>
    /// New Contract will be create.
    /// </summary>
    /// <param name="contract">Contract entity that needs to be create</param>
    /// <returns>New Contract will be create.</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/Contract/Contract
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Create(ContractVM contract)
    {
        ApiResultResponse<ContractVM> resultContract = new();

        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (contract.Name == null)
        {
            contract.Name = "";
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        string? jsonContract = JsonConvert.SerializeObject(contract);
        StringContent? contractContent = new(jsonContract, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseContract =
            await client.PostAsync("Contract/create-contract", contractContent);

        if (responseContract.IsSuccessStatusCode)
        {
            string? jsonResponseContract = await responseContract.Content.ReadAsStringAsync();
            resultContract = JsonConvert.DeserializeObject<ApiResultResponse<ContractVM>>(jsonResponseContract);
        }
        else
        {
            string? errorContent = await responseContract.Content.ReadAsStringAsync();
            resultContract = new ApiResultResponse<ContractVM>
            {
                IsSuccess = false,
                Message = responseContract.StatusCode + "ErrorContent: " + errorContent
            };
        }

        if (!resultContract!.IsSuccess)
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

    #region Edit Contract functionionality
    /// <summary>
    /// Edit the existing Contract.
    /// </summary>
    /// <param name="Id">Contract Guid that needs to be edit</param>
    /// <returns>Popup will be open to edit the contract details</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/Contract/Contract
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

        ApiResultResponse<ContractVM> contract = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        contract =
            await client.GetFromJsonAsync<ApiResultResponse<ContractVM>>("Contract/byid-contract/?Id=" + Id);

        if (!contract!.IsSuccess)
        {
            return View();
        }

        return PartialView("_Edit", contract.Data);
    }

    /// <summary>
    /// Update the existing Contract.
    /// </summary>
    /// <param name="contract">Contract entity to update the existing contract</param>
    /// <returns>Changes will be updated for the existing contract</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/Contract/Contract
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
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

        if (contract.Name == null)
        {
            contract.Name = "";
        }

        ApiResultResponse<ContractVM> resultContract = new();

        if (GuidExtensions.IsNullOrEmpty(contract.Id))
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        string? jsonContract = JsonConvert.SerializeObject(contract);
        StringContent? contractContent = new(jsonContract, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseContract =
            await client.PutAsync("Contract/update-contract/", contractContent);
        if (responseContract.IsSuccessStatusCode)
        {
            string? jsonResponseContract = await responseContract.Content.ReadAsStringAsync();
            resultContract = JsonConvert.DeserializeObject<ApiResultResponse<ContractVM>>(jsonResponseContract);
        }
        else
        {
            string? errorContent = await responseContract.Content.ReadAsStringAsync();
            resultContract = new ApiResultResponse<ContractVM>
            {
                IsSuccess = false,
                Message = responseContract.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        if (!resultContract!.IsSuccess)
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

    #region Delete Contract functionionality
    /// <summary>
    /// Delete the existing Contract.
    /// </summary>
    /// <param name="Id">Contract Guid that needs to be delete</param>
    /// <returns>Contract will be deleted</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/Contract/Contract
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

        ApiResultResponse<ContractVM> resultContract = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? responseContract = await client.DeleteAsync("Contract/delete-contract?Id=" + Id);
        if (responseContract.IsSuccessStatusCode)
        {
            string? jsonResponseContract = await responseContract.Content.ReadAsStringAsync();
            resultContract = JsonConvert.DeserializeObject<ApiResultResponse<ContractVM>>(jsonResponseContract);
        }
        else
        {
            string? errorContent = await responseContract.Content.ReadAsStringAsync();
            resultContract = new ApiResultResponse<ContractVM>
            {
                IsSuccess = false,
                Message = responseContract.StatusCode.ToString()
            };
        }

        if (!resultContract!.IsSuccess)
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






















