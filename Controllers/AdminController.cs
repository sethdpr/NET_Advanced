using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using NET_Advanced.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace NET_Advanced.Controllers
{
    public class AdminController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly UserManager<NET_AdvancedUser> _userManager;

        public AdminController(HttpClient httpClient, UserManager<NET_AdvancedUser> userManager)
        {
            _httpClient = httpClient;
            _userManager = userManager;
        }

        // GET: /Admin/Index
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();  // Haalt de gebruikers op uit de Identity DB.

            // Als er geen gebruikers zijn, geef een lege lijst door aan de view
            if (users == null || users.Count == 0)
            {
                return View(new List<NET_AdvancedUser>());
            }

            // Geef de gebruikerslijst door aan de view
            return View(users);
        }
    }
}