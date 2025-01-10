using AvivCRM.UI.Areas.Environment.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class PurchaseController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public PurchaseController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Purchase()
    {
        // Page Title
        ViewData["pTitle"] = "Purchases Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "Purchase";
        ViewData["bChild"] = "Purchase";

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        ApiResultResponse<List<PurchaseVM>>? purchasePrefixSettings =
            await client.GetFromJsonAsync<ApiResultResponse<List<PurchaseVM>>>("PurchaseSetting/all-purchasesetting");
        PurchaseVM? purchasePrefixSetting = purchasePrefixSettings!.Data!.FirstOrDefault();

        #region get CBPurchasePrefixVM for PurchasePrefixVM

        List<CBPurchasePrefixVM>? cbPrefixItems = purchasePrefixSetting != null
            ? JsonConvert.DeserializeObject<List<CBPurchasePrefixVM>>(purchasePrefixSetting.PurchasePrefixJsonSettings!)
            : [];
        CBPurchasePrefixVM? cbPrefixItem = new();
        if (cbPrefixItems!.Count > 0)
        {
            cbPrefixItem = cbPrefixItems?.FirstOrDefault();
        }

        CBPurchasePrefixVM finalPrefixItems = new()
        {
            PPurchaseVM = cbPrefixItem!.PPurchaseVM,
            PBillOrderVM = cbPrefixItem.PBillOrderVM,
            PVendorCreditVM = cbPrefixItem.PVendorCreditVM
        };

        #endregion

        purchasePrefixSetting!.CBPurchasePrefixVM = finalPrefixItems;

        return View(purchasePrefixSetting);
    }

    [HttpPost]
    public async Task<IActionResult> PurchasePrefixSettingUpdate(PurchaseVM purchasePrefixSetting)
    {
        string? jsonString = "[" + purchasePrefixSetting.PurchasePrefixJsonSettings + "]";
        purchasePrefixSetting.PurchasePrefixJsonSettings = jsonString;

        ApiResultResponse<PurchaseVM> fStatus = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? purchasePrefixSettingResponse =
            await client.PutAsJsonAsync("PurchaseSetting/update-purchasesetting/", purchasePrefixSetting);
        if (purchasePrefixSettingResponse.IsSuccessStatusCode)
        {
            string? jsonpurchasePrefixSettingSource = await purchasePrefixSettingResponse.Content.ReadAsStringAsync();
            fStatus = JsonConvert.DeserializeObject<ApiResultResponse<PurchaseVM>>(jsonpurchasePrefixSettingSource);
        }
        else
        {
            string? errorContent = await purchasePrefixSettingResponse.Content.ReadAsStringAsync();
            fStatus = new ApiResultResponse<PurchaseVM>
            {
                IsSuccess = false,
                Message =
                    purchasePrefixSettingResponse.StatusCode
                        .ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        // Server side Validation
        List<string> serverErrorMessageList = [];
        string serverErrorMessage = fStatus!.Message!;
        serverErrorMessageList.Add(serverErrorMessage);
        if (!fStatus!.IsSuccess)
        {
            return Json(new { success = false, errors = serverErrorMessageList });
        }

        return Json(new { success = true });
    }
}