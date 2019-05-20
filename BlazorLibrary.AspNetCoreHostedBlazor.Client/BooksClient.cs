using System;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorLibrary.Shared;
using Microsoft.AspNetCore.Components;

namespace BlazorLibrary.AspNetCoreHostedBlazor.Client
{
    public class BooksClient : IBooksClient
    {
        private readonly string _baseUri = "http://localhost:55384";
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
            await _httpClient.DeleteAsync(_baseUri + "/api/library/" + id);
        }

        public async Task<PagedResult<Book>> ListBooks(int page, int pageSize)
        {
            var url = _baseUri + "/api/library?page=" + page + "&pageSize=" + pageSize;

            return await _httpClient.GetJsonAsync<PagedResult<Book>>(url);
        }

        public async Task<Book> GetBook(int id)
        {
            return await _httpClient.GetJsonAsync<Book>(_baseUri + "/api/library/" + id);
        }

        public async Task SaveBook(Book book)
        {
            var url = _baseUri + "/api/library";

            if (book.Id == 0)
            {
                await _httpClient.PostJsonAsync(url, book);
            }
            else
            {
                await _httpClient.PutJsonAsync(url, book);
            }
        }

        public async Task<PagedResult<Book>> SearchBooks(string term, int page)
        {
            await Task.FromResult(0);

            throw new NotImplementedException();
        }
    }
}
