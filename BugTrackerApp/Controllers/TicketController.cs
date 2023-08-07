using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BugTrackerApp.Data;
using BugTrackerApp.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace BugTrackerApp.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
    public class TicketController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _UserManager;

        public TicketController(ApplicationDbContext context, UserManager<User> usermanager)
        {
            _context = context;
            _UserManager = usermanager;
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            // Check the role of the user
            if (User.IsInRole("Admin"))
            {
                // Admin can see all tickets
                var applicationDbContext = _context.Ticket.Include(t => t.Project);
                return View(await applicationDbContext.ToListAsync());
            }
            else if (User.IsInRole("Manager"))
            {
                // Retrieve projects assigned to the manager
                string userId = _UserManager.GetUserId(User);
                var managerProjects = _context.Projects.Where(p => p.UserId == userId).Select(p => p.Id).ToList();

                // Get tickets that belong to projects assigned to the manager
                var applicationDbContext = _context.Ticket.Include(t => t.Project)
                    .Where(t => managerProjects.Contains(t.ProjectId));
                return View(await applicationDbContext.ToListAsync());
            }
            else if (User.IsInRole("Developer"))
            {
                // Retrieve projects assigned to the developer
                string userId = _UserManager.GetUserId(User);
                var developerProjects = _context.Projects.Where(p => p.UserId == userId).Select(p => p.Id).ToList();

                // Get tickets that belong to projects assigned to the developer
                var applicationDbContext = _context.Ticket.Include(t => t.Project)
                    .Where(t => developerProjects.Contains(t.ProjectId));
                return View(await applicationDbContext.ToListAsync());
            }
            else
            {
                return Forbid(); // For other roles or users without roles, deny access
            }
        }


        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id, string origin)
        {
            if (id == null || _context.Ticket == null)
            {
                return NotFound();
            }

            ViewBag.Referrer = Request.Headers["Referer"].ToString();

            var ticket = await _context.Ticket
                .Include(t => t.Attachments)
                .Include(t => t.Comments)
                    .ThenInclude(t => t.User)
                .Include(t => t.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            ViewBag.Origin = origin;
            return View(ticket);
        }

        // GET: Tickets/Create
        public async Task<IActionResult> Create()
        {
            // The "Referer" header is a standard HTTP header that is automatically sent by browsers to
            // indicate the URL of the page the user was on before making the current request.
            // This Referrer URL is then stored in the ViewBag under the key "Referrer" so that it can be passed to the view.
            // ViewBag is a dynamic property in ASP.NET MVC that's used to pass temporary data from the controller to the view.
            // It's a wrapper around ViewData that provides a dynamic property for adding to the view data dictionary.
            ViewBag.Referrer = Request.Headers["Referer"].ToString();

            string userId = _UserManager.GetUserId(User);
            var user = await _UserManager.FindByIdAsync(userId);

            // Check the role of the user
            if (User.IsInRole("Admin"))
            {
                ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name");
            }
            else if (User.IsInRole("Manager"))
            {
                // Retrieve only projects assigned to the manager
                var managerProjects = _context.Projects.Where(p => p.UserId == userId).ToList();
                ViewData["ProjectId"] = new SelectList(managerProjects, "Id", "Name");
            }
            else if (User.IsInRole("Developer"))
            {
                // Retrieve only projects assigned to the developer
                var developerProjects = _context.Projects.Where(p => p.UserId == userId).ToList();
                ViewData["ProjectId"] = new SelectList(developerProjects, "Id", "Name");
            }
            else
            {
                // For other roles or users without roles, don't show any projects
                ViewData["ProjectId"] = new SelectList(Enumerable.Empty<Project>(), "Id", "Name");
            }

            return View();
        }


        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProjectId,Title,Description,Developer,Priority,Status,Created")]Ticket ticket, IFormFile attachment, string referrer)
        {
            // remove Project property from ModelState validation check since it isn't being passed from the view
            ModelState.Remove("Project");
            ModelState.Remove("User");
            ModelState.Remove("UserId");
            
            if (ModelState.IsValid)
            {
                // retrieve the user
                var user = await _UserManager.GetUserAsync(User);
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                ticket.UserId = user.Id;
                if (attachment != null && attachment.Length > 0)
                {
                    var newAttachment = new Attachment
                    {
                        FileName = Path.GetFileName(attachment.FileName),
                        ContentType = attachment.ContentType,
                        Ticket = ticket,
                        UserId = user.Id
                    };

                    using (var memoryStream = new MemoryStream())
                    {
                        await attachment.CopyToAsync(memoryStream);
                        newAttachment.FileData = memoryStream.ToArray();
                    }
                    _context.Attachments.Add(newAttachment);
                }
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                TempData["success"] = "Ticket created successfully";
                // Redirect to the referring URL if it exists; otherwise, redirect to the Index action
                if (!string.IsNullOrEmpty(referrer))
                {
                    return Redirect(referrer);
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
              
            }
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", ticket.ProjectId);

            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Ticket == null)
            {
                return NotFound();
            }

			ViewBag.Referrer = Request.Headers["Referer"].ToString();
			var ticket = await _context.Ticket.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", ticket.ProjectId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProjectId,Title,Description,Developer,Priority,Status,Created")] Ticket ticket, string referrer)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            // remove project from the model state validation check because it isn't passed from the view
            ModelState.Remove("Project");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "Ticket updated successfully";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
				if (!string.IsNullOrEmpty(referrer))
				{
					return Redirect(referrer);
				}

                else
				return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", ticket.ProjectId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Ticket == null)
            {
                return NotFound();
            }

			ViewBag.Referrer = Request.Headers["Referer"].ToString();
			var ticket = await _context.Ticket
                .Include(t => t.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string referrer)
        {
            if (_context.Ticket == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Ticket'  is null.");
            }
            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket != null)
            {
                _context.Ticket.Remove(ticket);
            }
            
            await _context.SaveChangesAsync();
            TempData["success"] = "Ticket deleted successfully";

			if (!string.IsNullOrEmpty(referrer))
			{
				return Redirect(referrer);
			}

            else
			return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
          return (_context.Ticket?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
