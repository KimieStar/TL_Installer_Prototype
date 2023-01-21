using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TL_Installer_Prototype.Controllers.Github.fpPS4
{
    public class fpPS4_Get_Workflows
    {
        public class Root
        {
            public int total_count { get; set; }
            public List<Workflow> workflows { get; set; }
        }

        public class Workflow
        {
            public int id { get; set; }
            public string node_id { get; set; }
            public string name { get; set; }
            public string path { get; set; }
            public string state { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }
            public string url { get; set; }
            public string html_url { get; set; }
            public string badge_url { get; set; }
        }


        public Root getAllWorkFlows(string GToken)
        {
            using (var client = new HttpClient())
            {
                Uri endpoint = new Uri("https://api.github.com/repos/red-prig/fpPS4/actions/workflows");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {GToken}");
                client.DefaultRequestHeaders.Add("User-Agent", "Kimie");
                var responseMsg = client.GetAsync(endpoint).Result;
                string responseBody = responseMsg.Content.ReadAsStringAsync().Result;
                Root allWorkFlowsJson = JsonConvert.DeserializeObject<Root>(responseBody);
                return allWorkFlowsJson;

            }
        }

        public string getMainWorkflowID(string GToken)
        {
            int i = 0;
            Root flows = getAllWorkFlows(GToken);
            while (true)
            {
                if (flows.workflows[i].path.ToString().Contains("main.yml"))
                {
                    break;
                }
                i++;
            }
            
            string flowid = flows.workflows[i].id.ToString();
            return flowid;
        }

    }
}
