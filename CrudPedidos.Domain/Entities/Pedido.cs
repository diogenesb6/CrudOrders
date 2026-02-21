namespace CrudPedidos.Domain.Entities;

public class Pedido
{
    public int Id { get; set; }
    public string NomeCliente { get; set; } = string.Empty;
    public string EmailCliente { get; set; } = string.Empty;
    public bool Pago { get; set; }
    public decimal ValorTotal { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public DateTime? DataAtualizacao { get; set; }

    private List<ItemPedido> _itensPedido = new();
    public IReadOnlyCollection<ItemPedido> ItensPedido => _itensPedido.AsReadOnly();

    public Pedido() { }

    public Pedido(string nomeCliente, string emailCliente, List<ItemPedido> itensPedido)
    {
        ValidarPedido(nomeCliente, emailCliente, itensPedido);

        NomeCliente = nomeCliente;
        EmailCliente = emailCliente;
        _itensPedido = itensPedido;
        CalcularValorTotal();
    }

    public void AdicionarItem(ItemPedido item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        item.PedidoId = Id;
        _itensPedido.Add(item);
        CalcularValorTotal();
    }

    public void RemoverItem(int itemId)
    {
        var item = _itensPedido.FirstOrDefault(i => i.Id == itemId);
        if (item != null)
        {
            _itensPedido.Remove(item);
            CalcularValorTotal();
        }
    }

    public void AtualizarPedido(string nomeCliente, string emailCliente, bool pago)
    {
        if (string.IsNullOrWhiteSpace(nomeCliente))
            throw new ArgumentException("Nome do cliente é obrigatório", nameof(nomeCliente));

        if (string.IsNullOrWhiteSpace(emailCliente))
            throw new ArgumentException("Email do cliente é obrigatório", nameof(emailCliente));

        NomeCliente = nomeCliente;
        EmailCliente = emailCliente;
        Pago = pago;
        DataAtualizacao = DateTime.UtcNow;
    }

    public void CalcularValorTotal()
    {
        ValorTotal = _itensPedido.Sum(item => item.CalcularSubtotal());
    }

    private static void ValidarPedido(string nomeCliente, string emailCliente, List<ItemPedido> itensPedido)
    {
        if (string.IsNullOrWhiteSpace(nomeCliente))
            throw new ArgumentException("Nome do cliente é obrigatório", nameof(nomeCliente));

        if (string.IsNullOrWhiteSpace(emailCliente))
            throw new ArgumentException("Email do cliente é obrigatório", nameof(emailCliente));

        if (itensPedido == null || itensPedido.Count == 0)
            throw new ArgumentException("Pedido deve conter pelo menos um item", nameof(itensPedido));
    }
}
