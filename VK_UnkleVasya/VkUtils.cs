using HierarchicalData;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security;
using System.Threading;
using VkNet;
using VkNet.Categories;
using VkNet.Enums.Filters;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;
using VkNet.Utils;

namespace VK_UnkleVasya
{
    public static class VkUtils
    {
        public static Photo[] GetRandomPictures(VkApi vk, int count)
        {
            var arr = new Photo[count];
            for (int i = 0; i < count; i++)
            {
                arr[i] = GetRandomPicture(vk);
                VkNet.VkUtils.TechnicalSleepForVk();
            }
            return arr;
        }

        public static Photo GetRandomPicture(VkApi vk)
        {
            var album = EroRepository.Repository.Albums[Utils.GetNextRandom(0, EroRepository.Repository.Albums.Length - 1)];
            return GetRandomPicture(vk, album);
        }

        public static Photo GetRandomPicture(VkApi vk, Album album)
        {
            if (album.PhotosCount == null)
                PrepareAlbumSize(vk, album);

            return GetPicture(vk, album, Utils.GetNextRandom(0, (int)album.PhotosCount.Value - 1), Utils.GetNextRandom(0, 1) == 0);
        }

        public static Photo GetPicture(VkApi vk, Album album, int index, bool fromEnd)
        {
            if (album.PhotosCount == null)
                PrepareAlbumSize(vk, album);

            var valuesDictionary = new Dictionary<string, string>();
            valuesDictionary["album_id"] = album.Id.ToString();
            valuesDictionary["owner_id"] = (album.GroupId * -1).ToString();
            valuesDictionary["rev"] = (fromEnd ? 1 : 0).ToString();
            valuesDictionary["offset"] = index.ToString();
            valuesDictionary["count"] = "1";

            var jToken = JToken.Parse(vk.Invoke("photos.get", valuesDictionary).Replace("{{", "{").Replace("}}", "}"))["response"][0];
            var photo = PhotoFromJson(new VkResponse(jToken));
            return photo;
        }

        public static void PrepareAlbumSize(VkApi vk, Album album)
        {
            album.PhotosCount =
                vk.Photo.GetAlbums(new PhotoGetAlbumsParams()
                {
                    AlbumIds = new[] { album.Id },
                    Count = 1,
                    OwnerId = album.GroupId * -1
                }, false).First().Size;
        }

        public static ApiAuthParams GetCredentials()
        {
            var hData = HierarchicalObject.FromFile(StringConstants.CredentialsFileName);
            var creds = new ApiAuthParams
            {
                ApplicationId = (ulong)hData["ApplicationId"],
                Login = hData["Login"],
                Password = hData["Password"],
                Settings = Settings.All | Settings.Offline
            };
            return creds;
        }

        private static Message[] _allDialogs;

        public static Message GetLastMessage(VkApi vk, long userOrChatId)
        {
            if (_allDialogs == null)
                _allDialogs = VkNet.VkUtils.GetAllDialogs(vk);
            return _allDialogs.FirstOrDefault(x => GetId(x).Equals(userOrChatId));
        }

        public static long GetId(Message message)
        {
            return (message.ChatId ?? message.UserId).Value * (message.ChatId != null ? -1 : 1);
        }

        public static KeyValuePair<Photo, string> GetNextPictureAndMessageForDialog_Ad(VkApi vk, DialogSettings dialog)
        {
            Photo photo = null;
            string message = null;
            if (AdPicturesSettings.IsNeedAd(dialog))
            {
                photo = AdPicturesSettings.GetNextPicAndCommit(vk);
                if (photo != null)
                    message = photo.Text;
            }
            if (photo == null)
            {
                photo = GetRandomPicture(vk);
                message = GetNextMessageForDialog_Ad(dialog, false);
            }
            dialog.IncrementImgsCountAndCommit();

            return new KeyValuePair<Photo, string>(photo, message);
        }

        public static string GetNextMessageForDialog_Ad(DialogSettings dialog, bool isMessageForIntervalDispatching)
        {
            string result = null;
            if (AdTextSettings.IsNeedAd(dialog))
                result = AdTextSettings.GetNextStringAndCommit();
            if (string.IsNullOrEmpty(result))
                result = isMessageForIntervalDispatching ? RandomMessages.GetNext_Interval() : RandomMessages.GetNext();

            return result;
        }

