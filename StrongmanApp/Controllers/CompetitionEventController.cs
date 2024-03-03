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
    public class CompetitionEventController : Controller
    {
        private readonly SportDbContext _context;

        public CompetitionEventController(SportDbContext context)
        {
            _context = context;
        }

        // GET: CompetitionEvents
        public async Task<IActionResult> Index(int id)
        {
            var sportDbContext = _context.CompetitionEvents.Where(a=>a.CompetitionId==id).Include(c => c.Competition).Include(c => c.Event);
            var compName=_context.Competitions.Where(a=>a.Id==id).FirstOrDefault()?.Name;
            ViewData["CompName"] = compName;
            ViewData["CompId"] = id;
            return View(await sportDbContext.ToListAsync());
        }

        // GET: CompetitionEvents/Details/5
        public async Task<IActionResult> Details(int? id, int compId)
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
            ViewData["CompId"] = compId;
            return View(competitionEvent);
        }
        [Authorize(Roles = "admin")]
        // GET: CompetitionEvents/Create
        public IActionResult Create(int compId)
        {
            //ViewData["CompetitionId"] = new SelectList(_context.Competitions, "Id", "Id");
            //ViewData["EventId"] = new SelectList(_context.Events, "Id", "Id");
            ViewData["CompetitionId"] = compId;
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
            if (_context.CompetitionEvents.Where(a=>a.CompetitionId==competitionEvent.CompetitionId && a.EventId == competitionEvent.EventId).Count() > 0)
            {
                ModelState.AddModelError("EventId", "This event is already in the competition");
            }
            if (ModelState.IsValid)
            {
                _context.Add(competitionEvent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new {id=competitionEvent.CompetitionId});
            }
            //ViewData["CompetitionId"] = new SelectList(_context.Competitions, "Id", "Id", competitionEvent.CompetitionId);
            //ViewData["EventId"] = new SelectList(_context.Events, "Id", "Id", competitionEvent.EventId);
            ViewData["CompetitionId"] = competitionEvent.CompetitionId;
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Name");
            return View(competitionEvent);
        }
        [Authorize(Roles = "admin")]
        // GET: CompetitionEvents/Edit/5
        public async Task<IActionResult> Edit(int? id, int compId)
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
            ViewData["CompetitionId"] = compId;
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
            if (_context.CompetitionEvents.Where(a => a.CompetitionId == competitionEvent.CompetitionId && a.EventId == competitionEvent.EventId).Count() > 0)
            {
                ModelState.AddModelError("EventId", "This event is already in the competition");
            }
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
                return RedirectToAction(nameof(Index), new { id = competitionEvent.CompetitionId });
            }
            //ViewData["CompetitionId"] = new SelectList(_context.Competitions, "Id", "Id", competitionEvent.CompetitionId);
            //ViewData["EventId"] = new SelectList(_context.Events, "Id", "Id", competitionEvent.EventId);
            ViewData["CompetitionId"] = competitionEvent.CompetitionId;
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Name", _context.Events.Where(a => a.Name != null));
            return View(competitionEvent);
        }
        [Authorize(Roles = "admin")]
        // GET: CompetitionEvents/Delete/5
        public async Task<IActionResult> Delete(int? id, int compId)
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
            ViewData["CompId"] = compId;
            return View(competitionEvent);
        }
        [Authorize(Roles = "admin")]
        // POST: CompetitionEvents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int compId)
        {
            var competitionEvent = await _context.CompetitionEvents.FindAsync(id);
            if (competitionEvent != null)
            {
                _context.CompetitionEvents.Remove(competitionEvent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new {id=compId});
        }

        private bool CompetitionEventExists(int id)
        {
            return _context.CompetitionEvents.Any(e => e.Id == id);
        }
    }
}
