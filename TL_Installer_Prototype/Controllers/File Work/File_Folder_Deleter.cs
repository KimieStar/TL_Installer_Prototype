using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TL_Installer_Prototype.Controllers
{
    internal class File_Folder_Deleter
    {
        public void Delete_Files_And_Folders(string srcPath)
        {

            if (Directory.Exists(srcPath))
            {

                foreach (var file in new DirectoryInfo(srcPath).GetFiles())
                {
                    string srcFileName = Path.Combine(srcPath, file.Name);
                    File.Delete(srcFileName);
                }

                int numofDir = new DirectoryInfo(srcPath).GetDirectories().Length;

                if (new DirectoryInfo(srcPath).GetDirectories().Count() > 0)
                {
                    foreach (var folder in new DirectoryInfo(srcPath).GetDirectories())
                    {
                        string directoryInDirectory = Path.Combine(srcPath, folder.Name);
                        Delete_Files_And_Folders(directoryInDirectory);
                        Directory.Delete(directoryInDirectory);

                    }
                }

            }
            else
            {
                throw new Exception("Folder does not exist");
            }

        }
    }
}
