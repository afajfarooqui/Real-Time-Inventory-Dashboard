using Inventory_API.Data;
using Inventory_API.DTO;
using Inventory_API.hubs;
using Inventory_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Inventory_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]    
    public class InventoryController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IHubContext<InventoryHub> _hub;

        public InventoryController(AppDbContext db, IHubContext<InventoryHub> hub)
        {
            _db = db;
            _hub = hub;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryItem>>> Get()
       => await _db.InventoryItems.AsNoTracking().ToListAsync();

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, InventoryItem dto)
        {
            var item = await _db.InventoryItems.FindAsync(id);
            if (item is null) return NotFound();

            item.Quantity = dto.Quantity;
            item.Price = dto.Price;
            item.Name = dto.Name;
            item.Sku = dto.Sku;

            await _db.SaveChangesAsync();

            // Broadcast update
            await _hub.Clients.All.SendAsync("InventoryUpdated", item);

            return Ok(item);
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> Get(int id)
        {
            var item = await _db.InventoryItems.FindAsync(id);

            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Post(InventoryItemCreate dto)
        {
            var model = new InventoryItem();
            model.Sku = dto.Sku;
            model.Name = dto.Name;
            model.Quantity = dto.Quantity;
            model.Price = dto.Price;
        
           

            _db.InventoryItems.Add(model);
            await _db.SaveChangesAsync();

            // Broadcast add
            await _hub.Clients.All.SendAsync("InventoryAdd", model);

            //    return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
            return CreatedAtAction(nameof(Get), new { id = model.Id }, model);

        }
    }
}
