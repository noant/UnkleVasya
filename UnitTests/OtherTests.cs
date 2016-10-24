using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VK_UnkleVasya;

namespace UnitTests
{
    [TestClass]
    public class OtherTests
    {
        [TestMethod]
        public void TestMethod()
        {
            Debug.WriteLine(Utils.GetValueBetween("asb 44 rr", "asb", "rr"));
        }
    }
}
