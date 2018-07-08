using System;
using System.Threading.Tasks;
using BlazorDemo.Shared;
using Microsoft.AspNetCore.Blazor.Browser.Interop;
using Microsoft.AspNetCore.Blazor.Components;

namespace BlazorDemo.AdalClient.Pages
{
    public class IndexModel : BaseComponent
    {
        [Parameter]
        protected string Page { get; set; } = "1";

        protected int DeleteId { get; set; } = 0;
        protected PagedResult<Book> Books;

        protected override void OnParametersSet()
        {
            LoadBooks(int.Parse(Page));
        }

        private void LoadBooks(int page)
        {
            Action<string> action = async (token) => 
            {
                BooksClient.Token = token;
                Books = await BooksClient.ListBooks(page);

                StateHasChanged();           
            };

            RegisteredFunction.InvokeUnmarshalled<bool>("executeWithToken", action);
        }

        protected void PagerPageChanged(int page)
        {
            UriHelper.NavigateTo("/page/" + page);
        }

        protected async Task AddNew()
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

        protected void DeleteBook()
        {
            RegisteredFunction.Invoke<bool>("hideDeleteDialog");

            Action<string> action = async (token) =>
            {
                BooksClient.Token = token;
                await BooksClient.DeleteBook(DeleteId);

                LoadBooks(int.Parse(Page));
                StateHasChanged();
            };

            RegisteredFunction.InvokeUnmarshalled<bool>("executeWithToken", action);
        }
    }
}