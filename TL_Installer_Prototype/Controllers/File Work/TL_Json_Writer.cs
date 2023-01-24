using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TL_Installer_Prototype.Controllers.Github.TemmieLauncher
{
    public class TL_Json_Writer
    {

        public class Installation
        {
            public string temmieLDownloadedSha { get; set; }
            public string installationPath { get; set; }
        }
        public class Root
        {
            public List<Installation> installations { get; set; }
        }
        public Root parseTLJson(string filePath)
        {
            
            Root tl_config = JsonConvert.DeserializeObject<Root>(File.ReadAllText(filePath));
            return tl_config;
        }
        public void addInstallationLog(string filePath, string installationPath, string temmieCommitSha)
        {
            Root tl_config = parseTLJson(filePath);
            tl_config.installations.Add(new Installation
            {
                installationPath = installationPath,
                temmieLDownloadedSha = temmieCommitSha

            });
            var conf = JsonConvert.SerializeObject(tl_config, Formatting.Indented);
            File.WriteAllText(filePath, conf);
        }
        public void createTlJson(string filePath, string installationPath, string commitSha)
        {
            List<Installation> installations = new List<Installation>{
                new Installation
                {
                    installationPath = installationPath,
                    temmieLDownloadedSha = commitSha

                }
            };

            Root conf = new Root
            {
                installations = installations
            };
            var tlconf = JsonConvert.SerializeObject(conf);
            File.WriteAllText(filePath, tlconf);
        }

        public bool checkIfInstallationExists(string filePath,string installationPath)
        {
            Root tl_config = parseTLJson(filePath);
            int i = 0;
            int a = tl_config.installations.Count() - 1;

            bool chk = false;
            while (true)
            {
                if (tl_config.installations[i].installationPath.Equals(installationPath))
                {
                    chk = true;
                    break;
                }

                if (i == a)
                {
                    break;
                }
                if (i < a)
                {
                    i++;
                }


            }
            if (chk == true)
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        public string getShaFromInstallation(string filePath, string installationPath)
        {
            Root tl_config = parseTLJson(filePath);
            int i = 0;
            int index = 0;
            int a = tl_config.installations.Count() - 1;

            bool chk = false;
            while (true)
            {
                if (tl_config.installations[i].installationPath.Equals(installationPath))
                {
                    chk = true;
                    index = i;
                    break;
                }

                if (i == a)
                {
                    break;
                }
                if (i < a)
                {
                    i++;
                }


            }
            string sha = tl_config.installations[index].temmieLDownloadedSha.ToString();
            return sha;

        }


        public void changeLatestSha(string filePath,string installationPath, string latestsha)
        {
            Root tl_config = parseTLJson(filePath);
            int i = 0;
            int index = 0;
            int a = tl_config.installations.Count() - 1;

            bool chk = false;
            while (true)
            {
                if (tl_config.installations[i].installationPath.Equals(installationPath))
                {
                    chk = true;
                    index = i;
                    break;
                }

                if (i == a)
                {
                    break;
                }
                if (i < a)
                {
                    i++;
                }


            }
            //string sha = tl_config.installations[index].temmieLDownloadedSha.ToString();
            tl_config.installations[index].temmieLDownloadedSha = latestsha;
            var tlconf = JsonConvert.SerializeObject(tl_config, Formatting.Indented);
            File.WriteAllText(filePath, tlconf);
        }

        public bool isTlJsonEmpty(string filePath)
        {
            
            if (File.ReadAllText(filePath) == "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
