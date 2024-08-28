using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ActualTeast.Models;
using Microsoft.AspNetCore.Identity;
using ActualTeast.ViewModels;

namespace ActualTeast.Controllers
{
    public class PlogsController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<User> _userManager;

        public PlogsController(ApplicationDBContext context,UserManager<User> userManager)
        {
            _userManager=userManager;
            _context=context;
        }

        // GET: Plogs
        public async Task<IActionResult> Index()
        {
            var applicationDBContext = _context.Plogs.Include(p => p.Owner);
            return View(await applicationDBContext.ToListAsync());
        }

        // GET: Plogs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if(id==null||_context.Plogs==null)
            {
                return NotFound();
            }

            var plog = await _context.Plogs
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(m => m.Id==id);
            if(plog==null)
            {
                return NotFound();
            }

            return View(plog);
        }

        // GET: Plogs/Create
        public IActionResult Create()
        {
            ViewData["Owner"]=User.Identity.Name;
            return View();
        }

        // POST: Plogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddPlogViewModel newPlog)
        {
            if(ModelState.IsValid)
            {
                Plog plog = new() {
                    PublishDate=DateTime.Now,
                    OwnerId=_userManager.GetUserId(User),
                    Content=newPlog.Content,
                    Title=newPlog.Title,

                };
                _context.Add(plog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index),"Home");
            }
            ViewData["Owner"]=User.Identity.Name;
            return View(newPlog);
        }

        // GET: Plogs/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if(id==null||_context.Plogs==null)
            {
                return NotFound();
            }

            var plog = await _context.Plogs.FindAsync(id);
            if(plog==null)
            {
                return NotFound();
            }
            ViewData["Owner"]=User.Identity.Name;
            AddPlogViewModel addPlogViewModel = new() {
                Content=plog.Content,
                Title=plog.Title,
                Id=plog.Id
            };
            return View(addPlogViewModel);
        }

        // POST: Plogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id,AddPlogViewModel newPlog)
        {
            if(id!=newPlog.Id)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                try
                {
                    Plog plog = new() {
                        Id=newPlog.Id,
                        Title=newPlog.Title,
                        PublishDate=DateTime.Now,
                        Content=newPlog.Content,
                        OwnerId=_userManager.GetUserId(User)
                    };
                    _context.Update(plog);
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException)
                {
                    if(!PlogExists(newPlog.Id))
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
            ViewData["Owner"]=User.Identity.Name;
            return View(newPlog);
        }

        // GET: Plogs/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if(id==null||_context.Plogs==null)
            {
                return NotFound();
            }

            var plog = await _context.Plogs
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(m => m.Id==id);
            if(plog==null)
            {
                return NotFound();
            }

            return View(plog);
        }

        // POST: Plogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if(_context.Plogs==null)
            {
                return Problem("Entity set 'ApplicationDBContext.Plogs'  is null.");
            }
            var plogComments =await _context.Comments.Where(c=>c.PlogId==id).ToListAsync();
            if(plogComments.Any())
                _context.RemoveRange(plogComments);
            var plogRatings = await _context.Ratings.Where(r=>r.PlogId==id).ToListAsync();
            if (plogRatings.Any())
            _context.RemoveRange(plogRatings);
            var plog = await _context.Plogs.FindAsync(id);
            if(plog!=null)
            {
                _context.Plogs.Remove(plog);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index),"Home");
        }

        private bool PlogExists(string id)
        {
            return (_context.Plogs?.Any(e => e.Id==id)).GetValueOrDefault();
        }
    }
}
