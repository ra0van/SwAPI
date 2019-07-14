using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SW.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sw.Tests
{
    [TestClass]
    public class HelperTests
    {
        [TestMethod]
        public void ExpectGettingRequestObjectToWorkCorrect()
        {
            const string ValidUrl = "http://testsite.com/";
            WebRequest request = new WebHelper().GetRequest(ValidUrl);
            Assert.AreEqual(ValidUrl, request.RequestUri.ToString());
        }

        [TestMethod]
        public void ExpectRetreiveResponseFromRequestReturnsCorrectValue()
        {
            var mock = new Mock<WebRequest>();

            const string ExpectedReturnValue = "Testing";
            mock.Setup(r => r.GetResponse())
                .Returns(new TestWebResponse(ExpectedReturnValue));

            WebResponse response = new WebHelper().GetResponse(mock.Object);
            StreamReader stream = new StreamReader(response.GetResponseStream());
            var actial = stream.ReadToEnd();

            stream.Dispose();

            Assert.AreEqual(ExpectedReturnValue, actial);
        }
    }
}
