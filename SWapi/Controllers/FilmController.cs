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
    public class FilmController : ApiController
    {
        FilmService<Film> service;
        protected FilmController()
        {
            service = new FilmService<Film>();
        }

        // GET api/film/
        public IEnumerable<Film> Get()
        {
            return service.Execute();
        }

        // GET api/film/
        [Route("api/film/{title}")]
        public Film Get(string title)
        {
            return service.GetByName(title);
        }
    }
}
