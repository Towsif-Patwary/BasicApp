namespace Basic.Application.DTOs;

public class EmployeeVm
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Company { get; set; }
}
public class EmployeeUpdateOrRemoveVm
{
    public string CodeOrName { get; set; }
}

