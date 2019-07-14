using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SW.Entities;
using SW.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sw.Tests
{
    [TestClass]
    public class ParsingTests
    {
        [TestMethod]
        public void ExpectToParseCorrectlyAllPropetiesOfFilm()
        {
            string returnResultData = $@"{{
                title: ""Return of the Jedi"",
                episode_id: 6,
	            opening_crawl: ""Luke Skywalker has returned to\r\nhis home planet of Tatooine in\r\nan attempt to rescue his\r\nfriend Han Solo from the\r\nclutches of the vile gangster\r\nJabba the Hutt.\r\n\r\nLittle does Luke know that the\r\nGALACTIC EMPIRE has secretly\r\nbegun construction on a new\r\narmored space station even\r\nmore powerful than the first\r\ndreaded Death Star.\r\n\r\nWhen completed, this ultimate\r\nweapon will spell certain doom\r\nfor the small band of rebels\r\nstruggling to restore freedom\r\nto the galaxy..."",
	            director: ""Richard Marquand"",
	            producer: ""Howard G. Kazanjian, George Lucas, Rick McCallum"",
	            release_date: ""1983-05-25"",
	            characters: [],
	            planets: [],
	            starships: [],
	            vehicles: [],
	            species: [],
	            created: ""2014-12-18T10:39:33.255000Z"",
	            edited: ""2015-04-11T09:46:05.220365Z"",
	            url: ""http://swapi.co/api/films/3/""
            }}";
            var mock = new Mock<IDataService>();
            mock.Setup(c => c.GetDataResult(It.IsAny<string>()))
                .Returns(returnResultData);

            Film testFilm = new Repository<Film>(mock.Object).GetById(3);

            var filmProperties = testFilm.GetType().GetProperties();
            foreach (var property in filmProperties)
            {
                var value = property.GetValue(testFilm);
                Assert.IsNotNull(value, property.Name);
            }

            Assert.AreEqual(0, testFilm.Characters.Count);
            Assert.AreEqual(0, testFilm.Vehicles.Count);
            Assert.AreEqual(0, testFilm.Species.Count);
            Assert.AreEqual(0, testFilm.Planets.Count);
            Assert.AreEqual(0, testFilm.Starships.Count);
        }

        [TestMethod]
        public void ExpectToParseCorrectlyAllPropertiesOfPlanet()
        {
            string returnResultData = $@"{{
                name: ""Alderaan"",
                rotation_period: ""24"",
	            orbital_period: ""364"",
	            diameter: ""12500"",
	            climate: ""temperate"",
	            gravity: ""1 standard"",
	            terrain: ""grasslands, mountains"",
	            surface_water: ""40"",
	            population: ""2000000000"",
	            residents: [
		            ""http://swapi.co/api/people/5/"",
		            ""http://swapi.co/api/people/68/"",
		            ""http://swapi.co/api/people/81/""
	            ],
	            films: [
		            ""http://swapi.co/api/films/6/"",
		            ""http://swapi.co/api/films/1/""
	            ],
	            created: ""2014-12-10T11:35:48.479000Z"",
	            edited: ""2014-12-20T20:58:18.420000Z"",
	            url: ""http://swapi.co/api/planets/2/""
            }}";

            var mock = new Mock<IDataService>();
            mock.Setup(c => c.GetDataResult(It.IsAny<string>()))
                .Returns(returnResultData);

            Planet testPlanet = new Repository<Planet>(mock.Object).GetById(3);

            var properties = testPlanet.GetType().GetProperties();
            foreach (var property in properties)
            {
                var value = property.GetValue(testPlanet);
                Assert.IsNotNull(value, property.Name);
            }

            Assert.AreEqual(2, testPlanet.Films.Count);
            Assert.AreEqual(3, testPlanet.Residents.Count);
        }
    }
}
