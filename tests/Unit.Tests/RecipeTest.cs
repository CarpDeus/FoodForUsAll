using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Domain;
using UseCases;

namespace Unit.Tests
{
    public class RecipeTest
    {
        public RecipeTest()
        {
            _recipeRepository = new InMemoryData.RecipeRepository();
            _recipeUseCases = new RecipeUseCases(_recipeRepository);
        }

        [Test]
        public async Task RemoveRecipeIngredient()
        {
            //Arrange
            Recipe recipe = new()
            {
                AuthorId = _firstAuthor,
                Name = "Fish and Chipz",
                Description = "Fish and potatoes deep fried to perfection.",
            };
            await _recipeUseCases.AddRecipe(recipe);

            Ingredient ingredient1 = new() { Name = "Fish", Description = "Breaded Fish" };
            await _recipeUseCases.AddIngredient(ingredient1);
            Ingredient ingredient2 = new() { Name = "Long Cut Potatoes", Description = "" };
            await _recipeUseCases.AddIngredient(ingredient2);

            IngredientSection ingredientSection = new() { Name = "Main", };
            await _recipeUseCases.AddIngredientSection(recipe.Id, ingredientSection);

            RecipeIngredient recipeIngredient1 = new() { Ingredient = ingredient1, OrderId = 1, Quantity = "4" };
            await _recipeUseCases.AddRecipeIngredient(recipe.Id, ingredientSection.Id, recipeIngredient1);
            RecipeIngredient recipeIngredient2 = new() { Ingredient = ingredient2, OrderId = 2, Quantity = "2 potatos" };
            await _recipeUseCases.AddRecipeIngredient(recipe.Id, ingredientSection.Id, recipeIngredient2);

            //Act
            await _recipeRepository.DeleteRecipeIngredient(recipe.Id, ingredientSection.Id, recipeIngredient1.Id);

            //Assert
            Assert.That(await _recipeRepository.GetRecipeIngredient(recipe.Id, recipeIngredient1.Id) == null);
            Assert.That(await _recipeRepository.GetRecipeIngredient(recipe.Id, recipeIngredient2.Id) != null);
        }

        [Test]
        public async Task SearchRecipeReturnsProperResults()
        {
            //Arrange
            Recipe recipe1 = new()
            {
                AuthorId = _firstAuthor,
                Name = "Beef and Bacon",
                Description = "A rich beef, mushroom, onion, and bacon dish served over noodles.",
            };
            await _recipeUseCases.AddRecipe(recipe1);

            Recipe recipe2 = new()
            {
                AuthorId = _firstAuthor,
                Name = "Chicken Mushroom Curry",
                Description = "A declicious rendition of a Indian take out favorite.",
            };
            await _recipeUseCases.AddRecipe(recipe2);

            Recipe recipe3 = new()
            {
                AuthorId = _firstAuthor,
                Name = "Chicken Jalfrezi",
                Description = "A declicious rendition of a Indian take out favorite.",
            };
            await _recipeUseCases.AddRecipe(recipe3);

            //Act
            List<Recipe> beefRecipes = await _recipeUseCases.GetRecipesByAuthorAndSearchByNameOrDescription(_firstAuthor, "BeeF");
            List<Recipe> mushroomRecipes = await _recipeUseCases.GetRecipesByAuthorAndSearchByNameOrDescription(_firstAuthor, "MUSHROOM");
            List<Recipe> indianRecipes = await _recipeUseCases.GetRecipesByAuthorAndSearchByNameOrDescription(_firstAuthor, "indian");
            List<Recipe> noMatchRecipes = await _recipeUseCases.GetRecipesByAuthorAndSearchByNameOrDescription(_firstAuthor, "AsDf");

            //Assert
            Assert.That(beefRecipes.Count == 1);
            Assert.That(mushroomRecipes.Count == 2);
            Assert.That(indianRecipes.Count == 2);
            Assert.That(noMatchRecipes.Count == 0);
        }

