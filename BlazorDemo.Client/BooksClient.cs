using System;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorDemo.Shared;
using Microsoft.AspNetCore.Blazor;

namespace BlazorDemo.Client
{
    public class BooksClient : IBooksClient
    {
        private readonly HttpClient _httpClient;
        private readonly string ApiHost;

        private const bool UseWithFunctions = true;

        public BooksClient(HttpClient httpClient)
        {
            _httpClient = httpClient;

            if(UseWithFunctions)
            {
                ApiHost = "http://sme1:7071/api";
            }
        }

        public async Task DeleteBook(Book book)
        {
            await DeleteBook(book.Id);
        }

        public async Task DeleteBook(int id)
        {
            await _httpClient.PostAsync(ApiHost + "/Books/Delete/" + id, null);
        }

        public async Task<PagedResult<Book>> ListBooks(int page)
        {
            var url = ApiHost + "/Books/Index/page/" + page;
            Console.WriteLine("URL: " + url);
            return await _httpClient.GetJsonAsync<PagedResult<Book>>(ApiHost + "/Books/Index/page/" + page);
        }

        public async Task<Book> GetBook(int id)
        {
            return await _httpClient.GetJsonAsync<Book>(ApiHost + "/Books/Get/" + id);
        }

        public async Task SaveBook(Book book)
        {
            try
            {
                var url = ApiHost + "/Books/Save";
                Console.WriteLine("URL: " + url);

                await _httpClient.PostJsonAsync<Book>(url, book);
            }
            catch(Exception ex)
            {
                Console.WriteLine(book);
                Console.WriteLine(ex);
            }
        }
    }
}
