using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TL_Installer_Prototype.Controllers.Github.fpPS4
{
    public class fpPS4_Action_Link_Grabber
    {
        public string LinkGrabber(Uri endpoint, string GToken)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {GToken}");
                client.DefaultRequestHeaders.Add("User-Agent", "Kimie");
                var responseMsg = client.GetAsync(endpoint).Result;
                string link = responseMsg.RequestMessage.RequestUri.ToString();
                return link;
            }
        }
    }
}
