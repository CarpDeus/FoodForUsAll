namespace Domain
{
    public class RecipeGroup
    {
        public int Id { get; set; }
        public RecipeGroup ParentRecipeGroup { get; set; }
        public string Name { get; set; } = "Name Missing";
    }
}
