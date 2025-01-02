using Microsoft.AspNetCore.Mvc;
using AvivCRM.UI.Areas.Environment.ViewModels;

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
    public async Task<IActionResult> Purchase(int id = 0)
    {
        ViewData["pTitle"] = "Purchases Profile";
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "Purchase";
        ViewData["bChild"] = "Purchase View";

        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        var purchaseVM = new PurchaseVM
        {
            BillOrders = new BillOrderVM(),
            PurchaseOrders = new PurchaseOrderVM(),
            VendorCreditOrders = new VendorCreditVM()
        };

        try
        {
            // Fetch BillOrder data
            var billOrder = await client.GetFromJsonAsync<BillOrderVM>($"BillOrder/GetById/?Id={id}");
            if (billOrder != null)
            {
                purchaseVM.BillOrders = billOrder;
            }

            // Fetch PurchaseOrder data
            var purchaseOrder = await client.GetFromJsonAsync<PurchaseOrderVM>($"PurchaseOrder/GetById/?Id={id}");
            if (purchaseOrder != null)
            {
                purchaseVM.PurchaseOrders = purchaseOrder;
            }

            // Fetch VendorCredit data
            var vendorCredit = await client.GetFromJsonAsync<VendorCreditVM>($"VendorCredit/GetById/?Id={id}");
            if (vendorCredit != null)
            {
                purchaseVM.VendorCreditOrders = vendorCredit;
            }
        }
        catch (Exception ex)
        {
            // Log error and add error message
            ModelState.AddModelError("", "Failed to fetch data from one or more APIs.");
        }

        return View(purchaseVM);
    }

    [HttpPost]
    public async Task<IActionResult> Purchase(PurchaseVM purchaseVM)
    {
        if (!ModelState.IsValid)
        {
            return View(purchaseVM); // Return the same view with validation errors
        }

        var client = _httpClientFactory.CreateClient("ApiGatewayCall");

        try
        {
            // Update BillOrder
            if (purchaseVM.BillOrders != null)
            {
                var response = await client.PutAsJsonAsync("BillOrder/Update", purchaseVM.BillOrders);
                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError("", "Failed to update BillOrder.");
                }
            }

            // Update PurchaseOrder
            if (purchaseVM.PurchaseOrders != null)
            {
                var response = await client.PutAsJsonAsync("PurchaseOrder/Update", purchaseVM.PurchaseOrders);
                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError("", "Failed to update PurchaseOrder.");
                }
            }

            // Update VendorCredit
            if (purchaseVM.VendorCreditOrders != null)
            {
                var response = await client.PutAsJsonAsync("VendorCredit/Update", purchaseVM.VendorCreditOrders);
                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError("", "Failed to update VendorCredit.");
                }
            }

            if (!ModelState.IsValid)
            {
                return View(purchaseVM); // Show validation errors on the same view
            }

            // Redirect to success or listing page
            return RedirectToAction("Purchase");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "An error occurred while updating the data. Please try again.");
            return View(purchaseVM);
        }
    }



    //[HttpGet]
    //public async Task<IActionResult> Create()
    //{
    //    PurchaseVM purchase = new();
    //    var client = _httpClientFactory.CreateClient("ApiGatewayCall");
    //    return PartialView("_Create", purchase);
    //}

    //[HttpPost]
    //public async Task<IActionResult> Create(PurchaseVM purchase)
    //{
    //    var client = _httpClientFactory.CreateClient("ApiGatewayCall");
    //    await client.PostAsJsonAsync<PurchaseVM>("Purchase/Create", purchase);
    //    return RedirectToAction("Purchase");
    //}

    //[HttpGet]
    //public async Task<IActionResult> Edit(int Id)
    //{
    //    if (Id == 0) return View();
    //    var client = _httpClientFactory.CreateClient("ApiGatewayCall");
    //    var purchase = await client.GetFromJsonAsync<PurchaseVM>("Purchase/GetById/?Id=" + Id);
    //    return PartialView("_Edit", purchase);
    //}

    //[HttpPost]
    //public async Task<IActionResult> Update(PurchaseVM purchase)
    //{
    //    if (purchase.Id == 0) return View();
    //    var client = _httpClientFactory.CreateClient("ApiGatewayCall");
    //    await client.PutAsJsonAsync<PurchaseVM>("Purchase/Update/", purchase);
    //    return RedirectToAction("Purchase");
    //}

    //[HttpGet]
    //public async Task<IActionResult> Delete(int Id)
    //{
    //    if (Id == 0) return View();
    //    var client = _httpClientFactory.CreateClient("ApiGatewayCall");
    //    var purchase = await client.GetFromJsonAsync<PurchaseVM>("Purchase/GetById/?Id=" + Id);
    //    return PartialView("_Delete", purchase);
    //}

    //[HttpPost]
    //public async Task<IActionResult> Delete(PurchaseVM purchase)
    //{
    //    if (purchase.Id == 0) return View();
    //    var client = _httpClientFactory.CreateClient("ApiGatewayCall");
    //    await client.DeleteAsync("Purchase/Delete?Id=" + purchase.Id);
    //    return RedirectToAction("Purchase");
    //}
}



