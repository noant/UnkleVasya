using System;
using System.Linq;
using VkNet;
using VkNet.Model.RequestParams;
using VkNet.Utils.AntiCaptcha;

namespace VkCaptchaSolver
{
    public class VkCaptchaSolver : ICaptchaSolver
    {
        #region static

        static VkCaptchaSolver()
        {
            Vk = new VkApi(new VkCaptchaUiSolver());
            Vk.Authorize(new ApiAuthParams()
            {
                ApplicationId = Settings.Current.ApplicationId,
                Login = Settings.Current.Login,
                Password = Settings.Current.Password,
                Settings = VkNet.Enums.Filters.Settings.All
                        | VkNet.Enums.Filters.Settings.Offline
            });
            UsersSolvers.InitializeUsersIds(Vk);
        }
        public static VkApi Vk { get; private set; }

        #endregion

        public void CaptchaIsFalse()
        {
            //do nothing
        }

        public string Solve(string url)
        {
            lock (Vk)
            {
                var userId = UsersSolvers.Current.GetRandUserId();
                Vk.Messages.Send(new MessagesSendParams()
                {
                    UserId = userId,
                    Message = url
                });
                var startDate = DateTime.Now;
                while (true)
                {
                    if ((DateTime.Now - startDate).TotalMilliseconds > Settings.Current.Timeout)
                        return string.Empty; //time is out

                    var dialogs = Vk.Messages.GetDialogs(new MessagesDialogsGetParams()
                    {
                        Unread = true,
                        Count = 200,
                        Offset = 0
                    });

                    var response = dialogs.Messages.FirstOrDefault(x => x.UserId == userId);

                    if (response != null)
                        return response.Body;

                    VkUtils.TechnicalSleepForVk();
                }
            }
        }
    }
}
