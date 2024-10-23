using Basic.Application.DTOs;
using Basic.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Basic.API.Controllers;

public class CompanyController : BaseController
{
    private readonly ICompanyService _companyService;

    public CompanyController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCompanies()
    {
        var result = await _companyService.GetAllCompaniesAsync();

        if (result.Success)
        {
            return Ok(result.Data);
        }

        return NotFound(result.Message);
    }

    [HttpGet("{companyData}")]
    public async Task<IActionResult> GetCompanyByNameOrCode(string companyData)
    {
        var companydataVm = new CompanyUpdateOrRemoveVm { CodeOrName = companyData };
        var result = await _companyService.GetCompanyByNameOrCodeAsync(companydataVm);

        if (result.Success)
        {
            return Ok(result.Data);
        }

        return NotFound(result.Message);
    }

    [HttpPost]
    public async Task<IActionResult> AddCompany([FromBody] CompanyVm companymodel)
    {
        var result = await _companyService.AddCompanyAsync(companymodel);

        if (result.Success)
        {
            return Ok(result.Message);
        }

        return BadRequest(result.Message);
    }

    [HttpPut("{companyData}")]
    public async Task<IActionResult> UpdateCompany(string companyData, [FromBody] CompanyVm companymodel)
    {
        var companydataVm = new CompanyUpdateOrRemoveVm { CodeOrName = companyData };
        var result = await _companyService.UpdateCompanyAsync(companydataVm, companymodel);

        if (result.Success)
        {
            return Ok(result.Message);
        }

        return BadRequest(result.Message);
    }

    [HttpDelete("{companyData}")]
    public async Task<IActionResult> RemoveCompany(string companyData)
    {
        var companydataVm = new CompanyUpdateOrRemoveVm { CodeOrName = companyData };
        var result = await _companyService.RemoveCompanyAsync(companydataVm);

        if (result.Success)
        {
            return Ok(result.Message);
        }

        return BadRequest(result.Message);
    }
}
