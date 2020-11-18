using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using XJwt.Net;
using XJwt.Net.Common;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            long now = 1465216929929L;
            string SECRET_KEY = "xld3dde";
            string AES_KEY = "jmzPKOCI+BsUTehdFGpOurjUtaiPLRBpT61sTVka5ms=";
            string data = "f/////////8CAAAAAAAABNI=.y7lEPixVX1i41fhIXV2MMg==.g9FXBolu2ablNJabit0LWw8+n5j+xheW1ETpPXyz+V0=";

            XJwtToken jwt = new XJwtToken(SECRET_KEY, AES_KEY, 0);

            //var jsonBytes = Encoding.UTF8.GetBytes(json).ToJavaByte().Order(ByteOrder.BIG_ENDIAN);

            var json = jwt.verifyAndDecrypt(data);
            Console.WriteLine(json);

            var token = jwt.encryptAndSign(XJwtToken.XJwtType.JSON, json, now + 1000);

            var json2 = jwt.verifyAndDecrypt(token);

            Assert.AreEqual(json, json2);
        }
    }
}
