using Autofac;
using RouletteWebApi.DataAccess.Interfaces;
using RouletteWebApi.Models;
using RouletteWebApi.Services.Contracts;
using RouletteWebApi.Transverse;
using RouletteWebApi.Transverse.Helpers;
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

        public async Task<ResponseEntity<Player>> GetPlayerById(long id)
        {
            ResponseEntity<Player> response = new ResponseEntity<Player>();
            try
            {
                response.Entity = await playerRepository.GetById(id);
                response.Code = Enumerators.State.Ok.GetDescription();
            }
            catch (Exception)
            {
                //TODO Save in log
                response.Code = Enumerators.State.Error.GetDescription();
                response.Message = "Error getting player by id.";
            }
            return response;
        }
    }
}
