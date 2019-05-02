using System.Net.Http;
using System.Threading.Tasks;
using BlazorDemo.Shared;
using Microsoft.AspNetCore.Components;

namespace BlazorDemo.Client
{
    public class BooksAzureFunctionsClient : IBooksClient
    {
        private readonly HttpClient _httpClient;

        private const string FunctionsHost = "https://*****.azurewebsites.net/api";
        private const string FunctionsKey = "<YOUR FUNCTIONS KEY>";

        public string Token { get; set; }

        public BooksAzureFunctionsClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task DeleteBook(Book book)
        {
            await DeleteBook(book.Id);
        }

        public async Task DeleteBook(int id)
        {
            var url = FunctionsHost + "/Books/Delete/" + id + "?code=" + FunctionsKey;

            await _httpClient.PostAsync(url, null);
        }

        public async Task<PagedResult<Book>> ListBooks(int page)
        {
            var url = FunctionsHost + "/Books/Index/page/" + page + "?code=" + FunctionsKey;

            return await _httpClient.GetJsonAsync<PagedResult<Book>>(url);
        }

        public async Task<Book> GetBook(int id)
        {
            var url = FunctionsHost + "/Books/Get/" + id + "?code=" + FunctionsKey;

            return await _httpClient.GetJsonAsync<Book>(url);
        }

        public async Task SaveBook(Book book)
        {
            var url = FunctionsHost + "/Books/Save" + "?code=" + FunctionsKey;

            await _httpClient.PostJsonAsync<Book>(url, book);
        }

        public async Task<PagedResult<Book>> SearchBooks(string term, int page)
        {
            if (!string.IsNullOrEmpty(Token))
            {
                _httpClient.DefaultRequestHeaders.Remove("Authorization");
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
            }

            var url = FunctionsHost + "/Books/Search/" + page + "/" + term + "?code=" + FunctionsKey;
            url += "&term=" + term;

            return await _httpClient.GetJsonAsync<PagedResult<Book>>(url);
        }
    }
}