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
    public partial class MyRecipeListModel: ComponentBase
    {
        [Inject]
        public RecipesState RecipesState { get; set; }
        [Inject]
        IRecipeUseCases RecipeUseCases { get; set; }
        public List<Recipe> Recipes { get; set; }

        public bool AddNewRecipeIsVisible { get; set; } = false;
        public string SearchString { get; set; }
        public string NewRecipeName { get; set; }
        public Recipe Recipe { get; set; } = new Recipe();

        protected override async Task OnInitializedAsync()
        {
            Recipes = await RecipeUseCases.GetRecipesByAuthor(new Guid("00000000-0000-0000-0000-000000000000"));

            await base.OnInitializedAsync();
        }

        public async Task AddNewRecipe()
        {
            if (!string.IsNullOrEmpty(NewRecipeName))
            {
                Recipe recipe = new() {
                    Name = NewRecipeName, Description = "<Add description here>",
                    AuthorId = new Guid("00000000-0000-0000-0000-000000000000"), };
                await RecipeUseCases.AddRecipe(recipe);

                IngredientSection ingredientSection = new() { OrderId = 1, Name = "Main", };
                await RecipeUseCases.AddIngredientSection(recipe.Id, ingredientSection);
                
                InstructionSection instructionSection = new InstructionSection { OrderId = 1, Name = "Main", };
                await RecipeUseCases.AddInstructionSection(recipe.Id, instructionSection);

                SetAddNewRecipeNotVisible();
                Recipes.Add(recipe);
                RecipesState.SetRecipeId(recipe.Id, true);

                SearchString = string.Empty;
                await Search();
            }
        }

        public async Task Search()
        {
            Recipes = await RecipeUseCases.GetRecipesByAuthorAndSearchByNameOrDescription(new Guid("00000000-0000-0000-0000-000000000000"), SearchString ?? string.Empty);
            RecipesState.SetRecipeId(0, false);
        }

        public void ViewRecipe(int recipeId)
        {
            RecipesState.SetRecipeId(recipeId, false);
        }

        public void EditRecipe(int recipeId)
        {
            RecipesState.SetRecipeId(recipeId, true);
        }

        public void SetAddNewRecipeIsVisible()
        {
            AddNewRecipeIsVisible = true;
        }

        public void SetAddNewRecipeNotVisible()
        {
            AddNewRecipeIsVisible = false;
        }
    }
}