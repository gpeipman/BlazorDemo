using System;
using System.Threading.Tasks;
using BlazorDemo.Shared;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.JSInterop;

namespace BlazorDemo.Client.Pages
{
    public class IndexModel : BaseComponent
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
            Books = await BooksClient.ListBooks(page);
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

            await JSRuntime.Current.InvokeAsync<bool>("blazorDemoInterop.confirmDelete", title);
        }

        protected async Task DeleteBook()
        {
            await JSRuntime.Current.InvokeAsync<bool>("blazorDemoInterop.hideDeleteDialog");

            await BooksClient.DeleteBook(DeleteId);
            await LoadBooks(int.Parse(Page));
        }
    }
}