using AutoMapper;
using Basic.Application.DTOs;
using Basic.Application.Interfaces.Repositories;
using Basic.Application.Interfaces.Services;
using Basic.Core.Common;
using Basic.Domain.Entity;

namespace Basic.Application.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public CompanyService(ICompanyRepository companyRepository, IMapper mapper)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<Company>>> GetAllCompaniesAsync()
        {
            return await _companyRepository.GetAllCompaniesAsync();
        }

        public async Task<Result<Company>> GetCompanyByNameOrCodeAsync(CompanyUpdateOrRemoveVm companydata)
        {
            return await _companyRepository.GetCompanyByNameOrCodeAsync(companydata);
        }

        public async Task<Result<string>> AddCompanyAsync(CompanyVm companymodel)
        {
            var company = _mapper.Map<Company>(companymodel);
            return await _companyRepository.AddCompanyAsync(company);
        }

        public async Task<Result<string>> UpdateCompanyAsync(CompanyUpdateOrRemoveVm companydata, CompanyVm companymodel)
        {
            return await _companyRepository.UpdateCompanyAsync(companydata, companymodel);
        }

        public async Task<Result<string>> RemoveCompanyAsync(CompanyUpdateOrRemoveVm companydata)
        {
            return await _companyRepository.RemoveCompanyAsync(companydata);
        }
    }

}
