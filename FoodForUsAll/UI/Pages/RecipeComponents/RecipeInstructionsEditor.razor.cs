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
    public partial class RecipeInstructionsEditorModel : ComponentBase
    {
        [Inject]
        IRecipeUseCases RecipeUseCases { get; set; }

        [CascadingParameter(Name = "RecipeId")]
        public int RecipeId { get; set; }
        [CascadingParameter(Name = "InstructionSectionId")]
        public int InstructionSectionId { get; set; }
        [CascadingParameter(Name = "IsEditable")]
        public bool IsEditable { get; set; }

        public bool AddNewInstructionDescriptionIsVisible { get; set; }
        public string NewInstructionDescription { get; set; }

        public InstructionSection InstructionSection { get; set; }

        public readonly static Dictionary<string, object> InputTextAreaAttributes = new Dictionary<string, object> { { "rows", "1" }, { "cols", "25" }, };

        protected override async Task OnInitializedAsync()
        {
            InstructionSection = await RecipeUseCases.GetInstructionSection(RecipeId, InstructionSectionId);
            await base.OnInitializedAsync();
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
        }

        public async Task DescriptionUpdated(int recipeId, int sectionId, int recipeInstructionId, string newInstructionDescription)
        {
            await RecipeUseCases.ChangeRecipeInstructionDescription(recipeId, sectionId, recipeInstructionId, newInstructionDescription);
        }

        public async Task AddRecipeInstruction(int recipeId, int sectionId, string newInstructionDescription)
        {
            RecipeInstruction recipeInstruction = new RecipeInstruction { Description = newInstructionDescription, };
            await RecipeUseCases.AddRecipeInstruction(recipeId, sectionId, recipeInstruction);
            SetNewInstructionNameNotVisible();
        }

        public void SetNewInstructionNameIsVisible()
        {
            AddNewInstructionDescriptionIsVisible = true;
            NewInstructionDescription = string.Empty;
        }

        public void SetNewInstructionNameNotVisible()
        {
            AddNewInstructionDescriptionIsVisible = false;
            NewInstructionDescription = string.Empty;
        }
    }
}