        [Test]
        public async Task AddingNewRecipeIngredientAddsItToTheEndOfTheList()
        {
            //Arrange
            Recipe recipe = new() { AuthorId = _defaultAuthor, Name = "Food", Description = "Basic recipe for food." };
            await _recipeUseCases.AddRecipe(recipe);

            IngredientSection ingredientSection = new() { Name = "Main", };
            await _recipeUseCases.AddIngredientSection(recipe.Id, ingredientSection);

            Ingredient ingredient1 = new() { Name = "Sugar", Description = "Granulated White Sugar" };
            await _recipeUseCases.AddIngredient(ingredient1);
            Ingredient ingredient2 = new() { Name = "Unsalted Butter", Description = "" };
            await _recipeUseCases.AddIngredient(ingredient2);

            RecipeIngredient recipeIngredient1 = new() { Ingredient = ingredient1, OrderId = 1, Quantity = "1 cup" };
            await _recipeUseCases.AddRecipeIngredient(recipe.Id, ingredientSection.Id, recipeIngredient1);
            RecipeIngredient recipeIngredient2 = new() { Ingredient = ingredient2, OrderId = 2, Quantity = "2 tbsp." };
            await _recipeUseCases.AddRecipeIngredient(recipe.Id, ingredientSection.Id, recipeIngredient2);

            //Act
            Ingredient ingredient3 = new() { Name = "Flour", Description = "All Purpose Flour" };
            await _recipeUseCases.AddIngredient(ingredient3);

            RecipeIngredient recipeIngredient3 = new() { Ingredient = ingredient3, Quantity = "2 cups" };
            await _recipeUseCases.AddRecipeIngredient(recipe.Id, ingredientSection.Id, recipeIngredient3);

            //Assert
            RecipeIngredient recipeIngredient = await _recipeUseCases.GetRecipeIngredient(recipe.Id, recipeIngredient3.Id);
            Assert.That(recipeIngredient.OrderId == 3);
        }

        [Test]
        public async Task CannotRemoveIngredientIfTheyAreOnRecipes()
        {
            //Arrange
            Recipe recipe = new() { AuthorId = _defaultAuthor, Name = "Food", Description = "Basic recipe for food." };
            await _recipeUseCases.AddRecipe(recipe);

            IngredientSection ingredientSection = new() { Name = "Main", OrderId = 1, };
            await _recipeUseCases.AddIngredientSection(recipe.Id, ingredientSection);

            Ingredient ingredient1 = new() { Name = "Water", Description = "" };
            await _recipeUseCases.AddIngredient(ingredient1);

            Ingredient ingredient2 = new() { Name = "Flour", Description = "Basic white wheat flour." };
            await _recipeUseCases.AddIngredient(ingredient2);

            RecipeIngredient recipeIngredient1 = new() { Ingredient = ingredient1, OrderId = 1, Quantity = "1 cup" };
            await _recipeUseCases.AddRecipeIngredient(recipe.Id, ingredientSection.Id, recipeIngredient1);
            RecipeIngredient recipeIngredient2 = new() { Ingredient = ingredient2, OrderId = 2, Quantity = "2 cups" };
            await _recipeUseCases.AddRecipeIngredient(recipe.Id, ingredientSection.Id, recipeIngredient2);

            //Act
            bool wasAbleToDeleteIngredient = await _recipeUseCases.DeleteIngredient(ingredient1.Id);

            //Assert
            Assert.That(wasAbleToDeleteIngredient == false);
        }

        [Test]
        public async Task CanGetAllRecipeIngredientsInNestedSections()
        {
            //Arrange
            Recipe headerOnlyRecipe = new() { AuthorId = _defaultAuthor, Name = "Food", Description = "Basic recipe for food." };
            await _recipeUseCases.AddRecipe(headerOnlyRecipe);

            Ingredient ingredient1 = new() { Name = "Steak", Description = "Filet Mignon Preferred" };
            await _recipeUseCases.AddIngredient(ingredient1);

            Ingredient ingredient2 = new() { Name = "Peas", Description = "Frozen or Fresh Peas" };
            await _recipeUseCases.AddIngredient(ingredient2);

            IngredientSection ingredientSectionParent = new() { Name = "Main", OrderId = 1, };
            await _recipeUseCases.AddIngredientSection(headerOnlyRecipe.Id, ingredientSectionParent);

            IngredientSection childIngredientSection = new() { Name = "Subsection", OrderId = 1, };
            await _recipeUseCases.AddIngredientSection(headerOnlyRecipe.Id, childIngredientSection, ingredientSectionParent.Id);

            //Act
            RecipeIngredient parentRecipeIngredient = new() { Quantity = "6oz", Ingredient = ingredient1, OrderId = 1 };
            await _recipeUseCases.AddRecipeIngredient(headerOnlyRecipe.Id, ingredientSectionParent.Id, parentRecipeIngredient);
            RecipeIngredient childRecipeIngredient = new() { Quantity = "3", Ingredient = ingredient2, OrderId = 2 };
            await _recipeUseCases.AddRecipeIngredient(headerOnlyRecipe.Id, ingredientSectionParent.Id, childRecipeIngredient);

            //Assert
            List<RecipeIngredient> recipeIngredients = await _recipeUseCases.GetRecipeIngredients(headerOnlyRecipe.Id);
            Assert.That(recipeIngredients.Count == 2);
            Assert.That(recipeIngredients.Exists(x => x.Id == parentRecipeIngredient.Id));
            Assert.That(recipeIngredients.Exists(x => x.Id == childRecipeIngredient.Id));
        }

