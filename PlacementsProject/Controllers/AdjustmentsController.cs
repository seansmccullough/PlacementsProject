using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlacementsProject.Data;
using PlacementsProject.Models;

namespace PlacementsProject.Controllers
{
    [Authorize]
    public class AdjustmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdjustmentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Adjustments/Create
        public async Task<IActionResult> Create(int id)
        {
            var lineItem = await _context.LineItems
                .Include(l => l.Campaign)
                .Include(l => l.Comments)
                .Include(l => l.Adjustments)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (lineItem == null)
            {
                return NotFound();
            }

            ViewData["LineItemId"] = lineItem.Id;
            ViewData["BookedAmount"] = lineItem.BookedAmount;
            ViewData["CurrentAdjustment"] = lineItem.AdjustedAmount;
            ViewData["CurrentActualAmount"] = lineItem.ActualAmount;
            return View();
        }

        // POST: Adjustments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LineItemId,AdjustmentAmount")] Adjustment adjustment)
        {
            if (ModelState.IsValid)
            {
                //TODO check if LineItem.Reviewed == true or if adjustment amount > booked amount
                adjustment.User = await _userManager.GetUserAsync(HttpContext.User);
                adjustment.DateTime = DateTime.UtcNow;
                _context.Add(adjustment);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", "LineItems", new { id = adjustment.LineItemId });
        }

        private bool AdjustmentExists(string id)
        {
            return _context.Adjustments.Any(e => e.Id == id);
        }
    }
}
