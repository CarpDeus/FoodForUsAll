using System.Collections.Generic;

namespace Domain
{
    public class IngredientSection : RecipeSection
    {
        public List<IngredientSection> Children { get; set; } = new List<IngredientSection>();
        public List<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
    }
}
