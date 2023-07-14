﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BugTrackerApp.Data;
using BugTrackerApp.Models;

namespace BugTrackerApp.Controllers
{
    public class AttachmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AttachmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Attachment
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Attachments.Include(a => a.Ticket);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Attachment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Attachments == null)
            {
                return NotFound();
            }

            var attachment = await _context.Attachments
                .Include(a => a.Ticket)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (attachment == null)
            {
                return NotFound();
            }

            return View(attachment);
        }

        // GET: Attachment/Create
        public IActionResult Create()
        {
            ViewData["TicketId"] = new SelectList(_context.Ticket, "Id", "Title");
            return View();
        }

        // POST: Attachment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TicketId,FileName,FileData,ContentType")] Attachment attachment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(attachment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TicketId"] = new SelectList(_context.Ticket, "Id", "Title", attachment.TicketId);
            return View(attachment);
        }

        // GET: Attachment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Attachments == null)
            {
                return NotFound();
            }

            var attachment = await _context.Attachments.FindAsync(id);
            if (attachment == null)
            {
                return NotFound();
            }
            ViewData["TicketId"] = new SelectList(_context.Ticket, "Id", "Title", attachment.TicketId);
            return View(attachment);
        }

        // POST: Attachment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TicketId,FileName,FileData,ContentType")] Attachment attachment)
        {
            if (id != attachment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attachment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttachmentExists(attachment.Id))
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
            ViewData["TicketId"] = new SelectList(_context.Ticket, "Id", "Title", attachment.TicketId);
            return View(attachment);
        }

        // GET: Attachment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Attachments == null)
            {
                return NotFound();
            }

            var attachment = await _context.Attachments
                .Include(a => a.Ticket)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (attachment == null)
            {
                return NotFound();
            }

            return View(attachment);
        }

        // POST: Attachment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Attachments == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Attachments'  is null.");
            }
            var attachment = await _context.Attachments.FindAsync(id);
            if (attachment != null)
            {
                _context.Attachments.Remove(attachment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AttachmentExists(int id)
        {
          return (_context.Attachments?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // download attachment action
        public async Task<IActionResult> Download(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attachment = await _context.Attachments
                .FirstOrDefaultAsync(a => a.Id == id);

            if (attachment == null)
            {
                return NotFound();
            }

            return File(attachment.FileData, attachment.ContentType, attachment.FileName);
        }

    }
}