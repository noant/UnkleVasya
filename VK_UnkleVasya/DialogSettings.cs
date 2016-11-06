using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml.Serialization;
using HierarchicalData;
using Logging;
using VkNet;
using VkNet.Model;
using VkNet.Model.Attachments;

namespace VK_UnkleVasya
{
    [Serializable]
    public class DialogSettings
    {
        public DialogSettings(long id, VkApi vk, Message message) : this()
        {
            Id = id;
            Vk = vk;
            Message = message;
        }

        public DialogSettings()
        {
            IntervalDispatchingValue = 60;
        }

        public long Id { get; set; }

        [XmlIgnore]
        public VkApi Vk { get; set; }

        [XmlIgnore]
        public Message Message { get; set; }

        public int TotalImagesSent { get; set; } //not by interval dispatching

        public decimal IntervalDispatchingValue { get; set; }

        public void IncrementImgsCountAndCommit()
        {
            TotalImagesSent++;
            SaveToFile();
        }

        private Thread IntervalDispatcher { get; set; }

        public bool IsIntervalDispatcherStarted { get; set; }

        public void SaveToFile()
        {
            var filename = StringConstants.SessionsFolder + this.Id + ".xml";
            var hData = new HierarchicalObject(filename);
            hData[0] = this;
            hData.SaveToFile();
        }

        public void StartIntervalDispatching(bool sendMessage, bool save)
        {
            if (IntervalDispatcher != null)
                StopIntervalDispatching(false, false);
            if (IntervalDispatcher == null)
            {
                IntervalDispatcher = new Thread(() =>
                {
                    while (true)
                    {
                        try
                        {
                            lock (Vk) VkNet.VkUtils.SendImages(Vk, Message, VkUtils.GetRandomPictures(Vk, 5), VkUtils.GetNextMessageForDialog_Ad(this, true));
                            Thread.Sleep((int)(1000 * 60 * 60 * IntervalDispatchingValue));
                        }
                        catch (Exception e)
                        {
                            Log.Write(e);
                        }
                    }
                });
                IsIntervalDispatcherStarted = true;
                IntervalDispatcher.Start();
                if (sendMessage)
                    lock (Vk) VkNet.VkUtils.SendMessage(Vk, Message, StringConstants.Dialog_IntervalOkResponse.Set(IntervalDispatchingValue));
                if (save)
                    SaveToFile();
            }
        }

        private void StopIntervalDispatching(bool sendMessage, bool save)
        {
            if (IntervalDispatcher != null)
            {
                IntervalDispatcher.Abort();
                IntervalDispatcher = null;

                if (sendMessage)
                    lock (Vk) VkNet.VkUtils.SendMessage(Vk, Message, StringConstants.Dialog_IntervalOkStopResponse);

                IsIntervalDispatcherStarted = true;

                if (save)
                    SaveToFile();

            }
            else if (sendMessage) VkNet.VkUtils.SendMessage(Vk, Message, StringConstants.Dialog_IntervalAlwaysNotDoResponse);
        }

        public void StartIntervalDispatching()
        {
            StartIntervalDispatching(true, true);
        }

        public void StopIntervalDispatching()
        {
            StopIntervalDispatching(true, true);
        }

        public void Initialize()
        {
            if (IsIntervalDispatcherStarted)
                StartIntervalDispatching(false, false);
        }

        private static readonly Dictionary<long, DialogSettings> Sessions = new Dictionary<long, DialogSettings>();
        public static DialogSettings GetSession(VkApi vk, Message message)
        {
            var id = VkUtils.GetId(message);
            if (!Sessions.ContainsKey(id))
                Sessions.Add(id, new DialogSettings(id, vk, message));
            return Sessions[id];
        }

        public static void LoadSessions()
        {
            foreach (var filename in Directory.GetFiles(StringConstants.SessionsFolder))
            {
                if (!filename.EndsWith(StringConstants.ExceptSessionFilename))
                {
                    var hData = HierarchicalObject.FromFile(filename);
                    DialogSettings dialog = hData[0];
                    dialog.Message = NeedMessage(dialog.Id);
                    dialog.Vk = NeedApi();
                    Sessions.Add(dialog.Id, dialog);
                    dialog.Initialize();
                }
            }
        }

        static DialogSettings()
        {
            HierarchicalObjectCrutch.Register(typeof(DialogSettings));
        }

        public static Func<VkApi> NeedApi { get; set; }
        public static Func<long, Message> NeedMessage { get; set; }
    }
}
