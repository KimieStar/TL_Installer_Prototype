using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TL_Installer_Prototype.Controllers
{
    public class File_Folder_Mover
    {
        public void Move_Files_And_Folders(string srcPath, string destPath)
        {

            if (Directory.Exists(srcPath))
            {

                if (Directory.Exists(destPath))
                {

                    foreach (var file in new DirectoryInfo(srcPath).GetFiles())
                    {
                        string fileExistPath = Path.Combine(destPath, file.Name);
                        string srcFileName = Path.Combine(srcPath, file.Name);
                        string destFileName = Path.Combine(destPath, file.Name);
                        if (File.Exists(fileExistPath))
                        {
                            File.Delete(fileExistPath);
                        }
                        File.Move(srcFileName, destFileName);
                    }

                    int numofDir = new DirectoryInfo(srcPath).GetDirectories().Length;

                    if (new DirectoryInfo(srcPath).GetDirectories().Count() > 0)
                    {
                        foreach (var folder in new DirectoryInfo(srcPath).GetDirectories())
                        {
                            string directoryExistsPath = Path.Combine(destPath, folder.Name);
                            string directoryInDirectory = Path.Combine(srcPath, folder.Name);
                            if (!Directory.Exists(directoryExistsPath))
                            {
                                Directory.CreateDirectory(directoryExistsPath);
                            }


                            Move_Files_And_Folders(directoryInDirectory, directoryExistsPath);
                            Directory.Delete(directoryInDirectory);

                        }
                    }

                }
                else
                {
                    foreach (var file in new DirectoryInfo(srcPath).GetFiles())
                    {
                        string fileExistPath = Path.Combine(destPath, file.Name);
                        string srcFileName = Path.Combine(srcPath, file.Name);
                        string destFileName = Path.Combine(destPath, file.Name);
                        if (File.Exists(fileExistPath))
                        {
                            File.Delete(fileExistPath);
                        }
                        File.Move(srcFileName, destFileName);
                    }

                    int numofDir = new DirectoryInfo(srcPath).GetDirectories().Length;

                    if (new DirectoryInfo(srcPath).GetDirectories().Count() > 0)
                    {
                        foreach (var folder in new DirectoryInfo(srcPath).GetDirectories())
                        {
                            string directoryExistsPath = Path.Combine(destPath, folder.Name);
                            string directoryInDirectory = Path.Combine(srcPath, folder.Name);
                            if (!Directory.Exists(directoryExistsPath))
                            {
                                Directory.CreateDirectory(directoryExistsPath);
                            }


                            Move_Files_And_Folders(directoryInDirectory, directoryExistsPath);
                            Directory.Delete(directoryInDirectory);

                        }
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

