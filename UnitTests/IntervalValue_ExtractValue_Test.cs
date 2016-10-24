using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VK_UnkleVasya.Commands;
using System.Diagnostics;

namespace UnitTests
{
    [TestClass]
    public class IntervalValue_ExtractValue_Test
    {
        [TestMethod]
        public void TestMethod1()
        {
            var setIntervalCommand = new SetIntervalValueCommand();
            var value = setIntervalCommand.ExtractValue("по 3 часов");
            Debug.WriteLine(value);
            if (value != 3)
                throw new Exception();
        }

        [TestMethod]
        public void TestMethod2()
        {
            var setIntervalCommand = new SetIntervalValueCommand();
            var value = setIntervalCommand.ExtractValue("по 2.1 часов");
            Debug.WriteLine(value);
            if (value != (decimal)2.1)
                throw new Exception();
        }

        [TestMethod]
        public void TestMethod3()
        {
            var setIntervalCommand = new SetIntervalValueCommand();
            var value = setIntervalCommand.ExtractValue("по6,1часа");
            Debug.WriteLine(value);
            if (value != (decimal)6.1)
                throw new Exception();
        }

        [TestMethod]
        public void TestMethod4()
        {
            var setIntervalCommand = new SetIntervalValueCommand();
            var value = setIntervalCommand.ExtractValue("по00,00часа");
            Debug.WriteLine(value);
            if (value != (decimal)0)
                throw new Exception();
        }
    }
}
