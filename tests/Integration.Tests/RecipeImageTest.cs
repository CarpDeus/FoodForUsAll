using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using DbData;
using Domain;
using UseCases;

namespace Integration.Tests
{
    // Recipe Images write to the file system, so we're including these in integration tests.
    [TestFixture]
    public class RecipeImageTest
    {
        public RecipeImageTest()
        {
            _recipeRepository = new InMemoryData.RecipeRepository();
            _recipeUseCases = new RecipeUseCases(_recipeRepository);
            _sampleImageBMP = File.ReadAllBytes("SampleBMPImage_190kbmb.bmp");
            _sampleImageGIF = File.ReadAllBytes("SampleGIFImage_40kbmb.gif");
            _sampleImageJPG = File.ReadAllBytes("SampleJPGImage_50kbmb.jpg");
            _sampleImagePNG = File.ReadAllBytes("SamplePNGImage_100kbmb.png");
        }

        //??? need to move file type validation out of UI
        //[Test]
        //public async Task AddPrimaryRecipeImageOfUnsupportedType()
        //{
        //    //Arrange
        //    Recipe recipe = new()
        //    {
        //        AuthorId = _defaultAuthor,
        //        Name = "Chocolate Flakes",
        //        Description = "Chocolate cereal made at home!",
        //    };
        //    await _recipeUseCases.AddRecipe(recipe);

        //    //Act
        //    await _recipeUseCases.AddRecipeImage(recipe.Id, "image name", _defaultAuthor, _sampleImageBMP, true);

        //    //Assert
        //    Assert.That(_recipeUseCases.GetPrimaryRecipeImage(recipe.Id).Id == 0);
        //}

        [Test]
        public async Task AddPrimaryRecipeImage()
        {
            //Arrange
            Recipe recipe = new()
            {
                AuthorId = _defaultAuthor,
                Name = "Mac N Cheeze",
                Description = "Cheezy and delicious",
            };
            await _recipeUseCases.AddRecipe(recipe);

            //Act
            await _recipeUseCases.AddRecipeImage(recipe.Id, "image name", _defaultAuthor, _sampleImageGIF, true);

            //Assert
            Assert.That(_recipeUseCases.GetPrimaryRecipeImage(recipe.Id).Id != 0);
        }

        #region private

        readonly InMemoryData.RecipeRepository _recipeRepository;
        readonly RecipeUseCases _recipeUseCases;
        static Guid _defaultAuthor = new("00000000-0000-0000-0000-000000000000");
        static byte[] _sampleImageBMP;
        static byte[] _sampleImageGIF;
        static byte[] _sampleImageJPG;
        static byte[] _sampleImagePNG;

        #endregion
    }
}