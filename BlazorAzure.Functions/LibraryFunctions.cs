using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorLibrary.Data;
using BlazorLibrary.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BlazorAzure.Functions
{
    public static class LibraryFunctions
    {
        [FunctionName("Index")]
        public static async Task<IActionResult> Index([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "Books/Index/page/{page}")]HttpRequest req, ILogger log, [FromRoute] int page)
        {
            if (page <= 0) page = 1;

            using (var context = (new BooksDbContextFactory()).CreateDbContext())
            {
                var books = await context.Books
                                         .OrderBy(b => b.Title)
                                         .GetPagedAsync(page, 10);

                return new JsonResult(books);
            }
        }

        [FunctionName("Get")]
        public static async Task<IActionResult> Get([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "Books/Get/{id}")]HttpRequest req, ILogger log, [FromRoute]int id)
        {
            using (var context = (new BooksDbContextFactory()).CreateDbContext())
            {
                var book = await context.Books.FirstOrDefaultAsync(b => b.Id == id);

                return new JsonResult(book);
            }
        }

        [FunctionName("Save")]
        public static async Task Save([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "Books/Save")]HttpRequest req, ILogger log)
        {
            using (var reader = new StreamReader(req.Body, Encoding.UTF8))
            using (var context = (new BooksDbContextFactory()).CreateDbContext())
            {
                var body = reader.ReadToEnd();
                var book = JsonConvert.DeserializeObject<Book>(body);

                context.Update(book);
                await context.SaveChangesAsync();

                //await AzureSearchClient.IndexBook(book, log);
            }
        }

        [FunctionName("Delete")]
        public static async Task Delete([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "Books/Delete/{id}")]HttpRequest req, ILogger log, [FromRoute]int id)
        {
            using (var context = (new BooksDbContextFactory()).CreateDbContext())
            {
                var book = await context.Books.FirstOrDefaultAsync(b => b.Id == id);
                if (book == null)
                {
                    return;
                }

                context.Books.Remove(book);
                await context.SaveChangesAsync();

                //await AzureSearchClient.RemoveBook(book.Id);
            }
        }

        [FunctionName("Search")]
        public static async Task<IActionResult> Search([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "Books/Search/{page}/{term}")]HttpRequest req, ILogger log, [FromRoute]string term, [FromRoute]int page)
        {
            var results = await AzureSearchClient.Search(term, page);

            return new JsonResult(results);
        }
    }
}
