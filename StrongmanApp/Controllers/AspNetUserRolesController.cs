using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StrongmanApp.Models;

namespace StrongmanApp.Controllers
{
    public class AspNetUserRolesController : Controller
    {
        private readonly SportDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AspNetUserRolesController(SportDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: AspNetUserRoles
        public async Task<IActionResult> Index()
        {
            var usersAndRoles = new List<AspNetUserRoles>();
            //var users = _context.AspNetUsers;
            var users = _userManager.Users.ToList();
            foreach (var user in users)
            {
               var list = await _userManager.GetRolesAsync(user);
                foreach (var role in list)
                {
                    var roleId = await _roleManager.FindByNameAsync(role);
                    usersAndRoles.Add(new AspNetUserRoles
                    {
                        UserId = user.Id,
                        RoleId=roleId.Id,
                        UserName = user.UserName,
                        RoleName = role
                    });
                }
            }
            //var sportDbContext = _context.AspNetUserRoles.Include(a => a.Role).Include(a => a.User);
            return View(usersAndRoles.ToList());
        }

        // GET: AspNetUserRoles/Details/5
        /*public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aspNetUserRoles = await _context.AspNetUserRoles
                .Include(a => a.Role)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (aspNetUserRoles == null)
            {
                return NotFound();
            }
            var usersAndRoles = new List<AspNetUserRoles>();
            //var users = _context.AspNetUsers;
            var users = _userManager.Users.ToList();
            foreach (var user in users)
            {
                var list = await _userManager.GetRolesAsync(user);
                foreach (var role in list)
                {
                    usersAndRoles.Add(new AspNetUserRoles
                    {
                        UserName = user.UserName,
                        RoleName = role
                    });
                }
            }

            return View(usersAndRoles);
        }*/

        // GET: AspNetUserRoles/Create
        public IActionResult Create()
        {
            //ViewData["RoleId"] = new SelectList(_context.AspNetRoles, "Id", "Name");
            //ViewData["RoleId"] = _roleManager.Roles.ToList();
            var list = _userManager.Users.ToList();
            //ViewData["UserId"] = new SelectList(_userManager.Users.ToList(), "Id", "Name");
            ViewData["UserId"] = new SelectList(list, "Id", "UserName");
            //ViewData["RoleId"] = new SelectList(_roleManager.Roles.ToList(), "Id", "Name", (_roleManager.Roles.ToList().Where(a => a.Name != null)));
            ViewData["RoleId"] = new SelectList(_roleManager.Roles.ToList());
            // ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Name");
            //ViewData["UserId"] = _userManager.Users.ToList();

            return View();
        }

        // POST: AspNetUserRoles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserName,RoleName")] AspNetUserRoles aspNetUserRoles)
        {
            var roleid= await _roleManager.FindByNameAsync(aspNetUserRoles.RoleName);
            aspNetUserRoles.RoleId = roleid.Id;
            //var userid = await _userManager.FindByNameAsync(aspNetUserRoles.UserName);
            aspNetUserRoles.UserId = aspNetUserRoles.UserName;
            ModelState.Remove("UserId");
            ModelState.Remove("RoleId");
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(aspNetUserRoles.UserName);
               
                await _userManager.AddToRoleAsync(user, aspNetUserRoles.RoleName);
                //_context.Add(aspNetUserRoles);
                //await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["RoleId"] = new SelectList(_context.AspNetRoles, "Id", "Id", aspNetUserRoles.RoleName);
            //ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", aspNetUserRoles.UserName);
            var list = _userManager.Users.ToList();
            
            ViewData["UserId"] = new SelectList(list, "Id", "UserName");
            
            ViewData["RoleId"] = new SelectList(_roleManager.Roles.ToList());
            return View(aspNetUserRoles);
        }
        
        // GET: AspNetUserRoles/Edit/5
       /* public async Task<IActionResult> Edit(string userId, string roleId)
        {
            if (userId == null || roleId==null)
            {
                return NotFound();
            }
          ;
            var aspNetUserRoles = new AspNetUserRoles();
            //var users = _context.AspNetUsers;
            var userName = await _userManager.FindByIdAsync(userId);
            var roleName = await _roleManager.FindByIdAsync(roleId);
            aspNetUserRoles.UserId = userId;
            aspNetUserRoles.RoleId = roleId;
            aspNetUserRoles.UserName = userName.UserName;
            aspNetUserRoles.RoleName = roleName.Name;
                    
               
                var aspNetUserRoles = await _context.AspNetUserRoles.FindAsync(id);
                if (aspNetUserRoles == null)
                {
                    return NotFound();
                }
                ViewData["RoleId"] = new SelectList(_context.AspNetRoles, "Id", "Id", aspNetUserRoles.RoleId);
                ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", aspNetUserRoles.UserId);
                return View(aspNetUserRoles);
        }

        // POST: AspNetUserRoles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("UserName,RoleName")] AspNetUserRoles aspNetUserRoles)
        {
            if (id != aspNetUserRoles.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aspNetUserRoles);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AspNetUserRolesExists(aspNetUserRoles.UserId))
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
            ViewData["RoleId"] = new SelectList(_context.AspNetRoles, "Id", "Id", aspNetUserRoles.RoleId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", aspNetUserRoles.UserId);
            return View(aspNetUserRoles);
        }*/

        // GET: AspNetUserRoles/Delete/5
        public async Task<IActionResult> Delete(string userId, string roleId)
        {
            if (userId == null || roleId==null)
            {
                return NotFound();
            }

            var aspNetUserRoles = new AspNetUserRoles();
            //var users = _context.AspNetUsers;
            var userName = await _userManager.FindByIdAsync(userId);
            var roleName = await _roleManager.FindByIdAsync(roleId);
            aspNetUserRoles.UserId = userId;
            aspNetUserRoles.RoleId = roleId;
            aspNetUserRoles.UserName = userName.UserName;
            aspNetUserRoles.RoleName = roleName.Name;

            return View(aspNetUserRoles);
        }

        // POST: AspNetUserRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string userId, string roleName)
        {
            /* var aspNetUserRoles = await _context.AspNetUserRoles.FindAsync(id);
             if (aspNetUserRoles != null)
             {
                 _context.AspNetUserRoles.Remove(aspNetUserRoles);
             }

             await _context.SaveChangesAsync();*/
            var user = await _userManager.FindByIdAsync(userId);
            await _userManager.RemoveFromRoleAsync(user, roleName);
            return RedirectToAction(nameof(Index));
        }

        /*private bool AspNetUserRolesExists(string id)
        {
            return _context.AspNetUserRoles.Any(e => e.UserId == id);
        }*/
    }
}
