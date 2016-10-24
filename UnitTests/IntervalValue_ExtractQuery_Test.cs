using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VK_UnkleVasya.Commands;

namespace UnitTests
{
    [TestClass]
    public class IntervalValue_ExtractQuery_Test
    {
        public void ExtractQuery(string strSource, string strResult)
        {
            var setIntervalCommand = new SetIntervalValueCommand();
            var value = setIntervalCommand.ExtractQuery(strSource);
            Debug.WriteLine(value);
            if (value != strResult)
                throw new Exception();
        }

        [TestMethod]
        public void TestMethod1()
        {
            ExtractQuery("по 3 часов", "");
        }

        [TestMethod]
        public void TestMethod2()
        {
            ExtractQuery("по 3 часовsadf", "sadf");
        }

        [TestMethod]
        public void TestMethod3()
        {
            ExtractQuery("по3.1часовsadf", "sadf");
        }

        [TestMethod]
        public void TestMethod4()
        {
            ExtractQuery("по часов sadf", " sadf");
        }
    }
}
