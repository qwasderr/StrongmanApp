using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StrongmanApp.Models;

namespace StrongmanApp.Controllers
{
    public class FederationsController : Controller
    {
        private readonly SportDbContext _context;

        public FederationsController(SportDbContext context)
        {
            _context = context;
        }

        // GET: Federations
        public async Task<IActionResult> Index()
        {
            return View(await _context.Federations.ToListAsync());
        }

        // GET: Federations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var federation = await _context.Federations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (federation == null)
            {
                return NotFound();
            }

            return View(federation);
        }
        [Authorize(Roles = "admin")]
        // GET: Federations/Create
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "admin")]
        // POST: Federations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,NumberofContests,FirstYearHeld,LastYearHeld,IsDeleted")] Federation federation)
        {
            federation.NumberofContests = 0;
            ModelState.Remove("NumberofContests");
            if (_context.Federations.Where(a => a.Name == federation.Name).Count() > 0)
            {
                ModelState.AddModelError("Name", "The name already exists");
            }
            if (ModelState.IsValid)
            {
                _context.Add(federation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(federation);
        }
        [Authorize(Roles = "admin")]
        // GET: Federations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var federation = await _context.Federations.FindAsync(id);
            if (federation == null)
            {
                return NotFound();
            }
            return View(federation);
        }
        [Authorize(Roles = "admin")]
        // POST: Federations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,NumberofContests,FirstYearHeld,LastYearHeld,IsDeleted")] Federation federation)
        {
            if (id != federation.Id)
            {
                return NotFound();
            }
            if (_context.Federations.Where(a => a.Name == federation.Name).Count() > 0)
            {
                ModelState.AddModelError("Name", "The name already exists");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(federation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FederationExists(federation.Id))
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
            return View(federation);
        }
        [Authorize(Roles = "admin")]
        // GET: Federations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var federation = await _context.Federations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (federation == null)
            {
                return NotFound();
            }

            return View(federation);
        }
        [Authorize(Roles = "admin")]
        // POST: Federations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var federation = await _context.Federations.FindAsync(id);
            if (federation != null)
            {
                _context.Federations.Remove(federation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FederationExists(int id)
        {
            return _context.Federations.Any(e => e.Id == id);
        }
    }
}
