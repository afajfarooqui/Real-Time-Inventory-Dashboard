namespace Inventory_API.Models
{
    public class InventoryItem
    {
        public int Id { get; set; }
        public string Sku { get; set; } = default!;
        public string Name { get; set; } = default!;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
