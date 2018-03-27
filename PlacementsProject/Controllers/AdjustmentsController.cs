using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlacementsProject.Data;
using PlacementsProject.Models;
using PlacementsProject.Models.ViewModels;

namespace PlacementsProject.Controllers
{
    /// <summary>
    /// Adjustments Controller
    /// </summary>
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
            // Get LineItem this Adjustment will be associated with
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
        public async Task<IActionResult> Create([Bind("LineItemId,AdjustmentAmount")] AdjustmentViewModel adjustmentViewModel)
        {
            LineItem lineItem = await _context.LineItems
                .Include(l => l.Campaign)
                .SingleOrDefaultAsync(m => m.Id == adjustmentViewModel.LineItemId);

            if (lineItem == null)
            {
                return NotFound();
            }

            // If LineItem or its Campaign have been marked as Reviewed
            if (lineItem.Reviewed || lineItem.Campaign.Reviewed)
            {
                return BadRequest();
            }

            // Adjustment amount must be > 0 and < the LineItem's BookedAmount
            if (adjustmentViewModel.AdjustmentAmount < 0 || adjustmentViewModel.AdjustmentAmount > lineItem.BookedAmount)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                // Create Adjustment
                Adjustment adjustment = new Adjustment();
                adjustment.User = await _userManager.GetUserAsync(HttpContext.User);
                adjustment.AdjustmentAmount = adjustmentViewModel.AdjustmentAmount;
                adjustment.DateTime = DateTime.UtcNow;
                adjustment.LineItem = lineItem;
                _context.Add(adjustment);

                // Update LineItem
                lineItem.AdjustedAmount = adjustment.AdjustmentAmount;
                lineItem.ActualAmount = lineItem.BookedAmount - adjustment.AdjustmentAmount;
                if (lineItem.Adjustments == null)
                {
                    lineItem.Adjustments = new List<Adjustment>();
                }
                lineItem.Adjustments.Add(adjustment);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", "LineItems", new { id = adjustmentViewModel.LineItemId });
        }
    }
}
