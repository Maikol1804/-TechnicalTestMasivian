﻿using Autofac;
using RouletteWebApi.DataAccess;
using RouletteWebApi.DataAccess.Interfaces;
using RouletteWebApi.DTO;
using RouletteWebApi.Models;
using RouletteWebApi.Services.Contracts;
using RouletteWebApi.Services.Implementations;
using RouletteWebApi.Services.Interfaces;
using RouletteWebApi.Transverse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteWebApi.Servicios
{
    public class AdministrationServices : IAdministrationServices 
    {
        private readonly IComponentContext components;
        private readonly IBet betRepository;
        private readonly IRoulette rouletteRepository;
        private readonly IPlayer playerRepository;

        public AdministrationServices(IComponentContext components)
        {
            this.components = components;
            betRepository = components.Resolve<IBet>();
            rouletteRepository = components.Resolve<IRoulette>();
            playerRepository = components.Resolve<IPlayer>();
        }

        #region Services

        #region Roulette

        public async Task<Roulette> GetRouletteById(long id)
        {
            try
            {
                return await rouletteRepository.GetById(id);
            }
            catch (Exception)
            {
                //TODO Save in log
                throw;
            }
        }

        public async Task<IEnumerable<Roulette>> GetAllRoulettes() 
        {
            try
            {
                return await rouletteRepository.GetAll();
            }
            catch (Exception)
            {
                //TODO Save in log
                throw;
            }
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

        #endregion

        #region Bet

        public async Task<Bet> GetBetById(long id)
        {
            try
            {
                return await betRepository.GetById(id);
            }
            catch (Exception)
            {
                //TODO Save in log
                throw;
            }
        }

        public async Task<IEnumerable<Bet>> GetAllBets()
        {
            try
            {
                return await betRepository.GetAll();
            }
            catch (Exception)
            {
                //TODO Save in log
                throw;
            }
        }

        public async Task<Response> SaveBet(Bet bet)
        {
            Response response = new Response();

            try
            {
                await betRepository.Add(bet);
                response.Code = Enumerators.State.Ok.GetDescription();
            }
            catch (Exception)
            {
                //TODO Save in log
                response.Code = Enumerators.State.Error.GetDescription();
                response.Message = "Error to save the bet.";
            }
            return response;
        }

        public async Task<Response> UpdateBet(Bet bet)
        {
            Response response = new Response();

            try
            {
                await betRepository.Update(bet);
                response.Code = Enumerators.State.Ok.GetDescription();
            }
            catch (Exception)
            {
                //TODO Save in log
                response.Code = Enumerators.State.Error.GetDescription();
                response.Message = "Error to update the bet.";
            }
            return response;
        }

        #endregion

        #region Player

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

        #endregion

        #endregion

        #region Validations

        #region BetController

        public async Task<Response> ValidateBetToRead(long id)
        {

            #region Validate Roulette
            var bet = await betRepository.GetById(id);
            if (bet == null)
            {
                return new Response()
                {
                    Code = Enumerators.State.Error.GetDescription(),
                    Message = "Bet not found."
                };
            }
            #endregion

            return new Response()
            {
                Code = Enumerators.State.Ok.GetDescription(),
                Message = "Validations passed."
            };

        }

        public async Task<Response> ValidateBetToSave(Bet bet)
        {
            #region Validate Roulette

            if (bet.Roulette == null || bet.Roulette.Id == 0)
            {
                return new Response()
                {
                    Code = Enumerators.State.Error.GetDescription(),
                    Message = "Roulette id is required."
                };
            }

            Roulette roullete = await rouletteRepository.GetById(bet.Roulette.Id);
            if (roullete == null)
            {
                return new Response()
                {
                    Code = Enumerators.State.Error.GetDescription(),
                    Message = "The roulette id is invalid."
                };
            }

            if (!roullete.IsOpen)
            {
                return new Response()
                {
                    Code = Enumerators.State.Error.GetDescription(),
                    Message = "The roulette is closed."
                };
            }

            #endregion

            #region Validate Bet Amount

            if (bet.Amount <= 0)
            {
                return new Response()
                {
                    Code = Enumerators.State.Error.GetDescription(),
                    Message = "The bet amount is invalid"
                };
            }

            if (bet.Amount > 10000)
            {
                return new Response()
                {
                    Code = Enumerators.State.Error.GetDescription(),
                    Message = "The maximum allowed bet is $10000."
                };
            }

            #endregion

            #region Validate Player

            if (bet.Player == null || bet.Player.Id == 0)
            {
                return new Response()
                {
                    Code = Enumerators.State.Error.GetDescription(),
                    Message = "Player id is required."
                };
            }

            Player player = await playerRepository.GetById(bet.Player.Id);
            /*if (player.Money <= bet.Amount) {
                return new Response()
                {
                    Code = Enumerators.State.Error.GetDescription(),
                    Message = "The player doesn't have enough monetary funds."
                };
            }*/

            if (player != null)
            {
                List<Bet> bets = await betRepository.GetAll();
                Bet currentBet = bets.FirstOrDefault(x => x.Roulette.Id == bet.Roulette.Id && x.Player.Id == bet.Player.Id);
                if (currentBet != null)
                {
                    return new Response()
                    {
                        Code = Enumerators.State.Error.GetDescription(),
                        Message = "The player already has a bet on this roulette."
                    };
                }
            }

            #endregion

            #region Validate Bet Type

            if (bet.BetType == null)
            {
                return new Response()
                {
                    Code = Enumerators.State.Error.GetDescription(),
                    Message = "Bet type is required. There are two types, with number or with color"
                };
            }

            if (string.IsNullOrEmpty(bet.BetType.Code))
            {
                return new Response()
                {
                    Code = Enumerators.State.Error.GetDescription(),
                    Message = "Bet type code is required"
                };
            }

            if (!bet.BetType.Code.Equals(Enumerators.BetTypes.Number.GetHashCode().ToString())
                && !bet.BetType.Code.Equals(Enumerators.BetTypes.Color.GetHashCode().ToString()))
            {
                return new Response()
                {
                    Code = Enumerators.State.Error.GetDescription(),
                    Message = "Bet type code is invalid."
                };
            }

            if (string.IsNullOrEmpty(bet.BetType.Value))
            {
                return new Response()
                {
                    Code = Enumerators.State.Error.GetDescription(),
                    Message = "Bet type value is required"
                };
            }

            if (bet.BetType.Code.Equals(Enumerators.BetTypes.Number.GetHashCode().ToString()))
            {
                var isNumericValue = int.TryParse(bet.BetType.Value, out int value);
                if (!isNumericValue || (value < 0 || value > 36))
                {
                    return new Response()
                    {
                        Code = Enumerators.State.Error.GetDescription(),
                        Message = "Bet type value is invalid. The value must be numeric, in the range of 0 to 36"
                    };
                }
            }

            if (bet.BetType.Code.Equals(Enumerators.BetTypes.Color.GetHashCode().ToString()))
            {
                string value = bet.BetType.Value.ToLower();

                if (!value.Equals(Enumerators.Colors.Black.GetDescription().ToLower())
                    && !value.Equals(Enumerators.Colors.Red.GetDescription().ToLower()))
                {
                    return new Response()
                    {
                        Code = Enumerators.State.Error.GetDescription(),
                        Message = "Bet type value is invalid. The value must be a string 'Black' or 'Red'"
                    };
                }
            }

            #endregion

            return new Response()
            {
                Code = Enumerators.State.Ok.GetDescription(),
                Message = "Validations passed."
            };

        }

        #endregion

        #region RouletteController

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

        public async Task<Response> ValidateOpenRoulette(long id)
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

            #endregion

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

        #endregion

    }
}
