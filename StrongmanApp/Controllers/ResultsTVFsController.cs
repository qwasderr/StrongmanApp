using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataTables.AspNetCore.Mvc.Binder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
//using StrongmanApp.Context;
using StrongmanApp.Models;

namespace StrongmanApp.Controllers
{
    public class ResultsTVFsController : Controller
    {
        private readonly SportDbContext _context;

        public ResultsTVFsController(SportDbContext context)
        {
            _context = context;
        }
        [HttpGet()]
        [Route("api/value/{compId}")]
        public async Task<IActionResult> Get([DataTablesRequest] DataTablesRequest dataRequest, int compId)
        {
            //IEnumerable<Product> products = Products.GetProducts();
            //int recordsTotal = products.Count();
            //int recordsFilterd = recordsTotal;

            //if (!string.IsNullOrEmpty(dataRequest.Search?.Value))
            //{
            //    products = products.Where(e => e.Name.Contains(dataRequest.Search.Value));
            //    recordsFilterd = products.Count();
            //}
            //products = products.Skip(dataRequest.Start).Take(dataRequest.Length);
            var lineup = await _context.Lineups.Where(a => a.CompetitionId == compId && a.IsConfirmed == 1).ToListAsync();
            List<AthlTVF> athlTVF = new List<AthlTVF>();
            foreach (var athlete in lineup)
            {
                AthlTVF athl = new AthlTVF();
                athl.athlete_ID = athlete.UserId;
                athl.athlete_name = _context.Users.Where(a => a.Id == athlete.UserId).First().Name;
                List<EventsTVF> events = new List<EventsTVF>();
                var events2 = _context.CompetitionEvents.Where(a => a.CompetitionId == compId).ToList();
                float? tot_pts = 0;
                foreach (var event1 in events2)
                {
                    EventsTVF eventsTVF1 = new EventsTVF();
                    eventsTVF1.EventName = _context.Events.Where(a => a.Id == event1.EventId).First().Name;
                    var res = _context.Results.Where(a => a.EventId == event1.EventId && a.UserId == athlete.UserId).FirstOrDefault();
                    if (res != null)
                    {
                        eventsTVF1.EventRes = res.Points;
                        eventsTVF1.Result = res.Result1;
                        tot_pts += res.Points;
                    }
                    events.Add(eventsTVF1);
                }
                athl.Events = events;

                athl.total_pts = tot_pts;
                athlTVF.Add(athl);
            }
            return Json(athlTVF.ToArray().ToDataTablesResponse(dataRequest));


            /* var res = _context.ResultsTVF(compId).ToList();
             if (res.Count == 0)
             {
                 var lineup =  await _context.Lineups.Where(a => a.CompetitionId == compId && a.IsConfirmed==1).ToListAsync();
                 List<AthlTVF> athlTVF = new List<AthlTVF>();
                 foreach (var athlete in lineup)
                 {
                     AthlTVF athl = new AthlTVF();
                     athl.athlete_ID = athlete.UserId;
                     athl.athlete_name = _context.Users.Where(a => a.Id == athlete.UserId).First().Name;
                     List<EventsTVF> events = new List<EventsTVF>();
                     var events2=_context.CompetitionEvents.Where(a=>a.CompetitionId==compId).ToList();
                     foreach(var event1 in events2)
                     {
                         EventsTVF eventsTVF1 = new EventsTVF();
                         eventsTVF1.EventName=_context.Events.Where(a=>a.Id==event1.EventId).First().Name;
                         events.Add(eventsTVF1);
                     }
                     athl.Events = events;
                     athlTVF.Add(athl);
                 }
                 return Json(athlTVF.ToArray().ToDataTablesResponse(dataRequest));
             }
             else
             {
                 var events_count = _context.CompetitionEvents.Where(a => a.CompetitionId == compId).Count();

                     List<AthlTVF> athlTVF = new List<AthlTVF>();
                     int pos = 0, posres = 0;
                     while (posres < res.Count)
                     {
                         if (posres % events_count == 0)
                         {
                             AthlTVF athl = new AthlTVF();
                             athl.athlete_ID = res[posres].athlete_ID;
                             athl.athlete_name = res[posres].athlete_name;
                             List<EventsTVF> events = new List<EventsTVF>();
                             EventsTVF eventsTVF1 = new EventsTVF();
                             eventsTVF1.EventName = res[posres].event_name;
                             eventsTVF1.EventRes = res[posres].event_points;
                             eventsTVF1.Result = res[posres].event_result;
                             events.Add(eventsTVF1);
                             athl.total_pts = res[posres].total_pts;
                             ++posres;
                             while (posres % events_count != 0 && posres<res.Count)
                             {
                                 EventsTVF eventsTVF = new EventsTVF();
                                 eventsTVF.EventName = res[posres].event_name;
                                 eventsTVF.EventRes = res[posres].event_points;
                                 eventsTVF.Result = res[posres].event_result;
                                 events.Add(eventsTVF);

                                 ++posres;

                             }
                             athl.Events = events;
                             athlTVF.Add(athl);
                         }
                     }
                     return Json(athlTVF.ToArray().ToDataTablesResponse(dataRequest));*/

        }
        /* return Json(res
             .Select(e => new
             {
                 Id = e.ID,
                 Events=
                 Name = e.Name,
                 Created = e.Created,
                 Salary = e.Salary,
                 Position = e.Position,
                 Office = e.Office
             })
             .ToDataTablesResponse(dataRequest, recordsTotal, recordsFilterd));*/


