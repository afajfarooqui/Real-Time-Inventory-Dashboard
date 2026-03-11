
using Inventory_API.CustomMiddlewares;
using Inventory_API.Data;
using Inventory_API.hubs;
using Inventory_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins("http://localhost:5173")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            builder.Services.AddControllers();
            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'InventoryDBConnection' not found.")));

            builder.Services.AddSignalR();

           

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                // Ensure database exists (for SQL Server, SQLite, etc.)
                db.Database.EnsureCreated();

                if (!db.InventoryItems.Any())
                {
                    db.InventoryItems.AddRange(
                        new InventoryItem
                        {
                           
                            Sku = "TSH-RED-L",
                            Name = "Red T-Shirt (Large)",
                            Quantity = 50
                        },
                        new InventoryItem
                        {
                           
                            Sku = "TSH-BLU-M",
                            Name = "Blue T-Shirt (Medium)",
                            Quantity = 30
                        },
                        new InventoryItem
                        {
                            
                            Sku = "MUG-WHT-001",
                            Name = "White Coffee Mug",
                            Quantity = 120
                        }
                    );

                    db.SaveChanges();
                }
            }


            // Configure the HTTP request pipeline.

            app.UseCors("AllowFrontend");

            //if (app.Environment.IsDevelopment())
            //{
                app.UseSwagger();
                app.UseSwaggerUI();
            //}

            app.UseMiddleware<RandomFailureMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.MapHub<InventoryHub>("/hubs/inventory");

            app.Run();
        }
    }
}
