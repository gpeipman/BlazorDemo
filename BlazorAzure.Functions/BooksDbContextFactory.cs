using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BlazorLibrary.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BlazorAzure.Functions
{
    public class BooksDbContextFactory : IDesignTimeDbContextFactory<BooksDbContext>
    {
        private static string _connectionString;

        public BooksDbContext CreateDbContext()
        {
            return CreateDbContext(null);
        }

        public BooksDbContext CreateDbContext(string[] args)
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                LoadConnectionString();
            }

            var builder = new DbContextOptionsBuilder<BooksDbContext>();
            builder.UseSqlServer(_connectionString);

            return new BooksDbContext(builder.Options);
        }

        private static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    LoadConnectionString();
                }

                return _connectionString;
            }
        }

        private static void LoadConnectionString()
        {
            var builder = new ConfigurationBuilder();
            var settingsPath = Path.Combine(Environment.CurrentDirectory, "local.settings.json");

            builder.AddJsonFile(settingsPath, optional: true);
            builder.AddEnvironmentVariables();

            var configuration = builder.Build();

            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
    }
}
