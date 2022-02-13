namespace Bdd.WebApi.Models
{
    public class PedidoCompra
    {
        public virtual int Numero { get; set; }
        public virtual PedidoCompraItem[] Itens { get; set; }
        public virtual decimal Total { get; set; }
    }
}