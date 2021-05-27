namespace Domain
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Name Missing";
        public string Description { get; set; } = string.Empty;

        public override string ToString()
        {
            return Name + ((!string.IsNullOrEmpty(Description)) ? ": " + Description : string.Empty);
        }
    }
}
