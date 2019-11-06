using BlazorLibrary.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorLibrary.ServerSideBlazor
{
    public abstract class LibraryPageBase : ComponentBase
    {
        [Inject]
        protected NavigationManager UriHelper { get; set; }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected BooksDbContext DataContext { get; set; }
    }
}
