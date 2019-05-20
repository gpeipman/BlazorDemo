using System.Threading.Tasks;
using BlazorLibrary.Data;
using BlazorLibrary.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlazorLibrary.AspNetCoreHostedBlazor.Server.Controllers
{
    [Route("api/[controller]")]
    public class LibraryController : Controller
    {
        private readonly BooksDbContext _dataContext;

        public LibraryController(BooksDbContext dataContext)
        {
            _dataContext = dataContext;
        }

        // GET: api/<controller>
        [HttpGet]
        public async Task<PagedResult<Book>> Get(int page=1, int pageSize=10)
        {
            return await _dataContext.Books.GetPagedAsync(page, pageSize);
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<Book> Get(int id)
        {
            var book = await _dataContext.Books.FirstOrDefaultAsync(b => b.Id == id);

            return book;
        }

        // POST api/<controller>
        [HttpPost]
        public async Task Post([FromBody]Book book)
        {
            _dataContext.Add(book);

            await _dataContext.SaveChangesAsync();
        }

        // PUT api/<controller>/5
        [HttpPut]
        public async Task Put([FromBody]Book book)
        {
            _dataContext.Update(book);

            await _dataContext.SaveChangesAsync();
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            var book = await _dataContext.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (book == null)
            {
                return;
            }

            _dataContext.Books.Remove(book);
            await _dataContext.SaveChangesAsync();
        }
    }
}