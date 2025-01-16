using System.Text;
using AvivCRM.UI.Areas.Environment.ViewModels;
using AvivCRM.UI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class ClientController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    #region Constructor
    public ClientController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    #endregion

    #region Retrieves a List of Clients
    /// <summary>
    /// Retrieves a list of Clients from the database.
    /// </summary>
    /// <param name=""></param>
    /// <returns>Modal popup will open to create New Client</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/Client/Client
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Kansheyam
    /// </remarks>
    public async Task<IActionResult> Client()
    {
        ViewData["pTitle"] = "Clients Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "Client";
        ViewData["bChild"] = "Client View";
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        ApiResultResponse<List<ClientVM>> clientList = new();

        // fetch all the Clients
        clientList =
                await client.GetFromJsonAsync<ApiResultResponse<List<ClientVM>>>("Client/all-client");

        return View(clientList!.Data);
    }
    #endregion

    #region Create Client functionionality
    /// <summary>
    /// Show the popup to create a new Client.
    /// </summary>
    /// <param name=""></param>
    /// <returns>New Client</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/Client/Client
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Kansheyam
    /// </remarks>
    [HttpGet]
    public IActionResult Create()
    {
        ClientVM client = new();
        return PartialView("_Create", client);
    }

    /// <summary>
    /// New Client will be create.
    /// </summary>
    /// <param name="client">Client entity that needs to be create</param>
    /// <returns>New Client will be create.</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/Client/Client
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Kansheyam
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Create(ClientVM clients)
    {
        ApiResultResponse<ClientVM> resultClient = new();

        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (clients.ClientName == null)
        {
            clients.ClientName = "";
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        string? jsonClient = JsonConvert.SerializeObject(client);
        StringContent? clientContent = new(jsonClient, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseClient =
            await client.PostAsync("Client/create-client", clientContent);

        if (responseClient.IsSuccessStatusCode)
        {
            string? jsonResponseClient = await responseClient.Content.ReadAsStringAsync();
            resultClient = JsonConvert.DeserializeObject<ApiResultResponse<ClientVM>>(jsonResponseClient);
        }
        else
        {
            string? errorContent = await responseClient.Content.ReadAsStringAsync();
            resultClient = new ApiResultResponse<ClientVM>
            {
                IsSuccess = false,
                Message = responseClient.StatusCode + "ErrorContent: " + errorContent
            };
        }

        if (!resultClient!.IsSuccess)
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

    #region Edit Client functionionality
    /// <summary>
    /// Edit the existing Client.
    /// </summary>
    /// <param name="Id">Client Guid that needs to be edit</param>
    /// <returns>Popup will be open to edit the client details</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/Client/Client
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Kansheyam
    /// </remarks>
    [HttpGet]
    public async Task<IActionResult> Edit(Guid Id)
    {
        if (GuidExtensions.IsNullOrEmpty(Id))
        {
            return View();
        }

        ApiResultResponse<ClientVM> clients = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        clients =
            await client.GetFromJsonAsync<ApiResultResponse<ClientVM>>("Client/byid-client/?Id=" + Id);

        if (!clients!.IsSuccess)
        {
            return View();
        }

        return PartialView("_Edit", clients.Data);
    }

    /// <summary>
    /// Update the existing Client.
    /// </summary>
    /// <param name="client">Client entity to update the existing client</param>
    /// <returns>Changes will be updated for the existing client</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/Client/Client
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Kansheyam
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Edit(ClientVM clients)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (clients.ClientName == null)
        {
            clients.ClientName = "";
        }

        ApiResultResponse<ClientVM> resultClient = new();

        if (GuidExtensions.IsNullOrEmpty(clients.Id))
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        string? jsonClient = JsonConvert.SerializeObject(client);
        StringContent? clientContent = new(jsonClient, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseClient =
            await client.PutAsync("Client/update-client/", clientContent);
        if (responseClient.IsSuccessStatusCode)
        {
            string? jsonResponseClient = await responseClient.Content.ReadAsStringAsync();
            resultClient = JsonConvert.DeserializeObject<ApiResultResponse<ClientVM>>(jsonResponseClient);
        }
        else
        {
            string? errorContent = await responseClient.Content.ReadAsStringAsync();
            resultClient = new ApiResultResponse<ClientVM>
            {
                IsSuccess = false,
                Message = responseClient.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        if (!resultClient!.IsSuccess)
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

    #region Delete Client functionionality
    /// <summary>
    /// Delete the existing Client.
    /// </summary>
    /// <param name="Id">Client Guid that needs to be delete</param>
    /// <returns>Lead source will be deleted</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/Client/Client
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Kansheyam
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

        ApiResultResponse<ClientVM> resultClient = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? responseClient = await client.DeleteAsync("Client/delete-client?Id=" + Id);
        if (responseClient.IsSuccessStatusCode)
        {
            string? jsonResponseClient = await responseClient.Content.ReadAsStringAsync();
            resultClient = JsonConvert.DeserializeObject<ApiResultResponse<ClientVM>>(jsonResponseClient);
        }
        else
        {
            string? errorContent = await responseClient.Content.ReadAsStringAsync();
            resultClient = new ApiResultResponse<ClientVM>
            {
                IsSuccess = false,
                Message = responseClient.StatusCode.ToString()
            };
        }

        if (!resultClient!.IsSuccess)
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