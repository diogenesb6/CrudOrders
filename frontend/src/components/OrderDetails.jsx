import { useEffect, useState } from 'react';
import { useParams, useNavigate, Link } from 'react-router-dom';
import { getOrderById, deleteOrder } from '../api/ordersApi.js';

export default function OrderDetails() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [order, setOrder] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    async function load() {
      try {
        setLoading(true);
        const data = await getOrderById(id);
        setOrder(data);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    }
    load();
  }, [id]);

  async function handleDelete() {
    if (!window.confirm(`Delete order #${id}?`)) return;
    try {
      await deleteOrder(id);
      navigate('/');
    } catch (err) {
      setError(err.message);
    }
  }

  if (loading) return <div className="spinner" />;

  if (error) {
    return (
      <div>
        <div className="error-box">{error}</div>
        <Link to="/" className="btn btn-secondary">← Back to Orders</Link>
      </div>
    );
  }

  if (!order) return null;

  return (
    <div>
      <div className="page-header">
        <h2>Order #{order.id}</h2>
        <div style={{ display: 'flex', gap: 8 }}>
          <Link to={`/orders/${order.id}/edit`} className="btn btn-primary">✏️ Edit</Link>
          <button className="btn btn-danger" onClick={handleDelete}>🗑 Delete</button>
        </div>
      </div>

      <div className="card">
        <h3 className="card-title">Customer Info</h3>
        <div className="card-body">
          <ul className="info-list">
            <li>
              <span className="label">Name</span>
              <span>{order.customerName}</span>
            </li>
            <li>
              <span className="label">Email</span>
              <span>{order.customerEmail}</span>
            </li>
            <li>
              <span className="label">Paid</span>
              <span className={`badge ${order.paid ? 'badge-paid' : 'badge-unpaid'}`}>
                {order.paid ? 'Paid' : 'Unpaid'}
              </span>
            </li>
            <li>
              <span className="label">Total</span>
              <span className="total-amount">${order.totalAmount?.toFixed(2)}</span>
            </li>
          </ul>
        </div>
      </div>

      <div className="card">
        <h3 className="card-title">Order Items ({order.orderItems?.length || 0})</h3>
        <div className="card-body" style={{ padding: order.orderItems?.length > 0 ? 0 : undefined }}>
          {order.orderItems?.length > 0 ? (
            <div className="table-wrapper">
              <table>
                <thead>
                  <tr>
                    <th>Product ID</th>
                    <th>Product Name</th>
                    <th>Unit Price</th>
                    <th>Quantity</th>
                    <th>Subtotal</th>
                  </tr>
                </thead>
                <tbody>
                  {order.orderItems.map((item) => (
                    <tr key={item.id}>
                      <td>{item.productId}</td>
                      <td>{item.productName}</td>
                      <td>${item.unitPrice?.toFixed(2)}</td>
                      <td>{item.quantity}</td>
                      <td>${(item.unitPrice * item.quantity).toFixed(2)}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          ) : (
            <p style={{ color: '#888' }}>No items in this order.</p>
          )}
        </div>
      </div>

      <div style={{ marginTop: 16 }}>
        <Link to="/" className="btn btn-secondary">← Back to Orders</Link>
      </div>
    </div>
  );
}
