using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using SewingCompany.DbModels;
using X.PagedList;


namespace SewingCompany.Controllers
{
    public class OrdersController : Controller
    {
        private readonly SewingCompanyContext _context;

        public OrdersController(SewingCompanyContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.IdSortParm = sortOrder == "id_desc" ? "id_asc" : "id_desc";
            ViewBag.ProductSortParm = sortOrder == "prod_name_asc" ? "prod_name_desc" : "prod_name_asc";
            ViewBag.CustomerSortParm = sortOrder == "cust_name_asc" ? "cust_name_desc" : "cust_name_asc";
            ViewBag.AmountSortParm = sortOrder == "amount_name_asc" ? "amount_name_desc" : "amount_name_asc";
            ViewBag.OrderDateSortParm = sortOrder == "orderdate_name_asc" ? "orderdate_name_desc" : "orderdate_name_asc";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var orders = from ord in _context.Orders
                         select ord;
            
            //(from p in _context.Products
            // join ord in orders
            // on p.Id equals ord.ProductId
            // select p).Load();
            orders = orders.Include(o => o.Customer).Include(o => o.Product).Include(o => o.Worker);

            if (!string.IsNullOrEmpty(searchString))
            {
                orders = orders.Where(ord => ord.Product.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "id_desc":
                    orders = orders.OrderByDescending(ord => ord.Id);
                    break;
                case "prod_name_asc":
                    orders = orders.OrderBy(ord => ord.Product.Name);
                    break;
                case "prod_name_desc":
                    orders = orders.OrderByDescending(ord => ord.Product.Name);
                    break;
                case "cust_name_asc":
                    orders = orders.OrderBy(ord => ord.Customer.Name);
                    break;
                case "cust_name_desc":
                    orders = orders.OrderByDescending(ord => ord.Customer.Name);
                    break;
                case "amount_name_asc":
                    orders = orders.OrderBy(ord => ord.Amount);
                    break;
                case "amount_name_desc":
                    orders = orders.OrderByDescending(ord => ord.Amount);
                    break;
                case "orderdate_name_asc":
                    orders = orders.OrderBy(ord => ord.OrderDate);
                    break;
                case "orderdate_name_desc":
                    orders = orders.OrderByDescending(ord => ord.OrderDate);
                    break;
                default:
                    orders = orders.OrderBy(ord => ord.Id);
                    break;
            }
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(orders.ToPagedList(pageNumber, pageSize));
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Product)
                .Include(o => o.Worker)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name");
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id");
            ViewData["WorkerId"] = new SelectList(_context.Workers, "Id", "Id");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustomerId,ProductId,Amount,OrderDate,ExecutionStartDate,ImplementationDate,DeliveryOrderDate,WorkerId")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", order.CustomerId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", order.ProductId);
            ViewData["WorkerId"] = new SelectList(_context.Workers, "Id", "Id", order.WorkerId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", order.CustomerId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", order.ProductId);
            ViewData["WorkerId"] = new SelectList(_context.Workers, "Id", "Id", order.WorkerId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustomerId,ProductId,Amount,OrderDate,ExecutionStartDate,ImplementationDate,DeliveryOrderDate,WorkerId")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", order.CustomerId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", order.ProductId);
            ViewData["WorkerId"] = new SelectList(_context.Workers, "Id", "Id", order.WorkerId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Product)
                .Include(o => o.Worker)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'SewingCompanyContext.Orders'  is null.");
            }
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
          return _context.Orders.Any(e => e.Id == id);
        }
    }
}
