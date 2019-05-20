using System.Threading.Tasks;
using BlazorLibrary.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace BlazorLibrary.ServerSideBlazor.Pages
{
    public class EditBookModel : LibraryPageBase
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
            CurrentBook = await DataContext.Books.FirstOrDefaultAsync(b => b.Id == id);
        }

        protected async Task Save()
        {
            await DataContext.SaveChangesAsync();
            UriHelper.NavigateTo("/");
        }
    }
}