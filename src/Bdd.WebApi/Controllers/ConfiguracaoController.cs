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
    public class ConfiguracaoController : ControllerBase
    {
        internal static readonly Configuracoes Configuracoes = new Configuracoes();

        readonly ILogger<ConfiguracaoController> _logger;

        public ConfiguracaoController(ILogger<ConfiguracaoController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(Configuracoes);
        }

        [HttpPut]
        public IActionResult Put([FromBody]Configuracoes request)
        {
            Configuracoes.PedidoCompraValorMinimo = request.PedidoCompraValorMinimo;

            _logger.LogInformation($"PedidoCompraValorMinimo alterado para {request.PedidoCompraValorMinimo}");

            return Ok();
        }
    }
}
