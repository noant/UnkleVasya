using System;
using System.Windows.Forms;

namespace UiControls
{
    public partial class CaptchaEnterForm : Form
    {
        public CaptchaEnterForm(string uri)
        {
            InitializeComponent();
            this.wbCaptcha.Url = new Uri(uri);
        }

        public string Value
        {
            get
            {
                return tbValue.Text;
            }
        }
    }
}
