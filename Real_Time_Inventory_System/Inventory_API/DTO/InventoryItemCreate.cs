namespace Inventory_API.DTO
{
    public class InventoryItemCreate
    {
        public string Sku { get; set; } = default!;
        public string Name { get; set; } = default!;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
