﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
//using StrongmanApp.Context;
using StrongmanApp.Models;

namespace StrongmanApp.Controllers
{
    [Authorize(Roles = "admin")]
    public class LineupsController : Controller
    {
        private readonly SportDbContext _context;

        public LineupsController(SportDbContext context)
        {
            _context = context;
        }

        // GET: Lineups
        public async Task<IActionResult> Index()
        {
            var sportDbContext = _context.Lineups.Include(l => l.Competition).Include(l => l.User);
            return View(await sportDbContext.ToListAsync());
        }

        // GET: Lineups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lineup = await _context.Lineups
                .Include(l => l.Competition)
                .Include(l => l.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lineup == null)
            {
                return NotFound();
            }

            return View(lineup);
        }

        // GET: Lineups/Create
        public IActionResult Create()
        {
            ViewData["CompetitionId"] = new SelectList(_context.Competitions, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name");
            return View();
        }

        // POST: Lineups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CompetitionId,UserId,Details,IsConfirmed,RegistrationDate,Place,Points")] Lineup lineup)
        {
            lineup.User = _context.Users.Where(a => a.Id == lineup.UserId).FirstOrDefault();
            lineup.Competition = _context.Competitions.Where(a => a.Id == lineup.CompetitionId).FirstOrDefault();
            ModelState.Remove("User");
            ModelState.Remove("Competition");
            lineup.RegistrationDate = DateTime.Now;
            if (_context.Lineups.Where(a=>a.UserId == lineup.UserId && a.CompetitionId == lineup.CompetitionId).Count() > 0)
            {
                ModelState.AddModelError("UserId", "This athlete has already registered for this competition");
            }
            if (ModelState.IsValid)
            {
                _context.Add(lineup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["CompetitionId"] = new SelectList(_context.Competitions, "Id", "Id", lineup.CompetitionId);
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", lineup.UserId);
            ViewData["CompetitionId"] = new SelectList(_context.Competitions, "Id", "Name", _context.Competitions.Where(a => a.Id != null));
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", _context.Users.Where(a => a.Id != null));
            return View(lineup);
        }

        // GET: Lineups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lineup = await _context.Lineups.FindAsync(id);
            if (lineup == null)
            {
                return NotFound();
            }
            ViewData["CompetitionId"] = new SelectList(_context.Competitions, "Id", "Name", _context.Competitions.Where(a=>a.Id!=null));
            ViewData["UsId"] = _context.Users.Where(a => a.Id == _context.Lineups.Where(a => a.Id == id).FirstOrDefault().UserId).FirstOrDefault();
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", _context.Users.Where(a => a.Id != null));
            return View(lineup);
        }

        // POST: Lineups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CompetitionId,UserId,Details,IsConfirmed,RegistrationDate,Place,Points")] Lineup lineup, int userId)
        {
            if (id != lineup.Id)
            {
                return NotFound();
            }
            lineup.User = _context.Users.Where(a => a.Id == lineup.UserId).FirstOrDefault();
            lineup.Competition = _context.Competitions.Where(a => a.Id == lineup.CompetitionId).FirstOrDefault();
            ModelState.Remove("User");
            ModelState.Remove("Competition");
            if (_context.Lineups.Where(a => a.UserId == lineup.UserId && a.CompetitionId == lineup.CompetitionId).Count() > 0 && (userId!=lineup.UserId))
            {
                ModelState.AddModelError("UserId", "This athlete has already registered for this competition");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var user = _context.Users.Where(a => a.Id == lineup.UserId).FirstOrDefault();
                    if (lineup.Place!=null && lineup.Points!=null)
                    {
                        if (user.FirstCompYear == null) user.FirstCompYear = Convert.ToInt32(DateTime.Now.Year);
                        user.LastCompYear = Convert.ToInt32(DateTime.Now.Year);
                    }
                    _context.Update(user);
                    _context.Update(lineup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LineupExists(lineup.Id))
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
            //ViewData["CompetitionId"] = new SelectList(_context.Competitions, "Id", "Id", lineup.CompetitionId);
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", lineup.UserId);
            ViewData["CompetitionId"] = new SelectList(_context.Competitions, "Id", "Name", _context.Competitions.Where(a => a.Id != null));
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", _context.Users.Where(a => a.Id != null));
            return View(lineup);
        }

        // GET: Lineups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lineup = await _context.Lineups
                .Include(l => l.Competition)
                .Include(l => l.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lineup == null)
            {
                return NotFound();
            }

            return View(lineup);
        }

        // POST: Lineups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lineup = await _context.Lineups.FindAsync(id);
            if (lineup != null)
            {
                _context.Lineups.Remove(lineup);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LineupExists(int id)
        {
            return _context.Lineups.Any(e => e.Id == id);
        }
    }
}
