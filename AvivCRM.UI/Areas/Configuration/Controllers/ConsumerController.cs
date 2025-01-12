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
public class ConsumerController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    #region Constructor
    public ConsumerController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    #endregion

    #region Retrieves a List of Consumers
    /// <summary>
    /// Retrieves a list of Consumers from the database.
    /// </summary>
    /// <param name=""></param>
    /// <returns>Modal popup will open to create New Consumer</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/Consumer/Consumer
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    public async Task<IActionResult> Consumer()
    {
        ViewData["pTitle"] = "Consumers Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "Consumer";
        ViewData["bChild"] = "Consumer View";
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        ApiResultResponseConfigVM<List<ConsumerVM>> consumerList = new();

        // fetch all the Consumers
        consumerList =
                await client.GetFromJsonAsync<ApiResultResponseConfigVM<List<ConsumerVM>>>("Consumer/all-consumer");

        return View(consumerList!.Data);
    }
    #endregion

    #region Create Consumer functionionality
    /// <summary>
    /// Show the popup to create a new Consumer.
    /// </summary>
    /// <param name=""></param>
    /// <returns>New Consumer</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/Consumer/Consumer
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    [HttpGet]
    public IActionResult Create()
    {
        ConsumerVM consumer = new();
        return PartialView("_Create", consumer);
    }

    /// <summary>
    /// New Consumer will be create.
    /// </summary>
    /// <param name="consumer">Consumer entity that needs to be create</param>
    /// <returns>New Consumer will be create.</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/Consumer/Consumer
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Create(ConsumerVM consumer)
    {
        ApiResultResponseConfigVM<ConsumerVM> resultConsumer = new();

        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (consumer.Name == null)
        {
            consumer.Name = "";
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        string? jsonConsumer = JsonConvert.SerializeObject(consumer);
        StringContent? consumerContent = new(jsonConsumer, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseConsumer =
            await client.PostAsync("Consumer/create-consumer", consumerContent);

        if (responseConsumer.IsSuccessStatusCode)
        {
            string? jsonResponseConsumer = await responseConsumer.Content.ReadAsStringAsync();
            resultConsumer = JsonConvert.DeserializeObject<ApiResultResponseConfigVM<ConsumerVM>>(jsonResponseConsumer);
        }
        else
        {
            string? errorContent = await responseConsumer.Content.ReadAsStringAsync();
            resultConsumer = new ApiResultResponseConfigVM<ConsumerVM>
            {
                IsSuccess = false,
                Message = responseConsumer.StatusCode + "ErrorContent: " + errorContent
            };
        }

        if (!resultConsumer!.IsSuccess)
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

    #region Edit Consumer functionionality
    /// <summary>
    /// Edit the existing Consumer.
    /// </summary>
    /// <param name="Id">Consumer Guid that needs to be edit</param>
    /// <returns>Popup will be open to edit the consumer details</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/Consumer/Consumer
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

        ApiResultResponseConfigVM<ConsumerVM> consumer = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        consumer =
            await client.GetFromJsonAsync<ApiResultResponseConfigVM<ConsumerVM>>("Consumer/byid-consumer/?Id=" + Id);

        if (!consumer!.IsSuccess)
        {
            return View();
        }

        return PartialView("_Edit", consumer.Data);
    }

    /// <summary>
    /// Update the existing Consumer.
    /// </summary>
    /// <param name="consumer">Consumer entity to update the existing consumer</param>
    /// <returns>Changes will be updated for the existing consumer</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/Consumer/Consumer
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Edit(ConsumerVM consumer)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (consumer.Name == null)
        {
            consumer.Name = "";
        }

        ApiResultResponseConfigVM<ConsumerVM> resultConsumer = new();

        if (GuidExtensions.IsNullOrEmpty(consumer.Id))
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        string? jsonConsumer = JsonConvert.SerializeObject(consumer);
        StringContent? consumerContent = new(jsonConsumer, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseConsumer =
            await client.PutAsync("Consumer/update-consumer/", consumerContent);
        if (responseConsumer.IsSuccessStatusCode)
        {
            string? jsonResponseConsumer = await responseConsumer.Content.ReadAsStringAsync();
            resultConsumer = JsonConvert.DeserializeObject<ApiResultResponseConfigVM<ConsumerVM>>(jsonResponseConsumer);
        }
        else
        {
            string? errorContent = await responseConsumer.Content.ReadAsStringAsync();
            resultConsumer = new ApiResultResponseConfigVM<ConsumerVM>
            {
                IsSuccess = false,
                Message = responseConsumer.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        if (!resultConsumer!.IsSuccess)
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

    #region Delete Consumer functionionality
    /// <summary>
    /// Delete the existing Consumer.
    /// </summary>
    /// <param name="Id">Consumer Guid that needs to be delete</param>
    /// <returns>Consumer will be deleted</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/Consumer/Consumer
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

        ApiResultResponseConfigVM<ConsumerVM> resultConsumer = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? responseConsumer = await client.DeleteAsync("Consumer/delete-consumer?Id=" + Id);
        if (responseConsumer.IsSuccessStatusCode)
        {
            string? jsonResponseConsumer = await responseConsumer.Content.ReadAsStringAsync();
            resultConsumer = JsonConvert.DeserializeObject<ApiResultResponseConfigVM<ConsumerVM>>(jsonResponseConsumer);
        }
        else
        {
            string? errorContent = await responseConsumer.Content.ReadAsStringAsync();
            resultConsumer = new ApiResultResponseConfigVM<ConsumerVM>
            {
                IsSuccess = false,
                Message = responseConsumer.StatusCode.ToString()
            };
        }

        if (!resultConsumer!.IsSuccess)
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






















