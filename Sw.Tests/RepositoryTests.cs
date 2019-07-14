using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SW.Entities;
using SW.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;

namespace SWapi.Controllers
{
    [TestClass]
    public class RepositoryTests
    {
        [TestMethod]
        public void ExpectDefaultRepositoryToWorks()
        {
            var mock = new Mock<IDataService>();
            mock.Setup(c => c.GetDataResult(It.IsAny<string>())).Returns(() => null);

            var starshipsRepository = new Repository<Film>(mock.Object);
            var nullResult = starshipsRepository.GetEntities();
            Assert.IsNull(nullResult);

            // verify mock setup.
            const string Expcted = "http://swapi.co/api/films/?page=1";

            mock.Verify(
                c =>
                c.GetDataResult(It.Is<string>(url => url == Expcted)),
                Times.Once());
        }

        [TestMethod]
        public void ExpectDefaultGetDataMethodToBeCalledWithCorrectUrlWithPage()
        {
            var mock = new Mock<IDataService>();
            mock.SetupSequence(c => c.GetDataResult(It.IsAny<string>()))
                .Returns(string.Empty);

            var filmsRepository = new Repository<Film>(mock.Object);
            const int Page = 5;

            var nullResult = filmsRepository.GetEntities(Page);
            Assert.IsNull(nullResult);

            string expcted = "http://swapi.co/api/films/?page=" + Page;
            mock.Verify(
                c =>
                c.GetDataResult(It.Is<string>(url => url == expcted)),
                Times.Once());
        }

        [TestMethod]
        public void ExpectDefaultGetDataMethodToBeCalledWithCorrectUrlWithPageAndSize()
        {
            var mock = new Mock<IDataService>();
            mock.SetupSequence(c => c.GetDataResult(It.IsAny<string>()))
                .Returns("{ \"next\" : \"someUrl\", \"count\" : 2, \"results\": [ { }, { }]}");

            var peopleRepository = new Repository<Planet>(mock.Object);
            const int Page = 5;
            const int Size = 1;

            var people = peopleRepository.GetEntities(Page, Size);
            Assert.AreEqual(Size, people.Count);

            string expcted = "http://swapi.co/api/planets/?page=" + Page;
            mock.Verify(
                c =>
                c.GetDataResult(It.Is<string>(url => url == expcted)),
                Times.Once());
        }

        [TestMethod]
        public void ExpectDefaultGetDataMethodToReturnCorrectDataWithPageAndSize()
        {
            var mock = new Mock<IDataService>();
            const string UrlData = "testUrl";
            mock.SetupSequence(c => c.GetDataResult(It.IsAny<string>()))
                .Returns("{ \"next\" : \"" + UrlData + "\", \"prev\": \"null\", \"count\" : 12, \"results\": [ { }, { }, { }, { },{ }, { }, { }, { }, { }, { }, { }, { }]}")
                .Returns("{ \"count\" : 12, \"results\": [ { }, { }, { }, { }, { }, { }, { }, { }, { }, { }, { }, { }]}");

            var speciesRepository = new Repository<Film>(mock.Object);
            const int Page = 3;
            const int Size = 18;

            var people = speciesRepository.GetEntities(Page, Size);
            Assert.AreEqual(Size, people.Count);

            string expcted = "http://swapi.co/api/film/?page=" + Page;
            mock.Verify(
                c =>
                c.GetDataResult(It.Is<string>(url => url == expcted || url == UrlData)),
                Times.Once());
        }

        [TestMethod]
        public void ExpectInitializationOfRepositoryToBeCalledWithDefaultParameters()
        {
            var repository = new Repository<Film>();
            var privateFields = repository
                .GetType()
                .GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            var urlDataField = privateFields.FirstOrDefault(f => f.Name == "urlData");
            if (urlDataField == null)
            {
                throw new NullReferenceException();
            }

            var urlDataFieldValue = urlDataField.GetValue(repository);
            const string Expected = "http://swapi.co/api/";
            Assert.AreEqual(Expected, urlDataFieldValue);

            var dataServiceField = privateFields.FirstOrDefault(f => f.Name == "dataService");
            if (dataServiceField == null)
            {
                throw new NullReferenceException();
            }
        }

        [TestMethod]
        public void ExpectToReutrnExaclyCountResultsWhenThereAreNotMorePages()
        {
            var mock = new Mock<IDataService>();
            var personRepository = new Repository<Planet>(mock.Object);

            mock.Setup(x => x.GetDataResult(It.IsAny<string>()))
                .Returns("{ results: [ { }, { }]}");

            const int Page = 1;
            const int Size = 2;
            var results = personRepository.GetEntities(Page, Size);
            Assert.AreEqual(Size, results.Count);
        }

        [TestMethod]
        public void ExpectToReturnCorrectResultsWhenGetEntitiesIsCalledWithSizeZero()
        {
            var mock = new Mock<IDataService>();
            mock.Setup(x => x.GetDataResult(It.IsAny<string>()))
                .Returns("{ results: [ { }, { }, { }, { }, { }, { } ]}");

            var personRepository = new Repository<Film>(mock.Object);

            const int Size = 0;
            var results = personRepository.GetEntities(size: Size);
            Assert.AreEqual(Size, results.Count);
        }

