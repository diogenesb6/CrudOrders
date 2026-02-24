import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { getOrders, deleteOrder, updateOrder } from '../api/ordersApi.js';

export default function OrderList() {
  const [orders, setOrders] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  async function loadOrders() {
    try {
      setLoading(true);
      setError(null);
      const data = await getOrders();
      setOrders(data);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    loadOrders();
  }, []);

  async function handleTogglePaid(order) {
    try {
      const payload = {
        customerName: order.customerName,
        customerEmail: order.customerEmail,
        paid: !order.paid,
        orderItems: order.orderItems?.map((i) => ({
          productId: i.productId,
          productName: i.productName,
          unitPrice: i.unitPrice,
          quantity: i.quantity,
        })) || [],
      };
      const updated = await updateOrder(order.id, payload);
      setOrders((prev) => prev.map((o) => (o.id === order.id ? updated : o)));
    } catch (err) {
      setError(err.message);
    }
  }

  async function handleDelete(id) {
    if (!window.confirm(`Delete order #${id}?`)) return;
    try {
      await deleteOrder(id);
      setOrders((prev) => prev.filter((o) => o.id !== id));
    } catch (err) {
      setError(err.message);
    }
  }

  if (loading) {
    return <div className="spinner" />;
  }

  if (error) {
    return (
      <div>
        <div className="error-box">{error}</div>
        <button className="btn btn-primary" onClick={loadOrders}>Retry</button>
      </div>
    );
  }

  if (orders.length === 0) {
    return (
      <div className="empty-state">
        <div className="icon">🛒</div>
        <p>No orders found.</p>
        <Link to="/orders/new" className="btn btn-primary">+ Create First Order</Link>
      </div>
    );
  }

  return (
    <div>
      <div className="page-header">
        <h2>Orders</h2>
        <Link to="/orders/new" className="btn btn-accent">+ New Order</Link>
      </div>
      <div className="card">
        <div className="table-wrapper">
          <table>
            <thead>
              <tr>
                <th>#</th>
                <th>Customer</th>
                <th>Email</th>
                <th>Items</th>
                <th>Total</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {orders.map((order) => (
                <tr key={order.id}>
                  <td>{order.id}</td>
                  <td>{order.customerName}</td>
                  <td>{order.customerEmail}</td>
                  <td>{order.orderItems?.length || 0}</td>
                  <td>${order.totalAmount?.toFixed(2)}</td>
                  <td>
                    <div className="actions-cell">
                      <button
                        className={`badge-btn ${order.paid ? 'badge-btn-paid' : 'badge-btn-unpaid'}`}
                        title={order.paid ? 'Mark as unpaid' : 'Mark as paid'}
                        onClick={() => handleTogglePaid(order)}
                      >
                        {order.paid ? '✓ Paid' : '✕ Unpaid'}
                      </button>
                      <Link to={`/orders/${order.id}`} className="btn-icon" title="View">👁</Link>
                      <Link to={`/orders/${order.id}/edit`} className="btn-icon" title="Edit">✏️</Link>
                      <button
                        className="badge-btn badge-btn-delete"
                        title="Delete"
                        onClick={() => handleDelete(order.id)}
                      >
                        🗑 Delete
                      </button>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
}
