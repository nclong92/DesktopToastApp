using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.UI.Notifications;

namespace DesktopToastsApp
{
    public class NewToastNotification
    {
        public NewToastNotification(string title, string content, int type)
        {
            ToastTemplateType toastTemplate = ToastTemplateType.ToastImageAndText01;
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);

            var uri = @"https://docs.microsoft.com/en-us/dotnet/standard/data/xml/reading-an-xml-document-into-the-dom";

            XmlNodeList bindingAttributes = toastXml.GetElementsByTagName("binding");
            ((XmlElement)bindingAttributes[0]).SetAttribute("baseUri", uri);

            XmlNodeList visualAttributes = toastXml.GetElementsByTagName("visual");
            ((XmlElement)visualAttributes[0]).SetAttribute("baseUri", uri);

            XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
            toastTextElements[0].AppendChild(toastXml.CreateTextNode($"{content}"));

            XmlNodeList toastImageAttributes = toastXml.GetElementsByTagName("image");
            ((XmlElement)toastImageAttributes[0]).SetAttribute("src", "C:\\Users\\IsaoTakashi\\Desktop\\desktop-toasts-master\\CS\\DesktopToastsApp\\Images\\883-364x202.jpg");
            ((XmlElement)toastImageAttributes[0]).SetAttribute("alt", "red graphic");

            IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
            ((XmlElement)toastNode).SetAttribute("duration", "long");
            //((XmlElement)toastNode).SetAttribute("launch", "{\"type\":\"toast\",\"param1\":\"12345\",\"param2\":\"67890\"}");
            //((XmlElement)toastNode).SetAttribute("launch", uri);

            ToastNotification toast = new ToastNotification(toastXml);

            toast.Activated += Toast_Activated;

            ToastNotificationManager.CreateToastNotifier($"{title}").Show(toast);
        }

        private void Toast_Activated(ToastNotification sender, object args)
        {
            NewToastNotification.OpenBrowser("http://vtc.org.vn/");
            Console.WriteLine("Hello world");
        }

        public static void OpenBrowser(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }

    }
}