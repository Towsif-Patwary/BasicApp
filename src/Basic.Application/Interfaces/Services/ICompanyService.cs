using Basic.Application.DTOs;
using Basic.Core.Common;
using Basic.Domain.Entity;

namespace Basic.Application.Interfaces.Services
{
    public interface ICompanyService
    {
        Task<Result<IEnumerable<Company>>> GetAllCompaniesAsync();
        Task<Result<Company>> GetCompanyByNameOrCodeAsync(CompanyUpdateOrRemoveVm companydata);
        Task<Result<string>> AddCompanyAsync(CompanyVm companymodel);
        Task<Result<string>> UpdateCompanyAsync(CompanyUpdateOrRemoveVm companydata, CompanyVm companymodel);
        Task<Result<string>> RemoveCompanyAsync(CompanyUpdateOrRemoveVm companydata);
    }
}
