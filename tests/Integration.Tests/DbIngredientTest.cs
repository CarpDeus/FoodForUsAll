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
    public class DbIngredientTest
    {
        public DbIngredientTest()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);
            var root = configurationBuilder.Build();
            recipeRepository = new RecipeRepository(root.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);
        }

        [SetUp]
        public async Task Setup()
        {
            _scope = new TransactionScope();
            roundSteakIngredient = new() { Name = "Round Steak", Description = "Fine Round Steak" };
        }

        [TearDown]
        public async Task TearDown()
        {
            _scope.Dispose(); // rollback
            _scope = null;
        }

        [Test]
        public async Task AddIngredientToDatabase()
        {
            await recipeRepository.AddIngredient(roundSteakIngredient);

            Assert.That(roundSteakIngredient.Id != 0);
        }

        TransactionScope _scope;

        #region private

        readonly RecipeRepository recipeRepository;
        Ingredient roundSteakIngredient;

        #endregion
    }
}