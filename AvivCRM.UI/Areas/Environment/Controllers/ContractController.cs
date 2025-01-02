using Microsoft.AspNetCore.Mvc;
using AvivCRM.UI.Areas.Environment.ViewModels;

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class ContractController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    public ContractController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;

    }
    public async Task<IActionResult> Index()
    {
        return View();
    }

    public async Task<IActionResult> Contract(int id=0)
    {
        // Page Title
        ViewData["pTitle"] = "Contracts Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "Contract";
        ViewData["bChild"] = "Contract View";

        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
        
        ContractVM contract;

        if (id > 0)
        {
            // Fetch the specific contract by ID
            contract = await client.GetFromJsonAsync<ContractVM>($"Contract/GetById/?Id=" + id);
        }
        else
        {
            // Fetch all contracts and take the first one
            var contractList = await client.GetFromJsonAsync<List<ContractVM>>("Contract/GetAll");
            contract = contractList?.FirstOrDefault();
        }

        if (contract == null)
        {
            return NotFound();
        }

        return View(contract); // Pass the contract data to the view for editing

    }


    [HttpPost]
    public async Task<IActionResult> Contract(ContractVM updatedContract)
    {
        if (!ModelState.IsValid)
        {
            return View(updatedContract); // Return the same view with validation errors
        }

        var client = _httpClientFactory.CreateClient("ApiGatewayCall");

        // Send the updated contract back to the API
        var response = await client.PutAsJsonAsync("Contract/Update", updatedContract);

        if (response.IsSuccessStatusCode)
        {
            // Redirect to the contract listing page or success message
            return RedirectToAction("Contract");
        }

        ModelState.AddModelError("", "Failed to update the contract. Please try again.");
        return View(updatedContract); // Show the error on the same view
    }
   


//    [HttpGet]
//    public async Task<IActionResult> Create()
//    {
//        ContractVM company = new();
//        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
//        return PartialView("_Create", company);
//    }

//    [HttpPost]
//    public async Task<IActionResult> Create(ContractVM currency)
//    {
//        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
//        await client.PostAsJsonAsync<ContractVM>("Contract/Create", currency);
//        return RedirectToAction("Contract");
//    }

//    [HttpGet]
//    public async Task<IActionResult> Edit(int Id)
//    {
//        if (Id == 0) return View();
//        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
//        var currency = await client.GetFromJsonAsync<ContractVM>("Contract/GetById/?Id=" + Id);
//        return PartialView("_Edit", currency);
//    }

//    [HttpPost]
//    public async Task<IActionResult> Update(ContractVM currency)
//    {
//        if (currency.Id == 0) return View();
//        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
//        await client.PutAsJsonAsync<ContractVM>("Contract/Update/", currency);
//        return RedirectToAction("Contract");
//    }

//    [HttpGet]
//    public async Task<IActionResult> Delete(int Id)
//    {
//        if (Id == 0) return View();
//        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
//        var currency = await client.GetFromJsonAsync<ContractVM>("Contract/GetById/?Id=" + Id);
//        return PartialView("_Delete", currency);
//    }

//    [HttpPost]
//    public async Task<IActionResult> Delete(ContractVM currency)
//    {
//        if (currency.Id == 0) return View();
//        var client = _httpClientFactory.CreateClient("ApiGatewayCall");
//        await client.DeleteAsync("Contract/Delete?Id=" + currency.Id);
//        return RedirectToAction("Contract");
//    }
}



