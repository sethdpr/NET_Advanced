using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NET_Advanced.Data;
using NET_Advanced.Areas.Identity.Data;
using NET_Advanced.Middleware;
using Microsoft.AspNetCore.Identity;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("IdentityContext");

builder.Services.AddDbContext<IdentityContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddRazorPages();
builder.Services.AddDefaultIdentity<NET_AdvancedUser>(options => 
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequiredLength = 6;
})
    .AddRoles<IdentityRole>()
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

await RoleInitializer.SeedDatabaseAsync(app.Services);
app.Run();

public static class RoleInitializer
{
    public static async Task SeedDatabaseAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<NET_AdvancedUser>>();

        string[] roleNames = { "Admin", "Employee" };
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        var users = new[]
        {
            new { Email = "seth.depreter@gmail.com", Voornaam = "Seth", Achternaam = "De Preter", Password = "Sethseth55!", Role = "Admin", IsAdmin = true },
            new { Email = "julie.beutels04@gmail.com", Voornaam = "Julie", Achternaam = "Beutels", Password = "Juliejulie55!", Role = "Employee", IsAdmin = false }
        };

        foreach (var userData in users)
        {
            var user = await userManager.FindByEmailAsync(userData.Email);
            if (user == null)
            {
                user = new NET_AdvancedUser
                {
                    UserName = userData.Email,
                    Email = userData.Email,
                    Voornaam = userData.Voornaam,
                    Achternaam = userData.Achternaam,
                    IsAdmin = userData.IsAdmin
                };
                await userManager.CreateAsync(user, userData.Password);
            }

            if (!await userManager.IsInRoleAsync(user, userData.Role))
            {
                await userManager.AddToRoleAsync(user, userData.Role);
            }
        }
    }
}