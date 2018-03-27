using System;
using System.Linq;
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
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.LineItem)
                .Include(c => c.User)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(new CommentViewModel(comment));
        }

        // GET: Comments/Create
        public IActionResult Create(int id)
        {
            ViewData["LineItemId"] = id;
            return View();
        }

        // POST: Comments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LineItemId,Text")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                //TODO check if LineItem or Campaign is locked
                comment.User = await _userManager.GetUserAsync(HttpContext.User);
                comment.ModifiedDateTime = DateTime.UtcNow;
                _context.Add(comment);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Details", "LineItems", new {Id = comment.LineItemId });
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Comment comment = await _context.Comments
                .Include(l => l.LineItem)
                .ThenInclude(l => l.Campaign)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            if (comment.LineItem.Reviewed || comment.LineItem.Campaign.Reviewed)
            {
                return BadRequest();
            }

            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (!comment.User.Equals(currentUser))
            {
                return Unauthorized();
            }
            ViewData["LineItemId"] = comment.LineItemId;
            return View(new CommentViewModel(comment));
        }

        // POST: Comments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Text")] Comment comment)
        {
            Comment previousComment = await _context.Comments
                .Include(l => l.LineItem)
                .ThenInclude(l => l.Campaign)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (previousComment == null)
            {
                return NotFound();
            }

            if (previousComment.LineItem.Reviewed || previousComment.LineItem.Campaign.Reviewed)
            {
                return BadRequest();
            }

            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (!previousComment.User.Equals(currentUser))
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    previousComment.ModifiedDateTime = DateTime.UtcNow;
                    previousComment.Text = comment.Text;
                    _context.Update(previousComment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return RedirectToAction("Details", "LineItems", new { Id = previousComment.LineItemId });
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Comment previousComment = await _context.Comments
                .Include(l => l.LineItem)
                .ThenInclude(l => l.Campaign)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (previousComment == null)
            {
                return NotFound();
            }

            if (previousComment.LineItem.Reviewed || previousComment.LineItem.Campaign.Reviewed)
            {
                return BadRequest();
            }

            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (!previousComment.User.Equals(currentUser))
            {
                return Unauthorized();
            }

            var comment = await _context.Comments
                .Include(c => c.LineItem)
                .Include(c => c.User)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            ViewData["LineItemId"] = comment.LineItemId;
            return View(new CommentViewModel(comment));
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Comment comment = await _context.Comments
                .Include(l => l.LineItem)
                .ThenInclude(l => l.Campaign)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (comment == null)
            {
                return NotFound();
            }

            if (comment.LineItem.Reviewed || comment.LineItem.Campaign.Reviewed)
            {
                return BadRequest();
            }

            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (!comment.User.Equals(currentUser))
            {
                return Unauthorized();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "LineItems", new { Id = comment.LineItemId });
        }

        private bool CommentExists(string id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}
