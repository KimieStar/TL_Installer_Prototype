using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TL_Installer_Prototype.Controllers.Github.fpPS4
{
    public class fpPS4_Action_Downloader
    {
        public void downloadLatestTrunkAction(string GToken)
        {

            fpPS4_Action_Link_Grabber githubLinkGrabber = new fpPS4_Action_Link_Grabber();
            fpPS4_Artifact_Json gj = new fpPS4_Artifact_Json();

            string downloadFilePath = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TL_Installer", "tempDownload", "fpPS4.zip"));
            Uri endpointArtifactJson = new Uri("https://api.github.com/repos/red-prig/fpPS4/actions/artifacts");
            int latestTrunkArtifactId = gj.getLatestTrunkArtifactID(GToken, endpointArtifactJson);
            Uri endpointLatestAction = new Uri("https://api.github.com/repos/red-prig/fpPS4/actions/artifacts/" + $"{latestTrunkArtifactId}" + "/zip");
            string link = githubLinkGrabber.LinkGrabber(endpointLatestAction, GToken);
            File_Downloader dw = new File_Downloader();
            dw.Downloader(link, downloadFilePath);
        }

        public void downloadLatestAction(string GToken)
        {
            fpPS4_Action_Link_Grabber githubLinkGrabber = new fpPS4_Action_Link_Grabber();
            fpPS4_Artifact_Json gj = new fpPS4_Artifact_Json();

            string downloadFilePath = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TL_Installer", "tempDownload", "fpPS4.zip")); 
            string latestArtifactId = gj.getLatestArtifactID(GToken);
            Uri endpointLatestAction = new Uri("https://api.github.com/repos/red-prig/fpPS4/actions/artifacts/" + $"{latestArtifactId}" + "/zip");
            string link = githubLinkGrabber.LinkGrabber(endpointLatestAction, GToken);
            File_Downloader dw = new File_Downloader();
            dw.Downloader(link, downloadFilePath);
        }
    }
}
