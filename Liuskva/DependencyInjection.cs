using JetBrains.Annotations;
using Liuskva.OedApi;
using Liuskva.Utilities;

namespace Liuskva
{
    public static class DependencyInjection
    {
        [NotNull] public static IFactory CreateFactory()
        {
            var factory = new Factory();

            factory.Register<IFactory>(() => factory);

            factory.Register<IBootstrap, Program>();
            factory.RegisterSingleton<IConfiguration, Configuration>();
            factory.Register<IOedClient, OedClient>();
            factory.RegisterSingleton<ISettings>(() => new Settings().Initialise(factory.GetInstance<IConfiguration>()));
            
            return factory;
        }
    }
}