        [TestMethod]
        public void ExpectToReturnCorectResultsWhenHighValuePassedAs()
        {
            StringBuilder returnMockResult = new StringBuilder();
            returnMockResult.Append("{ next: \"Url\", results : [");

            const int Size = 5000;
            for (int i = 0; i < Size - 1; i++)
            {
                returnMockResult.Append("{ }, ");
            }

            returnMockResult.Append("{ } ] }");

            var mock = new Mock<IDataService>();
            mock.SetupSequence(x => x.GetDataResult(It.IsAny<string>()))
                .Returns(returnMockResult.ToString())
                .Returns("{ results: [ ] }");

            var repostitory = new Repository<Film>(mock.Object);
            var result = repostitory.GetEntities(int.MaxValue, int.MaxValue);
            Assert.AreEqual(Size, result.Count);
        }

        [TestMethod]
        public void ExpectCallingConstuctorToSetCorrectConfigurationWithExtensionUrlEndsWithSlash()
        {
            const string TestUrl = "http://myTestUrl.com/";

            var mock = new Mock<IDataService>();
            mock.Setup(x => x.GetDataResult(It.IsAny<string>()))
                .Returns("{ results: [ ]}");
            var planetPath = new Planet().GetPath();
            var repository = new Repository<Planet>(mock.Object, TestUrl);
            repository.GetEntities();

            string expectedUrl = $"{ TestUrl }{ planetPath }?page=1";

            mock.Verify(c => c.GetDataResult(It.Is<string>(str => str == expectedUrl)), Times.AtLeastOnce);
        }

        [TestMethod]
        public void ExpectCallingConstructorToSetCorrectConfigurationWithExtensionUrlWithoutEndsWithSlash()
        {
            const string TestUrl = "http://myTestUrl.com";

            var mock = new Mock<IDataService>();
            mock.Setup(x => x.GetDataResult(It.IsAny<string>()))
                .Returns("{ results: [ ]}");
            var planetPath = new Planet().GetPath();
            var repository = new Repository<Planet>(mock.Object, TestUrl);
            repository.GetEntities();

            string expectedUrl = $"{ TestUrl }/{ planetPath }?page=1";

            mock.Verify(c => c.GetDataResult(It.Is<string>(str => str == expectedUrl)), Times.AtLeastOnce);
        }

        [TestMethod]
        public void ExpectPathToBeConfuguredCorrectFromConstructor()
        {
            const string UrlForPlanetRepository = "http://planets.com/api/swapi";
            const string UrlForFilmRepository = "http://films.com/";

            var mock = new Mock<IDataService>();

            var planetRepository = new Repository<Planet>(mock.Object, UrlForPlanetRepository);
            var filmRepository = new Repository<Film>(mock.Object, UrlForFilmRepository);

            Assert.AreEqual(UrlForPlanetRepository + "/", planetRepository.Path);
            Assert.AreEqual(UrlForFilmRepository, filmRepository.Path);
        }

        [TestMethod]
        public void ExpectThreeTimesFasterObjectInitialiingWithHelperInitializerClass()
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();

            const int TestsCount = 1000000;
            for (int i = 0; i < TestsCount; i++)
            {
                Activator.CreateInstance<Film>();
                Activator.CreateInstance<Planet>();
            }

            timer.Stop();
            var activatorTimer = timer.Elapsed;

            timer.Restart();

            for (int i = 0; i < TestsCount; i++)
            {
                HelperInitializer<Film>.Instance();
                HelperInitializer<Planet>.Instance();
            }

            timer.Stop();
            var helperTimer = timer.Elapsed;
            long expectedTimesDifference = 3;

            Assert.IsTrue(
                expectedTimesDifference <= (activatorTimer.Ticks / helperTimer.Ticks),
                "Difference : " + (activatorTimer.Ticks / helperTimer.Ticks));
        }

        [TestMethod]
        public void ExpectGetByIdToPassDefaultUrlForGetDataSource()
        {
            var mock = new Mock<IDataService>();
            mock.Setup(x => x.GetDataResult(It.IsAny<string>()))
                .Returns("{ results: [ ]}");

            var entity = new Planet();
            const int Id = 33;
            string expectUrl = $"http://swapi.co/api/{ entity.GetPath() }{ Id }";

            var repository = new Repository<Planet>(mock.Object);
            var result = repository.GetById(Id);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Planet));

            mock.Verify(c => c.GetDataResult(It.Is<string>(str => str == expectUrl)), Times.AtLeastOnce);
        }

        [TestMethod]
        public void ExpectGetByIdToReturnCorrectResult()
        {
            string returnedResult = $@"{{
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
                .Returns(returnedResult);

            Film returnOfJedi = new Repository<Film>(mock.Object).GetById(1);

            Assert.AreEqual(returnOfJedi.Title, "Return of the Jedi");
            Assert.AreEqual(returnOfJedi.Starships.Count, 0);
        }

        [TestMethod]
        public void ExpectToReturnNullValueWhenDataCannotBeFound()
        {
            string nullResult = null;
            var mock = new Mock<IDataService>();
            mock.Setup(c => c.GetDataResult(It.IsAny<string>()))
                .Returns(nullResult);

            Film result = new Repository<Film>(mock.Object).GetById(1);

            Assert.IsNull(result);
        }
        //// TODO: Parse objects test
    }
}