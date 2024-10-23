using Basic.Application.DTOs;
using Basic.Application.Interfaces.Repositories;
using Basic.Core.Common;
using Basic.Domain.Entity;
using Dapper;
using System.Data;

namespace Basic.Infrastructure.Data.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly IDbConnection _dbConnection;

    public CompanyRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<Result<IEnumerable<Company>>> GetAllCompaniesAsync()
    {
        string query = "SELECT * FROM Companies";

        var companies = await _dbConnection.QueryAsync<Company>(query);

        if (companies != null && companies.Any())
        {
            return Result<IEnumerable<Company>>.SuccessResult(companies, "Companies retrieved successfully.");
        }

        return Result<IEnumerable<Company>>.FailureResult("No companies found.");
    }

    public async Task<Result<Company>> GetCompanyByNameOrCodeAsync(CompanyUpdateOrRemoveVm companydata)
    {
        string query = "SELECT * FROM Companies WHERE (Code = @CodeOrName OR Name = @CodeOrName) ";
        var company = await _dbConnection.QueryFirstOrDefaultAsync<Company>(query, new { CodeOrName = companydata.CodeOrName });

        if (company != null)
        {
            return Result<Company>.SuccessResult(company, "Company retrieved successfully.");
        }

        return Result<Company>.FailureResult("Company not found.");
    }

    public async Task<Result<string>> AddCompanyAsync(Company companymodel)
    {
        companymodel.Code = await GenerateCompanyCodeAsync();

        string checkQuery = "SELECT COUNT(1) FROM Companies WHERE (Name != @Name AND Code != @Code)";

        var existingCompany = await _dbConnection.QueryFirstOrDefaultAsync<Company>(checkQuery, new { companymodel.Name, companymodel.Code });

        if (existingCompany != null && existingCompany.Name != null)
        {
            return Result<string>.FailureResult("An company with the same Code or Name already exists.");
        }

        string query = @"
            INSERT INTO Companies (Code, Name) 
            VALUES (@Code, @Name)";

        var result = await _dbConnection.ExecuteAsync(query, companymodel);

        if (result > 0)
        {
            return Result<string>.SuccessResult("Company added successfully.");
        }

        return Result<string>.FailureResult("Failed to add company.");
    }

    public async Task<Result<string>> UpdateCompanyAsync(CompanyUpdateOrRemoveVm companydata, CompanyVm companymodel)
    {
        string updateQuery = @" UPDATE Companies
                                    SET Name = @Name
                                    WHERE (Name = @CodeOrName OR Code = @CodeOrName)";

        var updateResult = await _dbConnection.ExecuteAsync(updateQuery, new
        {
            companymodel.Name,
            companydata.CodeOrName
        });

        if (updateResult > 0)
        {
            return Result<string>.SuccessResult("Company updated successfully.");
        }

        return Result<string>.FailureResult("Failed to update company.");
    }

    public async Task<Result<string>> RemoveCompanyAsync(CompanyUpdateOrRemoveVm companydata)
    {
        string query = "DELETE FROM Companies WHERE Code = @Code";

        var result = await _dbConnection.ExecuteAsync(query, new { Code = companydata.CodeOrName });

        if (result > 0)
        {
            return Result<string>.SuccessResult("Company deleted successfully.");
        }

        return Result<string>.FailureResult("Failed to delete company.");
    }

    public async Task<string> GenerateCompanyCodeAsync()
    {
        string query = "SELECT TOP 1 Code FROM Companies ORDER BY Id DESC";
        var lastCode = await _dbConnection.QueryFirstOrDefaultAsync<string>(query);

        if (string.IsNullOrEmpty(lastCode))
        {
            return "CMP001";
        }

        string numericPart = lastCode.Substring(3);

        if (int.TryParse(numericPart, out int number))
        {
            number++;
            string newCode = "CMP" + number.ToString("D3");
            return newCode;
        }

        throw new Exception("Failed to generate a new company code.");
    }
}
