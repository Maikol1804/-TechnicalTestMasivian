using Autofac;
using RouletteWebApi.DataAccess.Interfaces;
using RouletteWebApi.DTO;
using RouletteWebApi.Models;
using RouletteWebApi.Services.Contracts;
using RouletteWebApi.Services.Interfaces;
using RouletteWebApi.Transverse;
using RouletteWebApi.Transverse.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteWebApi.Services.Implementations
{
    public class BetServices : IBetService
    {
        private readonly IComponentContext components;
        private readonly IRoulette rouletteRepository;
        private readonly IBet betRepository;
        private readonly IPlayer playerRepository;

        public BetServices(IComponentContext components)
        {
            this.components = components;
            rouletteRepository = components.Resolve<IRoulette>();
            betRepository = components.Resolve<IBet>();
            playerRepository = components.Resolve<IPlayer>();
        }

        public async Task<ResponseEntity<Bet>> GetBetById(long id)
        {
            ResponseEntity<Bet> response = new ResponseEntity<Bet>();
            try
            {
                response.Entity = await betRepository.GetById(id);
                response.Code = Enumerators.State.Ok.GetDescription();
            }
            catch (Exception)
            {
                //TODO Save in log
                response.Code = Enumerators.State.Error.GetDescription();
                response.Message = "Error getting bet by id.";
            }
            return response;
        }

        public async Task<ResponseList<Bet>> GetAllBets()
        {
            ResponseList<Bet> response = new ResponseList<Bet>();
            try
            {
                response.List = await betRepository.GetAll();
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

        public async Task<ResponseList<Bet>> GetAllBetsByRoulleteId(long id)
        {

            ResponseList<Bet> response = new ResponseList<Bet>();

            try
            {
                IEnumerable<Bet> bets = await betRepository.GetAll();
                bets = bets.Where(x => x.Roulette.Id == id).ToList();

                response.List = bets;
                response.Code = Enumerators.State.Ok.GetDescription();
            }
            catch (Exception)
            {
                //TODO Save in log
                response.Code = Enumerators.State.Error.GetDescription();
                response.Message = "Error getting bets by roulette id.";
            }

            return response;
        }

        public async Task<Response> SaveBet(Bet bet)
        {
            Response response = new Response();

            try
            {
                Roulette roullete = await rouletteRepository.GetById(bet.Roulette.Id);
                if (roullete != null)
                    bet.Roulette = roullete;

                Player player = await playerRepository.GetById(bet.Player.Id);
                if (player != null)
                    bet.Player = player;

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

        #region Validations

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

    }
}
