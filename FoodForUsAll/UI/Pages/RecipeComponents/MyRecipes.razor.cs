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
    public partial class MyRecipesModel: ComponentBase, IDisposable
    {
        [Inject]
        public RecipesState RecipesState { get; set; }
        [Inject]
        IRecipeUseCases RecipeUseCases { get; set; }

        [Parameter]
        public int? RecipeId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if ((await RecipeUseCases.GetRecipesByAuthor(new Guid("00000000-0000-0000-0000-000000000000"))).Count == 0)
            {
                foreach (Ingredient ingredient in InMemoryData.Samples.Ingredients)
                    await RecipeUseCases.AddIngredient(ingredient);
                foreach (Recipe recipe in InMemoryData.Samples.Recipes)
                    await RecipeUseCases.AddRecipe(recipe);
            }

            RecipesState.OnRecipeChange += StateHasChanged;

            await base.OnInitializedAsync();
        }

        public void Dispose()
        {
            RecipesState.OnRecipeChange -= StateHasChanged;
        }

        #region private

        #endregion
    }
}