using Basic.Application.DTOs;
using Basic.Domain.Entity;
using Basic.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Basic.UI.Pages.Companies
{
    public class CreateModel : PageModel
    {
        private readonly CompanyService _companyService;

        [BindProperty]
        public CompanyVm Company { get; set; }

        public string Message { get; set; }
        public bool IsSuccess { get; set; }

        public CreateModel(CompanyService companyService)
        {
            _companyService = companyService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var resultMessage = await _companyService.AddCompanyAsync(Company);

            if (resultMessage.Contains("successfully"))
            {
                IsSuccess = true;
                Message = resultMessage;
                return RedirectToPage("/Companies/Index");
            }
            else
            {
                IsSuccess = false;
                Message = resultMessage;
            }

            return Page();
        }
    }
}
