using Basic.Application.DTOs;
using Basic.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Basic.API.Controllers;

public class EmployeeController : BaseController
{
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllEmployees()
    {
        var result = await _employeeService.GetAllEmployeesAsync();

        if (result.Success)
        {
            return Ok(result.Data);
        }

        return NotFound(result.Message);
    }

    [HttpGet("{employeeData}")]
    public async Task<IActionResult> GetEmployeeByNameOrCode(string employeeData)
    {
        var employeedataVm = new EmployeeUpdateOrRemoveVm { CodeOrName = employeeData };
        var result = await _employeeService.GetEmployeeByNameOrCodeAsync(employeedataVm);

        if (result.Success)
        {
            return Ok(result.Data);
        }

        return NotFound(result.Message);
    }

    [HttpPost]
    public async Task<IActionResult> AddEmployee([FromBody] EmployeeVm employee)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _employeeService.AddEmployeeAsync(employee);

        if (result.Success)
        {
            return Ok(result.Message);
        }

        return BadRequest(result.Message);
    }

    [HttpPut("{employeeData}")]
    public async Task<IActionResult> UpdateEmployee(string employeeData, [FromBody] EmployeeVm employee)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var employeedataVm = new EmployeeUpdateOrRemoveVm { CodeOrName = employeeData };
        var result = await _employeeService.UpdateEmployeeAsync(employeedataVm, employee);

        if (result.Success)
        {
            return Ok(result.Message);
        }

        return BadRequest(result.Message);
    }

    [HttpDelete("{employeeData}")]
    public async Task<IActionResult> RemoveEmployee(string employeeData)
    {
        var employeedataVm = new EmployeeUpdateOrRemoveVm { CodeOrName = employeeData };
        var result = await _employeeService.RemoveEmployeeAsync(employeedataVm);

        if (result.Success)
        {
            return Ok(result.Message);
        }

        return BadRequest(result.Message);
    }
}
