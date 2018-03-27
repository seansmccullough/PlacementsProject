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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LineItemId,AdjustmentAmount")] Adjustment adjustment)
        {
            if (ModelState.IsValid)
            {
                LineItem lineItem = await _context.LineItems
                    .Include(l => l.Campaign)
                    .SingleOrDefaultAsync(m => m.Id == adjustment.LineItemId);

                if (lineItem == null)
                {
                    return NotFound();
                }

                if (lineItem.Reviewed || lineItem.Campaign.Reviewed)
                {
                    return BadRequest();
                }

                //TODO check if adjustment amount > booked amount
                adjustment.User = await _userManager.GetUserAsync(HttpContext.User);
                adjustment.DateTime = DateTime.UtcNow;
                _context.Add(adjustment);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", "LineItems", new { id = adjustment.LineItemId });
        }
    }
}
