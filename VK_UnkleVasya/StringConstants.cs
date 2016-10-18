using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VK_UnkleVasya
{
    public static class StringConstants
    {
        public static string AlbumsFileName = "settings/albums.xml";
        public static string CredentialsFileName = "settings/credentials.xml";
        public static string RandomMessagesFileName = "settings/random_messages.xml";
        public static string RandomIntervalMessagesFileName = "settings/random_interval_messages.xml";
        public static string SessionsFolder = "sessions/";
        public static string CommandsFolder = "Commands/";
        public static string ExceptSessionFilename = "folder_contains_session_files";

        public static string Dialog_IntervalOkResponse = "Понял, командир.";
        public static string Dialog_IntervalAlwaysDoResponse = "Так то уже херачу по интервалу, слепой чтоли?";
        public static string Dialog_IntervalAlwaysNotDoResponse = "А я и не начинал. Тупень.";
        public static string Dialog_IntervalOkStopResponse = "Прекращаю, шеф.";
    }
}
