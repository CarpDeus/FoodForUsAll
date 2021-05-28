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
    public partial class RecipeEditorModel : ComponentBase
    {
        [Inject]
        public RecipesState RecipesState { get; set; }
        [Inject]
        IRecipeUseCases RecipeUseCases { get; set; }

        [CascadingParameter(Name = "RecipeId")]
        public int? RecipeId { get; set; }
        [CascadingParameter(Name = "IsEditable")]
        public bool IsEditable { get; set; } = false;

        public Dictionary<int,bool> AddNewIngredientNameIsVisible { get; set; }
        public string NewIngredientName { get; set; }

        public Recipe Recipe { get; set; }

        public readonly static Dictionary<string, object> InputTextAreaAttributes = new Dictionary<string, object> { { "rows", "1" }, { "cols", "25" }, };

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        protected override async Task OnParametersSetAsync()
        {
            if (RecipeId != null)
                Recipe = await RecipeUseCases.GetRecipe(RecipesState.RecipeId.Value);
            else
                Recipe = null;

            await base.OnParametersSetAsync();
        }

        public async Task RecipeNameUpdated(int recipeId, string newRecipeName)
        {
            await RecipeUseCases.ChangeRecipeName(recipeId, newRecipeName);
        }

        public async Task RecipeDescriptionUpdated(int recipeId, string newRecipeDescription)
        {
            await RecipeUseCases.ChangeRecipeDescription(recipeId, newRecipeDescription);
        }

        public async Task InstructionUpdated(int recipeId, int sectionId, int recipeInstructionId, string newDescription)
        {
            await RecipeUseCases.ChangeRecipeInstructionDescription(recipeId, sectionId, recipeInstructionId, newDescription);
        }
    }
}