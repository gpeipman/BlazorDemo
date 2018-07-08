using System;
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
            return new SearchIndexClient("<seach service name>", "<index name>", new SearchCredentials("<key>"));
        }

        public static PagedResult<Book> Search(string term, int page)
        {
            var searchParams = new SearchParameters();
            searchParams.IncludeTotalResultCount = true;
            searchParams.Skip = (page - 1) * 10;
            searchParams.Top = 10;

            using (var client = GetClient())
            {
                var results = client.Documents.Search<Book>(term, searchParams);
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

        public static void IndexBook(Book book, TraceWriter log)
        {
            log.Warning("Saving book: " + book.Title);

            using (var client = GetClient())
            {
                var azureBook = new { id = book.Id.ToString(), Title = book.Title, ISBN = book.ISBN };
                var batch = IndexBatch.MergeOrUpload(new [] { azureBook });

                var result = client.Documents.Index(batch).Results[0];
                log.Warning(result.Key + ": " + result.ErrorMessage);
            }
        }
    }
}
