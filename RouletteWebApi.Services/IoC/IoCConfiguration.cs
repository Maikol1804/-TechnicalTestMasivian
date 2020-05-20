using Autofac;
using RouletteWebApi.DataAccess;
using RouletteWebApi.DataAccess.Context;
using RouletteWebApi.DataAccess.Interfaces;
using RouletteWebApi.Services.Contracts;
using RouletteWebApi.Services.Implementations;
using RouletteWebApi.Services.Interfaces;
using RouletteWebApi.Servicios;

namespace RouletteWebApi.Services.IoC
{
    public class IoCConfiguration : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            RegisterContext(builder);
            RegisterServices(builder);
            RegisterRepositories(builder);
        }

        private static void RegisterContext(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(RouletteContext)).As(typeof(IContext)).InstancePerLifetimeScope();
        }

        private static void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<AdministrationServices>().As<IAdministrationServices>();
        }

        private static void RegisterRepositories(ContainerBuilder builder)
        {
            builder.RegisterType<BetRepository>().As<IBet>();
            builder.RegisterType<RouletteRepository>().As<IRoulette>();
            builder.RegisterType<PlayerRepository>().As<IPlayer>();
        }

    }
}
