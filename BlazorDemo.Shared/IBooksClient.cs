using System.Threading.Tasks;

namespace BlazorDemo.Shared
{
    public interface IBooksClient
    {
        Task<PagedResult<Book>> ListBooks(int page);
        Task<Book> GetBook(int id);
        Task SaveBook(Book book);
        Task DeleteBook(Book book);
        Task DeleteBook(int id);
        Task<PagedResult<Book>> SearchBooks(string term);
        string Token { get; set; }
    }
}
