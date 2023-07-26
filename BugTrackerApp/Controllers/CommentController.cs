using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BugTrackerApp.Data;
using BugTrackerApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace BugTrackerApp.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
    public class CommentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _UserManager;

        public CommentController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _UserManager = userManager; 
        }

        // GET: Comment
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Comment.Include(c => c.Ticket).Include(c => c.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Comment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Comment == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .Include(c => c.Ticket)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

    

  


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int TicketId, string Text, string origin)
        {
            // retrieve the user
            var user = await _UserManager.GetUserAsync(User);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var comment = new Comment
            {
                TicketId = TicketId,
                Message = Text,
                UserId = _UserManager.GetUserId(User),
            };

            _context.Add(comment);
            await _context.SaveChangesAsync();

            
            return RedirectToAction("Details", "Ticket", new { id = TicketId, origin = origin });
        }


        // GET: Comment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Comment == null)
            {
                return NotFound();
            }

            ViewBag.Referrer = Request.Headers["Referer"].ToString(); 
            var comment = await _context.Comment.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            ViewData["TicketId"] = new SelectList(_context.Ticket, "Id", "Title", comment.TicketId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", comment.UserId);
            return View(comment);
        }

        // POST: Comment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Message,TicketId,Created,UserId")] Comment comment, string referrer)
        {
            if (id != comment.Id)
            {
                return NotFound();
            }

            // remove these properties from the modelstate validation because they aren't being passed from the edit view
            ModelState.Remove("User");
            ModelState.Remove("Ticket");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment);
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
                if (!String.IsNullOrEmpty(referrer)) 
                {
                    return Redirect(referrer);
                }

                else
                return RedirectToAction("Home", "Index");
            }
            ViewData["TicketId"] = new SelectList(_context.Ticket, "Id", "Title", comment.TicketId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", comment.UserId);
            return View(comment);
        }

        // GET: Comment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Comment == null)
            {
                return NotFound();
            }

            ViewBag.Referrer = Request.Headers["Referer"].ToString();
            var comment = await _context.Comment
                .Include(c => c.Ticket)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string referrer)
        {
            if (_context.Comment == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Comment'  is null.");
            }
            var comment = await _context.Comment.FindAsync(id);
            if (comment != null)
            {
                _context.Comment.Remove(comment);
            }

            await _context.SaveChangesAsync();
            if (!string.IsNullOrEmpty(referrer))
            {
                return Redirect(referrer);
            }

            else
            return RedirectToAction("Index", "Home");
        }

        private bool CommentExists(int id)
        {
            return (_context.Comment?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
