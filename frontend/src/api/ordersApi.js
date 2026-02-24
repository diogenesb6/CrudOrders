const BASE_URL = '/api/orders';

async function handleResponse(response) {
  if (!response.ok) {
    const body = await response.json().catch(() => null);
    const message = body?.message || `Error: ${response.status}`;
    throw new Error(message);
  }
  if (response.status === 204) return null;
  return response.json();
}

export async function getOrders() {
  const res = await fetch(BASE_URL);
  return handleResponse(res);
}

export async function getOrderById(id) {
  const res = await fetch(`${BASE_URL}/${id}`);
  return handleResponse(res);
}

export async function createOrder(order) {
  const res = await fetch(BASE_URL, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(order),
  });
  return handleResponse(res);
}

export async function updateOrder(id, order) {
  const res = await fetch(`${BASE_URL}/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(order),
  });
  return handleResponse(res);
}

export async function deleteOrder(id) {
  const res = await fetch(`${BASE_URL}/${id}`, { method: 'DELETE' });
  return handleResponse(res);
}
