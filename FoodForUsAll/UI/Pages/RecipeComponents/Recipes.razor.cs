using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Domain;
using UseCases;

namespace UI.Pages
{
    public partial class RecipesModel: ComponentBase
    {
        [Inject]
        public RecipesState RecipesState { get; set; }

        [Parameter]
        public int? RecipeId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            RecipesState.OnRecipeChange += StateHasChanged;

            await base.OnInitializedAsync();
        }

        #region private

        #endregion
    }
}