using HierarchicalData;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace VK_UnkleVasya
{
    [Serializable]
    public class EroRepository
    {
        public Album[] Albums { get; set; }

        public static EroRepository Repository { get; private set; }

        static EroRepository()
        {
            HierarchicalObjectCrutch.Register(typeof(Album));
            HierarchicalObjectCrutch.Register(typeof(EroRepository));
        }

        public static void Reload()
        {
            var hData = HierarchicalObject.FromFile(StringConstants.AlbumsFileName);
            Repository = hData[0];
        }
    }

    public class GroupAlbums
    {
        public long GroupId { get; set; }
        public Album[] Albums { get; set; }
    }

    [Serializable]
    public class Album
    {
        [XmlIgnore]
        public long GroupId { get; private set; }

        [XmlIgnore]
        public long Id { get; private set; }

        [XmlIgnore]
        public long? PhotosCount { get; set; }

        public Album()
        {
            //do nothing
        }
        public Album(long groupId, long id)
        {
            this.GroupId = groupId;
            this.Id = id;
        }
        public string Link
        {
            get
            {
                return "https://vk.com/album-" + GroupId + "_" + Id;
            }
            set
            {
                var str_arr =
                    value.Replace("https://vk.com/album-", "")
                    .Replace("http://vk.com/album-", "")
                    .Replace("vk.com/album-", "")
                    .Split("_".ToCharArray());
                GroupId = long.Parse(str_arr[0]);
                Id = long.Parse(str_arr[1]);
            }
        }
    }
}
