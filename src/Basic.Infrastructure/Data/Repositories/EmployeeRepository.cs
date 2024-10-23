using Basic.Application.DTOs;
using Basic.Application.Interfaces.Repositories;
using Basic.Core.Common;
using Basic.Domain.Entity;
using Dapper;
using Microsoft.Extensions.Caching.Memory;
using System.Data;

namespace Basic.Infrastructure.Data.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly IDbConnection _dbConnection; 
    private readonly IMemoryCache _cache;

    public EmployeeRepository(IDbConnection dbConnection, IMemoryCache cache)
    {
        _dbConnection = dbConnection;
        _cache = cache;
    }

    public async Task<Result<IEnumerable<Employee>>> GetAllEmployeesAsync()
    {
        string query = "SELECT TOP (100) [Id],[Code],[Name],[Address] ,[Phone],[Email],[Company] FROM [Employees] ORDER BY NAME ASC";

        var employees = await _dbConnection.QueryAsync<Employee>(query);

        if (employees != null && employees.Any())
        {
            return Result<IEnumerable<Employee>>.SuccessResult(employees, "Employees retrieved successfully.");
        }

        return Result<IEnumerable<Employee>>.FailureResult("No employees found.");
    }

    public async Task<Result<Employee>> GetEmployeeByNameOrCodeAsync(EmployeeUpdateOrRemoveVm employeedata)
    {
        string query = "SELECT [Id],[Code],[Name],[Address] ,[Phone],[Email],[Company] FROM Employees WHERE (Code = @CodeOrName OR Name = @CodeOrName) ";
        var employee = await _dbConnection.QueryFirstOrDefaultAsync<Employee>(query, new { CodeOrName = employeedata.CodeOrName });

        if (employee != null)
        {
            return Result<Employee>.SuccessResult(employee, "Employee retrieved successfully.");
        }

        return Result<Employee>.FailureResult("Employee not found.");
    }

    public async Task<Result<string>> AddEmployeeAsync(Employee employee)
    {
        employee.Code = await GenerateEmployeeCodeAsync();

        string checkQuery = "SELECT COUNT(1) FROM Employees WHERE (Name != @Name AND Code != @Code)";

        var existingEmployee = await _dbConnection.QueryFirstOrDefaultAsync<Employee>(checkQuery, new { employee.Name, employee.Code });

        if (existingEmployee != null && existingEmployee.Name != null)
        {
            return Result<string>.FailureResult("An employee with the same Code or Name already exists.");
        }

        string query = @"
            INSERT INTO Employees (Code, Name, Address, Phone, Email, Company) 
            VALUES (@Code, @Name, @Address, @Phone, @Email, @Company)";

        var result = await _dbConnection.ExecuteAsync(query, employee);

        if (result > 0)
        {
            _cache.Remove(CacheKeys.AllEmployees);
            return Result<string>.SuccessResult("Employee added successfully.");
        }

        return Result<string>.FailureResult("Failed to add employee.");
    }

    public async Task<Result<string>> UpdateEmployeeAsync(EmployeeUpdateOrRemoveVm employeedata, EmployeeVm employee)
    {
        string updateQuery = @" UPDATE Employees
                                    SET Name = @Name, Address = @Address, Phone = @Phone, Email = @Email, Company = @Company
                                    WHERE (Name = @CodeOrName OR Code = @CodeOrName)";

        var updateResult = await _dbConnection.ExecuteAsync(updateQuery, new
        {
            employee.Name,
            employee.Address,
            employee.Phone,
            employee.Email,
            employee.Company,
            employeedata.CodeOrName
        });

        if (updateResult > 0)
        {
            return Result<string>.SuccessResult("Employee updated successfully.");
        }

        return Result<string>.FailureResult("Failed to update employee.");
    }

    public async Task<Result<string>> RemoveEmployeeAsync(EmployeeUpdateOrRemoveVm employeedata)
    {
        string query = "DELETE FROM Employees WHERE Code = @Code";

        var result = await _dbConnection.ExecuteAsync(query, new { Code = employeedata.CodeOrName });

        if (result > 0)
        {
            return Result<string>.SuccessResult("Employee deleted successfully.");
        }

        return Result<string>.FailureResult("Failed to delete employee.");
    }

    public async Task<string> GenerateEmployeeCodeAsync()
    {
        string query = "SELECT TOP 1 Code FROM Employees ORDER BY Id DESC";
        var lastCode = await _dbConnection.QueryFirstOrDefaultAsync<string>(query);

        if (string.IsNullOrEmpty(lastCode))
        {
            return "EMP001"; 
        }

        string numericPart = lastCode.Substring(3);

        if (int.TryParse(numericPart, out int number))
        {
            number++;
            string newCode = "EMP" + number.ToString("D3"); 
            return newCode;
        }

        throw new Exception("Failed to generate a new employee code.");
    }

}
