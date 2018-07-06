using System;
using System.Threading.Tasks;

namespace BlazorDemo.AdalClient
{
    public static class AdalHelper
    {
        public static async Task RunAction(Action<string> action, string token)
        {
            await Task.Run(() => action(token));
        }
    }
}