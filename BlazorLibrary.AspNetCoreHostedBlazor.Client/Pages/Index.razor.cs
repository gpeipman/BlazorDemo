using System.Threading.Tasks;
using BlazorLibrary.Shared;
using Microsoft.AspNetCore.Components;

namespace BlazorLibrary.AspNetCoreHostedBlazor.Client.Pages
{
    public class IndexModel : LibraryPageBase
    {
        [Parameter]
        protected string Page { get; set; } = "1";

        protected int DeleteId { get; set; } = 0;
        protected PagedResult<Book> Books;

        protected override async Task OnParametersSetAsync()
        {
            await LoadBooks(int.Parse(Page));
        }

        private async Task LoadBooks(int page)
        {
            Books = await BooksClient.ListBooks(page, 10);
        }

        protected void PagerPageChanged(int page)
        {
            UriHelper.NavigateTo("/page/" + page);
        }

        protected void AddNew()
        {
            UriHelper.NavigateTo("/edit/0");
        }

        protected void EditBook(int id)
        {
            UriHelper.NavigateTo("/edit/" + id);
        }

        protected async void ConfirmDelete(int id, string title)
        {
            DeleteId = id;

            await JSRuntime.InvokeAsync<bool>("blazorDemoInterop.confirmDelete", title);
        }

        protected async Task DeleteBook()
        {
            await JSRuntime.InvokeAsync<bool>("blazorDemoInterop.hideDeleteDialog");

            await BooksClient.DeleteBook(DeleteId);
            await LoadBooks(int.Parse(Page));
        }
    }
}
