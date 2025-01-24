using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NET_Advanced.Data;
using NET_Advanced.Models;
using NET_Advanced.ViewModels;

namespace NET_Advanced.Controllers
{
    public class BestellingModelsController : Controller
    {
        private readonly IdentityContext _context;

        public BestellingModelsController(IdentityContext context)
        {
            _context = context;
        }

        // GET: BestellingModels
        public async Task<IActionResult> Index(int id)
        {
            // Haal de bestellingen op die bij de klant-ID horen
            var bestellingen = _context.Bestellingen
                .Where(b => b.KlantId == id) // Filter op klant-ID
                .ToList();

            // Optioneel: voeg de klantnaam toe voor context in de view
            var klantNaam = _context.Klanten
                .Where(k => k.Id == id)
                .Select(k => k.Naam)
                .FirstOrDefault();

            // Maak een viewmodel om de bestellingen en klantinformatie door te geven
            var viewModel = new BestellingIndexViewModel
            {
                KlantId = id,
                KlantNaam = klantNaam,
                Bestellingen = bestellingen
            };

            return View(viewModel);
        }

        // GET: BestellingModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bestellingModel = await _context.Bestellingen
                .Include(b => b.Klant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bestellingModel == null)
            {
                return NotFound();
            }

            return View(bestellingModel);
        }

        // GET: BestellingModels/Create
        public IActionResult Create()
        {
            ViewData["KlantId"] = new SelectList(_context.Klanten, "Id", "Naam");
            return View();
        }

        // POST: BestellingModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Naam,KlantId")] BestellingModel bestellingModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bestellingModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = bestellingModel.KlantId });
            }
            ViewData["KlantId"] = new SelectList(_context.Klanten, "Id", "Naam", bestellingModel.KlantId);
            return View(bestellingModel);
        }

        // GET: BestellingModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bestellingModel = await _context.Bestellingen.FindAsync(id);
            if (bestellingModel == null)
            {
                return NotFound();
            }
            ViewData["KlantId"] = new SelectList(_context.Klanten, "Id", "Naam", bestellingModel.KlantId);
            return View(bestellingModel);
        }

        // POST: BestellingModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naam,KlantId")] BestellingModel bestellingModel)
        {
            if (id != bestellingModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bestellingModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BestellingModelExists(bestellingModel.Id))
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
            ViewData["KlantId"] = new SelectList(_context.Klanten, "Id", "Naam", bestellingModel.KlantId);
            return View(bestellingModel);
        }

        // GET: BestellingModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bestellingModel = await _context.Bestellingen
                .Include(b => b.Klant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bestellingModel == null)
            {
                return NotFound();
            }

            return View(bestellingModel);
        }

        // POST: BestellingModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bestellingModel = await _context.Bestellingen.FindAsync(id);
            if (bestellingModel != null)
            {
                _context.Bestellingen.Remove(bestellingModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BestellingModelExists(int id)
        {
            return _context.Bestellingen.Any(e => e.Id == id);
        }
    }
}