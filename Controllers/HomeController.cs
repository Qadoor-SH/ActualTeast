using System.Diagnostics;
using ActualTeast.Models;
using ActualTeast.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ActualTeast.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDBContext _context;

        public HomeController(ApplicationDBContext context,ILogger<HomeController> logger)
        {
            _context = context;
            _logger=logger;
        }

        public async Task< IActionResult > Index(string sortOrder,
    string currentFilter,
    string searchString,
    int? pageNumber)
        {
            ViewData["CurrentSort"]=sortOrder;
            ViewData["NameSortParm"]=String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["DateSortParm"]=sortOrder=="Date" ? "date_desc" : "Date";
            if(searchString!=null)
            {
                pageNumber=1;
            }
            else
            {
                searchString=currentFilter;
            }

            ViewData["CurrentFilter"]=searchString;

            var users = from s in _context.Plogs.Include(p => p.Ratings)
                .Include(p => p.Owner)
                .Include(p => p.Comments)
                .ThenInclude(c => c.Commenter)
                        select s;
            if(!String.IsNullOrEmpty(searchString))
            {
                    users=users.Where(s => s.Title.Contains(searchString)||s.Content.Contains(searchString));
            }
            switch(sortOrder)
            {
                case "title_desc":
                users=users.OrderByDescending(s => s.Title);
                break;
                case "date":
                users=users.OrderBy(s => s.PublishDate);
                break;
                case "date_desc":
                users=users.OrderByDescending(s => s.PublishDate);
                break;
                default:
                users=users.OrderBy(s => s.Title);
                break;
            }

            int pageSize = 4;
            return View(await PaginatedList<Plog>.CreateAsync(source: users.AsNoTracking(),pageNumber??1,pageSize));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0,Location = ResponseCacheLocation.None,NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId=Activity.Current?.Id??HttpContext.TraceIdentifier });
        }
    }
}