using DotNetBrowser;
using DotNetBrowser.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UiControls
{
    public partial class VkValidationForm : Form
    {
        Control wb;
        public VkValidationForm(string uri, WebProxy proxy)
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
                        BrowserType.HEAVYWEIGHT));
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
    }
}
