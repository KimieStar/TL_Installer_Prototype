using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TL_Installer_Prototype.Controllers.Github.fpPS4;

namespace TL_Installer_Prototype.Controllers.Github.TemmieLauncher
{
    public class TL_Downloader
    {
        public void downloadLatestRelease(string GToken)
        {
            TL_Json_And_DownloadLink tw = new TL_Json_And_DownloadLink();

            string downloadFilePath = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TL_Installer", "tempDownload", "Launcher.zip"));
            Uri endpointLatestRelease = new Uri("https://api.github.com/repos/temmieheartz/fpPS4-Temmie-s-Launcher/releases/latest");
            string link = tw.latestReleaseDownloadLink(GToken, endpointLatestRelease);
            File_Downloader dw = new File_Downloader();
            dw.Downloader(link, downloadFilePath);
        }

        public void downloadFromMain()
        {
            string downloadFilePath = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TL_Installer", "tempDownload", "fpPS4-Temmie-s-Launcher-main.zip"));
            File_Downloader dw = new File_Downloader();
            dw.Downloader("https://github.com/temmieheartz/fpPS4-Temmie-s-Launcher/archive/refs/heads/main.zip", downloadFilePath);
        }
    }
}
