using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
//using StrongmanApp.Context;
using StrongmanApp.Models;

namespace StrongmanApp.Controllers
{
    public class ResultsController : Controller
    {
        private readonly SportDbContext _context;

        public ResultsController(SportDbContext context)
        {
            _context = context;
        }

        // GET: Results
        public async Task<IActionResult> Index()
        {
            var sportDbContext = _context.Results.Include(r => r.Competition).Include(r => r.Event).Include(r => r.User);
            return View(await sportDbContext.ToListAsync());
        }

        // GET: Results/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _context.Results
                .Include(r => r.Competition)
                .Include(r => r.Event)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (result == null)
            {
                return NotFound();
            }

            return View(result);
        }
        [Authorize(Roles = "admin")]
        // GET: Results/Create
        public IActionResult Create()
        {
            ViewData["CompetitionId"] = new SelectList(_context.Competitions, "Id", "Name");
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name");
            return View();
        }
        [Authorize(Roles = "admin")]
        // POST: Results/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EventId,CompetitionId,UserId,Result1,Place,Points")] Result result)
        {
            result.User = _context.Users.Where(a => a.Id == result.UserId).FirstOrDefault();
            result.Competition = _context.Competitions.Where(a => a.Id == result.CompetitionId).FirstOrDefault();
            result.Event = _context.Events.Where(a => a.Id == result.EventId).FirstOrDefault();
            ModelState.Remove("User");
            ModelState.Remove("Competition");
            ModelState.Remove("Event");
            if (ModelState.IsValid)
            {
                _context.Results.Add(result);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompetitionId"] = new SelectList(_context.Competitions, "Id", "Name", result.CompetitionId);
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Name", result.EventId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", result.UserId);
            return View(result);
        }
        [Authorize(Roles = "admin")]
        // GET: Results/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _context.Results.FindAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            ViewData["CompetitionId"] = new SelectList(_context.Competitions, "Id", "Name", result.CompetitionId);
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Name", result.EventId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", result.UserId);
            return View(result);
        }
        [Authorize(Roles = "admin")]
        // POST: Results/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EventId,CompetitionId,UserId,Result1,Place,Points")] Result result)
        {
            if (id != result.Id)
            {
                return NotFound();
            }
            result.User = _context.Users.Where(a => a.Id == result.UserId).FirstOrDefault();
            result.Competition = _context.Competitions.Where(a => a.Id == result.CompetitionId).FirstOrDefault();
            result.Event = _context.Events.Where(a => a.Id == result.EventId).FirstOrDefault();
            ModelState.Remove("User");
            ModelState.Remove("Competition");
            ModelState.Remove("Event");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(result);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResultExists(result.Id))
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
            //ViewData["CompetitionId"] = new SelectList(_context.Competitions, "Id", "Id", result.CompetitionId);
            //ViewData["EventId"] = new SelectList(_context.Events, "Id", "Id", result.EventId);
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", result.UserId);
            ViewData["CompetitionId"] = new SelectList(_context.Competitions, "Id", "Name", result.CompetitionId);
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Name", result.EventId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", result.UserId);
            return View(result);
        }
        [Authorize(Roles = "admin")]
        // GET: Results/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _context.Results
                .Include(r => r.Competition)
                .Include(r => r.Event)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (result == null)
            {
                return NotFound();
            }

            return View(result);
        }
        [Authorize(Roles = "admin")]
        // POST: Results/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _context.Results.FindAsync(id);
            if (result != null)
            {
                var ln=_context.Lineups.Where(a => a.UserId == result.UserId && a.CompetitionId == result.CompetitionId).FirstOrDefault();
                if (ln!=null)
                {
                    ln.Place = null;
                    ln.Points = null;
                }
                _context.Results.Remove(result);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ResultExists(int id)
        {
            return _context.Results.Any(e => e.Id == id);
        }
    }
}
