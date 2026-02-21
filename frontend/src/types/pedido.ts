export interface ItemPedidoDTO {
  id: number;
  idProduto: number;
  nomeProduto: string;
  valorUnitario: number;
  quantidade: number;
}

export interface CriarItemPedidoDTO {
  idProduto: number;
  nomeProduto: string;
  valorUnitario: number;
  quantidade: number;
}

export interface PedidoDTO {
  id: number;
  nomeCliente: string;
  emailCliente: string;
  pago: boolean;
  valorTotal: number;
  itensPedido: ItemPedidoDTO[];
}

export interface CriarPedidoDTO {
  nomeCliente: string;
  emailCliente: string;
  pago: boolean;
  itensPedido: CriarItemPedidoDTO[];
}

export interface AtualizarPedidoDTO {
  nomeCliente: string;
  emailCliente: string;
  pago: boolean;
  itensPedido: CriarItemPedidoDTO[];
}
