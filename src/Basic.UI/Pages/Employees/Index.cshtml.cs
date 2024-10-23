using Basic.Application.DTOs;
using Basic.Domain.Entity;
using Basic.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Basic.UI.Pages.Employees
{
    public class IndexModel : PageModel
    {
        private readonly EmployeeService _employeeService;

        public IndexModel(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [BindProperty]
        public IEnumerable<Employee> Employees { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Employees = await _employeeService.GetAllEmployeesAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string codeOrName, string action)
        {
            if (action == "edit")
            {
                return RedirectToPage("/Employees/Update", new { codeOrName });
            }
            else if (action == "delete")
            {
                var employeeData = new EmployeeUpdateOrRemoveVm { CodeOrName = codeOrName };
                var result = await _employeeService.RemoveEmployeeAsync(employeeData);

                if (result == "Employee deleted successfully.")
                {
                    return RedirectToPage("/Employees/Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result);
                }
            }

            return Page();
        }

    }
}
