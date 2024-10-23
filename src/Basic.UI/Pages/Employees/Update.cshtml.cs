using AutoMapper;
using Basic.Application.DTOs;
using Basic.Domain.Entity;
using Basic.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Basic.UI.Pages.Employees
{
    public class UpdateModel : PageModel
    {
        private readonly EmployeeService _employeeService;
        private readonly CompanyService _companyService;
        private readonly IMapper _mapper;

        public UpdateModel(EmployeeService employeeService, CompanyService companyService, IMapper mapper)
        {
            _employeeService = employeeService;
            _companyService = companyService;
            _mapper = mapper;
        }

        [BindProperty]
        public EmployeeVm Employee { get; set; }

        public IEnumerable<Company> Companies { get; set; }

        public async Task<IActionResult> OnGetAsync(string codeOrName)
        {
            var employeeResult = await _employeeService.GetEmployeeByNameOrCodeAsync(new EmployeeUpdateOrRemoveVm { CodeOrName = codeOrName });
            if (employeeResult == null)
            {
                return NotFound("Employee not found.");
            }

            Employee = _mapper.Map<EmployeeVm>(employeeResult);

            Companies = await _companyService.GetAllCompanysAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string codeOrName)
        {
            if (!ModelState.IsValid)
            {
                Companies = await _companyService.GetAllCompanysAsync();
                return Page();
            }

            var employeeData = new EmployeeUpdateOrRemoveVm { CodeOrName = codeOrName };
            var result = await _employeeService.UpdateEmployeeAsync(employeeData, Employee);

            if (result == "Employee updated successfully.")
            {
                return RedirectToPage("/Employees/Index");
            }

            ModelState.AddModelError(string.Empty, result);
            Companies = await _companyService.GetAllCompanysAsync(); 
            return Page();
        }
    }
}
