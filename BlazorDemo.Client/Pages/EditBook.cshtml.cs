using System.Net.Http;
using System.Threading.Tasks;
using BlazorDemo.Shared;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.Services;

namespace BlazorDemo.Client.Pages
{
    public class EditBookModel : BlazorComponent
    {
        [Inject]
        protected IUriHelper UriHelper { get; set; }

        [Inject]
        protected HttpClient Http { get; set; }

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
            CurrentBook = await Http.GetJsonAsync<Book>("/Books/Get/" + id);
        }

        protected async Task Save()
        {
            await Http.PostJsonAsync("/Books/Save", CurrentBook);

            UriHelper.NavigateTo("/");
        }
    }
}