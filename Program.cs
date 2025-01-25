using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NET_Advanced.Data;
using NET_Advanced.Areas.Identity.Data;
using NET_Advanced.Middleware;
using Microsoft.AspNetCore.Identity;
using static System.Runtime.InteropServices.JavaScript.JSType;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("IdentityContext");

builder.Services.AddDbContext<IdentityContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddRazorPages();
builder.Services.AddDefaultIdentity<NET_AdvancedUser>(options => options.SignIn.RequireConfirmedAccount = false)
 .AddEntityFrameworkStores<IdentityContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApplicationName", Version = "v1" });
});

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

var supportedCultures = new[] { "en-US", "fr", "nl" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);
app.UseRequestLocalization(localizationOptions);

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapRazorPages();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();