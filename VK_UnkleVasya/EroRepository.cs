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

        public static Dictionary<string, List<Album>> TagsAlbums { get; private set; }

        static EroRepository()
        {
            HierarchicalObjectCrutch.Register(typeof(Album));
            HierarchicalObjectCrutch.Register(typeof(EroRepository));
        }

        public static void Reload()
        {
            var hData = HierarchicalObject.FromFile(StringConstants.AlbumsFileName);
            Repository = hData[0];
            TagsAlbums = new Dictionary<string, List<Album>>();

            //разворачивание тегов и их синонимов
            foreach (var album in Repository.Albums)
            {
                if (album.Tags != null)
                    foreach (var albumTag in album.Tags)
                    {
                        var tag = albumTag.ToLower().Trim();

                        if (!TagsAlbums.ContainsKey(tag))
                            TagsAlbums.Add(tag, new List<Album>());

                        TagsAlbums[tag].Add(album);

                        if (TagsContainer.Current.Dictionary.ContainsKey(tag))
                        {
                            foreach (var tagSynonym in TagsContainer.Current.Dictionary[tag])
                            {
                                if (!tagSynonym.Equals("#"))
                                {
                                    if (!TagsAlbums.ContainsKey(tagSynonym))
                                        TagsAlbums.Add(tagSynonym, new List<Album>());

                                    TagsAlbums[tagSynonym].Add(album);
                                }
                            }
                        }
                    }
            }
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

        public string[] Tags { get; set; }

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
