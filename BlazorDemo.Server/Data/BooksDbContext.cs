using BlazorDemo.Shared;
using Microsoft.EntityFrameworkCore;

namespace BlazorDemo.Server.Data
{
    public class BooksDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }

        public BooksDbContext(DbContextOptions<BooksDbContext> options) : base(options)
        {
        }        
    }
}
