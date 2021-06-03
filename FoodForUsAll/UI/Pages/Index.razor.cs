using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Domain;
using UseCases;

namespace UI.Pages
{
    public partial class IndexModel : ComponentBase
    {
        [Inject]
        IRecipeUseCases RecipeUseCases { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            if ((await RecipeUseCases.GetRecipesByAuthor(new Guid("00000000-0000-0000-0000-000000000000"))).Count == 0)
            {
                foreach (Ingredient ingredient in InMemoryData.Samples.Ingredients)
                    await RecipeUseCases.AddIngredient(ingredient);
                foreach (Recipe recipe in InMemoryData.Samples.Recipes)
                    await RecipeUseCases.AddRecipe(recipe);
            }
        }
    }
}