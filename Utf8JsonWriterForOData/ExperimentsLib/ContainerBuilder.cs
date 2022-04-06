using OData = Microsoft.OData;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utf8JsonWriterSamples
{
    internal class ContainerBuilder : OData.IContainerBuilder
    {
        private readonly ServiceCollection services = new ServiceCollection();
        public OData.IContainerBuilder AddService(OData.ServiceLifetime lifetime, Type serviceType, Type implementationType)
        {
            switch (lifetime)
            {
                case OData.ServiceLifetime.Transient:
                    services.AddTransient(serviceType, implementationType);
                    break;
                case OData.ServiceLifetime.Singleton:
                    services.AddSingleton(serviceType, implementationType);
                    break;
                case OData.ServiceLifetime.Scoped:
                    services.AddScoped(serviceType, implementationType);
                    break;
            }

            return this;
        }

        public OData.IContainerBuilder AddService(OData.ServiceLifetime lifetime, Type serviceType, Func<IServiceProvider, object> implementationFactory)
        {
            switch (lifetime)
            {
                case OData.ServiceLifetime.Transient:
                    services.AddTransient(serviceType, implementationFactory);
                    break;
                case OData.ServiceLifetime.Singleton:
                    services.AddSingleton(serviceType, implementationFactory);
                    break;
                case OData.ServiceLifetime.Scoped:
                    services.AddScoped(serviceType, implementationFactory);
                    break;
            }

            return this;
        }

        public IServiceProvider BuildContainer()
        {
            return services.BuildServiceProvider();
        }
    }
}
