using System;
using System.Collections.Generic;

namespace Domain
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Name Missing";
        public string Description { get; set; } = string.Empty;
        public Guid AuthorId { get; set; }
        public RecipeImage PrimaryImage { get; set; } = new();
        public List<RecipeImage> SecondaryImages { get; set; } = new List<RecipeImage>();
        public List<IngredientSection> IngredientSections { get; set; } = new List<IngredientSection>();
        public List<InstructionSection> InstructionSections { get; set; } = new List<InstructionSection>();

        public override string ToString()
        {
            return Name + ((!string.IsNullOrEmpty(Description)) ? ": " + Description : string.Empty);
        }
    }
}
