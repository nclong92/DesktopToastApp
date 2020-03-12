using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopToastsApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            NewToastNotification Window = new NewToastNotification("VTC RD Notification", "This is notify from VTC ITS Center. Thank you for watching.", 3);

            await Task.Delay(5000);
        }
    }
}
