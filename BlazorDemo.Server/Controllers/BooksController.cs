using System.Linq;
using System.Threading.Tasks;
using BlazorDemo.Data;
using BlazorDemo.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorDemo.Server.Controllers
{
    public class BooksController : Controller
    {
        private readonly BooksDbContext _context;

        public BooksController(BooksDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromRoute] int page)
        {
            var books = await _context.Books
                                      .OrderBy(b => b.Title)
                                      .GetPagedAsync(page, 10);
            return Json(books);
        }

        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);

            return Json(book);
        }

        [HttpPost]
        public async Task Save([FromBody] Book book)
        {
            _context.Update(book);

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            if(book == null)
            {
                return;
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
    }
}