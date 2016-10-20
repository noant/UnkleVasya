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
        public DialogSettings(long id, VkApi vk, Message message)
        {
            Id = id;
            Vk = vk;
            Message = message;
        }

        public DialogSettings() { }

        public long Id { get; set; }

        [XmlIgnore]
        public VkApi Vk { get; set; }

        [XmlIgnore]
        public Message Message { get; set; }

        public int TotalImagesSent { get; set; } //not by interval dispatching

        public void IncrementImgsCountAndCommit()
        {
            TotalImagesSent++;
            SaveToFile();
        }

        private Thread IntervalDispatcher { get; set; }

        public bool IsIntervalDispatcherStarted
        {
            get
            {
                return IntervalDispatcher != null;
            }
            set
            {
                if (value)
                    StartIntevalDispatching(false, false);
                else StopIntervalDispatching(false, false);
            }
        }

        public void SaveToFile()
        {
            var filename = StringConstants.SessionsFolder + this.Id + ".xml";
            var hData = new HierarchicalObject(filename);
            hData[0] = this;
            hData.SaveToFile();
        }

        private void StartIntevalDispatching(bool sendMessage, bool save)
        {
            if (IntervalDispatcher == null)
            {
                IntervalDispatcher = new Thread(() =>
                {
                    while (true)
                    {
                        try
                        {
                            lock (Vk) VkUtils.SendImages(Vk, Message, VkUtils.GetRandomPictures(Vk, 5), VkUtils.GetNextMessageForDialog_Ad(this, true));
                            Thread.Sleep(1000 * 60 * 60);
                        }
                        catch (Exception e)
                        {
                            Log.Write(e);
                        }
                    }
                });
                IntervalDispatcher.Start();
                if (sendMessage)
                    lock (Vk) VkUtils.SendMessage(Vk, Message, StringConstants.Dialog_IntervalOkResponse);
                if (save)
                    SaveToFile();
            }
            else if (sendMessage)
                lock (Vk) VkUtils.SendMessage(Vk, Message, StringConstants.Dialog_IntervalAlwaysDoResponse);
        }

        private void StopIntervalDispatching(bool sendMessage, bool save)
        {
            if (IntervalDispatcher != null)
            {
                IntervalDispatcher.Abort();
                IntervalDispatcher = null;

                if (sendMessage)
                    lock (Vk) VkUtils.SendMessage(Vk, Message, StringConstants.Dialog_IntervalOkStopResponse);

                if (save)
                    SaveToFile();
            }
            else if (sendMessage) VkUtils.SendMessage(Vk, Message, StringConstants.Dialog_IntervalAlwaysNotDoResponse);
        }

        public void StartIntervalDispatching()
        {
            StartIntevalDispatching(true, true);
        }

        public void StopIntervalDispatching()
        {
            StopIntervalDispatching(true, true);
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
