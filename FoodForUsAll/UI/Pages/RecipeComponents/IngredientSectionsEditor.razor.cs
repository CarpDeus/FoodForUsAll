using System;
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
    public partial class IngredientSectionsEditorModel : ComponentBase
    {
        [Inject]
        IRecipeUseCases RecipeUseCases { get; set; }

        [CascadingParameter(Name = "RecipeId")]
        public int RecipeId { get; set; }
        [CascadingParameter(Name = "IsEditable")]
        public bool IsEditable { get; set; }

        public Recipe Recipe { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Recipe = await RecipeUseCases.GetRecipe(RecipeId);
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
        }

        public async Task AddNewIngredientSection(int recipeId, int? parentIngredientSectionId)
        {
            await RecipeUseCases.AddIngredientSection(recipeId, new() { }, parentIngredientSectionId);
        }

        public async Task IngredientSectionNameUpdated(int recipeId, int ingredientSectionId, string newIngredientSectionName)
        {
            await RecipeUseCases.ChangeIngredientSectionName(recipeId, ingredientSectionId, newIngredientSectionName);
        }
    }
}