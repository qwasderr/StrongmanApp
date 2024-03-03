using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
//using StrongmanApp.Context;
using StrongmanApp.Models;

namespace StrongmanApp.Controllers
{
    public class CompetitionEventsController : Controller
    {
        private readonly SportDbContext _context;

        public CompetitionEventsController(SportDbContext context)
        {
            _context = context;
        }

        // GET: CompetitionEvents
        public async Task<IActionResult> Index()
        {
            var sportDbContext = _context.CompetitionEvents.Include(c => c.Competition).Include(c => c.Event);
            return View(await sportDbContext.ToListAsync());
        }

        // GET: CompetitionEvents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var competitionEvent = await _context.CompetitionEvents
                .Include(c => c.Competition)
                .Include(c => c.Event)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (competitionEvent == null)
            {
                return NotFound();
            }

            return View(competitionEvent);
        }
        [Authorize(Roles = "admin")]
        // GET: CompetitionEvents/Create
        public IActionResult Create()
        {
            //ViewData["CompetitionId"] = new SelectList(_context.Competitions, "Id", "Id");
            //ViewData["EventId"] = new SelectList(_context.Events, "Id", "Id");
            ViewData["CompetitionId"] = new SelectList(_context.Competitions, "Id", "Name");
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Name");
            return View();
        }
        [Authorize(Roles = "admin")]
        // POST: CompetitionEvents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CompetitionId,EventId,Details")] CompetitionEvent competitionEvent)
        {
            competitionEvent.Competition = _context.Competitions.Where(a => a.Id == competitionEvent.CompetitionId).FirstOrDefault();
            competitionEvent.Event = _context.Events.Where(a => a.Id == competitionEvent.EventId).FirstOrDefault();
            ModelState.Remove("Competition");
            ModelState.Remove("Event");
            if (ModelState.IsValid)
            {
                _context.Add(competitionEvent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["CompetitionId"] = new SelectList(_context.Competitions, "Id", "Id", competitionEvent.CompetitionId);
            //ViewData["EventId"] = new SelectList(_context.Events, "Id", "Id", competitionEvent.EventId);
            ViewData["CompetitionId"] = new SelectList(_context.Competitions, "Id", "Name");
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Name");
            return View(competitionEvent);
        }
        [Authorize(Roles = "admin")]
        // GET: CompetitionEvents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var competitionEvent = await _context.CompetitionEvents.FindAsync(id);
            if (competitionEvent == null)
            {
                return NotFound();
            }
            //ViewData["CompetitionId"] = new SelectList(_context.Competitions, "Id", "Id", competitionEvent.CompetitionId);
            //ViewData["EventId"] = new SelectList(_context.Events, "Id", "Id", competitionEvent.EventId);
            ViewData["CompetitionId"] = new SelectList(_context.Competitions, "Id", "Name", _context.Competitions.Where(a => a.Name != null));
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Name", _context.Events.Where(a => a.Name != null));
           
            return View(competitionEvent);
        }
        [Authorize(Roles = "admin")]
        // POST: CompetitionEvents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CompetitionId,EventId,Details")] CompetitionEvent competitionEvent)
        {
            if (id != competitionEvent.Id)
            {
                return NotFound();
            }
            competitionEvent.Competition = _context.Competitions.Where(a => a.Id == competitionEvent.CompetitionId).FirstOrDefault();
            competitionEvent.Event = _context.Events.Where(a => a.Id == competitionEvent.EventId).FirstOrDefault();
            ModelState.Remove("Competition");
            ModelState.Remove("Event");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(competitionEvent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompetitionEventExists(competitionEvent.Id))
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
            //ViewData["CompetitionId"] = new SelectList(_context.Competitions, "Id", "Id", competitionEvent.CompetitionId);
            //ViewData["EventId"] = new SelectList(_context.Events, "Id", "Id", competitionEvent.EventId);
            ViewData["CompetitionId"] = new SelectList(_context.Competitions, "Id", "Name", _context.Competitions.Where(a => a.Name != null));
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Name", _context.Events.Where(a => a.Name != null));
            return View(competitionEvent);
        }
        [Authorize(Roles = "admin")]
        // GET: CompetitionEvents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var competitionEvent = await _context.CompetitionEvents
                .Include(c => c.Competition)
                .Include(c => c.Event)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (competitionEvent == null)
            {
                return NotFound();
            }

            return View(competitionEvent);
        }
        [Authorize(Roles = "admin")]
        // POST: CompetitionEvents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var competitionEvent = await _context.CompetitionEvents.FindAsync(id);
            if (competitionEvent != null)
            {
                _context.CompetitionEvents.Remove(competitionEvent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompetitionEventExists(int id)
        {
            return _context.CompetitionEvents.Any(e => e.Id == id);
        }
    }
}
