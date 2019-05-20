using System.Threading.Tasks;

namespace BlazorLibrary.Shared
{
    public interface IBooksClient
    {
        Task<PagedResult<Book>> ListBooks(int page, int pageSize);
        Task<Book> GetBook(int id);
        Task SaveBook(Book book);
        Task DeleteBook(Book book);
        Task DeleteBook(int id);
        Task<PagedResult<Book>> SearchBooks(string term, int page);
        string Token { get; set; }
    }
}
