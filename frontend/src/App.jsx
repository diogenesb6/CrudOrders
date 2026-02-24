import { Routes, Route, Link, useLocation } from 'react-router-dom';
import OrderList from './components/OrderList.jsx';
import OrderForm from './components/OrderForm.jsx';
import OrderDetails from './components/OrderDetails.jsx';

export default function App() {
  const location = useLocation();

  return (
    <>
      <header className="app-header">
        <Link to="/" className="logo">CrudOrders</Link>
        <nav>
          <Link to="/">Orders</Link>
          {location.pathname !== '/orders/new' && (
            <Link to="/orders/new">+ New Order</Link>
          )}
        </nav>
      </header>
      <main className="page-content">
        <Routes>
          <Route path="/" element={<OrderList />} />
          <Route path="/orders/new" element={<OrderForm />} />
          <Route path="/orders/:id" element={<OrderDetails />} />
          <Route path="/orders/:id/edit" element={<OrderForm />} />
        </Routes>
      </main>
    </>
  );
}
