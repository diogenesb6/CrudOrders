import type { PedidoDTO, CriarPedidoDTO, AtualizarPedidoDTO } from '../types/pedido';

const API_BASE = '/api/pedidos';

async function handleResponse<T>(response: Response): Promise<T> {
  if (!response.ok) {
    const body = await response.json().catch(() => null);
    const message = body?.message ?? `Erro ${response.status}`;
    throw new Error(message);
  }
  if (response.status === 204) return undefined as T;
  return response.json();
}

export async function obterTodos(): Promise<PedidoDTO[]> {
  const res = await fetch(API_BASE);
  return handleResponse<PedidoDTO[]>(res);
}

export async function obterPorId(id: number): Promise<PedidoDTO> {
  const res = await fetch(`${API_BASE}/${id}`);
  return handleResponse<PedidoDTO>(res);
}

export async function criar(dto: CriarPedidoDTO): Promise<PedidoDTO> {
  const res = await fetch(API_BASE, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(dto),
  });
  return handleResponse<PedidoDTO>(res);
}

export async function atualizar(id: number, dto: AtualizarPedidoDTO): Promise<PedidoDTO> {
  const res = await fetch(`${API_BASE}/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(dto),
  });
  return handleResponse<PedidoDTO>(res);
}

export async function deletar(id: number): Promise<void> {
  const res = await fetch(`${API_BASE}/${id}`, { method: 'DELETE' });
  return handleResponse<void>(res);
}
