using System.Text;
using AvivCRM.UI.Areas.Environment.ViewModels;
using AvivCRM.UI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class MessageController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    #region Constructor
    public MessageController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    #endregion

    #region Retrieves a List of Messages
    /// <summary>
    /// Retrieves a list of Messages from the database.
    /// </summary>
    /// <param name=""></param>
    /// <returns>Modal popup will open to create New Message</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/Message/Message
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Kansheyam
    /// </remarks>
    public async Task<IActionResult> Message()
    {
        ViewData["pTitle"] = "Messages Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "Message";
        ViewData["bChild"] = "Message View";
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        ApiResultResponse<List<MessageVM>> messageList = new();

        // fetch all the Messages
        messageList =
                await client.GetFromJsonAsync<ApiResultResponse<List<MessageVM>>>("Message/all-message");

        return View(messageList!.Data);
    }
    #endregion

    #region Create Message functionionality
    /// <summary>
    /// Show the popup to create a new Message.
    /// </summary>
    /// <param name=""></param>
    /// <returns>New Message</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/Message/Message
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Kansheyam
    /// </remarks>
    [HttpGet]
    public IActionResult Create()
    {
        MessageVM message = new();
        return PartialView("_Create", message);
    }

    /// <summary>
    /// New Message will be create.
    /// </summary>
    /// <param name="message">Message entity that needs to be create</param>
    /// <returns>New Message will be create.</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/Message/Message
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Kansheyam
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Create(MessageVM message)
    {
        ApiResultResponse<MessageVM> resultMessage = new();

        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        //if (message.Name == null)
        //{
        //    message.Name = "";
        //}

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        string? jsonMessage = JsonConvert.SerializeObject(message);
        StringContent? messageContent = new(jsonMessage, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseMessage =
            await client.PostAsync("Message/create-message", messageContent);

        if (responseMessage.IsSuccessStatusCode)
        {
            string? jsonResponseMessage = await responseMessage.Content.ReadAsStringAsync();
            resultMessage = JsonConvert.DeserializeObject<ApiResultResponse<MessageVM>>(jsonResponseMessage);
        }
        else
        {
            string? errorContent = await responseMessage.Content.ReadAsStringAsync();
            resultMessage = new ApiResultResponse<MessageVM>
            {
                IsSuccess = false,
                Message = responseMessage.StatusCode + "ErrorContent: " + errorContent
            };
        }

        if (!resultMessage!.IsSuccess)
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

    #region Edit Message functionionality
    /// <summary>
    /// Edit the existing Message.
    /// </summary>
    /// <param name="Id">Message Guid that needs to be edit</param>
    /// <returns>Popup will be open to edit the message details</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/Message/Message
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

        ApiResultResponse<MessageVM> message = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        message =
            await client.GetFromJsonAsync<ApiResultResponse<MessageVM>>("Message/byid-message/?Id=" + Id);

        if (!message!.IsSuccess)
        {
            return View();
        }

        return PartialView("_Edit", message.Data);
    }

    /// <summary>
    /// Update the existing Message.
    /// </summary>
    /// <param name="message">Message entity to update the existing message</param>
    /// <returns>Changes will be updated for the existing message</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/Message/Message
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Kansheyam
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Edit(MessageVM message)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        //if (message.Name == null)
        //{
        //    message.Name = "";
        //}

        ApiResultResponse<MessageVM> resultMessage = new();

        if (GuidExtensions.IsNullOrEmpty(message.Id))
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        string? jsonMessage = JsonConvert.SerializeObject(message);
        StringContent? messageContent = new(jsonMessage, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseMessage =
            await client.PutAsync("Message/update-message/", messageContent);
        if (responseMessage.IsSuccessStatusCode)
        {
            string? jsonResponseMessage = await responseMessage.Content.ReadAsStringAsync();
            resultMessage = JsonConvert.DeserializeObject<ApiResultResponse<MessageVM>>(jsonResponseMessage);
        }
        else
        {
            string? errorContent = await responseMessage.Content.ReadAsStringAsync();
            resultMessage = new ApiResultResponse<MessageVM>
            {
                IsSuccess = false,
                Message = responseMessage.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        if (!resultMessage!.IsSuccess)
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

    #region Delete Message functionionality
    /// <summary>
    /// Delete the existing Message.
    /// </summary>
    /// <param name="Id">Message Guid that needs to be delete</param>
    /// <returns>Lead source will be deleted</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/Message/Message
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

        ApiResultResponse<MessageVM> resultMessage = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? responseMessage = await client.DeleteAsync("Message/delete-message?Id=" + Id);
        if (responseMessage.IsSuccessStatusCode)
        {
            string? jsonResponseMessage = await responseMessage.Content.ReadAsStringAsync();
            resultMessage = JsonConvert.DeserializeObject<ApiResultResponse<MessageVM>>(jsonResponseMessage);
        }
        else
        {
            string? errorContent = await responseMessage.Content.ReadAsStringAsync();
            resultMessage = new ApiResultResponse<MessageVM>
            {
                IsSuccess = false,
                Message = responseMessage.StatusCode.ToString()
            };
        }

        if (!resultMessage!.IsSuccess)
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