using Microsoft.AspNetCore.Mvc;
using NET_Advanced.Data;
using NET_Advanced.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NET_Advanced.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class APIController : ControllerBase
    {
        private readonly IdentityContext _context;

        public APIController(IdentityContext context)
        {
            _context = context;
        }

        // GET: /API
        [HttpGet]
        public async Task<ActionResult<IEnumerable<API>>> GetAllAPI()
        {
            var items = await _context.API.ToListAsync();
            return Ok(items);
        }

        // GET: /API/5
        [HttpGet("{id}")]
        public async Task<ActionResult<API>> GetAPIById(int id)
        {
            var api = await _context.API.FindAsync(id);

            if (api == null)
            {
                return NotFound();
            }

            return Ok(api);
        }

        // POST: /API
        [HttpPost]
        public async Task<ActionResult<API>> CreateAPI([FromBody] API newAPI)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.API.Add(newAPI);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAPIById), new { id = newAPI.Id }, newAPI);
        }

        // PUT: /API/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAPI(int id, [FromBody] API updatedAPI)
        {
            if (id != updatedAPI.Id)
            {
                return BadRequest("ID in URL komt niet overeen met ID in body");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingAPI = await _context.API.FindAsync(id);
            if (existingAPI == null)
            {
                return NotFound();
            }

            _context.Entry(existingAPI).CurrentValues.SetValues(updatedAPI);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: /API/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAPI(int id)
        {
            var api = await _context.API.FindAsync(id);
            if (api == null)
            {
                return NotFound();
            }

            _context.API.Remove(api);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

