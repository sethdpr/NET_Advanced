using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NET_Advanced.Data;
using NET_Advanced.Areas.Identity.Data;
using NET_Advanced.Middleware;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddDbContext<IdentityContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("IdentityContextConnection")));

builder.Services.AddRazorPages();
builder.Services.AddDefaultIdentity<NET_AdvancedUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequiredLength = 6;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<IdentityContext>();

builder.Services.AddControllersWithViews()
    .AddDataAnnotationsLocalization()
    .AddViewLocalization();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApplicationName", Version = "v1" });
});

var app = builder.Build();

app.UseMiddleware<NET_Advanced.Middleware.ErrorHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.Use(async (context, next) =>
{
    string cookie = string.Empty;
    if (context.Request.Cookies.TryGetValue("Language", out cookie))
    {
        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cookie);
        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(cookie);
    }
    else
    {
        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("nl-NL");
        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("nl-NL");
    }
    await next.Invoke();
});

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