        // GET: ResultsTVFs
        public async Task<IActionResult> Index(int compId)
        {
            var res = _context.ResultsTVF(compId).ToList();
            if (res.Count == 0)
            {
                var athletes = _context.Lineups.Where(a => a.CompetitionId == compId && a.IsConfirmed == 1).ToList();
                foreach (var athlete in athletes)
                {
                    ResultsTVF athlTVF = new ResultsTVF();
                    athlTVF.athlete_ID = athlete.UserId;
                    athlTVF.athlete_name = _context.Users.Where(a => a.Id == athlete.UserId).FirstOrDefault().Name;
                    res.Add(athlTVF);
                }
            }
            var res2 = _context.CompetitionEvents.Where(a => a.CompetitionId == compId).Include(a => a.Competition).Include(a => a.Event);
            //ViewData["EventsId"] = new SelectList(_context.CompetitionEvents.Where(a=>a.CompetitionId==5).Include(a=>a.Competition).Include(a=>a.Event), "Id","Name", _context.CompetitionEvents.Where(a => a.CompetitionId == 5).Include(a => a.Competition).Include(a => a.Event));
            ViewData["EventsId"] = res2;
            ViewData["CompId"] = compId;
            return View(res);
        }

        // GET: ResultsTVFs/Details/5
        /*public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resultsTVF = await _context.ResultsTVF_1
                .FirstOrDefaultAsync(m => m.ID == id);
            if (resultsTVF == null)
            {
                return NotFound();
            }

            return View(resultsTVF);
        }

        // GET: ResultsTVFs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ResultsTVFs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,athlete_ID,athlete_name,event_name,event_result,event_points,total_pts")] ResultsTVF resultsTVF)
        {
            if (ModelState.IsValid)
            {
                _context.Add(resultsTVF);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(resultsTVF);
        }

        // GET: ResultsTVFs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resultsTVF = await _context.ResultsTVF_1.FindAsync(id);
            if (resultsTVF == null)
            {
                return NotFound();
            }
            return View(resultsTVF);
        }

        // POST: ResultsTVFs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,athlete_ID,athlete_name,event_name,event_result,event_points,total_pts")] ResultsTVF resultsTVF)
        {
            if (id != resultsTVF.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(resultsTVF);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResultsTVFExists(resultsTVF.ID))
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
            return View(resultsTVF);
        }

        // GET: ResultsTVFs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resultsTVF = await _context.ResultsTVF_1
                .FirstOrDefaultAsync(m => m.ID == id);
            if (resultsTVF == null)
            {
                return NotFound();
            }

            return View(resultsTVF);
        }

        // POST: ResultsTVFs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var resultsTVF = await _context.ResultsTVF_1.FindAsync(id);
            if (resultsTVF != null)
            {
                _context.ResultsTVF_1.Remove(resultsTVF);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ResultsTVFExists(int id)
        {
            return _context.ResultsTVF_1.Any(e => e.ID == id);
        }*/
    }
}

