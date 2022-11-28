namespace WorldOfPowerTools.CartService.Models
{
#nullable disable
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public Category Category { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public bool Availability { get; set; }
    }
}
