using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NET_Advanced.Data;
using NET_Advanced.Models;
using NET_Advanced.ViewModels;

namespace NET_Advanced.Controllers
{
    [Authorize]
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
            var klant = await _context.Klanten.FindAsync(id);
            if (klant == null) return NotFound();

            var bestellingen = await _context.Bestellingen
                .Where(b => b.KlantId == id)
                .ToListAsync();

            var viewModel = new BestellingIndexViewModel
            {
                KlantId = id,
                KlantNaam = klant.Naam,
                Bestellingen = bestellingen
            };

            return View(viewModel);
        }

        // GET: BestellingModels/Create
        public IActionResult Create(int klantId)
        {
            var klant = _context.Klanten.FirstOrDefault(k => k.Id == klantId);
            if (klant == null) return NotFound();

            var producten = _context.Producten.ToList();
            ViewBag.Producten = producten;
            ViewData["KlantNaam"] = klant.Naam;

            return View(new BestellingModel { KlantId = klantId });
        }

        // POST: BestellingModels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BestellingModel bestellingModel)
        {
            if (ModelState.IsValid)
            {
                bestellingModel.GemaaktDoor = User.Identity?.Name ?? "Onbekend";
                bestellingModel.AangemaaktOp = DateTime.Now;

                _context.Bestellingen.Add(bestellingModel);
                await _context.SaveChangesAsync();

                var productKeys = Request.Form.Keys.Where(k => k.StartsWith("Producten["));
                foreach (var key in productKeys)
                {
                    var productIdStr = key.Replace("Producten[", "").Replace("]", "");
                    if (int.TryParse(productIdStr, out int productId) &&
                        int.TryParse(Request.Form[key], out int aantal) && aantal > 0)
                    {
                        _context.BestellingProducten.Add(new BestellingProductModel
                        {
                            BestellingId = bestellingModel.Id,
                            ProductId = productId,
                            Aantal = aantal
                        });
                    }
                }

                await _context.SaveChangesAsync();

                return RedirectToAction("Index", new { id = bestellingModel.KlantId });
            }

            var errors = ModelState
                .SelectMany(kvp => kvp.Value!.Errors.Select(e => new { Field = kvp.Key, Error = e.ErrorMessage }))
                .ToList();

            ViewBag.ValidationErrors = errors;

            ViewBag.Producten = await _context.Producten.ToListAsync();
            ViewData["KlantNaam"] = await _context.Klanten
                .Where(k => k.Id == bestellingModel.KlantId)
                .Select(k => k.Naam)
                .FirstOrDefaultAsync();

            return View(bestellingModel);
        }

        // GET: BestellingModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var bestellingModel = await _context.Bestellingen
                .Include(b => b.BestellingProducten)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (bestellingModel == null) return NotFound();

            var producten = await _context.Producten.ToListAsync();

            var productAantallen = bestellingModel.BestellingProducten
                .ToDictionary(bp => bp.ProductId, bp => bp.Aantal);

            ViewBag.Producten = producten;
            ViewBag.ProductAantallen = productAantallen;

            ViewData["KlantId"] = new SelectList(_context.Klanten, "Id", "Naam", bestellingModel.KlantId);
            return View(bestellingModel);
        }

        // POST: BestellingModels/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BestellingModel bestellingModel)
        {
            if (id != bestellingModel.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bestellingModel);
                    await _context.SaveChangesAsync();

                    var oudeProducten = _context.BestellingProducten.Where(bp => bp.BestellingId == id);
                    _context.BestellingProducten.RemoveRange(oudeProducten);

                    var productKeys = Request.Form.Keys.Where(k => k.StartsWith("Producten["));
                    foreach (var key in productKeys)
                    {
                        var productIdStr = key.Replace("Producten[", "").Replace("]", "");
                        if (int.TryParse(productIdStr, out int productId) &&
                            int.TryParse(Request.Form[key], out int aantal) && aantal > 0)
                        {
                            _context.BestellingProducten.Add(new BestellingProductModel
                            {
                                BestellingId = bestellingModel.Id,
                                ProductId = productId,
                                Aantal = aantal
                            });
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BestellingModelExists(bestellingModel.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction("Index", new { id = bestellingModel.KlantId });
            }

            var producten = await _context.Producten.ToListAsync();
            ViewBag.Producten = producten;
            ViewBag.ProductAantallen = new Dictionary<int, int>();

            ViewData["KlantId"] = new SelectList(_context.Klanten, "Id", "Naam", bestellingModel.KlantId);
            return View(bestellingModel);
        }


        // GET: BestellingModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var bestelling = await _context.Bestellingen
                .Include(b => b.Klant)
                .Include(b => b.BestellingProducten!)
                    .ThenInclude(bp => bp.Product)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (bestelling == null) return NotFound();

            return View(bestelling);
        }


        // GET: BestellingModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var bestellingModel = await _context.Bestellingen
                .Include(b => b.Klant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bestellingModel == null) return NotFound();

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
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { id = bestellingModel.KlantId });
            }

            return RedirectToAction("Index", "Klanten");
        }

        private bool BestellingModelExists(int id)
        {
            return _context.Bestellingen.Any(e => e.Id == id);
        }

        // GET: BestellingModels/Payment/5
        public async Task<IActionResult> Payment(int klantId)
        {
            var klant = await _context.Klanten
                .FirstOrDefaultAsync(k => k.Id == klantId);

            if (klant == null) return NotFound();

            var bestellingen = await _context.Bestellingen
                .Where(b => b.KlantId == klantId)
                .Include(b => b.BestellingProducten!)
                    .ThenInclude(bp => bp.Product)
                .ToListAsync();

            var totaalPrijs = bestellingen
                .SelectMany(b => b.BestellingProducten!)
                .Sum(bp => bp.Aantal * bp.Product.Prijs);

            ViewData["KlantNaam"] = klant.Naam;
            ViewData["TotaalPrijs"] = totaalPrijs;

            return View(bestellingen);
        }
    }
}
