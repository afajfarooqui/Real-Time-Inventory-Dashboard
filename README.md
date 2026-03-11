Layers:
Client: React + Vite + MUI DataGrid
API: ASP.NET Core Web API 
Real-time: SignalR for inventory updates
Database: SQL Server
Infrastructure: Docker Compose (API, SQL, frontend)

Flow:
Frontend loads inventory via GET /api/inventory.
API publishes a SignalR event (InventoryUpdated, InventoryCreated, etc.).
All connected clients update their DataGrid rows in real time.
