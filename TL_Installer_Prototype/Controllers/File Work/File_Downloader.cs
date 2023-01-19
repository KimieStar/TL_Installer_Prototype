using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TL_Installer_Prototype.Controllers
{
    public class File_Downloader
    {
        public void Downloader(string link, string downloadFilePath)
        {
            using (var webClient = new WebClient())
            {
                webClient.DownloadFile(link, downloadFilePath);
            }
        }
    }
}
