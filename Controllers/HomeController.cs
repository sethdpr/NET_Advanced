using System.Diagnostics;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using NET_Advanced.Models;
using NET_Advanced.Resources;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Localization;

namespace NET_Advanced.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStringLocalizer<Resource> _localizer;

        public HomeController(ILogger<HomeController> logger, IStringLocalizer<Resource> localizer)
        {
            _logger = logger;
            _localizer = localizer;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

        [HttpPost("change-language")]
        public IActionResult ChangeLanguage(string language)
        {
            if (!string.IsNullOrEmpty(language))
            {
                Response.Cookies.Append("Language", language, new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddYears(1)
                });
            }
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
