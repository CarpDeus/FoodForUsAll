using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace DbData
{
    public interface IRecipeRepository
    {
        #region ingredients
        Task<Ingredient> GetIngredient(int ingredientId);
        Task<List<Ingredient>> GetAllIngredients();
        Task<List<Ingredient>> GetAllIngredientsByAuthor(Guid authorId);
        Task AddIngredient(Ingredient ingredient);
        Task<bool> DeleteIngredient(int ingredientId);
        #endregion

        #region recipe detail
        Task UpdateRecipeName(int recipeId, string recipeName);
        Task UpdateRecipeDescription(int recipeId, string recipeDescription);
        Task<Recipe> GetRecipe(int recipeId);
        Task<IReadOnlyList<RecipeCard>> GetRecipeCardsByAuthor(Guid authorId);
        Task<IReadOnlyList<Recipe>> GetRecipesByAuthor(Guid authorId);
        Task<IReadOnlyList<RecipeCard>> GetRecipeCardsByAuthorAndSearchByNameOrDescription(Guid authorId, string searchString);
        Task<IReadOnlyList<Recipe>> GetRecipesByAuthorAndSearchByNameOrDescription(Guid authorId, string searchString);
        Task AddRecipe(Recipe recipe);
        Task AddRecipeImage(int recipeId, string name, Guid authorId, byte[] image, bool isPrimary);
        Task<RecipeImage> GetPrimaryRecipeImage(int recipeId);
        Task<List<RecipeImage>> GetSecondaryRecipeImages(int recipeId);
        Task DeleteRecipe(int recipeId);
        Task PermanentlyDeleteRecipe(int recipeId);
        #endregion region

        #region ingredient sections
        Task<IngredientSection> GetIngredientSection(int recipeId, int sectionId);
        Task AddIngredientSection(int recipeId, IngredientSection ingredientSection, int? parentIngredientSectionId = null);
        Task ChangeIngredientSectionName(int recipeId, int ingredientSectionId, string newIngredientSectionName);
        #endregion

        #region recipe ingredients
        Task<List<RecipeIngredient>> GetRecipeIngredients(int recipeId);
        Task<List<RecipeIngredient>> GetRecipeIngredientsByIngredient(int ingredientId);
        Task<RecipeIngredient> GetRecipeIngredient(int recipeId, int recipeIngredientId);
        Task AddRecipeIngredient(int recipeId, int sectionId, RecipeIngredient recipeIngredient);
        Task ChangeRecipeIngredientQuantity(int recipeId, int sectionId, int recipeIngredientId, string newQuantity);
        Task ChangeRecipeIngredientIngredient(int recipeId, int sectionId, int recipeIngredientId, int newIngredientId);
        Task DeleteRecipeIngredient(int recipeId, int sectionId, int recipeIngredientId);
        #endregion

        #region instruction sections
        Task<InstructionSection> GetInstructionSection(int recipeId, int sectionId);
        Task AddInstructionSection(int recipeId, InstructionSection instructionSection, int? parentInstructionSectionId = null);
        Task ChangeInstructionSectionName(int recipeId, int ingredientSectionId, string newInstructionSectionName);
        #endregion

        #region recipe instructions
        Task AddRecipeInstruction(int recipeId, int sectionId, RecipeInstruction recipeInstruction);
        Task ChangeRecipeInstructionDescription(int recipeId, int sectionId, int recipeInstructionId, string newDescription);
        #endregion
    }
}
