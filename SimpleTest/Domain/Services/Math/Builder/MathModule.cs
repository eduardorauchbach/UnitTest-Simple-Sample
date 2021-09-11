using Autofac;
using Autofac.Integration.Mef;
using System;

namespace SimpleTest.Domain.Services.Math.Builder
{
    public class MathModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            base.Load(builder);
            builder.RegisterMetadataRegistrationSources();

            _ = builder
                .RegisterType<CalculatorService>()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();
        }
    }
}
