using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SW.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sw.Tests
{
    [TestClass]
    public class DataServiceTests
    {
        [TestMethod]
        public void ExpectToReturnCorrectResultsWhenResponseIsOk()
        {
            var mock = new Mock<IWebHelper>();

            string expectedResult = string.Join(" ", Enumerable.Repeat("test", 100));
            mock.Setup(w => w.GetResponse(It.IsAny<WebRequest>()))
                .Returns(new TestWebResponse(expectedResult));

            var res = new DefaultDataService(mock.Object)
                .GetDataResult("http://testUrl.com");

            Assert.IsNotNull(res);
            Assert.AreEqual(expectedResult, res);
        }

        [TestMethod]
        public void ExpectToReturnNullWhenResponseHasAnException()
        {
            var mock = new Mock<IWebHelper>();

            mock.Setup(w => w.GetResponse(It.IsAny<WebRequest>()))
                .Throws(new WebException());

            var res = new DefaultDataService(mock.Object)
                .GetDataResult("http://testUrl.com");

            Assert.IsNull(res);
        }
    }
}
