using HierarchicalData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkCaptchaSolver
{
    [Serializable]
    public class Settings
    {
        static Settings()
        {
            HierarchicalObjectCrutch.Register(typeof(Settings));
            Current = HierarchicalObject.FromFile(Constants.SettingsFilePath)[0];
        }
        public static Settings Current { get; private set; }

        public ulong ApplicationId { get; set; }

        public string Login { get; set; }
        public string Password { get; set; }

        public int Timeout { get; set; }
    }
}
