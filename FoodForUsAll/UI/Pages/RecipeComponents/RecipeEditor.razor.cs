using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Configuration;
using Domain;
using UseCases;
using Services;

namespace UI.Pages
{
    public partial class RecipeEditorModel : ComponentBase
    {
        [Inject]
        IConfiguration Configuration { get; set; }
        [Inject]
        public RecipesState RecipesState { get; set; }
        [Inject]
        IRecipeUseCases RecipeUseCases { get; set; }
        [Inject]
        IImageAdder ImageAdder { get; set; }

        public bool IsDemoMode { get; private set; }
        public bool IsUploadingImage { get; private set; }
        public string ImageUploadError { get; private set; }

        public Dictionary<int,bool> AddNewIngredientNameIsVisible { get; set; }
        public string NewIngredientName { get; set; }

        public Recipe Recipe { get; set; }

        public readonly static Dictionary<string, object> InputTextAreaAttributes = new Dictionary<string, object> { { "rows", "1" }, { "cols", "25" }, };

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            IsDemoMode = bool.Parse(Configuration["AppSettings:IsDemoMode"]);
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
            await UploadImage(e, true);
        }

        public async Task UploadSecondaryImage(InputFileChangeEventArgs e)
        {
            await UploadImage(e, false);
        }

        #region private

        readonly long _fileSizeLimit = 2097152; // 2MB
        readonly string[] _allowedImageTypes = { "image/jpg", "image/jpeg", "image/pjpeg", "image/gif", "image/x-png", "image/spng" };

        async Task UploadImage(InputFileChangeEventArgs e, bool isPrimary)
        {
            ImageUploadError = string.Empty;
            IsUploadingImage = true;

            try
            {
                IBrowserFile file = e.File;

                if (e.File.Size > _fileSizeLimit)
                {
                    ImageUploadError = "Cannot be more than " + _fileSizeLimit / 1024 / 1024 + " MB(s).";
                    return;
                }

                if (!_allowedImageTypes.Contains(file.ContentType))
                {
                    ImageUploadError = "Must be a jpg, gif, or png type.";
                    return;
                }

                var imageByteArray = await ImageAdder.Validate(file, _fileSizeLimit);

                await RecipeUseCases.AddRecipeImage(Recipe.Id, Guid.NewGuid().ToString(), Recipe.AuthorId, imageByteArray, isPrimary);
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

        #endregion
    }
}