using System;
using BlazorLibrary.Shared;
using Microsoft.EntityFrameworkCore;

namespace BlazorLibrary.Data
{
    public class BooksDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }

        public BooksDbContext(DbContextOptions<BooksDbContext> options) : base(options)
        {
        }
    }
}
