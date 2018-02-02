using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Autofac;
using Geetest.Core.Configuration;

namespace Geetest.Core.Tests
{
    public class GeetestTestsBase
    {
        protected IContainer IocContainer { get; }

        public GeetestTestsBase()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(IGeetestManager)))
                .AsImplementedInterfaces();

            builder.Register(c => new GeetestConfiguration
            {
                Id = "624b8993f3ff3cf5111cc95a92355da8",
                Key = "4dd6927f03b8b600ca821830e66bb7f6"

            }).As<IGeetestConfiguration>().SingleInstance();

            IocContainer = builder.Build();
        }
    }
}
