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
    public class IntervalValue_IsIt_Test
    {
        public void IsIt(string query, bool isAllowed)
        {
            var setIntervalCommand = new SetIntervalValueCommand();
            var value = setIntervalCommand.IsIt(query);
            Debug.WriteLine(value);
            if (value != isAllowed)
                throw new Exception();
        }

        [TestMethod]
        public void TestMethod1()
        {
            IsIt("asdasd", false);
        }

        [TestMethod]
        public void TestMethod2()
        {
            IsIt("по 5 часов", true);
        }
        
        [TestMethod]
        public void TestMethod3()
        {
            IsIt("по 5 часовsdfsdf", true);
        }

        [TestMethod]
        public void TestMethod4()
        {
            IsIt("sdfпо 5 часовsdfsdf", false);
        }


        [TestMethod]
        public void TestMethod5()
        {
            IsIt("по 4 часовsdfsdf", true);
        }
    }
}