        [Test]
        public async Task CanAddBasicIngredientParentChildSectionsIncludingRecipeIngredientsToEach()
        {
            //Arrange
            Recipe headerOnlyRecipe = new() { AuthorId = _defaultAuthor, Name = "Food", Description = "Basic recipe for food." };
            await _recipeUseCases.AddRecipe(headerOnlyRecipe);

            Ingredient ingredient1 = new() { Name = "Rice", Description = "Jasmine or any long grain rice" };
            await _recipeUseCases.AddIngredient(ingredient1);

            Ingredient ingredient2 = new() { Name = "Eggs", Description = "" };
            await _recipeUseCases.AddIngredient(ingredient2);

            //Act
            IngredientSection ingredientSectionParent = new() { Name = "Main", OrderId = 1, };
            await _recipeUseCases.AddIngredientSection(headerOnlyRecipe.Id, ingredientSectionParent);

            IngredientSection ingredientSection = new() { Name = "Subsection", OrderId = 1, };
            await _recipeUseCases.AddIngredientSection(headerOnlyRecipe.Id, ingredientSection, ingredientSectionParent.Id);

            RecipeIngredient recipeIngredients1 = new() { Quantity = "2 cups", Ingredient = ingredient1, OrderId = 1 };
            await _recipeUseCases.AddRecipeIngredient(headerOnlyRecipe.Id, ingredientSectionParent.Id, recipeIngredients1);

            RecipeIngredient recipeIngredients2 = new() { Quantity = "3", Ingredient = ingredient2, OrderId = 2 };
            await _recipeUseCases.AddRecipeIngredient(headerOnlyRecipe.Id, ingredientSection.Id, recipeIngredients2);

            //Assert
            IngredientSection parentSection = await _recipeUseCases.GetIngredientSection(headerOnlyRecipe.Id, ingredientSectionParent.Id);
            Assert.That(parentSection != null);
            Assert.That(parentSection.RecipeIngredients.Count == 1);
            Assert.That(parentSection.RecipeIngredients.FirstOrDefault().Ingredient.Name == "Rice");
            IngredientSection childSection = await _recipeUseCases.GetIngredientSection(headerOnlyRecipe.Id, ingredientSection.Id);
            Assert.That(childSection != null);
            Assert.That(childSection.RecipeIngredients.Count == 1);
            Assert.That(childSection.RecipeIngredients.FirstOrDefault().Ingredient.Name == "Eggs");
        }

        [Test]
        public async Task CanAddEmptyIngredientSectionToRecipe()
        {
            //Arrange
            Recipe headerOnlyRecipe = new() { AuthorId = _defaultAuthor, Name = "Food", Description = "Basic recipe for food." };
            await _recipeUseCases.AddRecipe(headerOnlyRecipe);

            IngredientSection emptyIngredientSection = new() { OrderId = 1, Name = "Main" };

            //Act
            await _recipeUseCases.AddIngredientSection(headerOnlyRecipe.Id, emptyIngredientSection);

            //Assert
            IngredientSection section = await _recipeUseCases.GetIngredientSection(headerOnlyRecipe.Id, emptyIngredientSection.Id);
            Assert.That(section != null);
        }

        [Test]
        public async Task CanRemoveIngredient()
        {
            //Arrange
            Ingredient ingredient = new() { Name = "Food", Description = "Basic ingredient for food." };
            await _recipeUseCases.AddIngredient(ingredient);

            //Act
            bool hasDeletedIngredient = await _recipeUseCases.DeleteIngredient(ingredient.Id);

            //Assert
            Assert.That(hasDeletedIngredient);
            Ingredient r = await _recipeUseCases.GetIngredient(ingredient.Id);
            Assert.That(r == null);
        }

