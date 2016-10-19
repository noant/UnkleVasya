using HierarchicalData;
using System;
using System.Collections.Generic;
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
            var album = EroRepository.Repository.Albums[Utils.GetNextRandom(0, EroRepository.Repository.Albums.Length - 1)];
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
            for (int i = 0; i < count; i++)
            {
                var photo = vk.Photo.Get(new PhotoGetParams()
                {
                    AlbumId = PhotoAlbumType.Id(album.Id),
                    OwnerId = album.GroupId * -1,
                    Reversed = Utils.GetNextRandom(0, 1) == 0,
                    Count = 1,
                    Offset = (ulong?)Utils.GetNextRandom(0, (int)album.PhotosCount.Value - 1)
                }).FirstOrDefault();
                arr[i] = photo;
                TechnicalSleepForVk();
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

        public static Message[] GetAllDialogs(VkApi vk)
        {
            var allDialogs = new List<Message>();
            uint currentDialogsCnt = 1;
            uint counter = 0;
            while (currentDialogsCnt != 0)
            {
                var dialogs = vk.Messages.GetDialogs(new MessagesDialogsGetParams()
                {
                    Count = 200,
                    Offset = (int)counter
                });
                currentDialogsCnt = dialogs.TotalCount;
                counter += currentDialogsCnt;
                allDialogs.AddRange(dialogs.Messages);
                TechnicalSleepForVk();
            }
            return allDialogs.ToArray();
        }

        public static Message GetLastMessage(VkApi vk, long userOrChatId)
        {
            return vk.Messages.GetHistory(new MessagesGetHistoryParams()
            {
                Count = 1,
                UserId = userOrChatId,
                Offset = 0
            }).Messages.FirstOrDefault();
        }

        public static long GetId(Message message)
        {
            return (message.ChatId ?? message.UserId).Value * (message.ChatId != null ? -1 : 1);
        }

        public static void TechnicalSleepForVk()
        {
            Thread.Sleep(334); // 3 or less request per 1 second
        }
    }
}
