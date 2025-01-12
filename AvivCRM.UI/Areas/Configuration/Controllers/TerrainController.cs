#region Namespaces
using System.Text;
using AvivCRM.UI.Areas.Configuration.ViewModels;
using AvivCRM.UI.Areas.Configuraton.ViewModels;
using AvivCRM.UI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
#endregion

namespace AvivCRM.UI.Areas.Configuraton.Controllers;
[Area("Environment")]
public class TerrainController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    #region Constructor
    public TerrainController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    #endregion

    #region Retrieves a List of Terrains
    /// <summary>
    /// Retrieves a list of Terrains from the database.
    /// </summary>
    /// <param name=""></param>
    /// <returns>Modal popup will open to create New Terrain</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/Terrain/Terrain
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    public async Task<IActionResult> Terrain()
    {
        ViewData["pTitle"] = "Terrains Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "Terrain";
        ViewData["bChild"] = "Terrain View";
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        ApiResultResponseConfigVM<List<TerrainVM>> terrainList = new();

        // fetch all the Terrains
        terrainList =
                await client.GetFromJsonAsync<ApiResultResponseConfigVM<List<TerrainVM>>>("Terrain/all-terrain");

        return View(terrainList!.Data);
    }
    #endregion

    #region Create Terrain functionionality
    /// <summary>
    /// Show the popup to create a new Terrain.
    /// </summary>
    /// <param name=""></param>
    /// <returns>New Terrain</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/Terrain/Terrain
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        TerrainVM terrain = new();
        return PartialView("_Create", terrain);
    }

    /// <summary>
    /// New Terrain will be create.
    /// </summary>
    /// <param name="terrain">Terrain entity that needs to be create</param>
    /// <returns>New Terrain will be create.</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/Terrain/Terrain
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Create(TerrainVM terrain)
    {
        ApiResultResponseConfigVM<TerrainVM> resultTerrain = new();

        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (terrain.Name == null)
        {
            terrain.Name = "";
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        string? jsonTerrain = JsonConvert.SerializeObject(terrain);
        StringContent? terrainContent = new(jsonTerrain, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseTerrain =
            await client.PostAsync("Terrain/create-terrain", terrainContent);

        if (responseTerrain.IsSuccessStatusCode)
        {
            string? jsonResponseTerrain = await responseTerrain.Content.ReadAsStringAsync();
            resultTerrain = JsonConvert.DeserializeObject<ApiResultResponseConfigVM<TerrainVM>>(jsonResponseTerrain);
        }
        else
        {
            string? errorContent = await responseTerrain.Content.ReadAsStringAsync();
            resultTerrain = new ApiResultResponseConfigVM<TerrainVM>
            {
                IsSuccess = false,
                Message = responseTerrain.StatusCode + "ErrorContent: " + errorContent
            };
        }

        if (!resultTerrain!.IsSuccess)
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

    #region Edit Terrain functionionality
    /// <summary>
    /// Edit the existing Terrain.
    /// </summary>
    /// <param name="Id">Terrain Guid that needs to be edit</param>
    /// <returns>Popup will be open to edit the terrain details</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/Terrain/Terrain
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

        ApiResultResponseConfigVM<TerrainVM> terrain = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        terrain =
            await client.GetFromJsonAsync<ApiResultResponseConfigVM<TerrainVM>>("Terrain/byid-terrain/?Id=" + Id);

        if (!terrain!.IsSuccess)
        {
            return View();
        }

        return PartialView("_Edit", terrain.Data);
    }

    /// <summary>
    /// Update the existing Terrain.
    /// </summary>
    /// <param name="terrain">Terrain entity to update the existing terrain</param>
    /// <returns>Changes will be updated for the existing terrain</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/Terrain/Terrain
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Edit(TerrainVM terrain)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (terrain.Name == null)
        {
            terrain.Name = "";
        }

        ApiResultResponseConfigVM<TerrainVM> resultTerrain = new();

        if (GuidExtensions.IsNullOrEmpty(terrain.Id))
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        string? jsonTerrain = JsonConvert.SerializeObject(terrain);
        StringContent? terrainContent = new(jsonTerrain, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseTerrain =
            await client.PutAsync("Terrain/update-terrain/", terrainContent);
        if (responseTerrain.IsSuccessStatusCode)
        {
            string? jsonResponseTerrain = await responseTerrain.Content.ReadAsStringAsync();
            resultTerrain = JsonConvert.DeserializeObject<ApiResultResponseConfigVM<TerrainVM>>(jsonResponseTerrain);
        }
        else
        {
            string? errorContent = await responseTerrain.Content.ReadAsStringAsync();
            resultTerrain = new ApiResultResponseConfigVM<TerrainVM>
            {
                IsSuccess = false,
                Message = responseTerrain.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        if (!resultTerrain!.IsSuccess)
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

    #region Delete Terrain functionionality
    /// <summary>
    /// Delete the existing Terrain.
    /// </summary>
    /// <param name="Id">Terrain Guid that needs to be delete</param>
    /// <returns>Terrain will be deleted</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/Terrain/Terrain
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

        ApiResultResponseConfigVM<TerrainVM> resultTerrain = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? responseTerrain = await client.DeleteAsync("Terrain/delete-terrain?Id=" + Id);
        if (responseTerrain.IsSuccessStatusCode)
        {
            string? jsonResponseTerrain = await responseTerrain.Content.ReadAsStringAsync();
            resultTerrain = JsonConvert.DeserializeObject<ApiResultResponseConfigVM<TerrainVM>>(jsonResponseTerrain);
        }
        else
        {
            string? errorContent = await responseTerrain.Content.ReadAsStringAsync();
            resultTerrain = new ApiResultResponseConfigVM<TerrainVM>
            {
                IsSuccess = false,
                Message = responseTerrain.StatusCode.ToString()
            };
        }

        if (!resultTerrain!.IsSuccess)
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






















