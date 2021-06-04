using System;

namespace Domain
{
    public class RecipeCard
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Name Missing";
        public string Description { get; set; } = string.Empty;
        public Guid AuthorId { get; set; }
        public RecipeImage PrimaryImage { get; set; } = new();

        public override string ToString()
        {
            return Name + ((!string.IsNullOrEmpty(Description)) ? ": " + Description : string.Empty);
        }
    }
}
