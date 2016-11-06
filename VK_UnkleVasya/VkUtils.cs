using HierarchicalData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

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

            return vk.Photo.Get(new PhotoGetParams()
            {
                AlbumId = PhotoAlbumType.Id(album.Id),
                OwnerId = album.GroupId * -1,
                Reversed = fromEnd,
                Count = 1,
                Offset = (ulong)index
            }, false).FirstOrDefault();
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
    }
}
