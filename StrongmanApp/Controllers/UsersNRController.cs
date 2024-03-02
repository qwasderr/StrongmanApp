using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StrongmanApp.Models;

namespace StrongmanApp.Controllers
{
    public class UsersNRController : Controller
    {
        private readonly SportDbContext _context;

        public UsersNRController(SportDbContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            List<int> list1 = new List<int>();
            foreach (var user in _context.AspNetUsers)
            {
                list1.Add(user.Id);
            }
            //var sportDbContext = _context.Users.Where(a=>list1.Contains(a.Id)).Include(a=>a.Country);
            var sportDbContext = _context.Users.Include(a => a.Country);
            return View(await sportDbContext.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            //ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Id");
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", _context.Countries.Where(a => a.Name != null));
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,BirthDate,Email,IsContestant,Age,Weight,Height,FirstCompYear,LastCompYear,CountryId,PhotoUrl,IsAdmin,Sex,SportCategory,LastUpdate,IsDeleted")] User user)
        {
            user.Country = _context.Countries.Where(a => a.Id == user.CountryId).FirstOrDefault();
            ModelState.Remove("Country");
            user.LastUpdate = DateOnly.FromDateTime(DateTime.Now);
            ModelState.Remove("LastUpdate");
            ModelState.Remove("Id");
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Id", user.CountryId);
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", _context.Countries.Where(a => a.Name != null));
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string id)
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
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", _context.Countries.Where(a=>a.Name!=null));
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,BirthDate,Email,IsContestant,Age,Weight,Height,FirstCompYear,LastCompYear,CountryId,PhotoUrl,IsAdmin,Sex,SportCategory,LastUpdate,IsDeleted")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }
            user.Country=_context.Countries.Where(a=>a.Id==user.CountryId).FirstOrDefault();
            ModelState.Remove("Country");
            if (ModelState.IsValid)
            {
                try
                {
                    var user2=_context.AspNetUsers.Where(a=>a.Id == user.Id).FirstOrDefault();
                    _context.Update(user);
                    if (user2!=null) {
                        user2.UserName = user.Name; _context.Update(user2);
                        
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            //ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Id", user.CountryId);
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", _context.Countries.Where(a => a.Name != null));
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
