using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TL_Installer_Prototype.Controllers.Github.fpPS4
{
    public class Artifact
    {
        public int id { get; set; }
        public string node_id { get; set; }
        public string name { get; set; }
        public int size_in_bytes { get; set; }
        public string url { get; set; }
        public string archive_download_url { get; set; }
        public bool expired { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime expires_at { get; set; }
        public WorkflowRun workflow_run { get; set; }
    }

    public class Root
    {
        public int total_count { get; set; }
        public List<Artifact> artifacts { get; set; }
    }

    public class WorkflowRun
    {
        public object id { get; set; }
        public int repository_id { get; set; }
        public int head_repository_id { get; set; }
        public string head_branch { get; set; }
        public string head_sha { get; set; }
    }
    public class fpPS4_Artifact_Json
    {
        public Root getJsonAndParse(string GToken, Uri endpoint)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {GToken}");
                client.DefaultRequestHeaders.Add("User-Agent", "Kimie");
                var responseMsg = client.GetAsync(endpoint).Result;
                string responseBody = responseMsg.Content.ReadAsStringAsync().Result;
                Root artifactsJson = JsonConvert.DeserializeObject<Root>(responseBody);
                return artifactsJson;

            }
        }

        public int getLatestTrunkArtifactID(string GToken, Uri endpoint)
        {
            Root artifactsJson = getJsonAndParse(GToken, endpoint);
            int i = 0;
            while (true)
            {
                if (artifactsJson.artifacts[i].workflow_run.head_branch == "trunk")
                {

                    break;
                }
                i++;
            }

            int latestArtifactID = artifactsJson.artifacts[i].id;
            return latestArtifactID;
        }

        public int getLatestArtifactID(string GToken, Uri endpoint)
        {
            Root artifactsJson = getJsonAndParse(GToken, endpoint);
            int latestArtifactID = artifactsJson.artifacts[0].id;
            return latestArtifactID;
        }

        public string getLatestArtifactSha(string GToken, Uri endpoint)
        {
            Root artifactsJson = getJsonAndParse(GToken, endpoint);
            int i = 0;
            while (true)
            {
                if (artifactsJson.artifacts[i].workflow_run.head_branch == "trunk")
                {

                    break;
                }
                i++;
            }

            string latestArtifactID = artifactsJson.artifacts[i].workflow_run.head_sha;
            return latestArtifactID;
        }
    }
}
