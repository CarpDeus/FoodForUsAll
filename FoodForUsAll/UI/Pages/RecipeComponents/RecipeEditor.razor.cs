using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

using Domain;
using UseCases;

namespace UI.Pages
{
    public partial class RecipeEditorModel : ComponentBase
    {
        [Inject]
        public RecipesState RecipesState { get; set; }
        [Inject]
        IRecipeUseCases RecipeUseCases { get; set; }
        [Inject]
        IWebHostEnvironment Environment { get; set; }

        public bool IsUploadingImage { get; private set; }
        public string ImageUploadError { get; private set; }

        public Dictionary<int,bool> AddNewIngredientNameIsVisible { get; set; }
        public string NewIngredientName { get; set; }

        public Recipe Recipe { get; set; }

        public readonly static Dictionary<string, object> InputTextAreaAttributes = new Dictionary<string, object> { { "rows", "1" }, { "cols", "25" }, };

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            if (RecipesState.RecipeId != null)
                Recipe = await RecipeUseCases.GetRecipe(RecipesState.RecipeId.Value);
            else
                Recipe = null;
        }

        public async Task RecipeNameUpdated(int recipeId, string newRecipeName)
        {
            await RecipeUseCases.ChangeRecipeName(recipeId, newRecipeName);
        }

        public async Task RecipeDescriptionUpdated(int recipeId, string newRecipeDescription)
        {
            await RecipeUseCases.ChangeRecipeDescription(recipeId, newRecipeDescription);
        }

        public async Task InstructionUpdated(int recipeId, int sectionId, int recipeInstructionId, string newDescription)
        {
            await RecipeUseCases.ChangeRecipeInstructionDescription(recipeId, sectionId, recipeInstructionId, newDescription);
        }

        public async Task UploadPrimaryImage(InputFileChangeEventArgs e)
        {
            ImageUploadError = string.Empty;
            IsUploadingImage = true;

            try
            {
                var trustedFileNameForFileStorage = Path.GetRandomFileName();
                IBrowserFile file = e.File;

                if (e.File.Size > _fileSizeLimit)
                {
                    ImageUploadError = "Cannot be more than " + _fileSizeLimit / 1024 / 1024 + " MB(s).";
                    return;
                }

                if (!_allowedImageTypes.Contains(file.ContentType))
                {
                    ImageUploadError = "Image must be jpg, gif, or png type.";
                    return;
                }

                string path = Path.Combine(Environment.ContentRootPath, "unsafe_uploads", trustedFileNameForFileStorage);

                await using (FileStream fs = new(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None, 1024, FileOptions.DeleteOnClose))
                {
                    await file.OpenReadStream(_fileSizeLimit)
                        .CopyToAsync(fs);
                    fs.Position = 0;
                    var imageByteArray = new byte[fs.Length];
                    fs.Read(imageByteArray, 0, imageByteArray.Length);
                    await RecipeUseCases.AddRecipeImage(Recipe.Id, trustedFileNameForFileStorage, Recipe.AuthorId, imageByteArray, true);
                }
            }
            catch (Exception ex)
            {
                //??? definitely need a logger on this one
                //Logger.LogError("File: {Filename} Error: {Error}",
                //    file.Name, ex.Message);
            }
            finally
            {
                IsUploadingImage = false;
            }
        }

        #region private

        readonly long _fileSizeLimit = 3145728; // 3MB
        readonly string[] _allowedImageTypes = { "image/jpg", "image/jpeg", "image/pjpeg", "image/gif", "image/x-png", "image/spng" };

        #endregion
    }
}