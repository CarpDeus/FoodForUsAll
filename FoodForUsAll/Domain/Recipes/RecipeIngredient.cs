namespace Domain
{
    public class RecipeIngredient
    {
        public int Id { get; set; }
        public string Quantity { get; set; }
        public int OrderId { get; set; }
        public Ingredient Ingredient { get; set; }
    }
}
