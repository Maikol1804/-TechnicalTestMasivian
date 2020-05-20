using Autofac;
using Microsoft.AspNetCore.Mvc;
using RouletteWebApi.Services.Contracts;

namespace RouletteWebApi.Controllers
{

    public class BaseController : ControllerBase
    {
        public readonly IComponentContext component;
        public readonly IAdministrationServices administrationServices;

        public BaseController(IComponentContext component)
        {
            this.component = component;
            administrationServices = this.component.Resolve<IAdministrationServices>();
        }
    }
}