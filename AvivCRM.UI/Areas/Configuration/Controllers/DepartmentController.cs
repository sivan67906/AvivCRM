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
public class DepartmentController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    #region Constructor
    public DepartmentController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    #endregion

    #region Retrieves a List of Departments
    /// <summary>
    /// Retrieves a list of Departments from the database.
    /// </summary>
    /// <param name=""></param>
    /// <returns>Modal popup will open to create New Department</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/Department/Department
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    public async Task<IActionResult> Department()
    {
        ViewData["pTitle"] = "Departments Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "Department";
        ViewData["bChild"] = "Department View";
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        ApiResultResponseConfigVM<List<DepartmentVM>> departmentList = new();

        // fetch all the Departments
        departmentList =
                await client.GetFromJsonAsync<ApiResultResponseConfigVM<List<DepartmentVM>>>("Department/all-department");

        return View(departmentList!.Data);
    }
    #endregion

    #region Create Department functionionality
    /// <summary>
    /// Show the popup to create a new Department.
    /// </summary>
    /// <param name=""></param>
    /// <returns>New Department</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/Department/Department
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    [HttpGet]
    public IActionResult Create()
    {
        DepartmentVM department = new();
        return PartialView("_Create", department);
    }

    /// <summary>
    /// New Department will be create.
    /// </summary>
    /// <param name="department">Department entity that needs to be create</param>
    /// <returns>New Department will be create.</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/Department/Department
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Create(DepartmentVM department)
    {
        ApiResultResponseConfigVM<DepartmentVM> resultDepartment = new();

        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (department.Name == null)
        {
            department.Name = "";
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        string? jsonDepartment = JsonConvert.SerializeObject(department);
        StringContent? departmentContent = new(jsonDepartment, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseDepartment =
            await client.PostAsync("Department/create-department", departmentContent);

        if (responseDepartment.IsSuccessStatusCode)
        {
            string? jsonResponseDepartment = await responseDepartment.Content.ReadAsStringAsync();
            resultDepartment = JsonConvert.DeserializeObject<ApiResultResponseConfigVM<DepartmentVM>>(jsonResponseDepartment);
        }
        else
        {
            string? errorContent = await responseDepartment.Content.ReadAsStringAsync();
            resultDepartment = new ApiResultResponseConfigVM<DepartmentVM>
            {
                IsSuccess = false,
                Message = responseDepartment.StatusCode + "ErrorContent: " + errorContent
            };
        }

        if (!resultDepartment!.IsSuccess)
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

    #region Edit Department functionionality
    /// <summary>
    /// Edit the existing Department.
    /// </summary>
    /// <param name="Id">Department Guid that needs to be edit</param>
    /// <returns>Popup will be open to edit the department details</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/Department/Department
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

        ApiResultResponseConfigVM<DepartmentVM> department = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        department =
            await client.GetFromJsonAsync<ApiResultResponseConfigVM<DepartmentVM>>("Department/byid-department/?Id=" + Id);

        if (!department!.IsSuccess)
        {
            return View();
        }

        return PartialView("_Edit", department.Data);
    }

    /// <summary>
    /// Update the existing Department.
    /// </summary>
    /// <param name="department">Department entity to update the existing department</param>
    /// <returns>Changes will be updated for the existing department</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/Department/Department
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Edit(DepartmentVM department)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (department.Name == null)
        {
            department.Name = "";
        }

        ApiResultResponseConfigVM<DepartmentVM> resultDepartment = new();

        if (GuidExtensions.IsNullOrEmpty(department.Id))
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        string? jsonDepartment = JsonConvert.SerializeObject(department);
        StringContent? departmentContent = new(jsonDepartment, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseDepartment =
            await client.PutAsync("Department/update-department/", departmentContent);
        if (responseDepartment.IsSuccessStatusCode)
        {
            string? jsonResponseDepartment = await responseDepartment.Content.ReadAsStringAsync();
            resultDepartment = JsonConvert.DeserializeObject<ApiResultResponseConfigVM<DepartmentVM>>(jsonResponseDepartment);
        }
        else
        {
            string? errorContent = await responseDepartment.Content.ReadAsStringAsync();
            resultDepartment = new ApiResultResponseConfigVM<DepartmentVM>
            {
                IsSuccess = false,
                Message = responseDepartment.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        if (!resultDepartment!.IsSuccess)
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

    #region Delete Department functionionality
    /// <summary>
    /// Delete the existing Department.
    /// </summary>
    /// <param name="Id">Department Guid that needs to be delete</param>
    /// <returns>Department will be deleted</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/Department/Department
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

        ApiResultResponseConfigVM<DepartmentVM> resultDepartment = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? responseDepartment = await client.DeleteAsync("Department/delete-department?Id=" + Id);
        if (responseDepartment.IsSuccessStatusCode)
        {
            string? jsonResponseDepartment = await responseDepartment.Content.ReadAsStringAsync();
            resultDepartment = JsonConvert.DeserializeObject<ApiResultResponseConfigVM<DepartmentVM>>(jsonResponseDepartment);
        }
        else
        {
            string? errorContent = await responseDepartment.Content.ReadAsStringAsync();
            resultDepartment = new ApiResultResponseConfigVM<DepartmentVM>
            {
                IsSuccess = false,
                Message = responseDepartment.StatusCode.ToString()
            };
        }

        if (!resultDepartment!.IsSuccess)
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






















