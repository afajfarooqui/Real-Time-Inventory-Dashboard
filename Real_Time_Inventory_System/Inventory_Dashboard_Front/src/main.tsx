import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
//import App from './App.tsx'
import InventoryGrid from './InventoryGrid.tsx'

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <InventoryGrid/>
  </StrictMode>,
)
