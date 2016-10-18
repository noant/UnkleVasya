using HierarchicalData;
using System;
using System.Linq;
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

        public static Photo[] GetNextPictures(VkApi vk, int count)
        {
            var album = EroRepository.Repository.Albums[GetNextRandom(0, EroRepository.Repository.Albums.Length - 1)];
            if (album.PhotosCount == null)
            {
                album.PhotosCount =
                    vk.Photo.GetAlbums(new PhotoGetAlbumsParams()
                    {
                        AlbumIds = new[] { album.Id },
                        Count = 1,
                        OwnerId = album.GroupId * -1,
                    }).First().Size;
            }
            var arr = new Photo[count];
            for (; count > 0; count--)
            {
                var photo = vk.Photo.Get(new PhotoGetParams()
                {
                    AlbumId = PhotoAlbumType.Id(album.Id),
                    OwnerId = album.GroupId*-1,
                    Reversed = GetNextRandom(0, 1) == 0,
                    Count = 1,
                    Offset = (ulong?) GetNextRandom(0, (int) album.PhotosCount.Value - 1)
                }).FirstOrDefault();
                Thread.Sleep(340); //technical sleep for vk
                arr[count] = photo;
            }
            return arr;
        }

        public static Photo GetNextPicture(VkApi vk)
        {
            return GetNextPictures(vk, 1)[0];
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

        public static bool IsResponsePattern(string message)
        {
            var mes = message.ToLower();
            return mes.StartsWith("дв ") || mes.StartsWith("дядя вася ") || mes.StartsWith("васяныч ") ||
                mes.StartsWith("дв,") || mes.StartsWith("дядя вася,") || mes.StartsWith("васяныч,");
        }

        public static bool IsStartIntervalDispatcherRequest(string query)
        {
            return query.ToLower() == "качай интервалом";
        }

        public static bool IsStopIntervalDispatcherRequest(string query)
        {
            return query.ToLower() == "хорош интервалом";
        }

        public static string GetQuery(string message)
        {
            var mes = message.ToLower();
            return mes.Replace("дв ", "").Replace("дядя вася ", "").Replace("васяныч ", "")
                .Replace("дв,", "").Replace("дядя вася,", "").Replace("васяныч,", "").Trim();
        }

        public static void SendMessage(VkApi vk, Message message, string message_string)
        {
            if (message.ChatId == null)
            {
                vk.Messages.Send(new MessagesSendParams()
                {
                    UserId = message.UserId,
                    Message = message_string
                });
            }
            else
            {
                vk.Messages.Send(new MessagesSendParams()
                {
                    ChatId = message.ChatId,
                    Message = message_string
                });
            }
        }
        
        public static void SendImages(VkApi vk, Message message, Photo[] photos, string text)
        {
            var sendParams = new MessagesSendParams();
            sendParams.Message = text;
            if (message.ChatId == null)
                sendParams.UserId = message.UserId;
            else
                sendParams.ChatId = message.ChatId;

            sendParams.Attachments = photos;

            vk.Messages.Send(sendParams);
        }

        public static void SendImage(VkApi vk, Message message, Photo photo, string text)
        {
            SendImages(vk, message, new[] { photo }, text);
        }
    }
}
