using TestApp.Repository;
using TestApp;
using Microsoft.Extensions.DependencyInjection;
using Hangfire;


var builder = WebApplication.CreateBuilder(args);
//It will create the single instence throughout the application
builder.Services.AddSingleton<IConnectionString, ConnectionString>();

//Will create the One Scope per http request and single http request it will use that same scope
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IPayroll, Payroll>();

builder.Services.AddHangfire(option => option.UseSqlServerStorage(@"data source=database-1.cfycuqjk6ac8.us-east-1.rds.amazonaws.com,1433;initial catalog=info;User ID = Admin; Password = Adminadmin123?"));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();




app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Employee}/{action=Index}/{id?}");

app.Run();
