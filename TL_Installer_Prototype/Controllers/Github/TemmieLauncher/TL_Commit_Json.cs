using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TL_Installer_Prototype.Controllers.Github.TemmieLauncher
{
    public class TL_Commit_Json
    {
        public class Root
        {
            public string sha { get; set; }
        }
        public Root[] getCommitJson(string GToken)
        {
            using (var client = new HttpClient())
            {
                Uri endpointArtifactJson = new Uri("https://api.github.com/repos/temmieheartz/fpPS4-Temmie-s-Launcher/commits");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {GToken}");
                client.DefaultRequestHeaders.Add("User-Agent", "Kimie");
                var responseMsg = client.GetAsync(endpointArtifactJson).Result;
                string responseBody = responseMsg.Content.ReadAsStringAsync().Result;
                Root[] commitJson = JsonConvert.DeserializeObject<Root[]>(responseBody);
                //string latestcommitsha = artifactsJson[0].sha.ToString();
                //Console.WriteLine(latestcommitsha);
                return commitJson;

            }
        }

        public string commitSha(string GToken)
        {
            Root[] latestcommit = getCommitJson(GToken);
            string latestcommitsha = latestcommit[0].sha.ToString();
            return latestcommitsha;

        }
    }
}
