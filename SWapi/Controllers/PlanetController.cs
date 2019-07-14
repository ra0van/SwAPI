using SW.Application;
using SW.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SWapi.Controllers
{
    public class PlanetController : ApiController
    {
        PlanetService<Planet> service;
        protected PlanetController()
        {
            service = new PlanetService<Planet>();
        }

        // GET api/Planet/
        public IEnumerable<Planet> Get()
        {
            return service.Execute();
        }

        // GET api/Planet/Aldeeran
        [Route("api/Planet/{name}")]
        public Planet Get(string name)
        {
            return service.GetByName(name);
        }
    }
}
