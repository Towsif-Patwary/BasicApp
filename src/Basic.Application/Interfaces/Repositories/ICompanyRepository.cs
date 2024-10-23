using Basic.Application.DTOs;
using Basic.Core.Common;
using Basic.Domain.Entity;

namespace Basic.Application.Interfaces.Repositories
{
    public interface ICompanyRepository
    {
        Task<Result<IEnumerable<Company>>> GetAllCompaniesAsync();
        Task<Result<Company>> GetCompanyByNameOrCodeAsync(CompanyUpdateOrRemoveVm companydata);
        Task<Result<string>> AddCompanyAsync(Company companymodel);
        Task<Result<string>> UpdateCompanyAsync(CompanyUpdateOrRemoveVm companydata, CompanyVm companymodel);
        Task<Result<string>> RemoveCompanyAsync(CompanyUpdateOrRemoveVm companydata);
    }
}
