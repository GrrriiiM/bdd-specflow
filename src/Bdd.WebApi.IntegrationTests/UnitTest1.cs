using System;
using Xunit;

namespace Bdd.WebApi.IntegrationTests
{
    public class UnitTest1
    {
        [SkippableFact(DisplayName="Bdd.WebApi.IntegrationTests.UnitTest1.Test1")]
        public void Test1()
        {
            Assert.Equal(1,1);
        }
    }
}