        internal static Photo PhotoFromJson(VkResponse response)
        {
            Photo photo = new Photo();
            long? nullable1 = (long?)(response[(object)"photo_id"] ?? response[(object)"pid"] ?? response[(object)"id"]);
            photo.Id = nullable1;
            long? nullable2 = (long?)(response[(object)"album_id"] ?? response[(object)"aid"]);
            photo.AlbumId = nullable2;
            long? nullable3 = (long?)response[(object)"owner_id"];
            photo.OwnerId = nullable3;
            Uri uri1 = (Uri)(response[(object)"photo_75"] ?? response[(object)"src_small"]);
            photo.Photo75 = uri1;
            Uri uri2 = (Uri)(response[(object)"photo_130"] ?? response[(object)"src"]);
            photo.Photo130 = uri2;
            Uri uri3 = (Uri)(response[(object)"photo_604"] ?? response[(object)"src_big"]);
            photo.Photo604 = uri3;
            Uri uri4 = (Uri)(response[(object)"photo_807"] ?? response[(object)"src_xbig"]);
            photo.Photo807 = uri4;
            Uri uri5 = (Uri)(response[(object)"photo_1280"] ?? response[(object)"src_xxbig"]);
            photo.Photo1280 = uri5;
            Uri uri6 = (Uri)(response[(object)"photo_2560"] ?? response[(object)"src_xxxbig"]);
            photo.Photo2560 = uri6;
            int? nullable4 = (int?)response[(object)"width"];
            photo.Width = nullable4;
            int? nullable5 = (int?)response[(object)"height"];
            photo.Height = nullable5;
            string str1 = (string)response[(object)"text"];
            photo.Text = str1;
            DateTime? nullable6 = (DateTime?)(response[(object)"date"] ?? response[(object)"created"]);
            photo.CreateTime = nullable6;
            long? nullableLongId1 = GetNullableLongId(response[(object)"user_id"]);
            photo.UserId = nullableLongId1;
            long? nullableLongId2 = GetNullableLongId(response[(object)"post_id"]);
            photo.PostId = nullableLongId2;
            string str2 = (string)response[(object)"access_key"];
            photo.AccessKey = str2;
            long? nullableLongId3 = GetNullableLongId(response[(object)"placer_id"]);
            photo.PlacerId = nullableLongId3;
            DateTime? nullable7 = (DateTime?)response[(object)"tag_created"];
            photo.TagCreated = nullable7;
            long? nullable8 = (long?)response[(object)"tag_id"];
            photo.TagId = nullable8;
            Likes likes = (Likes)response[(object)"likes"];
            photo.Likes = likes;
            Comments comments = (Comments)response[(object)"comments"];
            photo.Comments = comments;
            bool? nullable9 = (bool?)response[(object)"can_comment"];
            photo.CanComment = nullable9;
            Tags tags = (Tags)response[(object)"tags"];
            photo.Tags = tags;
            Uri uri7 = (Uri)response[(object)"photo_src"];
            photo.PhotoSrc = uri7;
            string str3 = (string)response[(object)"photo_hash"];
            photo.PhotoHash = str3;
            Uri uri8 = (Uri)response[(object)"src_small"];
            photo.SmallPhotoSrc = uri8;
            float? nullable10 = (float?)response[(object)"lat"];
            double? nullable11 = nullable10.HasValue ? new double?((double)nullable10.GetValueOrDefault()) : new double?();
            photo.Latitude = nullable11;
            nullable10 = (float?)response[(object)"long"];
            double? nullable12 = nullable10.HasValue ? new double?((double)nullable10.GetValueOrDefault()) : new double?();
            photo.Longitude = nullable12;
            return photo;
        }
        private static long? GetNullableLongId(VkResponse response)
        {
            if (string.IsNullOrWhiteSpace(response != null ? response.ToString() : (string)null))
                return new long?();
            return new long?(Convert.ToInt64(response.ToString()));
        }
    }
}
