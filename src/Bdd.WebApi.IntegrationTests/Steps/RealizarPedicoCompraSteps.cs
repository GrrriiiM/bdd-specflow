using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Bdd.WebApi.IntegrationTests.Steps
{
    [Binding]
    public class RealizarPedicoCompraSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly HttpClient _httpClient;

        public RealizarPedicoCompraSteps(ScenarioContext scenarioContext, HttpClient httpClient)
        {
            _scenarioContext = scenarioContext;
            _httpClient = httpClient;
        }

        [Given(@"configuracao com valor minimo de (.*)")]
        public async Task ConfiguracaoComValorMinimoDe(int valorMinimo)
        {
            var data = new { pedidoCompraValorMinimo = valorMinimo };
            var response = await _httpClient.PutAsync("/Configuracao", new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
        }

        [Given(@"os seguintes produtos no pedido")]
        public void GivenOsSeguintesProdutosNoPedido(Table table)
        {
            var itens = table.Rows.Select(_ =>
                new
                {
                    sku = _["sku"],
                    quantidade = Convert.ToInt32(_["quantidade"]),
                    precoUnitario = Convert.ToDecimal(_["preco_unitario"])
                });
            _scenarioContext.Add("itens", itens);
        }

        [When(@"pedido realizado")]
        public async Task WhenPedidoConcluido()
        {
            var data = new
            {
                itens = _scenarioContext["itens"]
            };
            var response = await _httpClient.PostAsync("http://localhost:5000/PedidoCompra", new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json"));
            _scenarioContext.Add("response", response);
        }

        [Then(@"pedido deve ser concluido")]
        public void ThenPedidoDeveSerRealizado()
        {
            var response = (HttpResponseMessage)_scenarioContext["response"];
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Then(@"pedido NAO deve ser concluido")]
        public void ThenPedidoNaoDeveSerConcluido()
        {
            var response = (HttpResponseMessage)_scenarioContext["response"];
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Given(@"os estoques com os seguintes produtos:")]
        public void GivenOsEstoquesComOsSeguintesProdutos(Table table)
        {
            _scenarioContext.Add("estoques", table.Rows.ToDictionary(_ => _["sku"], _ => Convert.ToInt32(_["estoque"])));
        }

        [When(@"solicitado o produto (.*) com quantidade (.*)")]
        public void WhenSolicitadoOProdutoSkuComQuantidadeQuantidade(string sku, int quantidade)
        {
            var estoques = (Dictionary<string, int>)_scenarioContext["estoques"];
            var estoque = estoques[sku];
            _scenarioContext.Add("permitido", estoque >= quantidade ? "SIM" : "NAO");
        }

        [Then(@"(.*) podera ser permitido seguir com a compra")]
        public void ThenPermitidoPoderaSerPermitidoSeguirComACompra(string permitido)
        {
            _scenarioContext["permitido"].Should().Be(permitido);
        }
    }
}