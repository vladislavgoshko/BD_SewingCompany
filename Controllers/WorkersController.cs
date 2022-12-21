using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SewingCompany.DbModels;
using SewingCompany.ViewModels.Worker;
using X.PagedList;

namespace SewingCompany.Controllers
{
    [Authorize]
    public class WorkersController : Controller
    {
        private readonly SewingCompanyContext _context;

        public WorkersController(SewingCompanyContext context)
        {
            _context = context;
        }

        // GET: Workers
        public async Task<IActionResult> Index(IndexWorkerViewModel viewModel)
        {
            ViewBag.CurrentSort = viewModel.sortOrder;
            ViewBag.IdSortParm = viewModel.sortOrder == "id_desc" ? "id_asc" : "id_desc";
            ViewBag.NameSortParm = viewModel.sortOrder == "name_asc" ? "name_desc" : "name_asc";
            ViewBag.SectionSortParm = viewModel.sortOrder == "section_desc" ? "section_asc" : "section_desc";
            ViewBag.PositionSortParm = viewModel.sortOrder == "position_asc" ? "position_desc" : "position_asc";
            //ViewBag.isNotAtTime = viewModel.isNotAtTime;
            //ViewBag.isSection = viewModel.isSection;

            viewModel.sections = new SelectList(_context.Workers.OrderBy(x => x.Section).Select(x => x.Section).Distinct(), "Section");

            if (viewModel.searchString != null)
            {
                viewModel.page = 1;
            }
            else
            {
                viewModel.searchString = viewModel.currentFilter;
            }

            ViewBag.CurrentFilter = viewModel.searchString;

            var workers = from x in _context.Workers
                          select x;
            if (!string.IsNullOrEmpty(viewModel.searchString))
                workers = workers.Where(x => x.Name.Contains(viewModel.searchString));

            if (viewModel.isSection)
                workers = workers.Where(x => x.Section.Contains(viewModel.SelectedSection));

            if (viewModel.isNotAtTime)
                workers = (from w in workers
                           join ord in _context.Orders
                           on w.Id equals ord.WorkerId
                           where ord.ImplementationDate > ord.DeliveryOrderDate
                           select w).Distinct();

            switch (viewModel.sortOrder)
            {
                case "id_desc":
                    workers = workers.OrderByDescending(x => x.Id);
                    break;
                case "name_asc":
                    workers = workers.OrderBy(x => x.Name);
                    break;
                case "name_desc":
                    workers = workers.OrderByDescending(x => x.Name);
                    break;
                case "section_asc":
                    workers = workers.OrderBy(x => x.Section);
                    break;
                case "section_desc":
                    workers = workers.OrderByDescending(x => x.Section);
                    break;
                case "position_asc":
                    workers = workers.OrderBy(x => x.Position);
                    break;
                case "position_desc":
                    workers = workers.OrderByDescending(x => x.Position);
                    break;
                default:
                    workers = workers.OrderBy(x => x.Id);
                    break;
            }
            int pageSize = 20;
            int pageNumber = (viewModel.page ?? 1);
            viewModel.Workers = workers.ToPagedList(pageNumber, pageSize);

            return View(viewModel);
        }

        // GET: Workers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Workers == null)
            {
                return NotFound();
            }

            var workers = _context.Workers
                .Include(x => x.Orders)
                .Where(x => x.Id == id);
            if (workers == null)
            {
                return NotFound();
            }
            (from ord in _context.Orders
             join w in workers
             on ord.WorkerId equals w.Id
             select ord).Include(x => x.Product).Load();
            return View(workers.FirstOrDefault());
        }

        // GET: Workers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Workers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Section,Position")] DbModels.Worker worker)
        {
            if (ModelState.IsValid)
            {
                _context.Add(worker);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(worker);
        }

        // GET: Workers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Workers == null)
            {
                return NotFound();
            }

            var worker = await _context.Workers.FindAsync(id);
            if (worker == null)
            {
                return NotFound();
            }
            return View(worker);
        }

        // POST: Workers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Section,Position")] DbModels.Worker worker)
        {
            if (id != worker.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(worker);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkerExists(worker.Id))
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
            return View(worker);
        }

        // GET: Workers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Workers == null)
            {
                return NotFound();
            }

            var worker = await _context.Workers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (worker == null)
            {
                return NotFound();
            }

            return View(worker);
        }

        // POST: Workers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Workers == null)
            {
                return Problem("Entity set 'SewingCompanyContext.Workers'  is null.");
            }
            var worker = await _context.Workers.FindAsync(id);
            if (worker != null)
            {
                _context.Workers.Remove(worker);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkerExists(int id)
        {
            return _context.Workers.Any(e => e.Id == id);
        }
    }
}
