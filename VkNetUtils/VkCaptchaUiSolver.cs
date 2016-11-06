using UiControls;
using VkNet.Utils.AntiCaptcha;

namespace VkNet
{
    public class VkCaptchaUiSolver : ICaptchaSolver
    {
        public void CaptchaIsFalse()
        {
        }

        public string Solve(string url)
        {
            return Helper.GetCaptchaValue(url);
        }
    }
}
