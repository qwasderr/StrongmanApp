using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Bcpg;
using StrongmanApp.Models;

namespace StrongmanApp.Controllers
{
    public class CompetitionsRegController : Controller
    {
        private readonly SportDbContext _context;

        public CompetitionsRegController(SportDbContext context)
        {
            _context = context;
        }

        // GET: Competitions
        public async Task<IActionResult> Index()
        {
            var sportDbContext = _context.Competitions.Include(c => c.Federation).Include(c => c.Town);
            //var sportDbContext = _context.Competitions.Include(c => c.Federation).Include(c => c.Town).Where(a=>a.Date> DateOnly.FromDateTime(DateTime.Now));
            return View(await sportDbContext.ToListAsync());
        }

        // GET: Competitions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var competition = await _context.Competitions
                .Include(c => c.Federation)
                .Include(c => c.Town)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (competition == null)
            {
                return NotFound();
            }
            ViewData["MethodRef"] = this.isRegistred;
            return View(competition);
        }

        // GET: Competitions/Create
        public IActionResult Create()
        {
            ViewData["FederationId"] = new SelectList(_context.Federations, "Id", "Name");
            ViewData["TownId"] = new SelectList(_context.Towns, "Id", "Name");
            //ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "Id");
            return View();
        }

        // POST: Competitions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Division,CompScale,Date,TownId,FederationId,VideoUrl")] Competition competition)
        {
            if (ModelState.IsValid)
            {
                _context.Add(competition);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["FederationId"] = new SelectList(_context.Federations, "Id", "Id", competition.FederationId);
            //ViewData["TownId"] = new SelectList(_context.Towns, "Id", "Id", competition.TownId);
            ViewData["FederationId"] = new SelectList(_context.Federations, "Id", "Name", _context.Federations.Where(a => a.Name != null));
            ViewData["TownId"] = new SelectList(_context.Towns, "Id", "Name", _context.Towns.Where(a => a.Name != null));
            //ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "Id", competition.VideoId);
            return View(competition);
        }

        // GET: Competitions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var competition = await _context.Competitions.FindAsync(id);
            if (competition == null)
            {
                return NotFound();
            }
            ViewData["FederationId"] = new SelectList(_context.Federations, "Id", "Name", _context.Federations.Where(a => a.Name != null));
            ViewData["TownId"] = new SelectList(_context.Towns, "Id", "Name", _context.Towns.Where(a => a.Name != null));
            //ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "Url", _context.Videos.Where(a => a.Url != null));
            return View(competition);
        }

        // POST: Competitions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Division,CompScale,Date,TownId,FederationId,VideoUrl")] Competition competition)
        {
            if (id != competition.Id)
            {
                return NotFound();
            }
            competition.Town = _context.Towns.Where(a => a.Id == competition.TownId).FirstOrDefault();
            ModelState.Remove("Town");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(competition);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompetitionExists(competition.Id))
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
            //ViewData["FederationId"] = new SelectList(_context.Federations, "Id", "Id", competition.FederationId);
            //ViewData["TownId"] = new SelectList(_context.Towns, "Id", "Id", competition.TownId);
            //ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "Id", competition.VideoId);
            ViewData["FederationId"] = new SelectList(_context.Federations, "Id", "Name", _context.Federations.Where(a => a.Name != null));
            ViewData["TownId"] = new SelectList(_context.Towns, "Id", "Name", _context.Towns.Where(a => a.Name != null));
            //ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "Url", _context.Videos.Where(a => a.Url != null));
            return View(competition);
        }

        // GET: Competitions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var competition = await _context.Competitions
                .Include(c => c.Federation)
                .Include(c => c.Town)
                .Include(c => c.VideoUrl)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (competition == null)
            {
                return NotFound();
            }

            return View(competition);
        }

        // POST: Competitions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var competition = await _context.Competitions.FindAsync(id);
            if (competition != null)
            {
                _context.Competitions.Remove(competition);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompetitionExists(int id)
        {
            return _context.Competitions.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Register(string? id, int idComp)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            //ViewData["FederationId"] = new SelectList(_context.Federations, "Id", "Name", _context.Federations.Where(a => a.Name != null));
            //ViewData["TownId"] = new SelectList(_context.Towns, "Id", "Name", _context.Towns.Where(a => a.Name != null));
            Lineup ln=new Lineup();
            ln.IsConfirmed = 0;
            ln.UserId = id;
            ln.CompetitionId = idComp;
            ln.Competition=_context.Competitions.Where(a=>a.Id==idComp).FirstOrDefault();
            ln.User = _context.Users.Where(a => a.Id == id).FirstOrDefault();
            _context.Lineups.Add(ln);
            await _context.SaveChangesAsync();
            //ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "Url", _context.Videos.Where(a => a.Url != null));
            return RedirectToAction(nameof(Index));
        }
        public bool isRegistred(string UserId, int CompId)
        {
           var ln=_context.Lineups.Where(a=>a.UserId == UserId && a.CompetitionId==CompId).FirstOrDefault();
            if (ln == null) return false;
            return true;
        }
        
        public async Task<IActionResult> DeleteReg(string? id, int idComp)
        {
            var ln = _context.Lineups.Where(a=>a.UserId==id && a.CompetitionId==idComp).FirstOrDefault();
            if (ln != null)
            {
                _context.Lineups.Remove(ln);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
