export  type InventoryItem = {
  id: number;
  sku: string;
  name: string;
  quantity: number;
  price: number;
};

export type CreateInventoryItem = {
  sku: string;
  name: string;
  quantity: number;
  price: number;
};