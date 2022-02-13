using System.Text.Json.Serialization;

namespace Bdd.WebApi.Models
{
    public class PostPedidoCompraRequest : PedidoCompra
    {
        [JsonIgnore]
        public override int Numero { get; set; }
        [JsonIgnore]
        public override decimal Total { get; set; }
        [JsonIgnore]
        public override PedidoCompraItem[] Itens => _Itens;

        [JsonPropertyName("Itens")]
        public Item[] _Itens { get; set; }

        public class Item : PedidoCompraItem
        {
            [JsonIgnore]
            public override string ProdutoNome { get; set; }
            [JsonIgnore]
            public override decimal Total { get; set; }
        }
    }
}