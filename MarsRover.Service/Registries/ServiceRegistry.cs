using MarsRover.Service.Contracts;
using MarsRover.Service.Controllers;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;

namespace MarsRover.Service.Registries
{
    public class ServiceRegistry : Registry
    {
        public ServiceRegistry()
        {
            For<IMarsRoverService>().Use<MarsRoverService>().Ctor<HttpClient>().Is(x => new HttpClient()); ;
        }
    }
}
