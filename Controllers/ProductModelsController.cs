using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using NET_Advanced.Data;
using NET_Advanced.Models;
using Newtonsoft.Json;

namespace NET_Advanced.Controllers
{
    public class ProductModelsController : Controller
    {
        private readonly IdentityContext _context;
        private readonly IStringLocalizer<ProductModelsController> _localizer;

        public ProductModelsController(IdentityContext context, IStringLocalizer<ProductModelsController> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        // GET: ProductModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.Producten.ToListAsync());
        }
        // GET: ProductModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductModels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAjax()
        {
            string requestBody;
            try
            {
                using (var reader = new StreamReader(Request.Body))
                {
                    requestBody = await reader.ReadToEndAsync();
                }
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Error" });
            }

            ProductModel productModel;
            try
            {
                productModel = JsonConvert.DeserializeObject<ProductModel>(requestBody);
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Error" });
            }

            if (string.IsNullOrWhiteSpace(productModel.Naam))
            {
                return Json(new { success = false, message = "Error" });
            }

            if (productModel.Prijs <= 0)
            {
                return Json(new { success = false, message = "Error" });
            }

            try
            {
                _context.Add(productModel);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Succes" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error" });
            }
        }

        // GET: ProductModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productModel = await _context.Producten.FindAsync(id);
            if (productModel == null)
            {
                return NotFound();
            }
            return View(productModel);
        }

        // POST: ProductModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naam,Prijs")] ProductModel productModel)
        {
            if (id != productModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductModelExists(productModel.Id))
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
            return View(productModel);
        }

        // GET: ProductModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productModel = await _context.Producten
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productModel == null)
            {
                return NotFound();
            }

            return View(productModel);
        }

        // POST: ProductModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productModel = await _context.Producten.FindAsync(id);
            if (productModel != null)
            {
                _context.Producten.Remove(productModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductModelExists(int id)
        {
            return _context.Producten.Any(e => e.Id == id);
        }
    }
}