        [Test]
        public async Task CanAddIngredient()
        {
            //Arrange
            Ingredient ingredient = new() { Name = "Food", Description = "Basic ingredient for food." };

            //Act
            await _recipeUseCases.AddIngredient(ingredient);

            //Assert
            Ingredient r = await _recipeUseCases.GetIngredient(ingredient.Id);
            Assert.That(r != null);
        }

        [Test]
        public async Task CanRemoveRecipe()
        {
            //Arrange
            Recipe headerOnlyRecipe = new() { AuthorId = _defaultAuthor, Name = "Food", Description = "Basic recipe for food." };
            await _recipeUseCases.AddRecipe(headerOnlyRecipe);

            //Act
            await _recipeUseCases.DeleteRecipe(headerOnlyRecipe.Id);

            //Assert
            Recipe r = await _recipeUseCases.GetRecipe(headerOnlyRecipe.Id);
            Assert.That(r == null);
        }

        [Test]
        public async Task CanAddBasicRecipe()
        {
            //Arrange
            Recipe basicRecipe = new()
            {
                AuthorId = _defaultAuthor,
                Name = "Food",
                Description = "Basic recipe for food.",
            };

            //Act
            await _recipeUseCases.AddRecipe(basicRecipe);

            IngredientSection ingredientSection = new() { Name = "Main", OrderId = 1, };
            await _recipeUseCases.AddIngredientSection(basicRecipe.Id, ingredientSection);

            Ingredient ingredient = new() { Name = "Flank Steak", Description = "" };
            await _recipeUseCases.AddIngredient(ingredient);

            RecipeIngredient recipeIngredient = new() { Quantity = "6oz", Ingredient = ingredient, OrderId = 1, };
            await _recipeUseCases.AddRecipeIngredient(basicRecipe.Id, ingredientSection.Id, recipeIngredient);

            InstructionSection instructionSection = new() { Name = "Main", OrderId = 1, };
            await _recipeUseCases.AddInstructionSection(basicRecipe.Id, instructionSection);

            RecipeInstruction recipeInstruction = new() { Description = "Sear in butter.", OrderId = 1, };
            await _recipeUseCases.AddRecipeInstruction(basicRecipe.Id, instructionSection.Id, recipeInstruction);

            //Assert
            Recipe r = await _recipeUseCases.GetRecipe(basicRecipe.Id);
            Assert.That(r != null);
            Assert.That(r.IngredientSections.Count == 1);
            Assert.That(r.InstructionSections.Count == 1);
            Assert.That(r.IngredientSections.FirstOrDefault().RecipeIngredients.Count == 1);
            Assert.That(r.InstructionSections.FirstOrDefault().RecipeInstructions.Count == 1);
        }

        [Test]
        public async Task CanAddHeaderOnlyRecipe()
        {
            //Arrange
            Recipe headerOnlyRecipe = new() { AuthorId = _defaultAuthor, Name = "Food", Description = "Basic header only recipe for food." };

            //Act
            await _recipeUseCases.AddRecipe(headerOnlyRecipe);

            //Assert
            Recipe r = await _recipeUseCases.GetRecipe(headerOnlyRecipe.Id);
            Assert.That(r != null);
        }

        [Test]
        public void EmptyRecipeOutputsProperly()
        {
            //Arrange
            Recipe _emptyRecipe = new();

            //Assert
            Assert.That(_emptyRecipe.ToString() == "Name Missing");
        }

        [Test]
        public void RecipeNameAndDescriptionOutputsProperly()
        {
            //Arrange
            Recipe headerOnlyRecipe = new() { AuthorId = _defaultAuthor, Name = "Food", Description = "Basic recipe for food." };

            //Assert
            Assert.That(headerOnlyRecipe.ToString() == "Food: Basic recipe for food.");
        }

        #region private

        readonly InMemoryData.RecipeRepository _recipeRepository;
        readonly RecipeUseCases _recipeUseCases;

        static Guid _defaultAuthor = new("00000000-0000-0000-0000-000000000000");
        static Guid _firstAuthor = new("11111111-1111-1111-1111-111111111111");

        #endregion
    }
}