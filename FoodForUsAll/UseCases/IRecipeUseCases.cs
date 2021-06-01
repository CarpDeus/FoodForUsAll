using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DbData;
using Domain;

namespace UseCases
{
    public interface IRecipeUseCases
    {
        #region ingredients
        Task<Ingredient> GetIngredient(int ingredientId);
        Task<List<Ingredient>> GetAllIngredients();
        Task AddIngredient(Ingredient ingredient);
        Task<bool> DeleteIngredient(int ingredientId);
        #endregion

        #region recipe detail
        Task ChangeRecipeName(int recipeId, string newRecipeName);
        Task ChangeRecipeDescription(int recipeId, string newRecipeDescription);
        Task AddRecipe(Recipe recipe);
        Task AddRecipeImage(int recipeId, Guid authorId, byte[] image, bool isPrimary);
        Task<RecipeImage> GetPrimaryRecipeImage(int recipeId);
        Task DeleteRecipe(int recipeId);
        Task<Recipe> GetRecipe(int recipeId);
        Task<IReadOnlyList<Recipe>> GetRecipesByAuthor(Guid authorId);
        Task<IReadOnlyList<Recipe>> GetRecipesByAuthorAndSearchByNameOrDescription(Guid authorId, string searchString);
        #endregion

        #region ingredient sections
        Task<IngredientSection> GetIngredientSection(int recipeId, int sectionId);
        Task AddIngredientSection(int recipeId, IngredientSection ingredientSection, int? parentIngredientSectionId = null);
        Task ChangeIngredientSectionName(int recipeId, int ingredientSectionId, string newIngredientSectionName);
        #endregion

        #region recipe ingredients
        Task<List<RecipeIngredient>> GetRecipeIngredients(int recipeId);
        Task<RecipeIngredient> GetRecipeIngredient(int recipeId, int recipeIngredientId);
        Task AddRecipeIngredient(int recipeId, int sectionId, RecipeIngredient recipeIngredient);
        Task ChangeRecipeIngredientQuantity(int recipeId, int sectionId, int recipeIngredientId, string newQuantity);
        Task ChangeRecipeIngredientIngredient(int recipeId, int sectionId, int recipeIngredientId, int newIngredientId);
        Task DeleteRecipeIngredient(int recipeId, int sectionId, int recipeIngredientId);
        #endregion

        #region instruction sections
        Task<InstructionSection> GetInstructionSection(int recipeId, int sectionId);
        Task AddInstructionSection(int recipeId, InstructionSection instructionSection, int? parentInstructionSectionId = null);
        Task ChangeInstructionSectionName(int recipeId, int instructionSectionId, string newInstructionSectionName);
        #endregion

        Task AddRecipeInstruction(int recipeId, int sectionId, RecipeInstruction recipeInstruction);
        Task ChangeRecipeInstructionDescription(int recipeId, int sectionId, int recipeInstructionId, string newDescription);
    }
}
