using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlacementsProject.Data;
using PlacementsProject.Models;
using PlacementsProject.Models.ViewModels;

namespace PlacementsProject.Controllers
{
    [Authorize]
    public class CampaignsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CampaignsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Campaigns
        public async Task<IActionResult> Index(string sortOrder, int? page)
        {
            var campaigns = from s in _context.Campaigns
                select s;
            switch (sortOrder)
            {
                case "CampaignIdAsc":
                    campaigns = campaigns.OrderBy(r => r.Id);
                    break;
                case "CampaignIdDesc":
                    campaigns = campaigns.OrderByDescending(r => r.Id);
                    break;
                case "NameAsc":
                    campaigns = campaigns.OrderBy(r => r.Name);
                    break;
                case "NameDesc":
                    campaigns = campaigns.OrderByDescending(r => r.Name);
                    break;
                case "ReviewedAsc":
                    campaigns = campaigns.OrderBy(r => r.Reviewed);
                    break;
                case "ReviewedDesc":
                    campaigns = campaigns.OrderByDescending(r => r.Reviewed);
                    break;
                default:
                    sortOrder = "CampaignIdAsc";
                    campaigns = campaigns.OrderBy(r => r.Id);
                    break;
            }
            ViewData["CurrentSort"] = sortOrder;
            int pageSize = 25;
            var count = await campaigns.AsNoTracking().CountAsync();
            var items = await campaigns.AsNoTracking().Skip((page ?? 1 - 1) * pageSize).Take(pageSize).ToListAsync();
            var campaignViewModels = new List<CampaignViewModel>();
            foreach (var campaign in items)
            {
                campaignViewModels.Add(new CampaignViewModel(campaign));
            }
            return View(new PaginatedList<CampaignViewModel>(campaignViewModels, count, page ?? 1, pageSize));
        }

        // GET: Campaigns/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var campaign = await _context.Campaigns
                .SingleOrDefaultAsync(m => m.Id == id);
            if (campaign == null)
            {
                return NotFound();
            }

            return View(new CampaignViewModel(campaign));
        }

        // GET: Campaigns/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var campaign = await _context.Campaigns.SingleOrDefaultAsync(m => m.Id == id);
            if (campaign == null)
            {
                return NotFound();
            }
            return View(new CampaignViewModel(campaign));
        }

        // POST: Campaigns/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Reviewed")] Campaign campaign)
        {
            if (id != campaign.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(campaign);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CampaignExists(campaign.Id))
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
            return View(new CampaignViewModel(campaign));
        }

        private bool CampaignExists(int id)
        {
            return _context.Campaigns.Any(e => e.Id == id);
        }
    }
}
