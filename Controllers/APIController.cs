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
    //[Authorize]
    //Gebruikt in AdminController.cs
    public class APIController : ControllerBase
    {
        private readonly UserManager<NET_AdvancedUser> _userManager;

        public APIController(UserManager<NET_AdvancedUser> userManager)
        {
            _userManager = userManager;
        }

        // GET: api/users
        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();

            var userDtos = new List<UserDto>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userDtos.Add(new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Voornaam = user.Voornaam,
                    Achternaam = user.Achternaam,
                    Roles = roles
                });
            }

            return Ok(userDtos);
        }
        // DELETE: api/users/{id}
        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return NoContent();
            }

            return BadRequest(result.Errors);
        }
    }
}