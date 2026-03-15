using Microsoft.AspNetCore.SignalR;

namespace Inventory_API.hubs
{
    public class InventoryHub : Hub
    {
        // Broadcast a stock update to all connected clients
        public async Task SendStockUpdate(string sku, int quantity)
        {
            await Clients.All.SendAsync("InventoryUpdated", sku, quantity);
        }

        // Broadcast a stock update to all connected clients
        public async Task SendStockAdd()
        {
            await Clients.All.SendAsync("InventoryAdd");
        }
    }
}
