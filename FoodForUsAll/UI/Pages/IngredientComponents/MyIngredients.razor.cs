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
    public partial class MyIngredientsModel: ComponentBase
    {
        [Inject]
        IRecipeUseCases RecipeUseCases { get; set; }

        public IReadOnlyList<Ingredient> Ingredients { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            Ingredients = await RecipeUseCases.GetAllIngredientsByAuthor(new Guid("00000000-0000-0000-0000-000000000000"));
        }

        #region private

        #endregion
    }
}