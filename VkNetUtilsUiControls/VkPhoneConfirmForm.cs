using DotNetBrowser;
using DotNetBrowser.WinForms;
using System;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace UiControls
{
    public partial class VkPhoneConfirmForm : Form
    {
        Control wb;
        public VkPhoneConfirmForm(WebProxy proxy)
        {
            InitializeComponent();

            BrowserView browser = null;

            if (proxy != null)
            {
                var dataDir = Path.GetFullPath("chromium-data");
                var contextParams = new BrowserContextParams(dataDir);
                var proxyRules = "https=" + proxy.Address.Host + ":" + proxy.Address.Port;
                contextParams.ProxyConfig = new CustomProxyConfig(proxyRules);

                browser = new WinFormsBrowserView(
                    BrowserFactory.Create(
                        new BrowserContext(contextParams),
                        BrowserType.LIGHTWEIGHT));
            }
            else
            {
                browser = new WinFormsBrowserView();
            }

            wb = (Control)browser;
            this.Controls.Add(wb);
            wb.Dock = DockStyle.Fill;
            browser.Browser.LoadURL("https://vk.com");
        }

        private void btConfirm_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
