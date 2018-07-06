using System;
using BlazorDemo.Shared;
using Microsoft.AspNetCore.Blazor.Components;

namespace BlazorDemo.AdalClient.Shared
{
    public class PagerModel : BlazorComponent
    {
        [Parameter]
        protected PagedResultBase Result { get; set; }

        [Parameter]
        protected Action<int> PageChanged { get; set; }

        protected int StartIndex { get; private set; } = 0;
        protected int FinishIndex { get; private set; } = 0;

        protected override void OnParametersSet()
        {
            StartIndex = Math.Max(Result.CurrentPage - 5, 1);
            FinishIndex = Math.Min(Result.CurrentPage + 5, Result.PageCount);

            base.OnParametersSet();
        }

        protected void PagerButtonClicked(int page)
        {
            PageChanged?.Invoke(page);
        }
    }
}