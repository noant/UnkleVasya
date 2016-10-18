using Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
                    Count = vk.Messages.GetDialogs(new MessagesDialogsGetParams(){ Count = 999999999 }).TotalCount
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
                            Count = 200,
                            Unread = true
                        });

                        foreach (var message in newMessages.Messages)
                        {
                            if (!VkUtils.IsResponsePattern(message.Body)) continue;

                            var dialogSettings = GetDialogSettings(message, vk);

                            var query = VkUtils.GetQuery(message.Body);

                            if (VkUtils.IsStartIntervalDispatcherRequest(query))
                                dialogSettings.NeedStartIntervalDispatch();
                            else if (VkUtils.IsStopIntervalDispatcherRequest(query))
                                dialogSettings.NeedStopIntervalDispatch();
                            else
                            {
                                var result = VkUtils.GetNextPicture(vk);
                                if (result == null)
                                    VkUtils.SendMessage(vk, message, "Чет я туплю малость.");
                                else VkUtils.SendImage(vk, message, result, VkUtils.GetNextRandMessage_manual());
                            }

                            Console.WriteLine(message.Body);
                        }
                    }
                    Thread.Sleep(400);
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