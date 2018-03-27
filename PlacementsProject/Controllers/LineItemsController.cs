using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PlacementsProject.Data;
using PlacementsProject.Models;

namespace PlacementsProject.Controllers
{
    [Authorize]
    public class LineItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<Program> _logger;

        public LineItemsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<Program> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: LineItems
        public async Task<IActionResult> Index(string sortOrder, string searchString, int? page)
        {
            var lineItems = from s in _context.LineItems.Include(l => l.Campaign)
                select s;
            switch (sortOrder)
            {
                case "LineItemIdAsc":
                    lineItems = lineItems.OrderBy(r => r.Id);
                    break;
                case "LineItemIdDesc":
                    lineItems = lineItems.OrderByDescending(r => r.Id);
                    break;
                case "CampaignNameAsc":
                    lineItems = lineItems.OrderBy(r => r.Campaign.Name);
                    break;
                case "CampaignNameDesc":
                    lineItems = lineItems.OrderByDescending(r => r.Campaign.Name);
                    break;
                case "BookedAmountAsc":
                    lineItems = lineItems.OrderBy(r => r.BookedAmount);
                    break;
                case "BookedAmountDesc":
                    lineItems = lineItems.OrderByDescending(r => r.BookedAmount);
                    break;
                case "AdjustedAmountAsc":
                    lineItems = lineItems.OrderBy(r => r.AdjustedAmount);
                    break;
                case "AdjustedAmountDesc":
                    lineItems = lineItems.OrderByDescending(r => r.AdjustedAmount);
                    break;
                case "ActualAmountAsc":
                    lineItems = lineItems.OrderBy(r => r.ActualAmount);
                    break;
                case "ActualAmountDesc":
                    lineItems = lineItems.OrderByDescending(r => r.ActualAmount);
                    break;
                case "ReviewedAsc":
                    lineItems = lineItems.OrderBy(r => r.Reviewed);
                    break;
                case "ReviewedDesc":
                    lineItems = lineItems.OrderByDescending(r => r.Reviewed);
                    break;
                default:
                    sortOrder = "LineItemIdAsc";
                    lineItems = lineItems.OrderBy(r => r.Id);
                    break;
            }

            if (searchString != null)
            {
                lineItems = lineItems.Where(s => s.Campaign.Name.Contains(searchString));
            }
            ViewData["CurrentSort"] = sortOrder;
            ViewData["CurrentFilter"] = searchString;
            int pageSize = 25;
            return View(await PaginatedList<LineItem>.CreateAsync(lineItems.AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: LineItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lineItem = await _context.LineItems
                .Include(l => l.Campaign)
                .Include(l => l.Comments)
                    .ThenInclude(l => l.User)
                .Include(l => l.Adjustments)
                    .ThenInclude(l => l.User)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (lineItem == null)
            {
                return NotFound();
            }
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            ViewData["UserId"] = currentUser.Id;
            return View(lineItem);
        }

        // GET: LineItems/Create
        public IActionResult Create()
        {
            ViewData["CampaignId"] = new SelectList(_context.Campaigns, "Id", "Id");
            return View();
        }

        // POST: LineItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CampaignId,Reviewed,BookedAmount,AdjustedAmount,ActualAmount")] LineItem lineItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lineItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CampaignId"] = new SelectList(_context.Campaigns, "Id", "Id", lineItem.CampaignId);
            return View(lineItem);
        }

        // GET: LineItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lineItem = await _context.LineItems.SingleOrDefaultAsync(m => m.Id == id);
            if (lineItem == null)
            {
                return NotFound();
            }
            ViewData["CampaignId"] = new SelectList(_context.Campaigns, "Id", "Id", lineItem.CampaignId);
            return View(lineItem);
        }

        // POST: LineItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CampaignId,Reviewed,BookedAmount,AdjustedAmount,ActualAmount")] LineItem lineItem)
        {
            if (id != lineItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lineItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    _logger.LogError(e, "LineItem edit failed", new Object[] {lineItem});
                    throw e;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CampaignId"] = new SelectList(_context.Campaigns, "Id", "Id", lineItem.CampaignId);
            return View(lineItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsReviewed(int id)
        {
            LineItem lineItem = await _context.LineItems
                .Include(l => l.Campaign)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (lineItem == null)
            {
                return NotFound();
            }

            if (lineItem.Reviewed || lineItem.Campaign.Reviewed)
            {
                return BadRequest();
            }

            try
            {
                lineItem.Reviewed = true;
                _context.Update(lineItem);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Mark LineItem as Reviewed failed", new object[] {lineItem});
                throw;
            }
            return RedirectToAction("Details", "LineItems", new { id });
        }

        // GET: LineItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lineItem = await _context.LineItems
                .Include(l => l.Campaign)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (lineItem == null)
            {
                return NotFound();
            }

            return View(lineItem);
        }

        // POST: LineItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lineItem = await _context.LineItems.SingleOrDefaultAsync(m => m.Id == id);
            _context.LineItems.Remove(lineItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LineItemExists(int id)
        {
            return _context.LineItems.Any(e => e.Id == id);
        }
    }
}
