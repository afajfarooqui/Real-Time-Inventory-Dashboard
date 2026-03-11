// InventoryGrid.jsx
import  { useEffect, useState, useMemo } from "react";
import { DataGrid, type GridColDef } from "@mui/x-data-grid";
import * as signalR from "@microsoft/signalr";
import axios from "axios";
import type { InventoryItem } from "./Inventory_Item";
import AddInventoryForm from "./AddInventoryForm";

const API_BASE = "http://localhost:7125"; // adjust

export default function InventoryGrid() {  
  const [rows, setRows] = useState<InventoryItem[]>([]);
  const [loading, setLoading] = useState(true);

  // Fetch initial data
  useEffect(() => {
    let cancelled = false;

    const fetchData = async () => {
      try {
        const res = await axios.get(`${API_BASE}/api/inventory`);
        if (!cancelled) setRows(res.data);
      } catch (err) {
        console.error("API error:", err);
        // show toast / retry if needed
      } finally {
        if (!cancelled) setLoading(false);
      }
    };

    fetchData();
    return () => { cancelled = true; };
  }, []);

  // SignalR connection
  useEffect(() => {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl(`${API_BASE}/hubs/inventory`)
      .withAutomaticReconnect()
      .build();

    connection.on("InventoryUpdated", (item) => {
      setRows((prev) => {
        const idx = prev.findIndex((r) => r.id === item.id);
        if (idx === -1) return [...prev, item];
        const copy = [...prev];
        copy[idx] = item;
        return copy;
      });
    });

    connection
      .start()
      .catch((err) => console.error("SignalR connection error:", err));

    return () => {
      connection.stop();
    };
  }, []);

  // const columns = useMemo(
  //   () => [
  //     { field: "id", headerName: "ID", width: 70 },
  //     { field: "sku", headerName: "SKU", flex: 1 },
  //     { field: "name", headerName: "Name", flex: 1.5 },
  //     { field: "quantity", headerName: "Qty", type: "number", width: 100 },
  //     { field: "price", headerName: "Price", type: "number", width: 120 },
  //   ],
  //   []
  // );


  const columns: GridColDef<InventoryItem>[] = useMemo(
    () => [
  { field: "id", headerName: "ID", width: 90 },
  { field: "sku", headerName: "SKU", flex: 1 },
  { field: "name", headerName: "Name", flex: 1 },
  { field: "quantity", headerName: "Quantity", width: 120 },
   { field: "price", headerName: "Price", type: "number", width: 120 },
], []
  );

  async function handleAdd(item: any) {
    const newItem = await addInventory(item);
    setRows(prev => [...prev, newItem]);
  }

  async function addInventory(item: any) {
  const response = await fetch(`${API_BASE}/api/inventory`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(item)
  });

  if (!response.ok) {
    throw new Error("Failed to add item");
  }

  return await response.json();
}

  return (
     <>
      <AddInventoryForm onSubmit={handleAdd} />
    <div style={{ height: 600, width: "100%" }}>
    
      <DataGrid
        rows={rows}
        columns={columns}
        loading={loading}
        disableRowSelectionOnClick
        density="compact"
        pageSizeOptions={[25, 50, 100]}
        initialState={{
          pagination: { paginationModel: { pageSize: 25 } },
        }}
      />
    </div>
    </>
  );
}


