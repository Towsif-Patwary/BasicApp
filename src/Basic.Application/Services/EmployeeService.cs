using AutoMapper;
using Basic.Application.DTOs;
using Basic.Application.Interfaces.Repositories;
using Basic.Application.Interfaces.Services;
using Basic.Core.Common;
using Basic.Domain.Entity;
using Microsoft.Extensions.Caching.Memory;

namespace Basic.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper, IMemoryCache cache)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<IEnumerable<Employee>>> GetAllEmployeesAsync()
        {
            if (!_cache.TryGetValue(CacheKeys.AllEmployees, out Result<IEnumerable<Employee>> cachedEmployees))
            {
                var result = await _employeeRepository.GetAllEmployeesAsync();

                if (result.Success)
                {
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(30)) 
                        .SetAbsoluteExpiration(TimeSpan.FromHours(1)); 
                    
                    _cache.Set(CacheKeys.AllEmployees, result, cacheEntryOptions);
                }
                return result;
            }
            return cachedEmployees;
        }

        public async Task<Result<Employee>> GetEmployeeByNameOrCodeAsync(EmployeeUpdateOrRemoveVm employeedata)
        {
            return await _employeeRepository.GetEmployeeByNameOrCodeAsync(employeedata);
        }

        public async Task<Result<string>> AddEmployeeAsync(EmployeeVm employeedata)
        {
            var employee = _mapper.Map<Employee>(employeedata);
            return await _employeeRepository.AddEmployeeAsync(employee);
        }

        public async Task<Result<string>> UpdateEmployeeAsync(EmployeeUpdateOrRemoveVm employeedata, EmployeeVm employee)
        {
            return await _employeeRepository.UpdateEmployeeAsync(employeedata, employee);
        }

        public async Task<Result<string>> RemoveEmployeeAsync(EmployeeUpdateOrRemoveVm employeedata)
        {
            return await _employeeRepository.RemoveEmployeeAsync(employeedata);
        }
    }
}
