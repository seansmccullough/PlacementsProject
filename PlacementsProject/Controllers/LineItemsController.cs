using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PlacementsProject.Data;
using PlacementsProject.Models;

namespace PlacementsProject.Controllers
{
    [Authorize]
    public class LineItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LineItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LineItems
        public async Task<IActionResult> Index(string sortOrder, int? page)
        {
            var lineItems = from s in _context.LineItems
                           select s;
            switch (sortOrder)
            {
                case "LineItemIdAsc":
                    lineItems = lineItems.OrderBy(r => r.Id);
                    break;
                case "LineItemIdDesc":
                    lineItems = lineItems.OrderByDescending(r => r.Id);
                    break;
                case "CampaignIdAsc":
                    lineItems = lineItems.OrderBy(r => r.CampaignId);
                    break;
                case "CampaignIdDesc":
                    lineItems = lineItems.OrderByDescending(r => r.CampaignId);
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

            ViewData["CurrentSort"] = sortOrder;
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
                catch (DbUpdateConcurrencyException)
                {
                    if (!LineItemExists(lineItem.Id))
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
            ViewData["CampaignId"] = new SelectList(_context.Campaigns, "Id", "Id", lineItem.CampaignId);
            return View(lineItem);
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
