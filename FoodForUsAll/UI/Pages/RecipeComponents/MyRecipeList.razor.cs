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
        [Inject]
        NavigationManager NavManager { get; set; }

        public IReadOnlyList<Recipe> Recipes { get; set; }

        public bool AddNewRecipeIsVisible { get; set; } = false;
        public string SearchString { get; set; }
        public string NewRecipeName { get; set; }
        public Recipe Recipe { get; set; } = new Recipe();

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

            Recipes = await RecipeUseCases.GetRecipesByAuthor(new Guid("00000000-0000-0000-0000-000000000000"));
        }

        public async Task AddNewRecipe()
        {
            if (!string.IsNullOrEmpty(NewRecipeName))
            {
                Recipe recipe = new() {
                    Name = NewRecipeName, Description = "<Add description here>",
                    AuthorId = new Guid("00000000-0000-0000-0000-000000000000"),
                    IngredientSections = new List<IngredientSection> { new() { Name = "Main" } },
                    InstructionSections = new List<InstructionSection> { new() { Name = "Main" } },
                };
                await RecipeUseCases.AddRecipe(recipe);

                SetAddNewRecipeNotVisible();
                SearchString = string.Empty;
                NewRecipeName = string.Empty;

                Recipes = await RecipeUseCases.GetRecipesByAuthor(new Guid("00000000-0000-0000-0000-000000000000"));

                RecipesState.SetRecipeId(recipe.Id, true);
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
            NavManager.NavigateTo("/RecipeViewer");
        }

        public void EditRecipe(int recipeId)
        {
            RecipesState.SetRecipeId(recipeId, true);
            NavManager.NavigateTo("/RecipeEditor");
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