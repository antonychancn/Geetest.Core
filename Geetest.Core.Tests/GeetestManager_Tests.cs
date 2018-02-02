using System;
using System.Threading.Tasks;
using Autofac;
using Shouldly;
using Xunit;

namespace Geetest.Core.Tests
{
    public class GeetestManager_Tests : GeetestTestsBase
    {
        private readonly IGeetestManager _geetestManager;

        public GeetestManager_Tests()
        {
            _geetestManager = IocContainer.Resolve<IGeetestManager>();
        }

        [Fact]
        public async Task Register_Test()
        {
            var reg = await _geetestManager.Register();
            reg.Success.ShouldBeTrue();
        }
    }
}
