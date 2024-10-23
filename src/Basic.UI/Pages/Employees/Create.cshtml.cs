using Basic.Application.DTOs;
using Basic.Domain.Entity;
using Basic.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Basic.UI.Pages.Employees
{
    public class CreateModel : PageModel
    {
        private readonly EmployeeService _employeeService;
        private readonly CompanyService _companyService;

        [BindProperty]
        public EmployeeVm Employee { get; set; }

        public List<Company> Companies { get; set; } = new List<Company>();
        public string Message { get; set; }
        public bool IsSuccess { get; set; }

        public CreateModel(EmployeeService employeeService, CompanyService companyService)
        {
            _employeeService = employeeService;
            _companyService = companyService;
        }

        public async Task OnGetAsync()
        {
            Companies = (await _companyService.GetAllCompanysAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Companies = (await _companyService.GetAllCompanysAsync()).ToList();
                return Page();
            }

            var resultMessage = await _employeeService.AddEmployeeAsync(Employee);

            if (resultMessage.Contains("successfully"))
            {
                IsSuccess = true;
                Message = resultMessage; 
                return RedirectToPage("/Employees/Index");
            }
            else
            {
                IsSuccess = false;
                Message = resultMessage;
            }

            Companies = (await _companyService.GetAllCompanysAsync()).ToList();
            return Page();
        }
    }
}
