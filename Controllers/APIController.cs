using Microsoft.AspNetCore.Mvc;
using NET_Advanced.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NET_Advanced.Areas.Identity.Data;
using NET_Advanced.Data;

namespace NET_Advanced.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    //Gebruikt in AdminController.cs
    public class APIController : ControllerBase
    {
        private readonly UserManager<NET_AdvancedUser> _userManager;

        // Constructor die de UserManager injecteert voor toegang tot Identity
        public APIController(UserManager<NET_AdvancedUser> userManager)
        {
            _userManager = userManager;
        }

        // GET: api/users (Haalt alle gebruikers op)
        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<NET_AdvancedUser>>> GetAllUsers()
        {
            // Haal alle gebruikers op uit de IdentityDbContext
            var users = await _userManager.Users.ToListAsync();

            // Als er geen gebruikers zijn
            if (users == null || users.Count == 0)
            {
                return NotFound("Geen gebruikers gevonden.");
            }

            return Ok(users);  // Retourneer de lijst van gebruikers
        }
    }
}