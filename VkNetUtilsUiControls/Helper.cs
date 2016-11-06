using System.Net;

namespace UiControls
{
    public static class Helper
    {
        public static string GetCaptchaValue(string uri)
        {
            var form = new CaptchaEnterForm(uri);
            form.ShowDialog();
            return form.Value;
        }

        public static bool ConfirmVkForProxy(WebProxy proxy, string caption)
        {
            if (new VkPhoneConfirmForm(proxy)
            {
                Text = caption
            }.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                return true;
            else return false;
        }

        public static void ValidateVk(string uri, WebProxy proxy)
        {
            new VkValidationForm(uri, proxy).ShowDialog();
        }
    }
}
