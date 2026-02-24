import { useEffect, useState } from 'react';
import { useParams, useNavigate, Link } from 'react-router-dom';
import { createOrder, updateOrder, getOrderById } from '../api/ordersApi.js';

const emptyItem = () => ({ productId: '', productName: '', unitPrice: '', quantity: '' });

export default function OrderForm() {
  const { id } = useParams();
  const isEdit = Boolean(id);
  const navigate = useNavigate();

  const [customerName, setCustomerName] = useState('');
  const [customerEmail, setCustomerEmail] = useState('');
  const [paid, setPaid] = useState(false);
  const [items, setItems] = useState([emptyItem()]);
  const [loading, setLoading] = useState(false);
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState(null);

  useEffect(() => {
    if (!isEdit) return;
    async function load() {
      try {
        setLoading(true);
        const data = await getOrderById(id);
        setCustomerName(data.customerName);
        setCustomerEmail(data.customerEmail);
        setPaid(data.paid);
        setItems(
          data.orderItems?.length > 0
            ? data.orderItems.map((i) => ({
                productId: i.productId.toString(),
                productName: i.productName,
                unitPrice: i.unitPrice.toString(),
                quantity: i.quantity.toString(),
              }))
            : [emptyItem()]
        );
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    }
    load();
  }, [id, isEdit]);

  function updateItem(index, field, value) {
    setItems((prev) => prev.map((item, i) => (i === index ? { ...item, [field]: value } : item)));
  }

  function addItem() {
    setItems((prev) => [...prev, emptyItem()]);
  }

  function removeItem(index) {
    setItems((prev) => (prev.length === 1 ? [emptyItem()] : prev.filter((_, i) => i !== index)));
  }

  async function handleSubmit(e) {
    e.preventDefault();
    setError(null);

    const orderItems = items
      .filter((i) => i.productName.trim() !== '')
      .map((i) => ({
        productId: parseInt(i.productId) || 0,
        productName: i.productName,
        unitPrice: parseFloat(i.unitPrice) || 0,
        quantity: parseInt(i.quantity) || 0,
      }));

    const payload = { customerName, customerEmail, paid, orderItems };

    try {
      setSubmitting(true);
      if (isEdit) {
        await updateOrder(id, payload);
        navigate(`/orders/${id}`);
      } else {
        const created = await createOrder(payload);
        navigate(`/orders/${created.id}`);
      }
    } catch (err) {
      setError(err.message);
    } finally {
      setSubmitting(false);
    }
  }

  if (loading) return <div className="spinner" />;

  return (
    <div>
      <div className="page-header">
        <h2>{isEdit ? `Edit Order #${id}` : 'New Order'}</h2>
      </div>

      {error && <div className="error-box">{error}</div>}

      <form onSubmit={handleSubmit}>
        <div className="card">
          <h3 className="card-title">Customer Info</h3>
          <div className="card-body">
            <div className="form-group">
              <label htmlFor="customerName">Customer Name</label>
              <input
                type="text"
                id="customerName"
                value={customerName}
                onChange={(e) => setCustomerName(e.target.value)}
                required
                placeholder="Enter customer name"
              />
            </div>
            <div className="form-group">
              <label htmlFor="customerEmail">Customer Email</label>
              <input
                type="email"
                id="customerEmail"
                value={customerEmail}
                onChange={(e) => setCustomerEmail(e.target.value)}
                required
                placeholder="Enter customer email"
              />
            </div>
            <div className="checkbox-group">
              <input
                type="checkbox"
                id="paid"
                checked={paid}
                onChange={(e) => setPaid(e.target.checked)}
              />
              <label htmlFor="paid">Paid</label>
            </div>
          </div>
        </div>

        <div className="card">
          <h3 className="card-title">Order Items</h3>
          <div className="card-body">
            {items.map((item, index) => (
              <div key={index} className="order-item-row">
                <div className="form-group">
                  <label htmlFor={`productId-${index}`}>Product ID</label>
                  <input
                    type="number"
                    id={`productId-${index}`}
                    value={item.productId}
                    onChange={(e) => updateItem(index, 'productId', e.target.value)}
                    min="0"
                    placeholder="0"
                  />
                </div>
                <div className="form-group">
                  <label htmlFor={`productName-${index}`}>Product Name</label>
                  <input
                    type="text"
                    id={`productName-${index}`}
                    value={item.productName}
                    onChange={(e) => updateItem(index, 'productName', e.target.value)}
                    required
                    placeholder="Product name"
                  />
                </div>
                <div className="form-group">
                  <label htmlFor={`unitPrice-${index}`}>Unit Price</label>
                  <input
                    type="number"
                    id={`unitPrice-${index}`}
                    value={item.unitPrice}
                    onChange={(e) => updateItem(index, 'unitPrice', e.target.value)}
                    step="0.01"
                    min="0"
                    placeholder="0.00"
                  />
                </div>
                <div className="form-group">
                  <label htmlFor={`quantity-${index}`}>Qty</label>
                  <input
                    type="number"
                    id={`quantity-${index}`}
                    value={item.quantity}
                    onChange={(e) => updateItem(index, 'quantity', e.target.value)}
                    min="1"
                    placeholder="1"
                  />
                </div>
                <button type="button" className="btn-icon danger" title="Remove item" onClick={() => removeItem(index)}>
                  ✕
                </button>
              </div>
            ))}
            <button type="button" className="btn btn-secondary" onClick={addItem} style={{ marginTop: 8 }}>
              + Add Item
            </button>
          </div>
        </div>

        <div className="form-actions">
          <button type="submit" className="btn btn-primary" disabled={submitting}>
            {submitting ? 'Saving...' : isEdit ? 'Update Order' : 'Create Order'}
          </button>
          <Link to={isEdit ? `/orders/${id}` : '/'} className="btn btn-secondary">
            Cancel
          </Link>
        </div>
      </form>
    </div>
  );
}
