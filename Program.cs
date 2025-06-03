using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using test_expense_control_api.DomainApi.Services.Interfaces;
using test_expense_control_api.DomainApi.Services;
using test_expense_control_api.Persistence.Adapter.Context;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(connectionString));

// for jwt 
//builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

//builder.Services.AddCustomJwtAuth(builder.Configuration);
// Add services to the container.

builder.Services.AddTransient<IBudgetService, BudgetService>();
builder.Services.AddTransient<IDepositService, DepositService>();
builder.Services.AddTransient<IDetailExpensesService, DetailExpensesService>();
builder.Services.AddTransient<IExpenseTypeService, ExpenseTypeService>();
builder.Services.AddTransient<IHeaderExpensesService, HeaderExpensesService>();
builder.Services.AddTransient<IUserService, UserService>();


//builder.Services.AddAutoMapper(typeof(Program));


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseAuthorization();

app.MapControllers();

app.Run();
