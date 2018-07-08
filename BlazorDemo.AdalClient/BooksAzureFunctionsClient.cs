using System.Net.Http;
using System.Threading.Tasks;
using BlazorDemo.Shared;
using Microsoft.AspNetCore.Blazor;

namespace BlazorDemo.AdalClient
{
    public class BooksAzureFunctionsClient : IBooksClient
    {
        private readonly HttpClient _httpClient;

        private const string FunctionsHost = "<Your functions host here>/api";
        private const string FunctionsKey = "<Your functions key here>";

        public string Token { get; set; }

        public BooksAzureFunctionsClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task DeleteBook(Book book)
        {
            if(!string.IsNullOrEmpty(Token))
            {
                _httpClient.DefaultRequestHeaders.Remove("Authorization");
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
            }
            await DeleteBook(book.Id);
        }

        public async Task DeleteBook(int id)
        {
            if (!string.IsNullOrEmpty(Token))
            {
                _httpClient.DefaultRequestHeaders.Remove("Authorization");
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
            }

            var url = FunctionsHost + "/Books/Delete/" + id + "?code=" + FunctionsKey;

            await _httpClient.PostAsync(url, null);
        }

        public async Task<PagedResult<Book>> ListBooks(int page)
        {
            if(!string.IsNullOrEmpty(Token))
            {
                _httpClient.DefaultRequestHeaders.Remove("Authorization");
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
            }

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
            if (!string.IsNullOrEmpty(Token))
            {
                _httpClient.DefaultRequestHeaders.Remove("Authorization");
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
            }

            var url = FunctionsHost + "/Books/Save" + "?code=" + FunctionsKey;

            await _httpClient.PostJsonAsync<Book>(url, book);
        }

        public async Task<PagedResult<Book>> SearchBooks(string term)
        {
            if (!string.IsNullOrEmpty(Token))
            {
                _httpClient.DefaultRequestHeaders.Remove("Authorization");
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
            }

            var url = FunctionsHost + "/Books/Search/1/" + term + "?code=" + FunctionsKey;

            return await _httpClient.GetJsonAsync<PagedResult<Book>>(url);
        }
    }
}
