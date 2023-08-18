namespace YemekSiparisSistemi.Models
{
    public class ShoppingCardItem
    {
        public int Id { get; set; }
        public Product? Product { get; set; }

        public Menu? Menu { get; set; }

        public int Quantity { get; set; }
    }
}
