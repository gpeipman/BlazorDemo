using System;
using System.Threading.Tasks;
using BlazorDemo.Shared;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.JSInterop;

namespace BlazorDemo.AdalClient.Pages
{
    public class IndexModel : BaseComponent
    {
        [Parameter]
        protected string Page { get; set; } = "1";

        protected int DeleteId { get; set; } = 0;
        protected PagedResult<Book> Books { get; set; }
        [Parameter]
        protected string SearchTerm { get; set; }

        protected override async void OnParametersSet()
        {
            await SearchClick();
        }

        private async Task LoadBooks(int page)
        {
            Action<string> action = async (token) => 
            {
                BooksClient.Token = token;
                Books = await BooksClient.ListBooks(page);

                StateHasChanged();           
            };

            await JSRuntime.Current.InvokeAsync<bool>("blazorDemoInterop.executeWithToken", action);
        }

        protected void PagerPageChanged(int page)
        {
            if (string.IsNullOrEmpty(SearchTerm))
            {
                UriHelper.NavigateTo("/page/" + page);
            }
            else
            {
                UriHelper.NavigateTo("/page/" + page + "/" + SearchTerm);
            }
        }

        protected void AddNew()
        {
            UriHelper.NavigateTo("/edit/0");
        }

        protected async Task SearchClick()
        {
            if(string.IsNullOrEmpty(SearchTerm))
            {
                await LoadBooks(int.Parse(Page));
                return;
            }

            await Search(SearchTerm, int.Parse(Page));
        }

        private async Task Search(string term, int page)
        {
            Action<string> action = async (token) =>
            {
                BooksClient.Token = token;
                Books = await BooksClient.SearchBooks(term, page);

                StateHasChanged();
            };

            await JSRuntime.Current.InvokeAsync<bool>("blazorDemoInterop.executeWithToken", action);
        }

        protected async void SearchBoxKeyPress(UIKeyboardEventArgs ev)
        {
            if(ev.Key == "Enter")
            {
                await SearchClick();
            }
        }

        protected async Task ClearClick()
        {
            SearchTerm = "";
            await LoadBooks(1);
        }

        protected void EditBook(int id)
        {
            UriHelper.NavigateTo("/edit/" + id);
        }

        protected async Task ConfirmDelete(int id, string title)
        {
            DeleteId = id;

            await JSRuntime.Current.InvokeAsync<bool>("blazorDemoInterop.confirmDelete", title);
        }

        protected async void DeleteBook()
        {
            await JSRuntime.Current.InvokeAsync<bool>("blazorDemoInterop.hideDeleteDialog");

            Action<string> action = async (token) =>
            {
                BooksClient.Token = token;
                await BooksClient.DeleteBook(DeleteId);

                await LoadBooks(int.Parse(Page));
                StateHasChanged();
            };

            await JSRuntime.Current.InvokeAsync<bool>("blazorDemoInterop.executeWithToken", action);
        }
    }
}