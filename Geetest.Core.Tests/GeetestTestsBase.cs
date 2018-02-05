using System.Reflection;
using Autofac;
using Geetest.Core.Configuration;

namespace Geetest.Core.Tests
{
    public class GeetestTestsBase
    {
        public GeetestTestsBase()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(IGeetestManager)))
                .AsImplementedInterfaces();

            builder.RegisterType<GeetestConfiguration>().As<IGeetestConfiguration>().SingleInstance();

            var iocContainer = builder.Build();

            var geetestConfig = iocContainer.Resolve<IGeetestConfiguration>();
            geetestConfig.Id = "624b8993f3ff3cf5111cc95a92355da8";
            geetestConfig.Key = "4dd6927f03b8b600ca821830e66bb7f6";
        }

        protected IContainer IocContainer { get; }
    }
}