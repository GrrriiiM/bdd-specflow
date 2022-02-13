namespace Bdd.WebApi.Models
{
    public class PedidoCompraItem
    {
        public virtual string Sku { get; set; }
        public virtual string ProdutoNome { get; set; }
        public virtual int Quantidade { get; set; }
        public virtual decimal PrecoUnitario { get; set; }
        public virtual decimal Total { get; set; }
    }
}