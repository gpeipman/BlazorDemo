using BlazorLibrary.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorLibrary.AspNetCoreHostedBlazor.Client
{
    public abstract class LibraryPageBase : ComponentBase
    {
        [Inject]
        protected IUriHelper UriHelper { get; set; }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected IBooksClient BooksClient { get; set; }
    }
}