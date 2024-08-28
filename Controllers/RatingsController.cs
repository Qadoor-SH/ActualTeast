using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ActualTeast.Models;
using ActualTeast.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace ActualTeast.Controllers
{
    public class RatingsController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<User> _userManager;

        public RatingsController(ApplicationDBContext context,UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Ratings
        public async Task<IActionResult> Index()
        {
            var applicationDBContext = _context.Ratings.Include(p => p.Plog).Include(p => p.User);
            return View(await applicationDBContext.ToListAsync());
        }

        // GET: Ratings/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Ratings == null)
            {
                return NotFound();
            }

            var plogRatings = await _context.Ratings
                .Include(p => p.Plog)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (plogRatings == null)
            {
                return NotFound();
            }

            return View(plogRatings);
        }

        // GET: Ratings/Create
        public async Task<IActionResult> Create(string id)
        {
            if(id==null||_context.Ratings==null)
            {
                return NotFound();
            }
            var plog = await _context.Plogs.Where(p => p.Id==id)
                .Include(p => p.Owner)
                .Include(p => p.Ratings)
                .FirstOrDefaultAsync();
            if(plog==null)
                return NotFound();
            ViewData["Plog"] =plog;
            ViewData["User"] =User.Identity.Name;
            return View();
        }

        // POST: Ratings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string id,RateingPlogViewModel newPlogRatings)
        {
            if(id != newPlogRatings.PlogId)
            {
                return NotFound();
            }
            PlogRatings plogRatings = new() {
                PlogId = id,
                Rating = newPlogRatings.Rating,
                UserId = _userManager.GetUserId(User)
            } ;
            if (ModelState.IsValid)
            {
                var rate = await _context.Ratings.Where(r => r.UserId==plogRatings.UserId&&plogRatings.PlogId==r.PlogId).FirstOrDefaultAsync();
                if(rate==null)
                {

                _context.Add(plogRatings);
                }
                else
                {
                    rate.Rating=plogRatings.Rating;
                    _context.Update(rate);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index),"Home");
            }
            var plog = await _context.Plogs.Where(p => p.Id==id)
                .Include(p => p.Owner)
                .Include(p => p.Ratings)
                .FirstOrDefaultAsync();
            if(plog==null)
                return NotFound();
            ViewData["Plog"]=plog;
            ViewData["User"]=User.Identity.Name;
            return View(plogRatings);
        }

        // GET: Ratings/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Ratings == null)
            {
                return NotFound();
            }

            var plogRatings = await _context.Ratings.FindAsync(id);
            if (plogRatings == null)
            {
                return NotFound();
            }
            ViewData["PlogId"] = new SelectList(_context.Plogs, "Id", "Id", plogRatings.PlogId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", plogRatings.UserId);
            return View(plogRatings);
        }

        // POST: Ratings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("UserId,PlogId,Rating")] PlogRatings plogRatings)
        {
            if (id != plogRatings.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(plogRatings);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlogRatingsExists(plogRatings.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index),"Home");
            }
            ViewData["PlogId"] = new SelectList(_context.Plogs, "Id", "Id", plogRatings.PlogId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", plogRatings.UserId);
            return View(plogRatings);
        }

        // GET: Ratings/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Ratings == null)
            {
                return NotFound();
            }

            var plogRatings = await _context.Ratings
                .Include(p => p.Plog)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (plogRatings == null)
            {
                return NotFound();
            }

            return View(plogRatings);
        }

        // POST: Ratings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Ratings == null)
            {
                return Problem("Entity set 'ApplicationDBContext.Ratings'  is null.");
            }
            var plogRatings = await _context.Ratings.FindAsync(id);
            if (plogRatings != null)
            {
                _context.Ratings.Remove(plogRatings);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlogRatingsExists(string id)
        {
          return (_context.Ratings?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
