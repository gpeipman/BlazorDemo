using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorDemo.Data;
using BlazorDemo.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace BlazorDemo.AzureFunctionsBackend
{
    public static class Functions
    {
        [FunctionName("Index")]
        public static IActionResult Index([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "Books/Index/page/{page}")]HttpRequest req, TraceWriter log, [FromRoute] int page)
        {
            if (page <= 0) page = 1;

            using (var context = (new BooksDbContextFactory()).CreateDbContext())
            {
                var books = context.Books
                                   .OrderBy(b => b.Title)
                                   .GetPaged(page, 10);

                return new JsonResult(books);
            }
        }

        [FunctionName("Get")]
        public static IActionResult Get([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "Books/Get/{id}")]HttpRequest req, TraceWriter log, [FromRoute]int id)
        {
            using (var context = (new BooksDbContextFactory()).CreateDbContext())
            {
                var book = context.Books.FirstOrDefault(b => b.Id == id);

                return new JsonResult(book);
            }
        }

        [FunctionName("Save")]
        public static async Task Save([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "Books/Save")]HttpRequest req, TraceWriter log)
        {
            using (var reader = new StreamReader(req.Body, Encoding.UTF8))
            using (var context = (new BooksDbContextFactory()).CreateDbContext())
            {
                var body = reader.ReadToEnd();
                var book = JsonConvert.DeserializeObject<Book>(body);

                context.Update(book);
                context.SaveChanges();

                await AzureSearchClient.IndexBook(book, log);
            }
        }

        [FunctionName("Delete")]
        public static async Task Delete([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "Books/Delete/{id}")]HttpRequest req, TraceWriter log, [FromRoute]int id)
        {
            using (var context = (new BooksDbContextFactory()).CreateDbContext())
            {
                var book = context.Books.FirstOrDefault(b => b.Id == id);
                if (book == null)
                {
                    return;
                }

                context.Books.Remove(book);
                await context.SaveChangesAsync();

                await AzureSearchClient.RemoveBook(book.Id);
            }
        }

        [FunctionName("Search")]
        public static async Task<IActionResult> Search([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "Books/Search/{page}/{term}")]HttpRequest req, TraceWriter log, [FromRoute]string term, [FromRoute]int page)
        {
            var results = await AzureSearchClient.Search(term, page);

            return new JsonResult(results);
        }
    }
}