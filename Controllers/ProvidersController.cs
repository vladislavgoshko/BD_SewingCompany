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
    public class ProvidersController : Controller
    {
        private readonly SewingCompanyContext _context;

        public ProvidersController(SewingCompanyContext context)
        {
            _context = context;
        }

        // GET: Providers
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.IdSortParm = sortOrder == "id_desc" ? "id_asc" : "id_desc";
            ViewBag.NameSortParm = sortOrder == "name_asc" ? "name_desc" : "name_asc";
            ViewBag.AmountSortParm = sortOrder == "amount_desc" ? "amount_asc" : "amount_desc";
            ViewBag.PriceSortParm = sortOrder == "price_asc" ? "price_desc" : "price_asc";
            ViewBag.DateSortParm = sortOrder == "date_asc" ? "date_desc" : "date_asc";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            var providers = from x in _context.Providers
                           select x;
            if (!string.IsNullOrEmpty(searchString))
            {
                providers = providers.Where(x => x.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "id_desc":
                    providers = providers.OrderByDescending(x => x.Id);
                    break;
                case "name_asc":
                    providers = providers.OrderBy(x => x.Name);
                    break;
                case "name_desc":
                    providers = providers.OrderByDescending(x => x.Name);
                    break;
                case "amount_asc":
                    providers = providers.OrderBy(x => x.MaterialAmount);
                    break;
                case "amount_desc":
                    providers = providers.OrderByDescending(x => x.MaterialAmount);
                    break;
                case "price_asc":
                    providers = providers.OrderBy(x => x.Price);
                    break;
                case "price_desc":
                    providers = providers.OrderByDescending(x => x.Price);
                    break;
                case "date_asc":
                    providers = providers.OrderBy(x => x.DeliveryDate);
                    break;
                case "date_desc":
                    providers = providers.OrderByDescending(x => x.DeliveryDate);
                    break;
                default:
                    providers = providers.OrderBy(x => x.Id);
                    break;
            }

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(await providers.ToPagedListAsync(pageNumber, pageSize));
        }

        // GET: Providers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Providers == null)
            {
                return NotFound();
            }

            var provider = await _context.Providers
                .Include(x => x.Materials)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (provider == null)
            {
                return NotFound();
            }

            return View(provider);
        }

        // GET: Providers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Providers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,MaterialAmount,Price,DeliveryDate")] Provider provider)
        {
            if (ModelState.IsValid)
            {
                _context.Add(provider);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(provider);
        }

        // GET: Providers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Providers == null)
            {
                return NotFound();
            }

            var provider = await _context.Providers.FindAsync(id);
            if (provider == null)
            {
                return NotFound();
            }
            return View(provider);
        }

        // POST: Providers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,MaterialAmount,Price,DeliveryDate")] Provider provider)
        {
            if (id != provider.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(provider);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProviderExists(provider.Id))
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
            return View(provider);
        }

        // GET: Providers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Providers == null)
            {
                return NotFound();
            }

            var provider = await _context.Providers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (provider == null)
            {
                return NotFound();
            }

            return View(provider);
        }

        // POST: Providers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Providers == null)
            {
                return Problem("Entity set 'SewingCompanyContext.Providers'  is null.");
            }
            var provider = await _context.Providers.FindAsync(id);
            if (provider != null)
            {
                _context.Providers.Remove(provider);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProviderExists(int id)
        {
          return _context.Providers.Any(e => e.Id == id);
        }
    }
}
