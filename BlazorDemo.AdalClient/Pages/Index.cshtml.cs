using System;
using BlazorDemo.Shared;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Browser.Interop;
using Microsoft.AspNetCore.Blazor.Components;

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

        protected override void OnParametersSet()
        {
            SearchClick();
            //LoadBooks(int.Parse(Page));
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

        protected void SearchClick()
        {
            if(string.IsNullOrEmpty(SearchTerm))
            {
                LoadBooks(int.Parse(Page));
                return;
            }

            Search(SearchTerm, int.Parse(Page));
        }

        private void Search(string term, int page)
        {
            Action<string> action = async (token) =>
            {
                BooksClient.Token = token;
                Books = await BooksClient.SearchBooks(term, page);

                StateHasChanged();
            };

            RegisteredFunction.InvokeUnmarshalled<bool>("executeWithToken", action);
        }

        protected void SearchBoxKeyPress(UIKeyboardEventArgs ev)
        {
            if(ev.Key == "Enter")
            {
                SearchClick();
            }
        }

        protected void ClearClick()
        {
            SearchTerm = "";
            LoadBooks(1);
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