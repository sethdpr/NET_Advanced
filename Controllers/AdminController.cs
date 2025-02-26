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

        public AdminController(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
    }
}