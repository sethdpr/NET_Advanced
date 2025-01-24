using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NET_Advanced.Data;
using NET_Advanced.Models;

namespace NET_Advanced.Controllers
{
    public class KlantModelsController : Controller
    {
        private readonly IdentityContext _context;

        public KlantModelsController(IdentityContext context)
        {
            _context = context;
        }

        // GET: KlantModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.Klanten.ToListAsync());
        }

        // GET: KlantModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: KlantModels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Naam")] KlantModel klantModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(klantModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(klantModel);
        }

        // GET: KlantModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var klantModel = await _context.Klanten.FindAsync(id);
            if (klantModel == null)
            {
                return NotFound();
            }
            return View(klantModel);
        }

        // POST: KlantModels/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naam")] KlantModel klantModel)
        {
            if (id != klantModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(klantModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KlantModelExists(klantModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(klantModel);
        }

        // GET: KlantModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var klantModel = await _context.Klanten
                .FirstOrDefaultAsync(m => m.Id == id);
            if (klantModel == null)
            {
                return NotFound();
            }

            return View(klantModel);
        }

        // POST: KlantModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var klantModel = await _context.Klanten.FindAsync(id);
            if (klantModel != null)
            {
                _context.Klanten.Remove(klantModel);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool KlantModelExists(int id)
        {
            return _context.Klanten.Any(e => e.Id == id);
        }
    }
}