using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DbData;
using Domain;

namespace UseCases
{
    public class RecipeUseCases : IRecipeUseCases
    {
        public RecipeUseCases(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        #region ingredients

        public async Task<Ingredient> GetIngredient(int ingredientId)
        {
            return await _recipeRepository.GetIngredient(ingredientId);
        }

        public async Task<List<Ingredient>> GetAllIngredients()
        {
            return await _recipeRepository.GetAllIngredients();
        }

        public async Task AddIngredient(Ingredient ingredient)
        {
            await _recipeRepository.AddIngredient(ingredient);
        }

        public async Task<bool> DeleteIngredient(int ingredientId)
        {
            return await _recipeRepository.DeleteIngredient(ingredientId);
        }

        #endregion

        #region recipe detail

        public async Task ChangeRecipeName(int recipeId, string newRecipeName)
        {
            await _recipeRepository.UpdateRecipeName(recipeId, newRecipeName);
        }

        public async Task ChangeRecipeDescription(int recipeId, string newRecipeDescription)
        {
            await _recipeRepository.UpdateRecipeDescription(recipeId, newRecipeDescription);
        }

        public async Task<Recipe> GetRecipe(int recipeId)
        {
            return await _recipeRepository.GetRecipe(recipeId);
        }

        public async Task<IReadOnlyList<Recipe>> GetRecipesByAuthor(Guid recipeId)
        {
            return await _recipeRepository.GetRecipesByAuthor(recipeId);
        }

        public async Task<IReadOnlyList<Recipe>> GetRecipesByAuthorAndSearchByNameOrDescription(Guid authorId, string searchString)
        {
            return await _recipeRepository.GetRecipesByAuthorAndSearchByNameOrDescription(authorId, searchString);
        }

        public async Task AddRecipe(Recipe recipe)
        {
            await _recipeRepository.AddRecipe(recipe);
        }

        public async Task AddRecipeImage(int recipeId, Guid authorId, byte[] image, bool isPrimary)
        {
            await _recipeRepository.AddRecipeImage(recipeId, authorId, image, isPrimary);
        }

        public async Task<RecipeImage> GetPrimaryRecipeImage(int recipeId)
        {
            return await _recipeRepository.GetPrimaryRecipeImage(recipeId);
        }

        public async Task DeleteRecipe(int recipeId)
        {
            await _recipeRepository.DeleteRecipe(recipeId);
        }

        #endregion

        #region ingredient sections

        public async Task<IngredientSection> GetIngredientSection(int recipeId, int sectionId)
        {
            return await _recipeRepository.GetIngredientSection(recipeId, sectionId);
        }

        public async Task AddIngredientSection(int recipeId, IngredientSection ingredientSection, int? parentIngredientSectionId = null)
        {
            await _recipeRepository.AddIngredientSection(recipeId, ingredientSection, parentIngredientSectionId);
        }

        public async Task ChangeIngredientSectionName(int recipeId, int ingredientSectionId, string newIngredientSectionName)
        {
            await _recipeRepository.ChangeIngredientSectionName(recipeId, ingredientSectionId, newIngredientSectionName);
        }

        #endregion

        #region recipe ingredients

        public async Task<List<RecipeIngredient>> GetRecipeIngredients(int recipeId)
        {
            return await _recipeRepository.GetRecipeIngredients(recipeId);
        }

        public async Task<RecipeIngredient> GetRecipeIngredient(int recipeId, int recipeIngredientId)
        {
            return await _recipeRepository.GetRecipeIngredient(recipeId, recipeIngredientId);
        }

        public async Task AddRecipeIngredient(int recipeId, int sectionId, RecipeIngredient recipeIngredient)
        {
            await _recipeRepository.AddRecipeIngredient(recipeId, sectionId, recipeIngredient);
        }

        public async Task ChangeRecipeIngredientQuantity(int recipeId, int sectionId, int recipeIngredientId, string newQuantity)
        {
            await _recipeRepository.ChangeRecipeIngredientQuantity(recipeId, sectionId, recipeIngredientId, newQuantity);
        }

        public async Task ChangeRecipeIngredientIngredient(int recipeId, int sectionId, int recipeIngredientId, int newIngredientId)
        {
            await _recipeRepository.ChangeRecipeIngredientIngredient(recipeId, sectionId, recipeIngredientId, newIngredientId);
        }

        public async Task DeleteRecipeIngredient(int recipeId, int sectionId, int recipeIngredientId)
        {
            await _recipeRepository.DeleteRecipeIngredient(recipeId, sectionId, recipeIngredientId);
        }

        #endregion

        #region instruction sections

        public async Task<InstructionSection> GetInstructionSection(int recipeId, int sectionId)
        {
            return await _recipeRepository.GetInstructionSection(recipeId, sectionId);
        }

        public async Task AddInstructionSection(int recipeId, InstructionSection instructionSection, int? parentInstructionSectionId = null)
        {
            await _recipeRepository.AddInstructionSection(recipeId, instructionSection, parentInstructionSectionId);
        }

        public async Task ChangeInstructionSectionName(int recipeId, int instructionSectionId, string newInstructionSectionName)
        {
            await _recipeRepository.ChangeInstructionSectionName(recipeId, instructionSectionId, newInstructionSectionName);
        }

        #endregion

        public async Task AddRecipeInstruction(int recipeId, int sectionId, RecipeInstruction recipeInstruction)
        {
            await _recipeRepository.AddRecipeInstruction(recipeId, sectionId, recipeInstruction);
        }

        public async Task ChangeRecipeInstructionDescription(int recipeId, int sectionId, int recipeInstructionId, string newDescription)
        {
            await _recipeRepository.ChangeRecipeInstructionDescription(recipeId, sectionId, recipeInstructionId, newDescription);
        }

        #region private

        readonly IRecipeRepository _recipeRepository;

        #endregion
    }
}
