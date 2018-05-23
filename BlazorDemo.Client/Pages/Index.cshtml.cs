using System;
using System.Threading.Tasks;
using BlazorDemo.Shared;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Browser.Interop;
using Microsoft.AspNetCore.Blazor.Components;

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
            Console.WriteLine("Current page: " + Page);
            await LoadBooks(int.Parse(Page));
        }

        private async Task LoadBooks(int page)
        {
            //Books = await Http.GetJsonAsync<PagedResult<Book>>("/Books/Index/page/" + page);
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

        protected void ConfirmDelete(int id, string title)
        {
            DeleteId = id;
            RegisteredFunction.Invoke<bool>("confirmDelete", title);
        }

        protected async Task DeleteBook()
        {
            RegisteredFunction.Invoke<bool>("hideDeleteDialog");

            await BooksClient.DeleteBook(DeleteId);
            await LoadBooks(int.Parse(Page));
        }
    }
}