using Microsoft.AspNetCore.Blazor.Hosting;

namespace BlazorDemo.AdalClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            BlazorWebAssemblyHost.CreateDefaultBuilder()
                                 .UseBlazorStartup<Startup>()
                                 .Build()
                                 .Run();
        }
    }
}