#region Namespaces
using System.Text;
using AvivCRM.UI.Areas.Environment.ViewModels;
using AvivCRM.UI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
#endregion

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class NotificationController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    #region Constructor
    public NotificationController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    #endregion

    #region Retrieves a List of Notifications
    /// <summary>
    /// Retrieves a list of Notifications from the database.
    /// </summary>
    /// <param name=""></param>
    /// <returns>Modal popup will open to create New Notification</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/Notification/Notification
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    public async Task<IActionResult> Notification()
    {
        ViewData["pTitle"] = "Notifications Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "Notification";
        ViewData["bChild"] = "Notification View";
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        ApiResultResponse<List<NotificationVM>> notificationList = new();

        // fetch all the Notifications
        notificationList =
                await client.GetFromJsonAsync<ApiResultResponse<List<NotificationVM>>>("Notification/all-notification");

        return View(notificationList!.Data);
    }
    #endregion

    #region Create Notification functionionality
    /// <summary>
    /// Show the popup to create a new Notification.
    /// </summary>
    /// <param name=""></param>
    /// <returns>New Notification</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/Notification/Notification
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        NotificationVM notification = new();
        return PartialView("_Create", notification);
    }

    /// <summary>
    /// New Notification will be create.
    /// </summary>
    /// <param name="notification">Notification entity that needs to be create</param>
    /// <returns>New Notification will be create.</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/Notification/Notification
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Create(NotificationVM notification)
    {
        ApiResultResponse<NotificationVM> resultNotification = new();

        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (notification.Name == null)
        {
            notification.Name = "";
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        string? jsonNotification = JsonConvert.SerializeObject(notification);
        StringContent? notificationContent = new(jsonNotification, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseNotification =
            await client.PostAsync("Notification/create-notification", notificationContent);

        if (responseNotification.IsSuccessStatusCode)
        {
            string? jsonResponseNotification = await responseNotification.Content.ReadAsStringAsync();
            resultNotification = JsonConvert.DeserializeObject<ApiResultResponse<NotificationVM>>(jsonResponseNotification);
        }
        else
        {
            string? errorContent = await responseNotification.Content.ReadAsStringAsync();
            resultNotification = new ApiResultResponse<NotificationVM>
            {
                IsSuccess = false,
                Message = responseNotification.StatusCode + "ErrorContent: " + errorContent
            };
        }

        if (!resultNotification!.IsSuccess)
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

    #region Edit Notification functionionality
    /// <summary>
    /// Edit the existing Notification.
    /// </summary>
    /// <param name="Id">Notification Guid that needs to be edit</param>
    /// <returns>Popup will be open to edit the notification details</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/Notification/Notification
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

        ApiResultResponse<NotificationVM> notification = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        notification =
            await client.GetFromJsonAsync<ApiResultResponse<NotificationVM>>("Notification/byid-notification/?Id=" + Id);

        if (!notification!.IsSuccess)
        {
            return View();
        }

        return PartialView("_Edit", notification.Data);
    }

    /// <summary>
    /// Update the existing Notification.
    /// </summary>
    /// <param name="notification">Notification entity to update the existing notification</param>
    /// <returns>Changes will be updated for the existing notification</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/Notification/Notification
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Edit(NotificationVM notification)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (notification.Name == null)
        {
            notification.Name = "";
        }

        ApiResultResponse<NotificationVM> resultNotification = new();

        if (GuidExtensions.IsNullOrEmpty(notification.Id))
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        string? jsonNotification = JsonConvert.SerializeObject(notification);
        StringContent? notificationContent = new(jsonNotification, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseNotification =
            await client.PutAsync("Notification/update-notification/", notificationContent);
        if (responseNotification.IsSuccessStatusCode)
        {
            string? jsonResponseNotification = await responseNotification.Content.ReadAsStringAsync();
            resultNotification = JsonConvert.DeserializeObject<ApiResultResponse<NotificationVM>>(jsonResponseNotification);
        }
        else
        {
            string? errorContent = await responseNotification.Content.ReadAsStringAsync();
            resultNotification = new ApiResultResponse<NotificationVM>
            {
                IsSuccess = false,
                Message = responseNotification.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        if (!resultNotification!.IsSuccess)
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

    #region Delete Notification functionionality
    /// <summary>
    /// Delete the existing Notification.
    /// </summary>
    /// <param name="Id">Notification Guid that needs to be delete</param>
    /// <returns>Notification will be deleted</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/Notification/Notification
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

        ApiResultResponse<NotificationVM> resultNotification = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? responseNotification = await client.DeleteAsync("Notification/delete-notification?Id=" + Id);
        if (responseNotification.IsSuccessStatusCode)
        {
            string? jsonResponseNotification = await responseNotification.Content.ReadAsStringAsync();
            resultNotification = JsonConvert.DeserializeObject<ApiResultResponse<NotificationVM>>(jsonResponseNotification);
        }
        else
        {
            string? errorContent = await responseNotification.Content.ReadAsStringAsync();
            resultNotification = new ApiResultResponse<NotificationVM>
            {
                IsSuccess = false,
                Message = responseNotification.StatusCode.ToString()
            };
        }

        if (!resultNotification!.IsSuccess)
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






















