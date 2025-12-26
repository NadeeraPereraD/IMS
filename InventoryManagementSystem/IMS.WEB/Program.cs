using IMS.BAL.Services;
using IMS.WEB.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register encoding provider for RDLC reports
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
// Get connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Register services with connection string
builder.Services.AddScoped(sp => new SalesService(connectionString));
builder.Services.AddScoped(sp => new GRNService(connectionString));
builder.Services.AddScoped(sp => new InventoryService(connectionString));
// Register ReportService
builder.Services.AddScoped<ReportService>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
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
    name: "default",
    pattern: "{controller=Inventory}/{action=Index}/{id?}");

app.Run();
