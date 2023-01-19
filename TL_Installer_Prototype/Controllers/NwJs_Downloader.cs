using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TL_Installer_Prototype.Controllers
{
    public class NwJs_Downloader
    {
        public void nwJsDownload()
        {
            File_Downloader fd = new File_Downloader();
            string downloadFilePath = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TL_Installer", "tempDownload", "nwjs.zip"));
            fd.Downloader("https://dl.nwjs.io/v0.70.1/nwjs-v0.70.1-win-x64.zip", downloadFilePath);
        }
    }
}
