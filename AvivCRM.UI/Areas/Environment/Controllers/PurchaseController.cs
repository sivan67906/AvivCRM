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
    public async Task<IActionResult> Index()
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

        var client = _httpClientFactory.CreateClient("ApiGatewayCall");

        var purchasePrefixSettings = await client.GetFromJsonAsync<ApiResultResponse<List<PurchaseVM>>>("PurchaseSetting/all-purchasesetting");
        var purchasePrefixSetting = purchasePrefixSettings!.Data!.FirstOrDefault();

        #region get CBPurchasePrefixVM for PurchasePrefixVM
        var cbPrefixItems = purchasePrefixSetting != null ? JsonConvert.DeserializeObject<List<CBPurchasePrefixVM>>(purchasePrefixSetting.PurchasePrefixJsonSettings!) : new List<CBPurchasePrefixVM>();
        var cbPrefixItem = new CBPurchasePrefixVM();
        if (cbPrefixItems!.Count > 0)
        {
            cbPrefixItem = cbPrefixItems?.FirstOrDefault();
        }

        CBPurchasePrefixVM finalPrefixItems = new();
        finalPrefixItems.PPurchaseVM = cbPrefixItem!.PPurchaseVM;
        finalPrefixItems.PBillOrderVM = cbPrefixItem.PBillOrderVM;
        finalPrefixItems.PVendorCreditVM = cbPrefixItem.PVendorCreditVM;
        #endregion

        purchasePrefixSetting!.CBPurchasePrefixVM = finalPrefixItems;

        return View(purchasePrefixSetting);
    }

    [HttpPost]
    public async Task<IActionResult> PurchasePrefixSettingUpdate(PurchaseVM purchasePrefixSetting)
    {
        var jsonString = "[" + purchasePrefixSetting.PurchasePrefixJsonSettings + "]";
        purchasePrefixSetting.PurchasePrefixJsonSettings = jsonString;

        ApiResultResponse<PurchaseVM> fStatus = new();

        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        var purchasePrefixSettingResponse = await client.PutAsJsonAsync("PurchaseSetting/update-purchasesetting/", purchasePrefixSetting);
        if (purchasePrefixSettingResponse.IsSuccessStatusCode)
        {
            var jsonpurchasePrefixSettingSource = await purchasePrefixSettingResponse.Content.ReadAsStringAsync();
            fStatus = JsonConvert.DeserializeObject<ApiResultResponse<PurchaseVM>>(jsonpurchasePrefixSettingSource);
        }
        else
        {
            var errorContent = await purchasePrefixSettingResponse.Content.ReadAsStringAsync();
            fStatus = new ApiResultResponse<PurchaseVM>
            {
                IsSuccess = false,
                Message = purchasePrefixSettingResponse.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        // Server side Validation
        List<string> serverErrorMessageList = new List<string>();
        string serverErrorMessage = fStatus!.Message!.ToString();
        serverErrorMessageList.Add(serverErrorMessage);
        if (!fStatus!.IsSuccess)
            return Json(new { success = false, errors = serverErrorMessageList });
        else
            return Json(new { success = true });
    }
}