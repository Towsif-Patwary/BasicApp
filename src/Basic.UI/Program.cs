using Basic.Application.Mapper;
using Basic.UI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<EmployeeService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5000/");
    client.BaseAddress = new Uri("https://localhost:5001/");
});
builder.Services.AddHttpClient<CompanyService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5000/");
    client.BaseAddress = new Uri("https://localhost:5001/");
});

builder.Services.AddRazorPages();
builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting(); 

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.UseAuthorization();

app.MapRazorPages();

app.Run();