using System.Threading.Tasks;
using BlazorLibrary.Shared;
using Microsoft.AspNetCore.Components;

namespace BlazorDemo.Client.Pages
{
    public class EditBookModel : BaseComponent
    {
        [Parameter]
        protected string Id { get; private set; } = "0";
        protected string PageTitle { get; private set; }
        protected Book CurrentBook { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            if (Id == null || Id == "0")
            {
                PageTitle = "Add book";
                CurrentBook = new Book();                
            }
            else
            {
                PageTitle = "Edit book";

                await LoadBook(int.Parse(Id));
            }
        }

        protected async Task LoadBook(int id)
        {
            CurrentBook = await BooksClient.GetBook(id);
        }

        protected async Task Save()
        {            
            await BooksClient.SaveBook(CurrentBook);
            UriHelper.NavigateTo("/");
        }
    }
}