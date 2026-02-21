import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import type { PedidoDTO } from '../types/pedido';
import { obterTodos, deletar } from '../services/api';
import './PedidoList.css';

export default function PedidoList() {
  const [pedidos, setPedidos] = useState<PedidoDTO[]>([]);
  const [erro, setErro] = useState('');
  const [carregando, setCarregando] = useState(true);

  const carregar = async () => {
    try {
      setCarregando(true);
      setErro('');
      const data = await obterTodos();
      setPedidos(data);
    } catch (e) {
      setErro(e instanceof Error ? e.message : 'Erro ao carregar pedidos');
    } finally {
      setCarregando(false);
    }
  };

  useEffect(() => {
    carregar();
  }, []);

  const handleDeletar = async (id: number) => {
    if (!confirm(`Deseja realmente excluir o pedido #${id}?`)) return;
    try {
      await deletar(id);
      setPedidos((prev) => prev.filter((p) => p.id !== id));
    } catch (e) {
      setErro(e instanceof Error ? e.message : 'Erro ao deletar pedido');
    }
  };

  if (carregando) return <p className="loading">Carregando pedidos...</p>;

  return (
    <div className="pedido-list">
      <div className="pedido-list-header">
        <h1>Pedidos</h1>
        <Link to="/pedidos/novo" className="btn btn-primary">
          + Novo Pedido
        </Link>
      </div>

      {erro && <p className="error">{erro}</p>}

      {pedidos.length === 0 ? (
        <p className="empty">Nenhum pedido cadastrado.</p>
      ) : (
        <table className="pedido-table">
          <thead>
            <tr>
              <th>ID</th>
              <th>Cliente</th>
              <th>Email</th>
              <th>Pago</th>
              <th>Valor Total</th>
              <th>Itens</th>
              <th>Ações</th>
            </tr>
          </thead>
          <tbody>
            {pedidos.map((p) => (
              <tr key={p.id}>
                <td>{p.id}</td>
                <td>{p.nomeCliente}</td>
                <td>{p.emailCliente}</td>
                <td>
                  <span className={`badge ${p.pago ? 'badge-ok' : 'badge-pending'}`}>
                    {p.pago ? 'Sim' : 'Não'}
                  </span>
                </td>
                <td>R$ {p.valorTotal.toFixed(2)}</td>
                <td>{p.itensPedido.length}</td>
                <td className="actions">
                  <Link to={`/pedidos/${p.id}`} className="btn btn-sm btn-secondary">
                    Editar
                  </Link>
                  <button
                    className="btn btn-sm btn-danger"
                    onClick={() => handleDeletar(p.id)}
                  >
                    Excluir
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
}
