using Autofac;
using Microsoft.AspNetCore.Mvc;
using RouletteWebApi.Services.Contracts;

namespace RouletteWebApi.Controllers
{

    public class BaseController : ControllerBase
    {
        public readonly IComponentContext component;
        public readonly IRouletteService rouletteServices;
        public readonly IBetService betServices;
        public readonly IPlayerService playerServices;

        public BaseController(IComponentContext component)
        {
            this.component = component;
            rouletteServices = this.component.Resolve<IRouletteService>();
            betServices = this.component.Resolve<IBetService>();
            playerServices = this.component.Resolve<IPlayerService>();
        }
    }
}