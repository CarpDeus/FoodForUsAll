using System.Collections.Generic;

namespace Domain
{
    public class InstructionSection : RecipeSection
    {
        public List<InstructionSection> Children { get; set; } = new List<InstructionSection>();
        public List<RecipeInstruction> RecipeInstructions { get; set; } = new List<RecipeInstruction>();
    }
}
