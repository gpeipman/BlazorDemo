using System;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorDemo.Shared;
using Microsoft.AspNetCore.Components;

namespace BlazorDemo.Client
{
    public class BooksClient : IBooksClient
    {
        private readonly string _baseUri = "http://localhost:20497";
        private readonly HttpClient _httpClient;

        public string Token { get; set; }

        public BooksClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task DeleteBook(Book book)
        {
            await DeleteBook(book.Id);
        }

        public async Task DeleteBook(int id)
        {
            await _httpClient.PostAsync(_baseUri + "/Books/Delete/" + id, null);
        }

        public async Task<PagedResult<Book>> ListBooks(int page)
        {
            var url = _baseUri + "/Books/Index/page/" + page;

            return await _httpClient.GetJsonAsync<PagedResult<Book>>(url);
        }

        public async Task<Book> GetBook(int id)
        {
            return await _httpClient.GetJsonAsync<Book>(_baseUri + "/Books/Get/" + id);
        }

        public async Task SaveBook(Book book)
        {
            var url = _baseUri + "/Books/Save";

            await _httpClient.PostJsonAsync(url, book);
        }

        public async Task<PagedResult<Book>> SearchBooks(string term, int page)
        {
            await Task.FromResult(0);

            throw new NotImplementedException();
        }
    }
}