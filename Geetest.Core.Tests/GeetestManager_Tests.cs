using System.Threading.Tasks;
using Autofac;
using Xunit;

namespace Geetest.Core.Tests
{
    public class GeetestManager_Tests : GeetestTestsBase
    {
        public GeetestManager_Tests()
        {
            _geetestManager = IocContainer.Resolve<IGeetestManager>();
        }

        private readonly IGeetestManager _geetestManager;

        [Fact]
        public async Task Register_Test()
        {
        }

        [Fact]
        public async Task Validate_Test()
        {
        }
    }
}