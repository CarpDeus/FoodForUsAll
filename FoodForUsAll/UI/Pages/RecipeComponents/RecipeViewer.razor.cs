using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Domain;
using UseCases;

namespace UI.Pages
{
    public partial class RecipeViewerModel : ComponentBase
    {
        [Inject]
        IConfiguration Configuration { get; set; }
        [Inject]
        public RecipesState RecipesState { get; set; }
        [Inject]
        IRecipeUseCases RecipeUseCases { get; set; }
        [Inject]
        NavigationManager NavManager { get; set; }

        public bool IsDemoMode { get; private set; }

        public Recipe Recipe { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            IsDemoMode = bool.Parse(Configuration["AppSettings:IsDemoMode"]);
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            if (RecipesState.RecipeId != null)
                Recipe = await RecipeUseCases.GetRecipe(RecipesState.RecipeId.Value);
            else
                Recipe = null;
        }

        public void EditRecipe() {
            RecipesState.SetRecipeId(Recipe.Id, true);
            NavManager.NavigateTo("/RecipeEditor");
        }
    }
}