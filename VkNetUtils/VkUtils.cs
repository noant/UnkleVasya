using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VkNet;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

namespace VkNet
{
    public static class VkUtils
    {
        public static Message[] GetAllDialogs(VkApi vk)
        {
            var messages = new List<Message>();
            int counter = 0;
            while (counter >= 0)
            {
                var current = vk.Messages.GetDialogs(new MessagesDialogsGetParams()
                {
                    Count = 200,
                    Offset = counter
                });
                counter += 200;
                if (!current.Messages.Any())
                    counter = -1;
                else messages.AddRange(current.Messages);
                VkNet.VkUtils.TechnicalSleepForVk();
            }
            return messages.ToArray();
        }

        public static bool IsChat(Message message)
        {
            return message.ChatId != null;
        }

        public static void TechnicalSleepForVk()
        {
            Thread.Sleep(334); // 3 or less request per 1 second
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

            photos = photos.Where(x => x != null).ToArray();            
            if (photos.Any())
                sendParams.Attachments = photos.Where(x=>x != null);

            vk.Messages.Send(sendParams);
        }

        public static void SendImage(VkApi vk, Message message, Photo photo, string text)
        {
            SendImages(vk, message, new[] { photo }, text);
        }

        public static long? GetUserIdByUriName(VkApi vk, string name)
        {
            return vk.Utils.ResolveScreenName(name).Id;
        }

        //public static void SendImage(VkApi vk, long userId, string filePath)
        //{
        //    var serverInfo = vk.Photo.GetMessagesUploadServer();

        //    var uploader = new WebClient();
        //    var responseStr = Encoding.UTF8.GetString(uploader.UploadFile(serverInfo.UploadUrl, filePath));
        //    var responseJson = (JObject)JsonConvert.DeserializeObject(responseStr);
        //    Debug.Write(responseStr);
        //    var hash = responseJson["hash"].ToString();
        //    var server = responseJson["server"].ToString();
        //    var photo = responseJson["photo"];

        //    var photoVk = new Photo();
        //    photoVk.Id = long.Parse(photo["Id"].ToString());

        //    var values = new Dictionary<string, string>();
        //    values.Add("user_id", userId.ToString());
        //    values.Add("attachment", photo);
        //    vk.Invoke("messages.send", values);
        //}
    }
}
