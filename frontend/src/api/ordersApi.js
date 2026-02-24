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

function mapFromApi(raw) {
  return {
    id: raw.id,
    customerName: raw.nomeCliente,
    customerEmail: raw.emailCliente,
    paid: raw.pago,
    totalAmount: raw.valorTotal,
    orderItems: raw.itensPedido?.map((i) => ({
      id: i.id,
      productId: i.idProduto,
      productName: i.nomeProduto,
      unitPrice: i.valorUnitario,
      quantity: i.quantidade,
    })) || [],
  };
}

function mapToApi(order) {
  return {
    nomeCliente: order.customerName,
    emailCliente: order.customerEmail,
    pago: order.paid,
    itensPedido: order.orderItems?.map((i) => ({
      idProduto: i.productId,
      nomeProduto: i.productName,
      valorUnitario: i.unitPrice,
      quantidade: i.quantity,
    })) || [],
  };
}

export async function getOrders() {
  const res = await fetch(BASE_URL);
  const data = await handleResponse(res);
  return data.map(mapFromApi);
}

export async function getOrderById(id) {
  const res = await fetch(`${BASE_URL}/${id}`);
  const data = await handleResponse(res);
  return mapFromApi(data);
}

export async function createOrder(order) {
  const res = await fetch(BASE_URL, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(mapToApi(order)),
  });
  const data = await handleResponse(res);
  return mapFromApi(data);
}

export async function updateOrder(id, order) {
  const res = await fetch(`${BASE_URL}/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(mapToApi(order)),
  });
  const data = await handleResponse(res);
  return mapFromApi(data);
}

export async function deleteOrder(id) {
  const res = await fetch(`${BASE_URL}/${id}`, { method: 'DELETE' });
  return handleResponse(res);
}
