using Microsoft.AspNetCore.Mvc;
using NET_Advanced.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NET_Advanced.Areas.Identity.Data;

namespace NET_Advanced.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    //Gebruikt in AdminController.cs
    public class APIController : ControllerBase
    {
        private readonly UserManager<NET_AdvancedUser> _userManager;

        public APIController(UserManager<NET_AdvancedUser> userManager)
        {
            _userManager = userManager;
        }

        // GET: api/users
        [HttpGet("/users")]
        public async Task<ActionResult<IEnumerable<NET_AdvancedUser>>> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();

            if (users == null || users.Count == 0)
            {
                return NotFound("Geen gebruikers gevonden.");
            }

            return Ok(users);
        }
    }
}