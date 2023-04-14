using InvoiceGenerator.Data;
using InvoiceGenerator.Data.Repository;
using InvoiceGenerator.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();


//add services
#region Dependencies
builder.Services.AddTransient<ICustomerService, CustomerRepository>();
builder.Services.AddTransient<IProductService, ProductRepository>();
builder.Services.AddTransient<IStoreSettingService, StoreSettingRepository>();
builder.Services.AddTransient<ISalesInvoiceService, SalesInvoiceRepository>();
builder.Services.AddTransient<ISalesReportService, SalesReportRepository>();
builder.Services.AddTransient<IPaymentService, PaymentRepository>();
builder.Services.AddTransient<ISalesReturnService, SalesReturnRepository>();
#endregion


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.MapControllerRoute(
    name: "areaRoute",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapGet("/Identity/Account/Register", context => Task.Factory.StartNew(() => context.Response.Redirect("/Identity/Account/Login", true, true)));
app.MapPost("/Identity/Account/Register", context => Task.Factory.StartNew(() => context.Response.Redirect("/Identity/Account/Login", true, true)));
app.MapPost("/Identity/Account/ForgotPassword", context => Task.Factory.StartNew(() => context.Response.Redirect("/Identity/Account/Login", true, true)));
app.MapGet("/Identity/Account/ResetPassword", context => Task.Factory.StartNew(() => context.Response.Redirect("/Identity/Account/Login", true, true)));
app.MapPost("/Identity/Account/ResetPassword", context => Task.Factory.StartNew(() => context.Response.Redirect("/Identity/Account/Login", true, true)));


app.MapRazorPages();

app.Run();
