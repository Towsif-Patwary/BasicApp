using Basic.Application.DTOs;
using Basic.Domain.Entity;
using Basic.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Basic.UI.Pages.Companies
{
    public class IndexModel : PageModel
    {
        private readonly CompanyService _companyService;

        public IndexModel(CompanyService companyService)
        {
            _companyService = companyService;
        }

        [BindProperty]
        public IEnumerable<Company> Companies { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Companies = await _companyService.GetAllCompanysAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string codeOrName)
        {
            var employeeData = new CompanyUpdateOrRemoveVm { CodeOrName = codeOrName };
            var result = await _companyService.DeleteCompanyAsync(employeeData);

            if (result == "Company deleted successfully.")
            {
                Companies = await _companyService.GetAllCompanysAsync();
                return RedirectToPage();
            }

            ModelState.AddModelError(string.Empty, result);
            Companies = await _companyService.GetAllCompanysAsync();
            return Page();
        }
    }
}
