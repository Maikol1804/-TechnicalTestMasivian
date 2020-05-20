using Autofac;
using RouletteWebApi.DataAccess.Interfaces;
using RouletteWebApi.Models;
using RouletteWebApi.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RouletteWebApi.Services.Implementations
{
    public class PlayerServices : IPlayerService
    {
        private readonly IComponentContext components;
        private readonly IPlayer playerRepository;

        public PlayerServices(IComponentContext components)
        {
            this.components = components;
            playerRepository = components.Resolve<IPlayer>();
        }

        public async Task<Player> GetPlayerById(long id)
        {
            try
            {
                return await playerRepository.GetById(id);
            }
            catch (Exception)
            {
                //TODO Save in log
                throw;
            }
        }
    }
}
