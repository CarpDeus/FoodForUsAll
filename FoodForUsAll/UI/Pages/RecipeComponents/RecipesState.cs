using System;

namespace UI.Pages
{
    public class RecipesState
    {
        public int? RecipeId { get; private set; }
        public bool IsEditable { get; private set; }

        public event Action OnRecipeChange;

        public void SetRecipeId(int? recipeId, bool isEditable)
        {
            RecipeId = recipeId;
            IsEditable = isEditable;
            NotifyOnRecipeChanged();
        }

        void NotifyOnRecipeChanged() => OnRecipeChange?.Invoke();
    }
}
