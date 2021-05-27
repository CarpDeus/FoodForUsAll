using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Domain;
using UseCases;

namespace UI.Pages
{
    public partial class InstructionSectionsEditorModel : ComponentBase
    {
        [Inject]
        IRecipeUseCases RecipeUseCases { get; set; }

        [CascadingParameter(Name = "RecipeId")]
        public int RecipeId { get; set; }
        [CascadingParameter(Name = "IsEditable")]
        public bool IsEditable { get; set; }

        public Recipe Recipe { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Recipe = await RecipeUseCases.GetRecipe(RecipeId);

            await base.OnInitializedAsync();
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
        }

        public async Task AddNewInstructionSection(int recipeId, int? parentInstructionSectionId)
        {
            await RecipeUseCases.AddInstructionSection(recipeId, new InstructionSection { }, parentInstructionSectionId);
        }

        public async Task InstructionSectionNameUpdated(int recipeId, int instructionSectionId, string newInstructionSectionName)
        {
            await RecipeUseCases.ChangeInstructionSectionName(recipeId, instructionSectionId, newInstructionSectionName);
        }
    }
}