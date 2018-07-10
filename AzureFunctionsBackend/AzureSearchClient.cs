using System;
using System.Threading.Tasks;
using BlazorDemo.Shared;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Azure.WebJobs.Host;

namespace BlazorDemo.AzureFunctionsBackend
{
    
    public static class AzureSearchClient
    {
        private static SearchIndexClient GetClient()
        {
            return new SearchIndexClient("<service name>", "books", new SearchCredentials("<service key>"));
        }

        public static async Task<PagedResult<Book>> Search(string term, int page)
        {
            var searchParams = new SearchParameters();
            searchParams.IncludeTotalResultCount = true;
            searchParams.Skip = (page - 1) * 10;
            searchParams.Top = 10;
            searchParams.OrderBy = new[] { "Title" };

            using (var client = GetClient())
            {
                var results = await client.Documents.SearchAsync<Book>(term, searchParams);
                var paged = new PagedResult<Book>();
                paged.CurrentPage = page;
                paged.PageSize = 10;
                paged.RowCount = (int)results.Count;
                paged.PageCount = (int)Math.Ceiling((decimal)paged.RowCount / 10);

                foreach (var result in results.Results)
                {
                    paged.Results.Add(result.Document);
                }

                return paged;
            }
        }

        public static async Task IndexBook(Book book, TraceWriter log)
        {
            using (var client = GetClient())
            {
                var azureBook = new { id = book.Id.ToString(), Title = book.Title, ISBN = book.ISBN };
                var batch = IndexBatch.MergeOrUpload(new [] { azureBook });

                await client.Documents.IndexAsync(batch);
            }
        }

        public static async Task RemoveBook(int id)
        {
            using (var client = GetClient())
            {
                var batch = IndexBatch.Delete("id", new[] { id.ToString() });

                await client.Documents.IndexAsync(batch);
            }
        }
    }
}
