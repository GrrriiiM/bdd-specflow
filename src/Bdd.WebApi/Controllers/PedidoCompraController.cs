using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bdd.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bdd.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PedidoCompraController : ControllerBase
    {
        internal static readonly List<PedidoCompra> Pedidos = new List<PedidoCompra>();
        readonly ILogger<PedidoCompraController> _logger;

        public PedidoCompraController(ILogger<PedidoCompraController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(Pedidos);
        }

        [HttpPost]
        public IActionResult Post([FromBody]PostPedidoCompraRequest request, [FromServices]Configuracoes config)
        {
            var pedido = new PedidoCompra
            {
                Numero = Pedidos.Any() ? Pedidos.Max(_ => _.Numero) + 1 : 1,
                Itens = request.Itens.Select(_ => new PedidoCompraItem
                {
                    Sku = _.Sku,
                    PrecoUnitario = _.PrecoUnitario,
                    Quantidade = _.Quantidade,
                    ProdutoNome = "Teste Produto",
                    Total = _.PrecoUnitario * _.Quantidade
                }).ToArray()
            };
            pedido.Total = pedido.Itens.Sum(_ => _.Total);

            if (pedido.Total < config.PedidoCompraValorMinimo) return BadRequest(new 
            {
                Mensagem = "Total do pedido abaixo do valor minimo permitido",
                ValorMinimo = config.PedidoCompraValorMinimo,
                TotalPedido = pedido.Total
            });

            Pedidos.Add(pedido);

            return Created($"/{pedido.Numero}", pedido);
        }
    }
}
