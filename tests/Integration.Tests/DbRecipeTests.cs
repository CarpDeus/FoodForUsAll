using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using DbData;
using Domain;

namespace Integration.Tests
{
    [TestFixture]
    public class DbRecipeTests
    {
        public DbRecipeTests()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);
            var root = configurationBuilder.Build();
            recipeRepository = new RecipeRepository(root.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);
        }

        [OneTimeSetUp]
        public async Task Setup()
        {
            baconIngredient = new() { Name = "Bacon", Description = "Sliced bacon" };
            await recipeRepository.AddIngredient(baconIngredient);
            parmesanIngredient = new() { Name = "Parmesan" };
            await recipeRepository.AddIngredient(parmesanIngredient);
            largeEggIngredient = new() { Name = "Large Egg", Description = "Large Chicken Egg" };
            await recipeRepository.AddIngredient(largeEggIngredient);
            groundBlackPepperIngredient = new() { Name = "Ground Black Pepper" };
            await recipeRepository.AddIngredient(groundBlackPepperIngredient);
            waterIngredient = new() { Name = "Water", Description = "" };
            await recipeRepository.AddIngredient(waterIngredient);
            spaghettiIngredient = new() { Name = "Spaghetti", Description = "Medium Dried Spaghetti" };
            await recipeRepository.AddIngredient(spaghettiIngredient);
        }

        [Test]
        public async Task AddRecipeToDatabase()
        {
            //Arrage
            List<RecipeIngredient> recipeIngredients1 = new List<RecipeIngredient>();
            recipeIngredients1.Add(new() { Ingredient = baconIngredient, Quantity = "1lb" });
            recipeIngredients1.Add(new() { Ingredient = parmesanIngredient, Quantity = "1/4 cup grated" });
            recipeIngredients1.Add(new() { Ingredient = largeEggIngredient, Quantity = "4 yolks" });
            recipeIngredients1.Add(new() { Ingredient = groundBlackPepperIngredient, Quantity = "1 tsp." });

            List<RecipeIngredient> recipeIngredients2 = new List<RecipeIngredient>();
            recipeIngredients2.Add(new() { Ingredient = waterIngredient, Quantity = "8 cups" });
            recipeIngredients2.Add(new() { Ingredient = spaghettiIngredient, Quantity = "8oz" });

            IngredientSection ingredientSection1 = new() { Name = "Main", RecipeIngredients = recipeIngredients1, };
            IngredientSection ingredientSection2 = new() { Name = "Pasta", RecipeIngredients = recipeIngredients2, };

            List<RecipeInstruction> recipeInstructions1 = new List<RecipeInstruction>();
            recipeInstructions1.Add(new RecipeInstruction { Description = "In a large saucepan cook bacon until crisp and set aside." });
            List<RecipeInstruction> recipeInstructions2 = new List<RecipeInstruction>();
            recipeInstructions2.Add(new RecipeInstruction { Description = "Heat water in a pot until is begins to boil." });

            InstructionSection instructionSection1 = new InstructionSection { Name = "Main", RecipeInstructions = recipeInstructions1 };
            InstructionSection instructionSection2 = new InstructionSection { Name = "Pasta", RecipeInstructions = recipeInstructions2 };

            Recipe carbonaraRecipe = new Recipe
            {
                AuthorId = new Guid(),
                Name = "Carbonara",
                Description = "A rich, eggy noodle dish.",
                IngredientSections = new List<IngredientSection> { ingredientSection1, ingredientSection2 },
                InstructionSections = new List<InstructionSection> { instructionSection1, instructionSection2 },
            };

            //Act
            await recipeRepository.AddRecipe(carbonaraRecipe);

            //Assert
            Assert.That(carbonaraRecipe.Id != 0);

            await recipeRepository.PermanentlyDeleteRecipe(carbonaraRecipe.Id);
        }

        #region private

        readonly RecipeRepository recipeRepository;
        static Ingredient baconIngredient;
        static Ingredient parmesanIngredient;
        static Ingredient largeEggIngredient;
        static Ingredient groundBlackPepperIngredient;
        static Ingredient waterIngredient;
        static Ingredient spaghettiIngredient;

        #endregion
    }
}