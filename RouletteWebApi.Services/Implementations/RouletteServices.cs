using Autofac;
using RouletteWebApi.DTO;
using RouletteWebApi.Models;
using RouletteWebApi.Services.Contracts;
using RouletteWebApi.Services.Interfaces;
using RouletteWebApi.Transverse;
using RouletteWebApi.Transverse.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RouletteWebApi.Services.Implementations
{
    public class RouletteServices : IRouletteService
    {
        private readonly IComponentContext components;
        private readonly IRoulette rouletteRepository;

        public RouletteServices(IComponentContext components)
        {
            this.components = components;
            rouletteRepository = components.Resolve<IRoulette>();
        }

        public async Task<ResponseEntity<Roulette>> GetRouletteById(long id)
        {
            ResponseEntity<Roulette> response = new ResponseEntity<Roulette>();
            try
            {
                response.Entity = await rouletteRepository.GetById(id);
                response.Code = Enumerators.State.Ok.GetDescription();
            }
            catch (Exception)
            {
                //TODO Save in log
                response.Code = Enumerators.State.Error.GetDescription();
                response.Message = "Error getting roulette by id.";
            }
            return response;
        }

        public async Task<ResponseList<Roulette>> GetAllRoulettes()
        {
            ResponseList<Roulette> response = new ResponseList<Roulette>();
            try
            {
                response.List = await rouletteRepository.GetAll();
                response.Code = Enumerators.State.Ok.GetDescription();
            }
            catch (Exception)
            {
                //TODO Save in log
                response.Code = Enumerators.State.Error.GetDescription();
                response.Message = "Error getting all bets.";
            }
            return response;
        }

        public async Task<Response> SaveRoulette(Roulette roulette)
        {
            Response response = new Response();

            try
            {
                await rouletteRepository.Add(roulette);
                response.Code = Enumerators.State.Ok.GetDescription();
            }
            catch (Exception)
            {
                //TODO Save in log
                response.Code = Enumerators.State.Error.GetDescription();
                response.Message = "Error to save the roullete.";
            }
            return response;
        }

        public async Task<Response> UpdateRoulette(Roulette roulette)
        {
            Response response = new Response();

            try
            {
                await rouletteRepository.Update(roulette);
                response.Code = Enumerators.State.Ok.GetDescription();
            }
            catch (Exception)
            {
                //TODO Save in log
                response.Code = Enumerators.State.Error.GetDescription();
                response.Message = "Error to update the roullete.";
            }
            return response;
        }

        public async Task<Response> ChangeRouletteState(long id, bool isOpen)
        {
            Response response = new Response();
            try
            {
                Roulette roulette = await rouletteRepository.GetById(id);
                roulette.IsOpen = isOpen;
                await rouletteRepository.Update(roulette);
                response.Code = Enumerators.State.Ok.GetDescription();
            }
            catch (Exception)
            {
                //TODO Save in log
                response.Code = Enumerators.State.Error.GetDescription();
                response.Message = "Error changing the roullete state.";
            }
            return response;
        }

        public async Task<Response> ValidateRoulette(long id)
        {

            #region Validate Roulette
            var roulette = await rouletteRepository.GetById(id);
            if (roulette == null)
            {
                return new Response()
                {
                    Code = Enumerators.State.Error.GetDescription(),
                    Message = "Roulette not found."
                };
            }
            #endregion

            return new Response()
            {
                Code = Enumerators.State.Ok.GetDescription(),
                Message = "Validations passed."
            };

        }

        #region Validations

        public async Task<Response> ValidateOpenRoulette(long id)
        {

            var roulette = await rouletteRepository.GetById(id);

            if (roulette == null)
            {
                return new Response()
                {
                    Code = Enumerators.State.Error.GetDescription(),
                    Message = "Roulette not found."
                };
            }

            if (roulette.IsOpen)
            {
                return new Response()
                {
                    Code = Enumerators.State.Error.GetDescription(),
                    Message = "Roulette is currently open."
                };
            }

            if (roulette.UpdateDate != null)
            {
                return new Response()
                {
                    Code = Enumerators.State.Error.GetDescription(),
                    Message = "This roulette game has already been played. It cannot be opened again."
                };
            }

            return new Response()
            {
                Code = Enumerators.State.Ok.GetDescription(),
                Message = "Validations passed."
            };
        }

        public async Task<Response> ValidateCloseRoulette(long id)
        {
            var roulette = await rouletteRepository.GetById(id);

            if (roulette == null)
            {
                return new Response()
                {
                    Code = Enumerators.State.Error.GetDescription(),
                    Message = "Roulette not found."
                };
            }

            if (!roulette.IsOpen)
            {
                return new Response()
                {
                    Code = Enumerators.State.Error.GetDescription(),
                    Message = "Roulette is currently closed."
                };
            }

            return new Response()
            {
                Code = Enumerators.State.Ok.GetDescription(),
                Message = "Validations passed."
            };

        }

        #endregion

    }
}
