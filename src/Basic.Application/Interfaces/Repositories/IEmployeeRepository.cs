using Basic.Application.DTOs;
using Basic.Core.Common;
using Basic.Domain.Entity;

namespace Basic.Application.Interfaces.Repositories
{
    public interface IEmployeeRepository
    {
        Task<Result<IEnumerable<Employee>>> GetAllEmployeesAsync();
        Task<Result<Employee>> GetEmployeeByNameOrCodeAsync(EmployeeUpdateOrRemoveVm employeedata);
        Task<Result<string>> AddEmployeeAsync(Employee employeemodel);
        Task<Result<string>> UpdateEmployeeAsync(EmployeeUpdateOrRemoveVm employeedata, EmployeeVm employeemodel);
        Task<Result<string>> RemoveEmployeeAsync(EmployeeUpdateOrRemoveVm employeedata);
    }
}
