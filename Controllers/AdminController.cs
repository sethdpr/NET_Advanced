using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using NET_Advanced.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using NET_Advanced.Resources;

namespace NET_Advanced.Controllers
{
    public class AdminController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IStringLocalizer<Resource> _localizer;

        public AdminController(HttpClient httpClient, IStringLocalizer<Resource> localizer)
        {
            _httpClient = httpClient;
            _localizer = localizer;
        }

        // GET: /Admin/Index
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("https://localhost:7156/api/API/users");
            if (response.IsSuccessStatusCode)
            {
                var usersJson = await response.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<NET_AdvancedUser>>(usersJson);
                return View(users);
            }

            return View(new List<NET_AdvancedUser>());
        }
        // POST: /Admin/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7156/api/API/users/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Er is een fout opgetreden bij het verwijderen van de gebruiker.";
            return RedirectToAction("Index");
        }
    }
}