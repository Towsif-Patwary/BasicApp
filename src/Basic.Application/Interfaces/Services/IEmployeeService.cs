using Basic.Application.DTOs;
using Basic.Core.Common;
using Basic.Domain.Entity;

namespace Basic.Application.Interfaces.Services
{
    public interface IEmployeeService
    {
        Task<Result<IEnumerable<Employee>>> GetAllEmployeesAsync();
        Task<Result<Employee>> GetEmployeeByNameOrCodeAsync(EmployeeUpdateOrRemoveVm employeedata);
        Task<Result<string>> AddEmployeeAsync(EmployeeVm employee);
        Task<Result<string>> UpdateEmployeeAsync(EmployeeUpdateOrRemoveVm employeedata, EmployeeVm employee);
        Task<Result<string>> RemoveEmployeeAsync(EmployeeUpdateOrRemoveVm employeedata);
    }
}
