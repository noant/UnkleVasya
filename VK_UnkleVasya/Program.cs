using Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using VK_UnkleVasya.Commands;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

namespace VK_UnkleVasya
{
    class Program
    {
        static void Main(string[] args)
        {
            //authorization
            var vk = new VkApi();
            vk.Authorize(VkUtils.GetCredentials());

            //dialogs loading
            DialogSettings.NeedApi = () => vk;
            var allDialogs = vk.Messages.GetDialogs(new MessagesDialogsGetParams()
            {
                Count = vk.Messages.GetDialogs(new MessagesDialogsGetParams() { Count = 999999999 }).TotalCount
            });
            DialogSettings.NeedMessage = (id) => allDialogs.Messages.FirstOrDefault();

            //reloader
            var reloaderThread = new Thread(() =>
            {
                while (true)
                {
                    lock (vk)
                    {
                        EroRepository.Reload();
                        RandomMessages.Reload();
                    }

                    //Thread.Sleep(10000);
                    Thread.Sleep(1000 * 60 * 60);
                }
            });
            reloaderThread.Start();

            //main actions
            while (true)
            {
                try
                {
                    lock (vk)
                    {
                        var newMessages = vk.Messages.GetDialogs(new MessagesDialogsGetParams()
                        {
                            Offset = 0,
                            Count = 999999999,
                            Unread = true
                        });

                        foreach (var message in newMessages.Messages)
                        {
                            if (CommandUtils.StartCommand.IsIt(message.Body))
                                CommandUtils.StartCommand.Execute(vk, message, message.Body);
                        }
                    }
                    Thread.Sleep(333);
                }
                catch (Exception e)
                {
                    if (!e.Message.Contains("Flood"))
                        Log.Write(e);
                }
            }
        }
    }
}