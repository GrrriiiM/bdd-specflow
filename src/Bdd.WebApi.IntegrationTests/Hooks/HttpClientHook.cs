using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Bdd.WebApi.IntegrationTests.Steps;
using BoDi;
using Microsoft.Extensions.DependencyInjection;
using TechTalk.SpecFlow;

namespace Bdd.WebApi.IntegrationTests.Hooks
{
    [Binding]
    public class HttpClientHook
    {
        readonly IObjectContainer _objectContainer;
        private IServiceScope _serviceScope;

        public HttpClientHook(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }
        
        [BeforeScenario()]
        public void Start()
        {
            
            var services = new ServiceCollection();
            services.AddHttpClient("RealizarPedicoCompra")
                .ConfigureHttpClient(c => {
                    c.BaseAddress = new Uri("http://localhost:5000");
                });
                    
            var serviceProvider = services.BuildServiceProvider();
            _serviceScope = serviceProvider.CreateScope();
            _objectContainer.RegisterFactoryAs<HttpClient>(_ => _serviceScope.ServiceProvider.GetService<IHttpClientFactory>().CreateClient("RealizarPedicoCompra"));
        }

        [AfterScenario()]
        public void End()
        {
            _objectContainer.Dispose();
            _serviceScope.Dispose();
        }
    }
}