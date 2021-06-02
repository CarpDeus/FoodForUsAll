using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InMemoryData
{
    public class RecipeRepository : DbData.IRecipeRepository
    {
        readonly Dictionary<int, Ingredient> _ingredients = new Dictionary<int, Ingredient>();
        readonly Dictionary<int, Recipe> _recipes = new Dictionary<int, Recipe>();
        readonly Dictionary<int, RecipeImage> _recipeImages = new Dictionary<int, RecipeImage>();
        readonly Dictionary<int, RecipeSection> _recipeSections = new Dictionary<int, RecipeSection>();
        readonly Dictionary<int, RecipeIngredient> _recipeIngredients = new Dictionary<int, RecipeIngredient>();
        readonly Dictionary<int, RecipeInstruction> _recipeInstructions = new Dictionary<int, RecipeInstruction>();

        #region ingredients

        public async Task<Ingredient> GetIngredient(int ingredientId)
        {
            if (_ingredients.ContainsKey(ingredientId))
            {
                return _ingredients[ingredientId];
            }
            return null;
        }

        public async Task<List<Ingredient>> GetAllIngredients()
        {
            return _ingredients.Values.OrderBy(x => x.Name).ToList();
        }

        public async Task AddIngredient(Ingredient ingredient)
        {
            if (ingredient.Id == 0)
            {
                ingredient.Id = (_ingredients.Count == 0) ? 10000 :_ingredients.Max(x => (x.Key)) + 1;
                _ingredients[ingredient.Id] = ingredient;
            }
            else
                _ingredients[ingredient.Id] = ingredient;
        }

        public async Task<bool> DeleteIngredient(int ingredientId)
        {
            List<RecipeIngredient> recipeIngredients = await GetRecipeIngredientsByIngredient(ingredientId);
            if (recipeIngredients.Select(x => x.Ingredient.Id).Contains(ingredientId))
                return false;
            else
                return _ingredients.Remove(ingredientId); 
        }

        #endregion

        #region recipe detail

        public async Task UpdateRecipeName(int recipeId, string recipeName)
        {
            _recipes[recipeId].Name = recipeName;
        }

        public async Task UpdateRecipeDescription(int recipeId, string recipeDescription)
        {
            _recipes[recipeId].Description = recipeDescription;
        }

        public async Task<Recipe> GetRecipe(int recipeId)
        {
            if (_recipes.ContainsKey(recipeId))
            {
                return _recipes[recipeId];
            }
            return null;
        }

        public async Task<IReadOnlyList<Recipe>> GetRecipesByAuthor(Guid authorId)
        {
            return _recipes.Where(x => x.Value.AuthorId == authorId).Select(y => y.Value).ToList();
        }

        public async Task<IReadOnlyList<Recipe>> GetRecipesByAuthorAndSearchByNameOrDescription(Guid authorId, string searchString)
        {
            List<Recipe> recipes = _recipes.Where(x => x.Value.AuthorId == authorId).Select(y => y.Value).ToList();
            return recipes.Where(
                x => x.Name.Contains(searchString, StringComparison.InvariantCultureIgnoreCase) ||
                x.Description.Contains(searchString, StringComparison.InvariantCultureIgnoreCase)
                ).ToList();
        }

        public async Task AddRecipe(Recipe recipe)
        {
            if (recipe.Id == 0)
                recipe.Id = (_recipes.Count == 0) ? 10000 : _recipes.Max(x => x.Key) + 1;

            AddRecipeImage(recipe.Id, recipe.PrimaryImage.Name, recipe.AuthorId, recipe.PrimaryImage.Image, true);

            foreach (IngredientSection ingredientSection in recipe.IngredientSections)
            {
                AddIngredientSection(recipe.Id, ingredientSection);
                foreach(IngredientSection childIngredientSection in ingredientSection.Children)
                {
                    AddIngredientSection(recipe.Id, childIngredientSection, ingredientSection.Id);
                    foreach (IngredientSection grandchildIngredientSection in childIngredientSection.Children)
                    {
                        AddIngredientSection(recipe.Id, grandchildIngredientSection, childIngredientSection.Id);
                    }
                }
            }

            foreach (InstructionSection instructionSection in recipe.InstructionSections)
            {
                AddInstructionSection(recipe.Id, instructionSection);
                foreach (InstructionSection childInstructionSection in instructionSection.Children)
                {
                    AddInstructionSection(recipe.Id, childInstructionSection, instructionSection.Id);
                    foreach (InstructionSection grandchildInstructionSection in childInstructionSection.Children)
                    {
                        AddInstructionSection(recipe.Id, grandchildInstructionSection, childInstructionSection.Id);
                    }
                }
            }

            _recipes[recipe.Id] = recipe;
        }

        public async Task AddRecipeImage(int recipeId, string name, Guid authorId, byte[] image, bool isPrimary)
        {
            int id = (_recipeImages.Count == 0) ? 100000 : _recipeImages.Max(x => x.Key) + 1;

            RecipeImage recipeImage = new() { Id = id, Name = name, AuthorId = authorId, Image = image, };

            if (isPrimary)
                _recipes[recipeId].PrimaryImage = recipeImage;
            else
            {
                _recipeImages[id] = recipeImage;
                _recipes[recipeId].SecondaryImages.Add(recipeImage);
            }
        }

        public async Task<RecipeImage> GetPrimaryRecipeImage(int recipeId)
        {
            return _recipes[recipeId].PrimaryImage;
        }

        public async Task DeleteRecipe(int recipeId)
        {
            _recipes.Remove(recipeId);
        }

        public async Task PermanentlyDeleteRecipe(int recipeId)
        {
            //???
            _recipes.Remove(recipeId);
        }

        #endregion

        #region ingredient sections

        public async Task<IngredientSection> GetIngredientSection(int recipeId, int sectionId)
        {
            List<IngredientSection> ingredientSections = GetAllIngredientSectionsByRecipe(recipeId);
            return ingredientSections.Where(x => x.Id == sectionId).FirstOrDefault();
        }

        public async Task AddIngredientSection(int recipeId, IngredientSection ingredientSection, int? parentIngredientSectionId = null)
        {
            if (ingredientSection.Id == 0)
                ingredientSection.Id = (_recipeSections.Count == 0) ? 100000 : _recipeSections.Max(x => x.Key) + 1;

            _recipeSections[ingredientSection.Id] = ingredientSection;

            int orderId = 1;

            foreach (RecipeIngredient recipeIngredient in ingredientSection.RecipeIngredients)
            {
                if (recipeIngredient.Id == 0)
                    recipeIngredient.Id = (_recipeIngredients.Count == 0) ? 1000000 : _recipeIngredients.Max(x => x.Key) + 1;

                recipeIngredient.OrderId = orderId;
                orderId++;

                _recipeIngredients[recipeIngredient.Id] = recipeIngredient;
            }

            if (parentIngredientSectionId == null)
            {
                ingredientSection.OrderId = (_recipes[recipeId].IngredientSections.Count == 0) ? 1 : _recipes[recipeId].IngredientSections.Max(x => x.OrderId) + 1;
                _recipes[recipeId].IngredientSections.Add(ingredientSection);
            }
            else
            {
                IngredientSection parentIngredientSection = await Task.Run(() => GetIngredientSection(recipeId, parentIngredientSectionId.Value));
                ingredientSection.OrderId = (parentIngredientSection.Children.Count == 0) ? 1 : parentIngredientSection.Children.Max(x => x.OrderId) + 1;
                parentIngredientSection.Children.Add(ingredientSection);
            }
        }

        public async Task ChangeIngredientSectionName(int recipeId, int ingredientSectionId, string newIngredientSectionName)
        {
            IngredientSection ingredientSection = await Task.Run(() => GetIngredientSection(recipeId, ingredientSectionId));
            ingredientSection.Name = newIngredientSectionName;
        }

        #endregion

        #region recipe ingredients

        public async Task<List<RecipeIngredient>> GetRecipeIngredients(int recipeId)
        {
            return GetAllRecipeIngredientsByRecipe(recipeId);
        }

        public async Task<List<RecipeIngredient>> GetRecipeIngredientsByIngredient(int ingredientId)
        {
            return GetAllRecipeIngredientsByIngredient(ingredientId);
        }

        public async Task<RecipeIngredient> GetRecipeIngredient(int recipeId, int recipeIngredientId)
        {
            if (GetAllRecipeIngredientsByRecipe(recipeId).Any(x => x.Id == recipeIngredientId))
                return GetAllRecipeIngredientsByRecipe(recipeId).Where(x => x.Id == recipeIngredientId).FirstOrDefault();

            return null;
        }

        public async Task AddRecipeIngredient(int recipeId, int sectionId, RecipeIngredient recipeIngredient)
        {
            if (recipeIngredient.Id == 0)
                recipeIngredient.Id = (_recipeIngredients.Count == 0) ? 1000000 : _recipeIngredients.Max(x => x.Key) + 1;

            IngredientSection ingredientSection = GetIngredientSectionByRecipeAndSectionId(recipeId, sectionId);

            if (recipeIngredient.OrderId == 0)
                recipeIngredient.OrderId = (ingredientSection.RecipeIngredients.Count == 0) ? 1 : ingredientSection.RecipeIngredients.Max(x => x.OrderId) + 1;

            _recipeIngredients[recipeIngredient.Id] = recipeIngredient;

            ingredientSection.RecipeIngredients.Add(recipeIngredient);
        }

        public async Task ChangeRecipeIngredientQuantity(int recipeId, int sectionId, int recipeIngredientId, string newQuantity)
        {
            IngredientSection ingredientSection = GetIngredientSectionByRecipeAndSectionId(recipeId, sectionId);
            ingredientSection.RecipeIngredients.Where(x => x.Id == recipeIngredientId).FirstOrDefault().Quantity = newQuantity;
        }

        public async Task ChangeRecipeIngredientIngredient(int recipeId, int sectionId, int recipeIngredientId, int newIngredientId)
        {
            RecipeIngredient recipeIngredient =
                GetIngredientSectionByRecipeAndSectionId(recipeId, sectionId)
                    .RecipeIngredients.Where(x => x.Id == recipeIngredientId).FirstOrDefault();
            recipeIngredient.Ingredient = _ingredients[newIngredientId];
        }

        public async Task DeleteRecipeIngredient(int recipeId, int sectionId, int recipeIngredientId)
        {
            List<IngredientSection> ingredientSections = GetAllIngredientSectionsByRecipe(recipeId);
            IngredientSection ingredientSection = ingredientSections.Where(x => x.Id == sectionId).FirstOrDefault();
            ingredientSection.RecipeIngredients.RemoveAll(x => x.Id == recipeIngredientId);
        }

        #endregion

        #region instruction sections

        public async Task<InstructionSection> GetInstructionSection(int recipeId, int sectionId)
        {
            return GetAllInstructionSectionsByRecipe(recipeId).Where(x => x.Id == sectionId).FirstOrDefault();
        }

        public async Task AddInstructionSection(int recipeId, InstructionSection instructionSection, int? parentInstructionSectionId = null)
        {
            if (instructionSection.Id == 0)
                instructionSection.Id = (_recipeSections.Count == 0) ? 100000 : _recipeSections.Max(x => x.Key) + 1;

            _recipeSections[instructionSection.Id] = instructionSection;

            foreach (RecipeInstruction recipeInstruction in instructionSection.RecipeInstructions)
                await AddRecipeInstruction(recipeId, instructionSection.Id, recipeInstruction);

            if (parentInstructionSectionId == null)
            {
                instructionSection.OrderId = (_recipes[recipeId].InstructionSections.Count == 0) ? 1 : _recipes[recipeId].InstructionSections.Max(x => x.OrderId) + 1;
                _recipes[recipeId].InstructionSections.Add(instructionSection);
            }
            else
            {
                InstructionSection parentInstructionSection = await Task.Run(() => GetInstructionSection(recipeId, parentInstructionSectionId.Value));
                instructionSection.OrderId = (parentInstructionSection.Children.Count == 0) ? 1 : parentInstructionSection.Children.Max(x => x.OrderId) + 1;
                parentInstructionSection.Children.Add(instructionSection);
            }
        }

        public async Task ChangeInstructionSectionName(int recipeId, int ingredientSectionId, string newInstructionSectionName)
        {
            InstructionSection instructionSection = await Task.Run(() => GetInstructionSection(recipeId, ingredientSectionId));
            instructionSection.Name = newInstructionSectionName;
        }

        #endregion

        #region recipe instructions

        public async Task AddRecipeInstruction(int recipeId, int sectionId, RecipeInstruction recipeInstruction)
        {
            if (recipeInstruction.Id == 0)
                recipeInstruction.Id = (_recipeInstructions.Count == 0) ? 1000000 : _recipeInstructions.Max(x => x.Key) + 1;

            InstructionSection instructionSection = GetInstructionSectionByRecipeAndSectionId(recipeId, sectionId);

            if (recipeInstruction.OrderId == 0)
                recipeInstruction.OrderId = (instructionSection.RecipeInstructions.Count == 0) ? 1 : instructionSection.RecipeInstructions.Max(x => x.OrderId) + 1;

            _recipeInstructions[recipeInstruction.Id] = recipeInstruction;

            instructionSection.RecipeInstructions.Add(recipeInstruction);
        }

        public async Task ChangeRecipeInstructionDescription(int recipeId, int sectionId, int recipeInstructionId, string newDescription)
        {
            _recipes[recipeId]
                .InstructionSections.Where(x => x.Id == sectionId).FirstOrDefault()
                .RecipeInstructions.Where(x => x.Id == recipeInstructionId).FirstOrDefault()
                .Description = newDescription;
        }

        #endregion

        #region private

        List<IngredientSection> GetAllIngredientSectionsByRecipe(int recipeId)
        {
            List<IngredientSection> ingredientSections = new List<IngredientSection>();

            foreach (IngredientSection ingredientSection in _recipes[recipeId].IngredientSections)
                ingredientSections.AddRange(GetIngredientSectionsBySection(ingredientSection));

            return ingredientSections;
        }

        IngredientSection GetIngredientSectionByRecipeAndSectionId(int recipeId, int sectionId)
        {
            foreach (IngredientSection ingredientSection in _recipes[recipeId].IngredientSections)
            {
                if (ingredientSection.Id == sectionId)
                    return ingredientSection;
                else
                {
                    IngredientSection ingSec = GetIngredientSectionByParentSectionAndSectionId(ingredientSection, sectionId);
                    if (ingSec != null) return ingSec;
                }
            }
            return null;
        }

        IngredientSection GetIngredientSectionByParentSectionAndSectionId(IngredientSection ingredientSection, int sectionId)
        {
            foreach (IngredientSection childIngredientSection in ingredientSection.Children)
            {
                if (childIngredientSection.Id == sectionId)
                    return childIngredientSection;
                else
                {
                    IngredientSection ingSec = GetIngredientSectionByParentSectionAndSectionId(childIngredientSection, sectionId);
                    if (ingSec != null) return ingSec;
                }
            }
            return null;
        }

        InstructionSection GetInstructionSectionByRecipeAndSectionId(int recipeId, int sectionId)
        {
            foreach (InstructionSection instructionSection in _recipes[recipeId].InstructionSections)
            {
                if (instructionSection.Id == sectionId)
                    return instructionSection;
                else
                {
                    InstructionSection instSec = GetInstructionSectionByParentSectionAndSectionId(instructionSection, sectionId);
                    if (instSec != null) return instSec;
                }
            }
            return null;
        }

        InstructionSection GetInstructionSectionByParentSectionAndSectionId(InstructionSection instructionSection, int sectionId)
        {
            foreach (InstructionSection childInstructionSection in instructionSection.Children)
            {
                if (childInstructionSection.Id == sectionId)
                    return childInstructionSection;
                else
                {
                    InstructionSection instSec = GetInstructionSectionByParentSectionAndSectionId(childInstructionSection, sectionId);
                    if (instSec != null) return instSec;
                }
            }
            return null;
        }

        List<InstructionSection> GetAllInstructionSectionsByRecipe(int recipeId)
        {
            List<InstructionSection> instructionSections = new List<InstructionSection>();

            foreach (InstructionSection instructionSection in _recipes[recipeId].InstructionSections)
                instructionSections.AddRange(GetInstructionSectionsBySection(instructionSection));

            return instructionSections;
        }

        List<RecipeIngredient> GetAllRecipeIngredientsByRecipe(int recipeId)
        {
            List<RecipeIngredient> recipeIngredients = new List<RecipeIngredient>();

            foreach (IngredientSection ingredientSection in _recipes[recipeId].IngredientSections)
                recipeIngredients.AddRange(GetRecipeIngredientsBySectionIncludingSubSections(ingredientSection));

            return recipeIngredients;
        }

        List<RecipeIngredient> GetAllRecipeIngredientsByIngredient(int ingredientId)
        {
            List<RecipeIngredient> recipeIngredients = new List<RecipeIngredient>();

            foreach(KeyValuePair<int, Recipe> recipe in _recipes)
                foreach (IngredientSection ingredientSection in recipe.Value.IngredientSections)
                    recipeIngredients.AddRange(GetRecipeIngredientsBySectionByIngredientIncludingSubsections(ingredientSection, ingredientId));

            return recipeIngredients;
        }

        IEnumerable<IngredientSection> GetIngredientSectionsBySection(IngredientSection ingredientSection)
        {
            List<IngredientSection> ingredientSections = new List<IngredientSection> { ingredientSection };

            foreach (IngredientSection ingredientSectionChild in ingredientSection.Children)
                ingredientSections.AddRange(GetIngredientSectionsBySection(ingredientSectionChild));

            return ingredientSections;
        }

        IEnumerable<InstructionSection> GetInstructionSectionsBySection(InstructionSection instructionSection)
        {
            List<InstructionSection> instructionSections = new List<InstructionSection> { instructionSection };

            foreach (InstructionSection ingredientSectionChild in instructionSection.Children)
                instructionSections.AddRange(GetInstructionSectionsBySection(ingredientSectionChild));

            return instructionSections;
        }

        IEnumerable<RecipeIngredient> GetRecipeIngredientsBySectionIncludingSubSections(IngredientSection ingredientSection)
        {
            List<RecipeIngredient> recipeIngredients = new List<RecipeIngredient>();
            recipeIngredients.AddRange(ingredientSection.RecipeIngredients);

            foreach (IngredientSection ingredientSectionChild in ingredientSection.Children)
                recipeIngredients.AddRange(GetRecipeIngredientsBySectionIncludingSubSections(ingredientSectionChild));

            return recipeIngredients;
        }

        IEnumerable<RecipeIngredient> GetRecipeIngredientsBySectionByIngredientIncludingSubsections(IngredientSection ingredientSection, int ingredientId)
        {
            List<RecipeIngredient> recipeIngredients = new List<RecipeIngredient>();
            recipeIngredients.AddRange(ingredientSection.RecipeIngredients.Where(x => x.Ingredient.Id == ingredientId));

            foreach (IngredientSection ingredientSectionChild in ingredientSection.Children)
                recipeIngredients.AddRange(GetRecipeIngredientsBySectionByIngredientIncludingSubsections(ingredientSectionChild, ingredientId));

            return recipeIngredients;
        }

        #endregion

    }
}
