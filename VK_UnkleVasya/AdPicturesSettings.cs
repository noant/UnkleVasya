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
    public class AdPicturesSettings
    {
        public static void Reload(VkApi vk)
        {
            var hData = HierarchicalObject.FromFile(StringConstants.AdPicturesSettingsFileName);
            Album = new Album() { Link = hData[0] };
            Interval = hData[1];
            ImagePosition = hData[2];
            IsAllow = hData[3];
            VkUtils.PrepareAlbumSize(vk, Album);
        }

        private static void Save()
        {
            var hData = new HierarchicalObject(StringConstants.AdPicturesSettingsFileName);
            hData[0] = Album.Link;
            hData[1] = Interval;
            hData[2] = ImagePosition;
            hData[3] = IsAllow;
            hData.SaveToFile();
        }

        public static bool IsAllow { get; private set; }
        public static Album Album { get; private set; }
        public static int Interval { get; private set; }
        private static int ImagePosition { get; set; }

        public static bool IsNeedAd(DialogSettings dialog)
        {
            return IsAllow && dialog.TotalImagesSent > Interval && dialog.TotalImagesSent%Interval == 0;
        }

        public static Photo GetNextPicAndCommit(VkApi vk)
        {
            if (Album.PhotosCount.Value == 0)
                return null;

            var index = ImagePosition;
            ImagePosition++;
            if (ImagePosition >= Album.PhotosCount.Value)
                ImagePosition = 0;
            Save();

            return VkUtils.GetPicture(vk, Album, index, false);
        }
    }
}
