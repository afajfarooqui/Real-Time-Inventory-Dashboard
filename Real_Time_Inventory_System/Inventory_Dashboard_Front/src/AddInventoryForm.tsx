import { useState } from "react";
import type { CreateInventoryItem } from "./Inventory_Item";

type AddInventoryFormProps = {
  onSubmit: (item: CreateInventoryItem ) => void;
};

export default function AddInventoryForm({ onSubmit } : AddInventoryFormProps) {
  const [item, setItem] = useState({
    sku: "",
    name: "",
    quantity: 0,
    price : 0
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setItem({ ...item, [e.target.name]: e.target.value });
  };

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    onSubmit(item);
  };

  return (
    <form onSubmit={handleSubmit}>
      <input name="sku" placeholder="SKU" onChange={handleChange} />
      <input name="name" placeholder="Name" onChange={handleChange} />
      <input name="quantity" placeholder="quantity" type="number" onChange={handleChange} />
       <input name="price" placeholder="price" type="number" onChange={handleChange} />
      <button type="submit">Add Item</button>
    </form>
  );
}
