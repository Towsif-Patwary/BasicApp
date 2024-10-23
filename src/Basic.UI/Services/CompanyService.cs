using Basic.Application.DTOs;
using Basic.Domain.Entity;
using Basic.UI.Common;

namespace Basic.UI.Services
{
    public class CompanyService
    {
        private readonly HttpClient _httpClient;

        public CompanyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Company>> GetAllCompanysAsync()
        {
            try
            {
                var companys = await _httpClient.GetFromJsonAsync<IEnumerable<Company>>(StaticData.UrlHttp + "api/company");
                return companys;
            }
            catch (Exception e)
            {
                try
                {
                    var companys = await _httpClient.GetFromJsonAsync<IEnumerable<Company>>(StaticData.UrlHttp + "api/company");
                    return companys;
                }
                catch (Exception ex)
                {
                    return Enumerable.Empty<Company>();
                }
            }
        }

        public async Task<Company> GetCompanyByNameOrCodeAsync(CompanyUpdateOrRemoveVm companyData)
        {
            return await _httpClient.GetFromJsonAsync<Company>(StaticData.UrlHttp + $"api /company/{companyData.CodeOrName}");
        }

        public async Task<string> AddCompanyAsync(CompanyVm company)
        {
            var response = await _httpClient.PostAsJsonAsync(StaticData.UrlHttp + "api/company", company);
            return response.IsSuccessStatusCode ? "Company added successfully." : "Failed to add company.";
        }

        public async Task<string> UpdateCompanyAsync(CompanyUpdateOrRemoveVm companyData, CompanyVm company)
        {
            var response = await _httpClient.PutAsJsonAsync(StaticData.UrlHttp + $"api/company/{companyData.CodeOrName}", company);
            return response.IsSuccessStatusCode ? "Company updated successfully." : "Failed to update company.";
        }

        public async Task<string> DeleteCompanyAsync(CompanyUpdateOrRemoveVm companyData)
        {
            var response = await _httpClient.DeleteAsync(StaticData.UrlHttp + $"api/company/{companyData.CodeOrName}");
            return response.IsSuccessStatusCode ? "Company deleted successfully." : "Failed to delete company.";
        }
    }
}
