#region Namespaces
using System.Text;
using AvivCRM.UI.Areas.Environment.ViewModels;
using AvivCRM.UI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
#endregion

namespace AvivCRM.UI.Areas.Environment.Controllers;
[Area("Environment")]
public class EmployeeController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    #region Constructor
    public EmployeeController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    #endregion

    #region Retrieves a List of Employees
    /// <summary>
    /// Retrieves a list of Employees from the database.
    /// </summary>
    /// <param name=""></param>
    /// <returns>Modal popup will open to create New Employee</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/Employee/Employee
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    public async Task<IActionResult> Employee()
    {
        ViewData["pTitle"] = "Employees Profile";

        // Breadcrumb
        ViewData["bGParent"] = "Environment";
        ViewData["bParent"] = "Employee";
        ViewData["bChild"] = "Employee View";
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        ApiResultResponse<List<EmployeeVM>> employeeList = new();

        // fetch all the Employees
        employeeList =
                await client.GetFromJsonAsync<ApiResultResponse<List<EmployeeVM>>>("Employee/all-employee");

        return View(employeeList!.Data);
    }
    #endregion

    #region Create Employee functionionality
    /// <summary>
    /// Show the popup to create a new Employee.
    /// </summary>
    /// <param name=""></param>
    /// <returns>New Employee</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/Employee/Employee
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        EmployeeVM employee = new();
        return PartialView("_Create", employee);
    }

    /// <summary>
    /// New Employee will be create.
    /// </summary>
    /// <param name="employee">Employee entity that needs to be create</param>
    /// <returns>New Employee will be create.</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/Employee/Employee
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Create(EmployeeVM employee)
    {
        ApiResultResponse<EmployeeVM> resultEmployee = new();

        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (employee.Name == null)
        {
            employee.Name = "";
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");

        string? jsonEmployee = JsonConvert.SerializeObject(employee);
        StringContent? employeeContent = new(jsonEmployee, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseEmployee =
            await client.PostAsync("Employee/create-employee", employeeContent);

        if (responseEmployee.IsSuccessStatusCode)
        {
            string? jsonResponseEmployee = await responseEmployee.Content.ReadAsStringAsync();
            resultEmployee = JsonConvert.DeserializeObject<ApiResultResponse<EmployeeVM>>(jsonResponseEmployee);
        }
        else
        {
            string? errorContent = await responseEmployee.Content.ReadAsStringAsync();
            resultEmployee = new ApiResultResponse<EmployeeVM>
            {
                IsSuccess = false,
                Message = responseEmployee.StatusCode + "ErrorContent: " + errorContent
            };
        }

        if (!resultEmployee!.IsSuccess)
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

    #region Edit Employee functionionality
    /// <summary>
    /// Edit the existing Employee.
    /// </summary>
    /// <param name="Id">Employee Guid that needs to be edit</param>
    /// <returns>Popup will be open to edit the employee details</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// GET /Environment/Employee/Employee
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

        ApiResultResponse<EmployeeVM> employee = new();

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        employee =
            await client.GetFromJsonAsync<ApiResultResponse<EmployeeVM>>("Employee/byid-employee/?Id=" + Id);

        if (!employee!.IsSuccess)
        {
            return View();
        }

        return PartialView("_Edit", employee.Data);
    }

    /// <summary>
    /// Update the existing Employee.
    /// </summary>
    /// <param name="employee">Employee entity to update the existing employee</param>
    /// <returns>Changes will be updated for the existing employee</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/Employee/Employee
    /// </example>
    /// <remarks> 
    /// Created: 12-Jan-2025 by Sivan T
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Edit(EmployeeVM employee)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        if (employee.Name == null)
        {
            employee.Name = "";
        }

        ApiResultResponse<EmployeeVM> resultEmployee = new();

        if (GuidExtensions.IsNullOrEmpty(employee.Id))
        {
            return View();
        }

        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        string? jsonEmployee = JsonConvert.SerializeObject(employee);
        StringContent? employeeContent = new(jsonEmployee, Encoding.UTF8, "application/json");
        HttpResponseMessage? responseEmployee =
            await client.PutAsync("Employee/update-employee/", employeeContent);
        if (responseEmployee.IsSuccessStatusCode)
        {
            string? jsonResponseEmployee = await responseEmployee.Content.ReadAsStringAsync();
            resultEmployee = JsonConvert.DeserializeObject<ApiResultResponse<EmployeeVM>>(jsonResponseEmployee);
        }
        else
        {
            string? errorContent = await responseEmployee.Content.ReadAsStringAsync();
            resultEmployee = new ApiResultResponse<EmployeeVM>
            {
                IsSuccess = false,
                Message = responseEmployee.StatusCode.ToString() //$"Error: {response.StatusCode}. {errorContent}" }; 
            };
        }

        if (!resultEmployee!.IsSuccess)
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

    #region Delete Employee functionionality
    /// <summary>
    /// Delete the existing Employee.
    /// </summary>
    /// <param name="Id">Employee Guid that needs to be delete</param>
    /// <returns>Employee will be deleted</returns>
    /// <exception cref=""></exception>
    /// <example>
    /// POST /Environment/Employee/Employee
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

        ApiResultResponse<EmployeeVM> resultEmployee = new();
        HttpClient? client = _httpClientFactory.CreateClient("ApiGatewayCall");
        HttpResponseMessage? responseEmployee = await client.DeleteAsync("Employee/delete-employee?Id=" + Id);
        if (responseEmployee.IsSuccessStatusCode)
        {
            string? jsonResponseEmployee = await responseEmployee.Content.ReadAsStringAsync();
            resultEmployee = JsonConvert.DeserializeObject<ApiResultResponse<EmployeeVM>>(jsonResponseEmployee);
        }
        else
        {
            string? errorContent = await responseEmployee.Content.ReadAsStringAsync();
            resultEmployee = new ApiResultResponse<EmployeeVM>
            {
                IsSuccess = false,
                Message = responseEmployee.StatusCode.ToString()
            };
        }

        if (!resultEmployee!.IsSuccess)
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






















