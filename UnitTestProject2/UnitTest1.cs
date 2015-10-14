using System;
using System.Linq;
using HotelAssessment;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject2 {
    [TestClass]
    public class UnitTest1 {
        [TestMethod]
            //assert true if date1 comes before date2
        public void TestMethod1()
        {
            //mm/dd/yyyy
            String date1 = "10/10/2014";
            String date2 = "10/11/2014";
            Assert.IsTrue(DateUtil.compare(date1,date2));
        }
        [TestMethod]
        public void testDateFiltering()
        {
            //mm/dd/yyyy
            String date1 = "10/12/2014";
            String date2 = "11/3/2014";
            //assert true if date1 comes before date2
            Assert.IsFalse(DateUtil.compare(date1, date2));
        }


    }
}
