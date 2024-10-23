using Basic.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Basic.Infrastructure.Data.Contexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Employee> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed some initial data
        modelBuilder.Entity<Employee>().HasData(
            new Employee { Id = 1, Code = "EMP001", Name = "John Doe", Address = "123 Main St", Phone = "555-1234", Email = "john@example.com", Company = "CMP001" },
            new Employee { Id = 2, Code = "EMP002", Name = "Jane Smith", Address = "456 Elm St", Phone = "555-5678", Email = "jane@example.com", Company = "CMP002" }
        );

        modelBuilder.Entity<Company>().HasData(
            new Company { Id = 1, Code = "CMP001", Name = "ABC Corp" },
            new Company { Id = 2, Code = "CMP002", Name = "XYZ Ltd" }
        );
    }
}
