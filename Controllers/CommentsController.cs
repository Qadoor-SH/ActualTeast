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
    public class CommentsController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<User> _userManager;

        public CommentsController(ApplicationDBContext context,UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Comments
        public async Task<IActionResult> Index()
        {
            var applicationDBContext = _context.Comments.Include(c => c.Commenter).Include(c => c.Plog);
            return View(await applicationDBContext.ToListAsync());
        }

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Comments == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.Commenter)
                .Include(c => c.Plog).ThenInclude(c=>c.Ratings)
                .Include(c => c.Plog).ThenInclude(c=>c.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // GET: Comments/Create
        public async Task<IActionResult> Create(string id)
        {
            if(id==null||_context.Plogs==null)
            {
                return NotFound();
            }
            var plog = await _context.Plogs.Where(p=>p.Id == id)
                .Include(p => p.Owner)
                .Include(p => p.Ratings)
                .FirstOrDefaultAsync();
            if (plog == null)
                return NotFound();
            ViewData["Commenter"] = User.Identity.Name;
            ViewData["Plog"] = plog;
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string id, AddCommentViewModel newComment)
        {
            if (ModelState.IsValid)
            {
                var comment = new Comment() {
                    CommentDate=DateTime.Now,
                    CommenterId=_userManager.GetUserId(User),
                    PlogId=newComment.PlogId,
                    Text=newComment.Text
                };
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index),"Home");
            }
            var plog = await _context.Plogs.Where(p => p.Id==id)
                .Include(p => p.Owner)
                .Include(p => p.Ratings)
                .FirstOrDefaultAsync();
            if(plog==null)
                return NotFound();
            ViewData["Commenter"]=User.Identity.Name;
            ViewData["Plog"]=plog;
            return View(newComment);
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Comments == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments.Where(p=>p.Id==id)
                .Include(c => c.Plog).ThenInclude(c => c.Ratings)
                .Include(c => c.Plog).ThenInclude(c => c.Owner)
                .Include(c=>c.Commenter)
                .FirstOrDefaultAsync();
            if (comment == null)
            {
                return NotFound();
            }
            if(comment.Plog==null)
                return NotFound();
            ViewData["Commenter"]=User.Identity.Name;
            ViewData["Plog"]=comment.Plog;
            var commentView = new AddCommentViewModel() {
                Id=id,
                PlogId=comment.PlogId,
                Text=comment.Text
            };
            return View(commentView);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id,AddCommentViewModel newComment)
        {
            if (id != newComment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var comment = new Comment() {
                        Id = id,
                        CommentDate=DateTime.Now,
                        CommenterId=_userManager.GetUserId(User),
                        PlogId=newComment.PlogId,
                        Text=newComment.Text
                    };
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(newComment.Id))
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
            var currentComment = await _context.Comments.Where(p => p.Id==id)
                .Include(p => p.Plog)
                .ThenInclude(p=>p.Owner)
                .FirstOrDefaultAsync();
            if(currentComment?.Plog==null)
                return NotFound();
            ViewData["Commenter"]=User.Identity.Name;
            ViewData["Plog"]=currentComment.Plog;
            return View(newComment);
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Comments == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.Commenter)
                .Include(c => c.Plog)
                .ThenInclude(p => p.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Comments == null)
            {
                return Problem("Entity set 'ApplicationDBContext.Comments'  is null.");
            }
            var comment = await _context.Comments.FindAsync(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index),"Home");
        }

        private bool CommentExists(string id)
        {
          return (_context.Comments?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
