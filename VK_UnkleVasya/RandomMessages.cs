using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VK_UnkleVasya
{
    public static class RandomMessages
    {
        private static string[] _messages;
        private static string[] _messages_interval;

        public static string GetNext()
        {
            lock (_messages)
            {
                return _messages[Utils.GetNextRandom(0, _messages.Length - 1)];
            }
        }

        public static string GetNext_Interval()
        {
            lock (_messages)
            {
                return _messages_interval[Utils.GetNextRandom(0, _messages_interval.Length - 1)];
            }
        }

        public static void Reload()
        {
            lock (_messages)
            {
                _messages = Utils.LoadStringData(StringConstants.RandomMessagesFileName);
                _messages_interval = Utils.LoadStringData(StringConstants.RandomIntervalMessagesFileName);
            }
        }
    }
}
