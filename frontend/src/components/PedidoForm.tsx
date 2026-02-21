import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import type { CriarItemPedidoDTO } from '../types/pedido';
import { obterPorId, criar, atualizar } from '../services/api';
import './PedidoForm.css';

const itemVazio: CriarItemPedidoDTO = {
  idProduto: 0,
  nomeProduto: '',
  valorUnitario: 0,
  quantidade: 1,
};

export default function PedidoForm() {
  const { id } = useParams<{ id: string }>();
  const isEditing = id !== undefined;
  const navigate = useNavigate();

  const [nomeCliente, setNomeCliente] = useState('');
  const [emailCliente, setEmailCliente] = useState('');
  const [pago, setPago] = useState(false);
  const [itens, setItens] = useState<CriarItemPedidoDTO[]>([{ ...itemVazio }]);
  const [erro, setErro] = useState('');
  const [salvando, setSalvando] = useState(false);

  useEffect(() => {
    if (isEditing) {
      obterPorId(Number(id))
        .then((p) => {
          setNomeCliente(p.nomeCliente);
          setEmailCliente(p.emailCliente);
          setPago(p.pago);
          setItens(
            p.itensPedido.map((i) => ({
              idProduto: i.idProduto,
              nomeProduto: i.nomeProduto,
              valorUnitario: i.valorUnitario,
              quantidade: i.quantidade,
            })),
          );
        })
        .catch((e) => setErro(e instanceof Error ? e.message : 'Erro ao carregar pedido'));
    }
  }, [id, isEditing]);

  const handleItemChange = (index: number, field: keyof CriarItemPedidoDTO, value: string | number) => {
    setItens((prev) =>
      prev.map((item, i) => (i === index ? { ...item, [field]: value } : item)),
    );
  };

  const adicionarItem = () => setItens((prev) => [...prev, { ...itemVazio }]);

  const removerItem = (index: number) => {
    setItens((prev) => prev.filter((_, i) => i !== index));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setErro('');
    setSalvando(true);

    const dto = { nomeCliente, emailCliente, pago, itensPedido: itens };

    try {
      if (isEditing) {
        await atualizar(Number(id), dto);
      } else {
        await criar(dto);
      }
      navigate('/');
    } catch (err) {
      setErro(err instanceof Error ? err.message : 'Erro ao salvar pedido');
    } finally {
      setSalvando(false);
    }
  };

  return (
    <div className="pedido-form-container">
      <h1>{isEditing ? `Editar Pedido #${id}` : 'Novo Pedido'}</h1>

      {erro && <p className="error">{erro}</p>}

      <form onSubmit={handleSubmit} className="pedido-form">
        <fieldset>
          <legend>Dados do Cliente</legend>

          <div className="field">
            <label htmlFor="nomeCliente">Nome do Cliente</label>
            <input
              id="nomeCliente"
              type="text"
              required
              value={nomeCliente}
              onChange={(e) => setNomeCliente(e.target.value)}
            />
          </div>

          <div className="field">
            <label htmlFor="emailCliente">Email do Cliente</label>
            <input
              id="emailCliente"
              type="email"
              required
              value={emailCliente}
              onChange={(e) => setEmailCliente(e.target.value)}
            />
          </div>

          <div className="field checkbox-field">
            <label>
              <input
                type="checkbox"
                checked={pago}
                onChange={(e) => setPago(e.target.checked)}
              />
              Pago
            </label>
          </div>
        </fieldset>

        <fieldset>
          <legend>Itens do Pedido</legend>

          {itens.map((item, index) => (
            <div key={index} className="item-row">
              <input
                type="number"
                placeholder="ID Produto"
                required
                min={1}
                value={item.idProduto || ''}
                onChange={(e) => handleItemChange(index, 'idProduto', Number(e.target.value))}
              />
              <input
                type="text"
                placeholder="Nome do Produto"
                required
                value={item.nomeProduto}
                onChange={(e) => handleItemChange(index, 'nomeProduto', e.target.value)}
              />
              <input
                type="number"
                placeholder="Valor Unitário"
                required
                min={0.01}
                step={0.01}
                value={item.valorUnitario || ''}
                onChange={(e) => handleItemChange(index, 'valorUnitario', Number(e.target.value))}
              />
              <input
                type="number"
                placeholder="Qtd"
                required
                min={1}
                value={item.quantidade || ''}
                onChange={(e) => handleItemChange(index, 'quantidade', Number(e.target.value))}
              />
              {itens.length > 1 && (
                <button type="button" className="btn btn-sm btn-danger" onClick={() => removerItem(index)}>
                  ✕
                </button>
              )}
            </div>
          ))}

          <button type="button" className="btn btn-secondary" onClick={adicionarItem}>
            + Adicionar Item
          </button>
        </fieldset>

        <div className="form-actions">
          <button type="submit" className="btn btn-primary" disabled={salvando}>
            {salvando ? 'Salvando...' : isEditing ? 'Atualizar' : 'Criar Pedido'}
          </button>
          <button type="button" className="btn btn-secondary" onClick={() => navigate('/')}>
            Cancelar
          </button>
        </div>
      </form>
    </div>
  );
}
