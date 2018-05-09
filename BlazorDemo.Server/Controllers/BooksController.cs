using System.Linq;
using System.Threading.Tasks;
using BlazorDemo.Server.Data;
using BlazorDemo.Shared;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Index([FromRoute] int page)
        {
            var books = _context.Books
                                .OrderBy(b => b.Title)
                                .GetPaged(page, 10);
            return Json(books);
        }

        public IActionResult Get([FromRoute] int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id);

            return Json(book);
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] Book book)
        {
            _context.Update(book);

            await _context.SaveChangesAsync();

            return Json("");
        }
    }
}