using BlazorLibrary.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorAzure.WebApp
{
    public abstract class BlazorAzurePageBase : ComponentBase
    {
        [Inject]
        protected IUriHelper UriHelper { get; set; }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected IBooksClient BooksClient { get; set; }
    }
}