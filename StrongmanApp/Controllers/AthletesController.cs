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
    public class AthletesController : Controller
    {
        private readonly SportDbContext _context;

        public AthletesController(SportDbContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var sportDbContext = _context.Users.Include(u => u.Country);
            DateTime zeroTime = new DateTime(1, 1, 1);
            foreach (var user in sportDbContext)
            {
                if (user.BirthDate != null)
                {
                    TimeSpan? span = DateTime.Now - user.BirthDate;

                    int years = (zeroTime + span).Value.Year - 1;
                    user.Age = years;
                    _context.Update(user);
                }
            }
            _context.SaveChanges();
            return View(await _context.Users.Where(a=>a.IsContestant==true).Include(u => u.Country).ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
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
        [Authorize(Roles = "admin")]
        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name");
            return View();
        }
        [Authorize(Roles = "admin")]
        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,BirthDate,Email2,IsContestant,Age,Weight,Height,FirstCompYear,LastCompYear,CountryId,PhotoUrl,IsAdmin,Sex,SportCategory,LastUpdate,IsDeleted,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] User user, string? password)
        {
            user.Country=_context.Countries.Where(a=>a.Id==user.CountryId).First();
            ModelState.Remove("Country");
            user.LastUpdate = DateOnly.FromDateTime(DateTime.Now);
            ModelState.Remove("LastUpdate");
            user.EmailConfirmed = false;
            ModelState.Remove("EmailConfirmed");
            user.LockoutEnabled = true;
            ModelState.Remove("LockoutEnabled");
            user.TwoFactorEnabled= false;
            ModelState.Remove("TwoFactorEnabled");
            user.AccessFailedCount= 0;
            ModelState.Remove("AccessFailedCount");
            user.PhoneNumberConfirmed= false;
            ModelState.Remove("PhoneNumberConfirmed");
            if (user.BirthDate > DateTime.Now)
            {
                ModelState.AddModelError("BirthDate", "Enter a right Birth Date");
            }
            if (_context.Users.Where(a => a.Name == user.Name).Count() != 0)
            {
                ModelState.AddModelError("Name", "This name already exists");
            }
            if (user.Age <= 0)
            {
                ModelState.AddModelError("Age", "Enter a correct age");
            }
            if (user.Weight <= 0)
            {
                ModelState.AddModelError("Weight", "Enter a correct weight");
            }
            if (user.Height <= 0)
            {
                ModelState.AddModelError("Weight", "Enter a correct height");
            }
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", user.CountryId);
            return View(user);
        }
        [Authorize(Roles = "admin")]
        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", user.CountryId);
            ViewData["UserName"] = _context.Users.Where(a => a.Id == id).FirstOrDefault().Name;
            return View(user);
        }
        [Authorize(Roles = "admin")]
        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,BirthDate,Email2,IsContestant,Age,Weight,Height,FirstCompYear,LastCompYear,CountryId,PhotoUrl,IsAdmin,Sex,SportCategory,LastUpdate,IsDeleted,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] User user, string name)
        {
            if (id != user.Id)
            {
                return NotFound();
            }
            user.Country = _context.Countries.Where(a => a.Id == user.CountryId).FirstOrDefault();
            ModelState.Remove("Country");
            if (user.BirthDate > DateTime.Now)
            {
                ModelState.AddModelError("BirthDate", "Enter a right Birth Date");
            }
            if (_context.Users.Where(a => a.Name == user.Name).Count() != 0 && (name!=user.Name))
            {
                ModelState.AddModelError("Name", "This name already exists");
            }
            if (user.Age <= 0)
            {
                ModelState.AddModelError("Age", "Enter a correct age");
            }
            if (user.Weight <= 0)
            {
                ModelState.AddModelError("Weight", "Enter a correct weight");
            }
            if (user.Height <= 0)
            {
                ModelState.AddModelError("Weight", "Enter a correct height");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
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
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", user.CountryId);
            return View(user);
        }
        [Authorize(Roles = "admin")]
        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
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
        [Authorize(Roles = "admin")]
        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
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
