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
    public partial class RecipeIngredientsEditorModel : ComponentBase
    {
        [Inject]
        IRecipeUseCases RecipeUseCases { get; set; }

        [CascadingParameter(Name = "RecipeId")]
        public int RecipeId { get; set; }
        [CascadingParameter(Name = "IngredientSectionId")]
        public int IngredientSectionId { get; set; }
        [CascadingParameter(Name = "IsEditable")]
        public bool IsEditable { get; set; }

        public bool AddNewIngredientNameIsVisible { get; set; }
        public string NewIngredientName { get; set; }
        public List<Ingredient> Ingredients { get; set; }

        public IngredientSection IngredientSection { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            Ingredients = await RecipeUseCases.GetAllIngredients();
            IngredientSection = await RecipeUseCases.GetIngredientSection(RecipeId, IngredientSectionId);
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
        }

        public async Task QuantityUpdated(int recipeId, int sectionId, int recipeIngredientId, string newQuantity)
        {
            await RecipeUseCases.ChangeRecipeIngredientQuantity(recipeId, sectionId, recipeIngredientId, newQuantity);
        }

        public async Task IngredientUpdated(int recipeId, int sectionId, int recipeIngredientId, int newIngredientId)
        {
            await RecipeUseCases.ChangeRecipeIngredientIngredient(recipeId, sectionId, recipeIngredientId, newIngredientId);
        }

        public async Task AddRecipeIngredient(int recipeId, int sectionId, string newIngredientName)
        {
            Ingredient ingredient = new() { Name = newIngredientName, Description = string.Empty };
            await RecipeUseCases.AddIngredient(ingredient);
            RecipeIngredient recipeIngredient = new() { Quantity = string.Empty, Ingredient = ingredient, };
            await RecipeUseCases.AddRecipeIngredient(recipeId, sectionId, recipeIngredient);
            SetNewIngredientNameNotVisible();
            Ingredients = await RecipeUseCases.GetAllIngredients();
            IngredientSection = await RecipeUseCases.GetIngredientSection(RecipeId, IngredientSectionId);
        }

        public void SetNewIngredientNameIsVisible()
        {
            AddNewIngredientNameIsVisible = true;
            NewIngredientName = string.Empty;
        }

        public void SetNewIngredientNameNotVisible()
        {
            AddNewIngredientNameIsVisible = false;
            NewIngredientName = string.Empty;
        }
    }
}