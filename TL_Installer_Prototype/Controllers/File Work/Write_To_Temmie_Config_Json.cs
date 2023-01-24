using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TL_Installer_Prototype.Controllers
{
    internal class Write_To_Temmie_Config_Json
    {
        public class Sha
        {
            public string latestCommitSha { get; set; }
        }

        public void parseAndWriteTemmieJsonConfig_sha(string JsonFilePath, string newSha)
        {
            Sha newSettings = new Sha();
            newSettings.latestCommitSha = newSha;
            var newJson = JsonConvert.SerializeObject(newSettings, Formatting.None);
            File.WriteAllText(JsonFilePath, newJson);

        }
    }
}
