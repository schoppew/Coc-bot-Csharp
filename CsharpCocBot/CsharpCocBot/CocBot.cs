using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace CoC.Bot
{
    class CocBot
    {
        const string botVersion = "5.6";
        const string botTitle = "COC Bot v" + botVersion;

        public CocBot()
        {
            DownloadLicense();
            CreateDirectories();
        }

        private void CreateDirectories()
        {
            if (!Directory.Exists(@"COCBot\Loots\"))
            {
                Directory.CreateDirectory(@"COCBot\Loots\");
            }

            if(!Directory.Exists(@"COCBot\logs\"))
            {
                Directory.CreateDirectory(@"COCBot\logs\");
            }
        }

        private void DownloadLicense()
        {
            if(!File.Exists("License.txt"))
            {
                WebClient wc = new WebClient();
                string license = wc.DownloadString("http://www.gnu.org/licenses/gpl-3.0.txt");
                File.WriteAllText("License.txt", license);
            }
        }
    }
}
