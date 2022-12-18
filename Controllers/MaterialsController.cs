using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SewingCompany.DbModels;
using X.PagedList;
namespace SewingCompany.Controllers
{
    [Authorize]
    public class MaterialsController : Controller
    {
        private readonly SewingCompanyContext _context;

        public MaterialsController(SewingCompanyContext context)
        {
            _context = context;
        }

        // GET: Materials
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.IdSortParm = sortOrder == "id_desc" ? "id_asc" : "id_desc";
            ViewBag.NameSortParm = sortOrder == "name_asc" ? "name_desc" : "name_asc";
            ViewBag.TypeSortParm = sortOrder == "type_desc" ? "type_asc" : "type_desc";
            ViewBag.AmountSortParm = sortOrder == "amount_asc" ? "amount_desc" : "amount_asc";
            ViewBag.ProviderSortParm = sortOrder == "provider_asc" ? "provider_desc" : "provider_asc";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            var materials = from x in _context.Materials
                           select x;
            materials = materials.Include(x => x.Provider);

            if (!string.IsNullOrEmpty(searchString))
            {
                materials = materials.Where(x => x.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "id_desc":
                    materials = materials.OrderByDescending(x => x.Id);
                    break;
                case "name_asc":
                    materials = materials.OrderBy(x => x.Name);
                    break;
                case "name_desc":
                    materials = materials.OrderByDescending(x => x.Name);
                    break;
                case "amount_asc":
                    materials = materials.OrderBy(x => x.Provider.MaterialAmount);
                    break;
                case "amount_desc":
                    materials = materials.OrderByDescending(x => x.Provider.MaterialAmount);
                    break;
                case "type_asc":
                    materials = materials.OrderBy(x => x.Type);
                    break;
                case "type_desc":
                    materials = materials.OrderByDescending(x => x.Type);
                    break;
                case "provider_asc":
                    materials = materials.OrderBy(x => x.Provider.Name);
                    break;
                case "provider_desc":
                    materials = materials.OrderByDescending(x => x.Provider.Name);
                    break;
                default:
                    materials = materials.OrderBy(x => x.Id);
                    break;
            }

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(await materials.ToPagedListAsync(pageNumber, pageSize));
        }

        // GET: Materials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Materials == null)
            {
                return NotFound();
            }

            var material = await _context.Materials
                .Include(m => m.Provider)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (material == null)
            {
                return NotFound();
            }

            return View(material);
        }

        // GET: Materials/Create
        public IActionResult Create()
        {
            ViewData["ProviderId"] = new SelectList(_context.Providers, "Id", "Id");
            return View();
        }

        // POST: Materials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Type,QuantityInStock,ProviderId")] Material material)
        {
            if (ModelState.IsValid)
            {
                _context.Add(material);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProviderId"] = new SelectList(_context.Providers, "Id", "Name", material.ProviderId);
            return View(material);
        }

        // GET: Materials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Materials == null)
            {
                return NotFound();
            }

            var material = await _context.Materials.FindAsync(id);
            if (material == null)
            {
                return NotFound();
            }
            ViewData["ProviderId"] = new SelectList(_context.Providers, "Id", "Name", material.ProviderId);
            return View(material);
        }

        // POST: Materials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Type,QuantityInStock,ProviderId")] Material material)
        {
            if (id != material.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(material);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaterialExists(material.Id))
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
            ViewData["ProviderId"] = new SelectList(_context.Providers, "Id", "Name", material.ProviderId);
            return View(material);
        }

        // GET: Materials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Materials == null)
            {
                return NotFound();
            }

            var material = await _context.Materials
                .Include(m => m.Provider)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (material == null)
            {
                return NotFound();
            }

            return View(material);
        }

        // POST: Materials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Materials == null)
            {
                return Problem("Entity set 'SewingCompanyContext.Materials'  is null.");
            }
            var material = await _context.Materials.FindAsync(id);
            if (material != null)
            {
                _context.Materials.Remove(material);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MaterialExists(int id)
        {
          return _context.Materials.Any(e => e.Id == id);
        }
    }
}
