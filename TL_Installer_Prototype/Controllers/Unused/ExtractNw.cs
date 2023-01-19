using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace TL_Installer_Prototype.Controllers.Unused
{
    public class ExtractNw
    {
        public void Extract(string NameSpace, string outDir, string internalFilePath, string resourceName)
        {
            string Name = NameSpace + "." + (internalFilePath == "" ? "" : internalFilePath + ".") + resourceName;
            Assembly assembly = Assembly.GetCallingAssembly();
            Stream s = assembly.GetManifestResourceStream(Name);
            if (s == null)
            {
                throw new ApplicationException();
            }
            using (BinaryReader r = new BinaryReader(s))
            using (FileStream fs = new FileStream(outDir + "\\" + resourceName, FileMode.OpenOrCreate))
            using (BinaryWriter w = new BinaryWriter(fs))
                w.Write(r.ReadBytes((int)s.Length));
        }

    }
}
