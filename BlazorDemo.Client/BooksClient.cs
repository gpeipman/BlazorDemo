using System.Net.Http;
using System.Threading.Tasks;
using BlazorDemo.Shared;
using Microsoft.AspNetCore.Blazor;

namespace BlazorDemo.Client
{
public class BooksClient : IBooksClient
{
    private readonly HttpClient _httpClient;

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
        await _httpClient.PostAsync("/Books/Delete/" + id, null);
    }

    public async Task<PagedResult<Book>> ListBooks(int page)
    {
        return await _httpClient.GetJsonAsync<PagedResult<Book>>("/Books/Index/page/" + page);
    }

    public async Task<Book> GetBook(int id)
    {
        return await _httpClient.GetJsonAsync<Book>("/Books/Get/" + id);
    }

    public async Task SaveBook(Book book)
    {
        await _httpClient.PostJsonAsync("/Books/Save", book);
    }
}
}
