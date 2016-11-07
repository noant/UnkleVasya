using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VK_UnkleVasya;
using VkNet.Enums.SafetyEnums;

namespace UnitTests
{
    [TestClass]
    public class OtherTests
    {
        [TestMethod]
        public void TestMethod()
        {
            PhotoAlbumType.Id(237935674);
        }

        [TestMethod]
        public void TestCaptchaSolver()
        {
            var cs = new VkCaptchaSolver.VkCaptchaSolver();
            var res = cs.Solve("http://vk");
            Debug.WriteLine(res);
        }

        [TestMethod]
        public void TestGetIdByScreenName()
        {
            Debug.WriteLine(VkNet.VkUtils.GetUserIdByUriName(VkCaptchaSolver.VkCaptchaSolver.Vk, "anton.novgorodcev"));
        }


        [TestMethod]
        public void TestSendImg()
        {
            var userId = VkNet.VkUtils.GetUserIdByUriName(VkCaptchaSolver.VkCaptchaSolver.Vk, "anton.novgorodcev");
            //VkNet.VkUtils.SendImage(VkCaptchaSolver.VkCaptchaSolver.Vk, userId.Value, @"D:\Pictures\Wallpapers\Разное_3\1f5ea29477.jpg");
        }
    }
}
