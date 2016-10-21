using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using HierarchicalData;
using VkNet;
using VkNet.Model.Attachments;

namespace VK_UnkleVasya
{
    public class AdTextSettings
    {
        public static void Reload(VkApi vk)
        {
            var hData = HierarchicalObject.FromFile(StringConstants.AdTextSettingsFileName);
            Interval = hData[0];
            ValuePosition = hData[1];
            IsAllow = hData[2];
            AllAdTexts = Utils.LoadStringData(StringConstants.AdTextValuesFileName);
        }

        private static void Save()
        {
            var hData = new HierarchicalObject(StringConstants.AdTextSettingsFileName);
            hData[0] = Interval;
            hData[1] = ValuePosition;
            hData[2] = IsAllow;
            hData.SaveToFile();
        }

        public static bool IsAllow { get; private set; }
        public static int Interval { get; private set; }
        private static int ValuePosition { get; set; }
        private static string[] AllAdTexts { get; set; }

        public static bool IsNeedAd(DialogSettings dialog)
        {
            return IsAllow && dialog.TotalImagesSent >= Interval && dialog.TotalImagesSent % Interval == 0;
        }

        public static string GetNextStringAndCommit()
        {
            if (!AllAdTexts.Any())
                return null;

            var index = ValuePosition;
            ValuePosition++;
            if (ValuePosition >= AllAdTexts.Length)
                ValuePosition = 0;
            Save();

            return AllAdTexts[index];
        }
    }
}
