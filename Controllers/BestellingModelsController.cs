using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
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
            var klant = _context.Klanten.FirstOrDefault(k => k.Id == id);
            if (klant == null)
            {
                return NotFound();
            }
            var bestellingen = _context.Bestellingen
                .Where(b => b.KlantId == id)
                .ToList();

            var klantNaam = _context.Klanten
                .Where(k => k.Id == id)
                .Select(k => k.Naam)
                .FirstOrDefault();

            var viewModel = new BestellingIndexViewModel
            {
                KlantId = id,
                KlantNaam = klant.Naam,
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
        public IActionResult Create(int klantId)
        {
            var klant = _context.Klanten.FirstOrDefault(k => k.Id == klantId);
            if (klant == null)
            {
                return NotFound();
            }
            ViewData["Title"] = "Create";
            ViewData["KlantNaam"] = klant.Naam;
            var producten = _context.Producten.ToList();
            ViewBag.Producten = producten;

            return View(new BestellingModel { KlantId = klantId });
        }

        // POST: BestellingModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
        [Bind("Id,Naam,KlantId")] BestellingModel bestellingModel,
        Dictionary<int, int> Producten)
        {
            Debug.WriteLine("Create actie POST is aangeroepen.");

            if (ModelState.IsValid)
            {
                Debug.WriteLine("ModelState is geldig.");

                _context.Add(bestellingModel);
                await _context.SaveChangesAsync();

                Debug.WriteLine($"Bestelling opgeslagen. BestellingId = {bestellingModel.Id}");

                if (Producten != null)
                {
                    Debug.WriteLine("Producten ontvangen:");
                    foreach (var productId in Producten.Keys)
                    {
                        var aantal = Producten[productId];
                        Debug.WriteLine($"ProductId = {productId}, Aantal = {aantal}");

                        if (aantal > 0)
                        {
                            var bestellingProduct = new BestellingProductModel
                            {
                                BestellingId = bestellingModel.Id,
                                ProductId = productId,
                                Aantal = aantal
                            };

                            _context.Add(bestellingProduct);
                            Debug.WriteLine($"Bestelling product toegevoegd: ProductId = {productId}, Aantal = {aantal}");
                        }
                    }

                    await _context.SaveChangesAsync();
                    Debug.WriteLine("Bestellingproducten opgeslagen.");
                }
                else
                {
                    Debug.WriteLine("Geen producten ontvangen.");
                }

                return RedirectToAction("Index", "BestellingModels", new { id = bestellingModel.KlantId });
            }
            else
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Debug.WriteLine($"Error: {error.ErrorMessage}");
                }
                Debug.WriteLine("ModelState is niet geldig.");
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