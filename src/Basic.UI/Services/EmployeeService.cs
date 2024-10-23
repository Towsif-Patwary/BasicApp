using Basic.Application.DTOs;
using Basic.Domain.Entity;
using Basic.UI.Common;
using System.Net.Http.Json;

namespace Basic.UI.Services;

public class EmployeeService
{
    private readonly HttpClient _httpClient;

    public EmployeeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
    {
        try
        {
            var employees = await _httpClient.GetFromJsonAsync<IEnumerable<Employee>>(StaticData.UrlHttp + "api/employee");
            return employees;
        }
        catch (Exception e)
        {
            try
            {
                var employees = await _httpClient.GetFromJsonAsync<IEnumerable<Employee>>(StaticData.UrlHttps + "api/employee");
                return employees;
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<Employee>(); 
            }
        }
    }

    public async Task<Employee> GetEmployeeByNameOrCodeAsync(EmployeeUpdateOrRemoveVm employeeData)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<Employee>(StaticData.UrlHttp + $"api/employee/{employeeData.CodeOrName}");
        }
        catch (Exception ex)
        {
            return null;
        }
       
    }

    public async Task<string> AddEmployeeAsync(EmployeeVm employee)
    {
        var response = await _httpClient.PostAsJsonAsync(StaticData.UrlHttp + "api/employee", employee);
        return response.IsSuccessStatusCode ? "Employee added successfully." : "Failed to add employee.";
    }

    public async Task<string> UpdateEmployeeAsync(EmployeeUpdateOrRemoveVm employeeData, EmployeeVm employee)
    {
        var response = await _httpClient.PutAsJsonAsync(StaticData.UrlHttp + $"api/employee/{employeeData.CodeOrName}", employee);
        return response.IsSuccessStatusCode ? "Employee updated successfully." : "Failed to update employee.";
    }

    public async Task<string> RemoveEmployeeAsync(EmployeeUpdateOrRemoveVm employeeData)
    {
        var response = await _httpClient.DeleteAsync(StaticData.UrlHttp + $"api/employee/{employeeData.CodeOrName}");
        return response.IsSuccessStatusCode ? "Employee deleted successfully." : "Failed to delete employee.";
    }
}
