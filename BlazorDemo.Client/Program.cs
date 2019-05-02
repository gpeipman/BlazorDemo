using BlazorDemo.Shared;
using Microsoft.AspNetCore.Blazor.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorDemo.Client
{
    public class Program
    {
        //static void Main(string[] args)
        //{
        //    //var serviceProvider = new BrowserServiceProvider(services =>
        //    //{
        //    //    services.AddSingleton<IBooksClient, BooksClient>();
        //    //    //services.AddSingleton<IBooksClient, BooksAzureFunctionsClient>();
        //    //});

        //    //new BrowserRenderer(serviceProvider).AddComponent<App>("app");
        //}

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IWebAssemblyHostBuilder CreateHostBuilder(string[] args) =>
            BlazorWebAssemblyHost.CreateDefaultBuilder()
                .UseBlazorStartup<Startup>();
    }
}