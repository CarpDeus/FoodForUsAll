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
        public List<IngredientSection> IngredientSections { get; set; } = new List<IngredientSection>();
        public List<InstructionSection> InstructionSections { get; set; } = new List<InstructionSection>();

        public override string ToString()
        {
            return Name + ((!string.IsNullOrEmpty(Description)) ? ": " + Description : string.Empty);
        }
    }
}
