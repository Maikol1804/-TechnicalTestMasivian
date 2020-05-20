using Autofac;
using RouletteWebApi.DataAccess;
using RouletteWebApi.DataAccess.Context;
using RouletteWebApi.DataAccess.Interfaces;
using RouletteWebApi.Services.Contracts;
using RouletteWebApi.Services.Implementations;
using RouletteWebApi.Services.Interfaces;

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
            builder.RegisterType<RouletteServices>().As<IRouletteService>();
            builder.RegisterType<BetServices>().As<IBetService>();
            builder.RegisterType<PlayerServices>().As<IPlayerService>();
        }

        private static void RegisterRepositories(ContainerBuilder builder)
        {
            builder.RegisterType<RouletteRepository>().As<IRoulette>();
            builder.RegisterType<BetRepository>().As<IBet>();
            builder.RegisterType<PlayerRepository>().As<IPlayer>();
        }

    }
}
