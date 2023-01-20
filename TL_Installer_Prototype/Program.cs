using System.IO;
using TL_Installer_Prototype.Controllers;
using TL_Installer_Prototype.Controllers.Github.fpPS4;
using TL_Installer_Prototype.Controllers.Github.TemmieLauncher;
using System.Configuration;
using TL_Installer_Prototype.Controllers.File_Work;
using System.Runtime.InteropServices;

namespace TL_Installer_Prototype
{
    class Program
    {
        static void Main(string[] args)
        {
            ////////////////Declaring Variables//////////////////////////
            
            ///
            /// Declaring common string variables
            ///
            string label_processComplete = "\n~~~~~~~~~~~~~~~~~~~~> Done!",
                label_pressAnyKey = "\n~~~~~~~~~~~~~~~~~~~~> Press any key to exit",
                label_extractPleaseWait = "\n~~~~~~~~~~~~~~~~~~~~> Extracting files - please wait...",
                label_downloadingFiles = "\n~~~~~~~~~~~~~~~~~~~~> Downloading required files! Please wait.",
                label_pathNotFound = "\n~~~~~~~~~~~~~~~~~~~~> This is an invalid path (or provided path was not found) - Please try again";

            ///
            /// Declaring Menu Operation Variables
            ///
            string operation,
                InstallDir,
                customInstallDir,
                operationMenuInstall;

            ///
            /// Declaring Manual Download File Paths
            ///
            // (nwJsPath) This seems to be declared - but it's never used [Warn list]
            string fpps4Path,
                temmiePath,
                temmie_mainPath;

            ///
            /// Declaring Bools
            ///
            bool instSoF = false,
                checkForTL,
                checkForNw,
                checkForFp,
                checkForTL_Main;

            ///
            /// Declaring Enviorment Paths
            ///
            string TLFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TL_Installer"),
                tempDownloadFolder = Path.Combine(TLFolder, "tempDownload"),
                downloadFolderInstallDir = Path.Combine(Path.Combine(System.Environment.GetEnvironmentVariable("USERPROFILE"), "Downloads"), "Temmie Launcher");

            ///
            ///  Declaring Files with Extensions(zip)
            ///

            string tLFileNameWIthExtension,
                fpps4FileNameWIthExtension,
                tL_Main_FileNameWIthExtension,
                nwjs_FileNameWithExtension = Path.GetFileName(Path.Combine(tempDownloadFolder,"nwjs.zip"));

            ///
            /// Declaring References to classes
            ///

            Logos logos = new Logos();
            Extract_Files extractor = new Extract_Files();
            TL_Downloader tlDownloader = new TL_Downloader();
            File_Folder_Mover mover = new File_Folder_Mover();
            NwJs_Downloader nwjsDownloader = new NwJs_Downloader();
            File_Folder_Deleter deleter = new File_Folder_Deleter();
            fpPS4_Artifact_Json artifactJson = new fpPS4_Artifact_Json();
            fpPS4_Action_Downloader actionDownloader = new fpPS4_Action_Downloader();
            Write_To_Temmie_Config_Json writeTemmieConfig = new Write_To_Temmie_Config_Json();
            

                         /////////Declaring fpPS4 Action Variables//////////
            string latestArtifactSha, GToken = "";
            Uri endpointArtifactJson = new Uri("https://api.github.com/repos/red-prig/fpPS4/actions/artifacts");
            latestArtifactSha = artifactJson.getLatestArtifactSha(GToken,endpointArtifactJson);

            ///
            /// Creating TL_Installer Custom Folder
            ///

            if (!Directory.Exists(TLFolder))
            {
                DirectoryInfo di = Directory.CreateDirectory(TLFolder);
                di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                Directory.CreateDirectory(tempDownloadFolder);
            }

            ///
            /// Visuals
            ///

            while (true)
            {
                logos.Logo();
                operation = Console.ReadLine();

                if (operation == "1")
                {
                    Console.Clear();

                    while (true)
                    {
                        logos.Logo3();
                        operationMenuInstall = Console.ReadLine();

                        if (operationMenuInstall == "1")
                        {
                            ///
                            /// Extracting or Downloading files if needed
                            ///
                            logos.Logo4();
                            if (!File.Exists(Path.Combine(tempDownloadFolder, "fpPS4.zip")))
                            {
                                Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> Downloading fpPS4! Please wait.");
                                actionDownloader.downloadLatestAction(GToken);
                                Console.WriteLine(label_processComplete);
                                checkForFp = true;
                            }
                            else if (File.Exists(Path.Combine(tempDownloadFolder, "fpPS4.zip")))
                            {
                                Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> fpPS4 is already downloaded.");
                                checkForFp = true;
                            }
                            else
                            {
                                checkForFp = false;
                            }
                            Console.WriteLine(label_extractPleaseWait);
                            extractor.ExtractZipContent(Path.Combine(tempDownloadFolder,"fpPS4.zip"),Path.Combine(tempDownloadFolder,"fpPS4"));
                            Console.WriteLine(label_processComplete);
                            checkForFp = true;
                            if (!File.Exists(Path.Combine(tempDownloadFolder, "nwjs.zip")))
                            {
                                Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> Downloading Nw.js! Please wait.");
                                nwjsDownloader.nwJsDownload();
                                Console.WriteLine(label_processComplete);
                                checkForNw = true;
                            }
                            else if (File.Exists(Path.Combine(tempDownloadFolder, "nwjs.zip")))
                            {
                                Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> Nw.js is already downloaded.");
                                checkForNw = true;
                            }
                            else
                            {
                                checkForNw = false;
                            }
                            Console.WriteLine(label_extractPleaseWait);
                            extractor.ExtractZipContent(Path.Combine(tempDownloadFolder, "nwjs.zip"), Path.Combine(tempDownloadFolder, "nwjs"));
                            Console.WriteLine(label_processComplete);
                            checkForNw = true;
                            if (!File.Exists(Path.Combine(tempDownloadFolder, "Launcher.zip")))
                            {
                                Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> Downloading Launcher! Please wait.");
                                tlDownloader.downloadLatestRelease(GToken);
                                Console.WriteLine(label_processComplete);
                                checkForTL = true;
                            }
                            else if (File.Exists(Path.Combine(tempDownloadFolder, "Launcher.zip")))
                            {
                                Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> Launcher is already downloaded.");
                                checkForTL = true;
                            }
                            else
                            {
                                checkForTL = false;
                            }
                            Console.WriteLine(label_extractPleaseWait);
                            extractor.ExtractZipContent(Path.Combine(tempDownloadFolder, "Launcher.zip"), Path.Combine(tempDownloadFolder, "Launcher"));
                            Console.WriteLine(label_processComplete);
                            if (!File.Exists(Path.Combine(tempDownloadFolder, "fpPS4-Temmie-s-Launcher-main.zip")))
                            {
                                Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> Downloading fpPS4-Temmie-s-Launcher-main! Please wait.");
                                tlDownloader.downloadFromMain();
                                Console.WriteLine(label_processComplete);
                                checkForTL_Main = true;
                            }
                            else if (File.Exists(Path.Combine(tempDownloadFolder, "fpPS4-Temmie-s-Launcher-main.zip")))
                            {
                                Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> fpPS4-Temmie-s-Launcher-main is already downloaded.");
                                checkForTL_Main = true;
                            }
                            else
                            {
                                checkForTL_Main = false;
                            }
                            Console.WriteLine(label_extractPleaseWait);
                            extractor.ExtractZipContent(Path.Combine(tempDownloadFolder, "fpPS4-Temmie-s-Launcher-main.zip"), Path.Combine(tempDownloadFolder, "fpPS4-Temmie-s-Launcher-main"));
                            Console.WriteLine(label_processComplete);

                            logos.Logo4();
                            /// Checks if the needed files exist
                            /// Getting Installation Directory
                            /// Installation
                            if (checkForFp == true && checkForNw == true && checkForTL == true && checkForTL_Main == true)
                            {
                                


                                while (true)
                                {
                                    Console.Write("\n~~~~~~~~~~~~~~~~~~~~> Enter installation path (Leave blank to install in Downloads folder): ");
                                    InstallDir = Console.ReadLine();
                                    if (InstallDir.StartsWith("\"") && InstallDir.EndsWith("\""))
                                    {
                                        InstallDir = InstallDir.Remove(0, 1);
                                        InstallDir = InstallDir.Remove(InstallDir.Length - 1, 1);
                                    }
                                    customInstallDir = Path.Combine(InstallDir, "Temmie Launcher");
                                    if (Directory.Exists(InstallDir) || InstallDir == "")
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine(label_pathNotFound);
                                    }
                                }

                                ///
                                /// Installing to Download Folder
                                ///
                                if (InstallDir == "")
                                {
                                    string GamesFolderDownloadInstallDir = Path.Combine(downloadFolderInstallDir, "Games");
                                    ///
                                    /// Checking if downloadFolderInstallDir Exists
                                    ///
                                    if (!Directory.Exists(downloadFolderInstallDir))
                                    {
                                        Directory.CreateDirectory(downloadFolderInstallDir);


                                        ///
                                        /// Creating/Deleting Directories and Moving files to their appropriate folders 
                                        ///
                                        Console.WriteLine($"\n~~~~~~~~~~~~~~~~~~~~> Moving Files...");
                                        mover.Move_Files_And_Folders(Path.Combine(tempDownloadFolder,"nwjs", "nwjs-v0.70.1-win-x64"), downloadFolderInstallDir);
                                        Directory.Delete(Path.Combine(tempDownloadFolder, "nwjs", "nwjs-v0.70.1-win-x64"));
                                        Directory.Delete(Path.Combine(tempDownloadFolder, "nwjs"));
                                        mover.Move_Files_And_Folders(Path.Combine(tempDownloadFolder, "Launcher"),downloadFolderInstallDir);
                                        Directory.Delete(Path.Combine(tempDownloadFolder, "Launcher"));
                                        mover.Move_Files_And_Folders(Path.Combine(tempDownloadFolder, "fpPS4-Temmie-s-Launcher-main", "fpPS4-Temmie-s-Launcher-main"), downloadFolderInstallDir);
                                        Directory.Delete(Path.Combine(tempDownloadFolder, "fpPS4-Temmie-s-Launcher-main", "fpPS4-Temmie-s-Launcher-main"));
                                        Directory.Delete(Path.Combine(tempDownloadFolder, "fpPS4-Temmie-s-Launcher-main"));
                                        Directory.CreateDirectory(Path.Combine(downloadFolderInstallDir, "Emu"));
                                        mover.Move_Files_And_Folders(Path.Combine(tempDownloadFolder, "fpPS4"), Path.Combine(downloadFolderInstallDir,"Emu"));
                                        Directory.Delete(Path.Combine(tempDownloadFolder, "fpPS4"));

                                        ///
                                        /// Creating Games and Creating Settings.json
                                        ///
                                        Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> Creating 'Games' folder");
                                        Directory.CreateDirectory(GamesFolderDownloadInstallDir);
                                        writeTemmieConfig.parseAndWriteTemmieJsonConfig_sha(Path.Combine(downloadFolderInstallDir, "Settings.json"),latestArtifactSha);
                                        File.Delete(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TL_Installer", "tempDownload", "Launcher.zip")));
                                        File.Delete(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TL_Installer", "tempDownload", "fpPS4-Temmie-s-Launcher-main.zip")));
                                        File.Delete(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TL_Installer", "tempDownload", "fpPS4.zip")));
                                        instSoF = true;
                                    }
                                    else if (Directory.Exists(downloadFolderInstallDir))
                                    {
                                        if (Directory.GetFileSystemEntries(downloadFolderInstallDir).Count() > 0)
                                        {
                                            string answer = "";
                                            Console.Write("\n~~~~~~~~~~~~~~~~~~~~> Do you want to overwrite folder 'Temmie Launcher' (Y/n): ");

                                            while (true)
                                            {
                                                answer = Console.ReadLine();
                                                if (answer == "y" || answer == "Y")
                                                {
                                                    ///
                                                    /// Creating/Deleting Directories and Moving files to their appropriate folders 
                                                    ///

                                                    deleter.Delete_Files_And_Folders(downloadFolderInstallDir);

                                                    Console.WriteLine($"\n~~~~~~~~~~~~~~~~~~~~> Moving Files...");
                                                    mover.Move_Files_And_Folders(Path.Combine(tempDownloadFolder, "nwjs", "nwjs-v0.70.1-win-x64"), downloadFolderInstallDir);
                                                    Directory.Delete(Path.Combine(tempDownloadFolder, "nwjs", "nwjs-v0.70.1-win-x64"));
                                                    Directory.Delete(Path.Combine(tempDownloadFolder, "nwjs"));
                                                    mover.Move_Files_And_Folders(Path.Combine(tempDownloadFolder, "Launcher"), downloadFolderInstallDir);
                                                    Directory.Delete(Path.Combine(tempDownloadFolder, "Launcher"));
                                                    mover.Move_Files_And_Folders(Path.Combine(tempDownloadFolder, "fpPS4-Temmie-s-Launcher-main", "fpPS4-Temmie-s-Launcher-main"), downloadFolderInstallDir);
                                                    Directory.Delete(Path.Combine(tempDownloadFolder, "fpPS4-Temmie-s-Launcher-main", "fpPS4-Temmie-s-Launcher-main"));
                                                    Directory.Delete(Path.Combine(tempDownloadFolder, "fpPS4-Temmie-s-Launcher-main"));
                                                    Directory.CreateDirectory(Path.Combine(downloadFolderInstallDir, "Emu"));
                                                    mover.Move_Files_And_Folders(Path.Combine(tempDownloadFolder, "fpPS4"), Path.Combine(downloadFolderInstallDir, "Emu"));
                                                    Directory.Delete(Path.Combine(tempDownloadFolder, "fpPS4"));

                                                    ///
                                                    /// Creating Games and Creating Settings.json
                                                    ///
                                                    Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> Creating 'Games' folder");
                                                    Directory.CreateDirectory(GamesFolderDownloadInstallDir);
                                                    writeTemmieConfig.parseAndWriteTemmieJsonConfig_sha(Path.Combine(downloadFolderInstallDir, "Settings.json"), latestArtifactSha);
                                                    File.Delete(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TL_Installer", "tempDownload", "Launcher.zip")));
                                                    File.Delete(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TL_Installer", "tempDownload", "fpPS4-Temmie-s-Launcher-main.zip")));
                                                    File.Delete(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TL_Installer", "tempDownload", "fpPS4.zip")));
                                                    instSoF = true;
                                                    break;
                                                }
                                                else if (answer == "n" || answer == "N")
                                                {
                                                    instSoF = false;
                                                    break;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> Invalid expression!");
                                                    Console.Write("\n~~~~~~~~~~~~~~~~~~~~> Do you want to overwrite folder 'Temmie Launcher' (Y/n): ");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            ///
                                            /// Creating/Deleting Directories and Moving files to their appropriate folders 
                                            ///
                                            Directory.CreateDirectory(downloadFolderInstallDir);

                                            Console.WriteLine($"\n~~~~~~~~~~~~~~~~~~~~> Moving Files...");
                                            mover.Move_Files_And_Folders(Path.Combine(tempDownloadFolder, "nwjs", "nwjs-v0.70.1-win-x64"), downloadFolderInstallDir);
                                            Directory.Delete(Path.Combine(tempDownloadFolder, "nwjs", "nwjs-v0.70.1-win-x64"));
                                            Directory.Delete(Path.Combine(tempDownloadFolder, "nwjs"));
                                            mover.Move_Files_And_Folders(Path.Combine(tempDownloadFolder, "Launcher"), downloadFolderInstallDir);
                                            Directory.Delete(Path.Combine(tempDownloadFolder, "Launcher"));
                                            mover.Move_Files_And_Folders(Path.Combine(tempDownloadFolder, "fpPS4-Temmie-s-Launcher-main", "fpPS4-Temmie-s-Launcher-main"), downloadFolderInstallDir);
                                            Directory.Delete(Path.Combine(tempDownloadFolder, "fpPS4-Temmie-s-Launcher-main", "fpPS4-Temmie-s-Launcher-main"));
                                            Directory.Delete(Path.Combine(tempDownloadFolder, "fpPS4-Temmie-s-Launcher-main"));
                                            Directory.CreateDirectory(Path.Combine(downloadFolderInstallDir, "Emu"));
                                            mover.Move_Files_And_Folders(Path.Combine(tempDownloadFolder, "fpPS4"), Path.Combine(downloadFolderInstallDir, "Emu"));
                                            Directory.Delete(Path.Combine(tempDownloadFolder, "fpPS4"));

                                            ///
                                            /// Creating Games and Creating Settings.json
                                            ///
                                            Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> Creating 'Games' folder");
                                            Directory.CreateDirectory(GamesFolderDownloadInstallDir);
                                            writeTemmieConfig.parseAndWriteTemmieJsonConfig_sha(Path.Combine(downloadFolderInstallDir, "Settings.json"), latestArtifactSha);
                                            File.Delete(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TL_Installer", "tempDownload", "Launcher.zip")));
                                            File.Delete(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TL_Installer", "tempDownload", "fpPS4-Temmie-s-Launcher-main.zip")));
                                            File.Delete(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TL_Installer", "tempDownload", "fpPS4.zip")));
                                            instSoF = true;
                                        }
                                    }
                                }
                                ///
                                /// Installing to Custom Directory
                                ///
                                else
                                {
                                    string GamesFolderCustomInstallDir = Path.Combine(customInstallDir, "Games");
                                    ///
                                    /// Checking if the customInstallDir Exists and if not creating it
                                    ///
                                    if (!Directory.Exists(customInstallDir))
                                    {
                                        ///
                                        /// Creating/Deleting Directories and Moving files to their appropriate folders 
                                        ///
                                        Directory.CreateDirectory(customInstallDir);

                                        Console.WriteLine($"\n~~~~~~~~~~~~~~~~~~~~> Moving Files...");
                                        mover.Move_Files_And_Folders(Path.Combine(tempDownloadFolder, "nwjs", "nwjs-v0.70.1-win-x64"), customInstallDir);
                                        Directory.Delete(Path.Combine(tempDownloadFolder, "nwjs", "nwjs-v0.70.1-win-x64"));
                                        Directory.Delete(Path.Combine(tempDownloadFolder, "nwjs"));
                                        mover.Move_Files_And_Folders(Path.Combine(tempDownloadFolder, "Launcher"), customInstallDir);
                                        Directory.Delete(Path.Combine(tempDownloadFolder, "Launcher"));
                                        mover.Move_Files_And_Folders(Path.Combine(tempDownloadFolder, "fpPS4-Temmie-s-Launcher-main", "fpPS4-Temmie-s-Launcher-main"),  customInstallDir);
                                        Directory.Delete(Path.Combine(tempDownloadFolder, "fpPS4-Temmie-s-Launcher-main", "fpPS4-Temmie-s-Launcher-main"));
                                        Directory.Delete(Path.Combine(tempDownloadFolder, "fpPS4-Temmie-s-Launcher-main"));
                                        Directory.CreateDirectory(Path.Combine(customInstallDir, "Emu"));
                                        mover.Move_Files_And_Folders(Path.Combine(tempDownloadFolder, "fpPS4"), Path.Combine(customInstallDir, "Emu"));
                                        Directory.Delete(Path.Combine(tempDownloadFolder, "fpPS4"));

                                        ///
                                        /// Creating Games and Creating Settings.json
                                        ///
                                        Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> Creating 'Games' folder");
                                        Directory.CreateDirectory(GamesFolderCustomInstallDir);
                                        writeTemmieConfig.parseAndWriteTemmieJsonConfig_sha(Path.Combine(customInstallDir, "Settings.json"), latestArtifactSha);
                                        File.Delete(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TL_Installer", "tempDownload", "Launcher.zip")));
                                        File.Delete(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TL_Installer", "tempDownload", "fpPS4-Temmie-s-Launcher-main.zip")));
                                        File.Delete(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TL_Installer", "tempDownload", "fpPS4.zip")));
                                        instSoF = true;
                                    }
                                    else if (Directory.Exists(customInstallDir))
                                    {
                                        if (Directory.GetFileSystemEntries(customInstallDir).Count() > 0)
                                        {
                                            string answer = "";
                                            Console.Write("\n~~~~~~~~~~~~~~~~~~~~> Do you want to overwrite folder 'Temmie Launcher' (Y/n): ");

                                            while (true)
                                            {
                                                answer = Console.ReadLine();
                                                if (answer == "y" || answer == "Y")
                                                {
                                                    ///
                                                    /// Creating/Deleting Directories and Moving files to their appropriate folders 
                                                    ///
                                                    deleter.Delete_Files_And_Folders(customInstallDir);
                                                    Directory.CreateDirectory(customInstallDir);

                                                    Console.WriteLine($"\n~~~~~~~~~~~~~~~~~~~~> Moving Files...");
                                                    mover.Move_Files_And_Folders(Path.Combine(tempDownloadFolder, "nwjs", "nwjs-v0.70.1-win-x64"), customInstallDir);
                                                    Directory.Delete(Path.Combine(tempDownloadFolder, "nwjs", "nwjs-v0.70.1-win-x64"));
                                                    Directory.Delete(Path.Combine(tempDownloadFolder, "nwjs"));
                                                    mover.Move_Files_And_Folders(Path.Combine(tempDownloadFolder, "Launcher"), customInstallDir);
                                                    Directory.Delete(Path.Combine(tempDownloadFolder, "Launcher"));
                                                    mover.Move_Files_And_Folders(Path.Combine(tempDownloadFolder, "fpPS4-Temmie-s-Launcher-main", "fpPS4-Temmie-s-Launcher-main"), customInstallDir);
                                                    Directory.Delete(Path.Combine(tempDownloadFolder, "fpPS4-Temmie-s-Launcher-main", "fpPS4-Temmie-s-Launcher-main"));
                                                    Directory.Delete(Path.Combine(tempDownloadFolder, "fpPS4-Temmie-s-Launcher-main"));
                                                    Directory.CreateDirectory(Path.Combine(customInstallDir, "Emu"));
                                                    mover.Move_Files_And_Folders(Path.Combine(tempDownloadFolder, "fpPS4"), Path.Combine(customInstallDir, "Emu"));
                                                    Directory.Delete(Path.Combine(tempDownloadFolder, "fpPS4"));

                                                    ///
                                                    /// Creating Games and Creating Settings.json
                                                    ///
                                                    Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> Creating 'Games' folder");
                                                    Directory.CreateDirectory(GamesFolderCustomInstallDir);
                                                    instSoF = true;
                                                    writeTemmieConfig.parseAndWriteTemmieJsonConfig_sha(Path.Combine(customInstallDir, "Settings.json"), latestArtifactSha);
                                                    File.Delete(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TL_Installer", "tempDownload", "Launcher.zip")));
                                                    File.Delete(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TL_Installer", "tempDownload", "fpPS4-Temmie-s-Launcher-main.zip")));
                                                    File.Delete(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TL_Installer", "tempDownload", "fpPS4.zip")));
                                                    break;
                                                }
                                                else if (answer == "n" || answer == "N")
                                                {
                                                    instSoF = false;
                                                    break;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> Invalid expression!");
                                                    Console.Write("\n~~~~~~~~~~~~~~~~~~~~> Do you want to overwrite folder 'Temmie Launcher' (Y/n): ");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            ///
                                            /// Creating/Deleting Directories and Moving files to their appropriate folders 
                                            ///
                                            Directory.CreateDirectory(downloadFolderInstallDir);

                                            Console.WriteLine($"\n~~~~~~~~~~~~~~~~~~~~> Moving Files...");
                                            mover.Move_Files_And_Folders(Path.Combine(tempDownloadFolder, "nwjs", "nwjs-v0.70.1-win-x64"), customInstallDir);
                                            Directory.Delete(Path.Combine(tempDownloadFolder, "nwjs", "nwjs-v0.70.1-win-x64"));
                                            Directory.Delete(Path.Combine(tempDownloadFolder, "nwjs"));
                                            mover.Move_Files_And_Folders(Path.Combine(tempDownloadFolder, "Launcher"), customInstallDir);
                                            Directory.Delete(Path.Combine(tempDownloadFolder, "Launcher"));
                                            mover.Move_Files_And_Folders(Path.Combine(tempDownloadFolder, "fpPS4-Temmie-s-Launcher-main", "fpPS4-Temmie-s-Launcher-main"), customInstallDir);
                                            Directory.Delete(Path.Combine(tempDownloadFolder, "fpPS4-Temmie-s-Launcher-main", "fpPS4-Temmie-s-Launcher-main"));
                                            Directory.Delete(Path.Combine(tempDownloadFolder, "fpPS4-Temmie-s-Launcher-main"));
                                            Directory.CreateDirectory(Path.Combine(customInstallDir, "Emu"));
                                            mover.Move_Files_And_Folders(Path.Combine(tempDownloadFolder, "fpPS4"), Path.Combine(customInstallDir, "Emu"));
                                            Directory.Delete(Path.Combine(tempDownloadFolder, "fpPS4"));

                                            ///
                                            /// Creating Games and Creating Settings.json
                                            ///
                                            Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> Creating 'Games' folder");
                                            Directory.CreateDirectory(GamesFolderCustomInstallDir);
                                            writeTemmieConfig.parseAndWriteTemmieJsonConfig_sha(Path.Combine(customInstallDir, "Settings.json"), latestArtifactSha);
                                            File.Delete(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TL_Installer", "tempDownload", "Launcher.zip")));
                                            File.Delete(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TL_Installer", "tempDownload", "fpPS4-Temmie-s-Launcher-main.zip")));
                                            File.Delete(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TL_Installer", "tempDownload", "fpPS4.zip")));
                                            instSoF = true;
                                        }
                                    }

                                }
                            }
                            else
                            {
                                throw new Exception("Paths or Files are missing");
                            }

                            if (instSoF == true)
                            {

                                Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> Installation finished successfully!");
                                Console.Write(label_pressAnyKey);
                                Console.ReadKey();
                                Console.Clear();
                                break;
                            }
                            else
                            {
                                Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> Installation Canceled");
                                Console.Write(label_pressAnyKey);
                                Console.ReadKey();
                                Console.Clear();
                                break;
                            }
                        }
                        else if (operationMenuInstall == "2")
                        {
                            logos.Logo2();
                            ///
                            /// Getting the Temmie Launcher Path
                            ///

                            while (true)
                            {
                                temmiePath = Console.ReadLine();
                                

                                if (temmiePath.StartsWith("\"") && temmiePath.EndsWith("\""))
                                {
                                    temmiePath = temmiePath.Remove(0, 1);
                                    temmiePath = temmiePath.Remove(temmiePath.Length - 1, 1);
                                }

                                tLFileNameWIthExtension = Path.GetFileName(temmiePath);

                                if (File.Exists(temmiePath) && temmiePath.Contains(".zip") && temmiePath.Contains("Launcher.zip"))
                                {
                                    Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> Launcher.zip Found!");
                                    checkForTL = true;
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> Launcher.zip not found");
                                    Console.Write("\n~~~~~~~~~~~~~~~~~~~~> Enter path to Launcher.zip: ");
                                }
                            }
                            ///
                            /// Getting the fpPS4-Temmie-s-Launcher-main Launcher Path
                            ///

                            Console.Write("\n~~~~~~~~~~~~~~~~~~~~> Enter path to fpPS4-Temmie-s-Launcher-main.zip: ");
                            while (true)
                            {

                                temmie_mainPath = Console.ReadLine();
                                tL_Main_FileNameWIthExtension = Path.GetFileName(temmie_mainPath);

                                if (temmie_mainPath.StartsWith("\"") && temmie_mainPath.EndsWith("\""))
                                {
                                    temmie_mainPath = temmie_mainPath.Remove(0, 1);
                                    temmie_mainPath = temmie_mainPath.Remove(temmie_mainPath.Length - 1, 1);
                                }

                                tL_Main_FileNameWIthExtension = Path.GetFileName(temmie_mainPath);

                                if (File.Exists(temmie_mainPath) && temmie_mainPath.Contains(".zip") && temmie_mainPath.Contains("fpPS4-Temmie-s-Launcher-main.zip"))
                                {
                                    Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> fpPS4-Temmie-s-Launcher-main.zip Found!");
                                    checkForTL_Main = true;
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> fpPS4-Temmie-s-Launcher-main.zip not found");
                                    Console.Write("\n~~~~~~~~~~~~~~~~~~~~> Enter path to fpPS4-Temmie-s-Launcher-main.zip: ");
                                }
                            }

                            ///
                            /// Getting the fpPS4 Launcher Path
                            ///
                            Console.Write("\n~~~~~~~~~~~~~~~~~~~~> Enter path to fpPS4.zip: ");
                            while (true)
                            {
                                fpps4Path = Console.ReadLine();


                                if (fpps4Path.StartsWith("\"") && fpps4Path.EndsWith("\""))
                                {
                                    fpps4Path = fpps4Path.Remove(0, 1);
                                    fpps4Path = fpps4Path.Remove(fpps4Path.Length - 1, 1);
                                }

                                fpps4FileNameWIthExtension = Path.GetFileName(fpps4Path);

                                if (File.Exists(fpps4Path) && fpps4Path.Contains(".zip") && fpps4Path.Contains("fpPS4"))
                                {
                                    Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> fpPS4.zip Found!");
                                    checkForFp = true;
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> fpPS4.zip not found");
                                    Console.Write("\n~~~~~~~~~~~~~~~~~~~~> Enter path to fpPS4.zip: ");
                                }
                            }

                            ///
                            /// Checks if the needed files exist
                            /// Getting Installation Directory
                            /// Installation
                            ///
                            if (checkForFp == true &&  checkForTL == true && checkForTL_Main == true)
                            {
                                while (true)
                                {
                                    Console.Write("\n~~~~~~~~~~~~~~~~~~~~> Enter installation path (Leave blank to install in Downloads folder): ");
                                    InstallDir = Console.ReadLine();
                                    
                                    if (InstallDir.StartsWith("\"") && InstallDir.EndsWith("\""))
                                    {
                                        InstallDir = InstallDir.Remove(0, 1);
                                        InstallDir = InstallDir.Remove(InstallDir.Length - 1, 1);
                                    }
                                    customInstallDir = Path.Combine(InstallDir, "Temmie Launcher");
                                    if (Directory.Exists(InstallDir) || InstallDir == "")
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine(label_pathNotFound);
                                    }
                                }

                                ///
                                /// Installing to Download Folder
                                ///
                                if (InstallDir == "")
                                {
                                    string GamesFolderDownloadInstallDir = Path.Combine(downloadFolderInstallDir, "Games");
                                    ///
                                    /// Checking if downloadFolderInstallDir Exists
                                    ///
                                    if (!Directory.Exists(downloadFolderInstallDir))
                                    {
                                        Directory.CreateDirectory(downloadFolderInstallDir);

                                        ///
                                        /// Checking if nwjs.zip exists and if not it downloads
                                        ///
                                        if (File.Exists(Path.Combine(tempDownloadFolder, "nwjs.zip")))
                                        {
                                            Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> Nw.js already downloaded. Proceeding...");
                                        }
                                        else
                                        {
                                            Console.WriteLine(label_downloadingFiles);
                                            nwjsDownloader.nwJsDownload();
                                            Console.WriteLine(label_processComplete);
                                        }
                                        
                                        Console.WriteLine($"\n~~~~~~~~~~~~~~~~~~~~> Extracting! {nwjs_FileNameWithExtension}");
                                        extractor.ExtractZipContent(Path.Combine(tempDownloadFolder, "nwjs.zip"), Path.Combine(tempDownloadFolder, "nwjs"));
                                        mover.Move_Files_And_Folders(Path.Combine(tempDownloadFolder, "nwjs", "nwjs-v0.70.1-win-x64"), downloadFolderInstallDir);
                                        Directory.Delete(Path.Combine(tempDownloadFolder,"nwjs", "nwjs-v0.70.1-win-x64"));
                                        Directory.Delete(Path.Combine(tempDownloadFolder, "nwjs"));
                                        Console.WriteLine(label_processComplete);

                                        ///
                                        /// Extracting Temmie Launcher
                                        ///
                                        Console.WriteLine($"\n~~~~~~~~~~~~~~~~~~~~> Extracting {tLFileNameWIthExtension}");
                                        extractor.ExtractZipContent(temmiePath, downloadFolderInstallDir);
                                        Console.WriteLine(label_processComplete);

                                        ///
                                        /// Extracting Temmie Launcher Main
                                        ///
                                        Console.WriteLine($"\n~~~~~~~~~~~~~~~~~~~~> Extracting {tL_Main_FileNameWIthExtension}");
                                        Console.WriteLine(label_processComplete);
                                        extractor.ExtractZipContent(temmie_mainPath, downloadFolderInstallDir);
                                        Console.WriteLine($"\n~~~~~~~~~~~~~~~~~~~~> Moving Files...");
                                        mover.Move_Files_And_Folders(Path.Combine(downloadFolderInstallDir,"fpPS4-Temmie-s-Launcher-main"), downloadFolderInstallDir);
                                        Directory.Delete(Path.Combine(downloadFolderInstallDir, "fpPS4-Temmie-s-Launcher-main"));


                                        ///
                                        /// Extracting fpPS4.zip
                                        ///
                                        Console.WriteLine($"\n~~~~~~~~~~~~~~~~~~~~> Extracting {fpps4FileNameWIthExtension}");
                                        extractor.ExtractZipContent(fpps4Path, Path.Combine(downloadFolderInstallDir, "Emu"));
                                        Console.WriteLine(label_processComplete);

                                        ///
                                        /// Creating Games and Creating Settings.json
                                        ///
                                        Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> Creating 'Games' folder");
                                        Directory.CreateDirectory(GamesFolderDownloadInstallDir);
                                        writeTemmieConfig.parseAndWriteTemmieJsonConfig_sha(Path.Combine(downloadFolderInstallDir, "Settings.json"), latestArtifactSha);
                                        File.Delete(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TL_Installer", "tempDownload", "Launcher.zip")));
                                        File.Delete(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TL_Installer", "tempDownload", "fpPS4-Temmie-s-Launcher-main.zip")));
                                        File.Delete(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TL_Installer", "tempDownload", "fpPS4.zip")));
                                        instSoF = true;
                                    }
                                    else if (Directory.Exists(downloadFolderInstallDir))
                                    {
                                        if (Directory.GetFileSystemEntries(downloadFolderInstallDir).Count() > 0)
                                        {
                                            string answer = "";
                                            Console.Write("\n~~~~~~~~~~~~~~~~~~~~> Do you want to overwrite folder 'Temmie Launcher' (Y/n): ");

                                            while (true)
                                            {
                                                answer = Console.ReadLine();
                                                if (answer == "y" || answer == "Y")
                                                {
                                                    ///
                                                    /// Checking if nwjs.zip exists and if not it downloads
                                                    ///

                                                    deleter.Delete_Files_And_Folders(downloadFolderInstallDir);
                                                    Directory.CreateDirectory(downloadFolderInstallDir);

                                                    if (File.Exists(Path.Combine(tempDownloadFolder, "nwjs.zip")))
                                                    {
                                                        Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> Nw.js already downloaded. Proceeding...");
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine(label_downloadingFiles);
                                                        nwjsDownloader.nwJsDownload();
                                                        Console.WriteLine(label_processComplete);
                                                    }
                                                    ///
                                                    /// Extracting nwjs
                                                    ///
                                                    Console.WriteLine($"\n~~~~~~~~~~~~~~~~~~~~> Extracting! {nwjs_FileNameWithExtension} Please wait.");
                                                    extractor.ExtractZipContent(Path.Combine(tempDownloadFolder, "nwjs.zip"), Path.Combine(tempDownloadFolder, "nwjs"));
                                                    mover.Move_Files_And_Folders(Path.Combine(tempDownloadFolder, "nwjs", "nwjs-v0.70.1-win-x64"), downloadFolderInstallDir);
                                                    Directory.Delete(Path.Combine(tempDownloadFolder, "nwjs", "nwjs-v0.70.1-win-x64"));
                                                    Directory.Delete(Path.Combine(tempDownloadFolder, "nwjs"));
                                                    Console.WriteLine(label_processComplete);

                                                    ///
                                                    /// Extracting Temmie Launcher
                                                    ///
                                                    Console.WriteLine($"\n~~~~~~~~~~~~~~~~~~~~> Extracting {tLFileNameWIthExtension}");
                                                    extractor.ExtractZipContent(temmiePath, downloadFolderInstallDir);
                                                    Console.WriteLine(label_processComplete);

                                                    ///
                                                    /// Extracting Temmie Launcher Main
                                                    ///

                                                    Console.WriteLine($"\n~~~~~~~~~~~~~~~~~~~~> Extracting {tL_Main_FileNameWIthExtension}");
                                                    extractor.ExtractZipContent(temmie_mainPath, downloadFolderInstallDir);
                                                    Console.WriteLine(label_processComplete);
                                                    Console.WriteLine($"\n~~~~~~~~~~~~~~~~~~~~> Moving Files...");
                                                    mover.Move_Files_And_Folders(Path.Combine(downloadFolderInstallDir, "fpPS4-Temmie-s-Launcher-main"), downloadFolderInstallDir);
                                                    Directory.Delete(Path.Combine(downloadFolderInstallDir, "fpPS4-Temmie-s-Launcher-main"));

                                                    ///
                                                    /// Extracting fpPS4.zip
                                                    ///
                                                    Console.WriteLine($"\n~~~~~~~~~~~~~~~~~~~~> Extracting {fpps4FileNameWIthExtension}");
                                                    extractor.ExtractZipContent(fpps4Path, Path.Combine(downloadFolderInstallDir, "Emu"));
                                                    Console.WriteLine(label_processComplete);

                                                    ///
                                                    /// Creating Games and Creating Settings.json
                                                    ///
                                                    Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> Creating 'Games' folder");
                                                    Directory.CreateDirectory(GamesFolderDownloadInstallDir);
                                                    writeTemmieConfig.parseAndWriteTemmieJsonConfig_sha(Path.Combine(downloadFolderInstallDir, "Settings.json"), latestArtifactSha);
                                                    File.Delete(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TL_Installer", "tempDownload", "Launcher.zip")));
                                                    File.Delete(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TL_Installer", "tempDownload", "fpPS4-Temmie-s-Launcher-main.zip")));
                                                    File.Delete(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TL_Installer", "tempDownload", "fpPS4.zip")));
                                                    instSoF = true;
                                                    break;
                                                }
                                                else if (answer == "n" || answer == "N")
                                                {
                                                    instSoF = false;
                                                    break;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> Invalid expression!");
                                                    Console.Write("\n~~~~~~~~~~~~~~~~~~~~> Do you want to overwrite folder 'Temmie Launcher' (Y/n): ");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            ///
                                            /// Checking if nwjs.zip exists and if not it downloads
                                            ///

                                            if (File.Exists(Path.Combine(tempDownloadFolder, "nwjs.zip")))
                                            {
                                                Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> Nw.js already downloaded. Proceeding...");
                                            }
                                            else
                                            {
                                                Console.WriteLine(label_downloadingFiles);
                                                nwjsDownloader.nwJsDownload();
                                                Console.WriteLine(label_processComplete);
                                            }
                                            ///
                                            /// Extracing nwjs
                                            ///
                                            Console.WriteLine($"\n~~~~~~~~~~~~~~~~~~~~> Extracting! {nwjs_FileNameWithExtension}");
                                            extractor.ExtractZipContent(Path.Combine(tempDownloadFolder, "nwjs.zip"), Path.Combine(tempDownloadFolder, "nwjs"));
                                            mover.Move_Files_And_Folders(Path.Combine(tempDownloadFolder, "nwjs", "nwjs-v0.70.1-win-x64"), downloadFolderInstallDir);
                                            Directory.Delete(Path.Combine(tempDownloadFolder, "nwjs", "nwjs-v0.70.1-win-x64"));
                                            Directory.Delete(Path.Combine(tempDownloadFolder, "nwjs"));
                                            Console.WriteLine(label_processComplete);

                                            ///
                                            /// Extracting Temmie Launcher
                                            ///
                                            Console.WriteLine($"\n~~~~~~~~~~~~~~~~~~~~> Extracting {tLFileNameWIthExtension}");
                                            extractor.ExtractZipContent(temmiePath, downloadFolderInstallDir);
                                            Console.WriteLine(label_processComplete);

                                            ///
                                            /// Extracting Temmie Launcher Main
                                            ///
                                            Console.WriteLine($"\n~~~~~~~~~~~~~~~~~~~~> Extracting {tL_Main_FileNameWIthExtension}");
                                            extractor.ExtractZipContent(temmie_mainPath, downloadFolderInstallDir);
                                            Console.WriteLine(label_processComplete);
                                            Console.WriteLine($"\n~~~~~~~~~~~~~~~~~~~~> Moving Files...");
                                            mover.Move_Files_And_Folders(Path.Combine(downloadFolderInstallDir, "fpPS4-Temmie-s-Launcher-main"), downloadFolderInstallDir);
                                            Directory.Delete(Path.Combine(downloadFolderInstallDir, "fpPS4-Temmie-s-Launcher-main"));

                                            ///
                                            /// Extracting fpPS4.zip
                                            ///
                                            Console.WriteLine($"\n~~~~~~~~~~~~~~~~~~~~> Extracting {fpps4FileNameWIthExtension}");
                                            extractor.ExtractZipContent(fpps4Path, Path.Combine(downloadFolderInstallDir, "Emu"));
                                            Console.WriteLine(label_processComplete);

                                            ///
                                            /// Creating Games and Creating Settings.json
                                            ///
                                            Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> Creating 'Games' folder");
                                            writeTemmieConfig.parseAndWriteTemmieJsonConfig_sha(Path.Combine(downloadFolderInstallDir, "Settings.json"), latestArtifactSha);
                                            Directory.CreateDirectory(GamesFolderDownloadInstallDir);
                                            File.Delete(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TL_Installer", "tempDownload", "Launcher.zip")));
                                            File.Delete(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TL_Installer", "tempDownload", "fpPS4-Temmie-s-Launcher-main.zip")));
                                            File.Delete(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TL_Installer", "tempDownload", "fpPS4.zip")));
                                            instSoF = true;
                                        }
                                    }
                                }
                                ///
                                /// Installing to Custom Directory
                                ///
                                else
                                {
                                    string GamesFolderCustomInstallDir = Path.Combine(customInstallDir, "Games");
                                    ///
                                    /// Checking if the customInstallDir Exists and if not creating it
                                    ///
                                    if (!Directory.Exists(customInstallDir))
                                    {
                                        Directory.CreateDirectory(customInstallDir);

                                        ///
                                        /// Checking if nwjs.zip exists and if not it downloads
                                        ///

                                        if (File.Exists(Path.Combine(tempDownloadFolder, "nwjs.zip")))
                                        {
                                            Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> Nw.js already downloaded. Proceeding...");
                                        }
                                        else
                                        {
                                            Console.WriteLine(label_downloadingFiles);
                                            nwjsDownloader.nwJsDownload();
                                            Console.WriteLine(label_processComplete);
                                        }

                                        ///
                                        /// Extracting nwjs 
                                        ///
                                        Console.WriteLine($"\n~~~~~~~~~~~~~~~~~~~~> Extracting! {nwjs_FileNameWithExtension}");
                                        extractor.ExtractZipContent(Path.Combine(tempDownloadFolder, "nwjs.zip"), Path.Combine(tempDownloadFolder, "nwjs"));
                                        mover.Move_Files_And_Folders(Path.Combine(tempDownloadFolder, "nwjs", "nwjs-v0.70.1-win-x64"), customInstallDir);
                                        Directory.Delete(Path.Combine(tempDownloadFolder, "nwjs", "nwjs-v0.70.1-win-x64"));
                                        Directory.Delete(Path.Combine(tempDownloadFolder, "nwjs"));
                                        Console.WriteLine(label_processComplete);

                                        ///
                                        /// Extracting Temmie Launcher
                                        ///
                                        Console.WriteLine($"\n~~~~~~~~~~~~~~~~~~~~> Extracting {tLFileNameWIthExtension}");
                                        extractor.ExtractZipContent(temmiePath, customInstallDir);
                                        Console.WriteLine(label_processComplete);

                                        ///
                                        /// Extracting Temmie Launcher Main
                                        ///

                                        Console.WriteLine($"\n~~~~~~~~~~~~~~~~~~~~> Extracting {tL_Main_FileNameWIthExtension}");
                                        extractor.ExtractZipContent(temmie_mainPath, customInstallDir);
                                        Console.WriteLine(label_processComplete);
                                        Console.WriteLine($"\n~~~~~~~~~~~~~~~~~~~~> Moving Files...");
                                        mover.Move_Files_And_Folders(Path.Combine(customInstallDir, "fpPS4-Temmie-s-Launcher-main"), customInstallDir);
                                        Directory.Delete(Path.Combine(customInstallDir, "fpPS4-Temmie-s-Launcher-main"));

                                        ///
                                        /// Extracting fpPS4.zip
                                        ///
                                        Console.WriteLine($"\n~~~~~~~~~~~~~~~~~~~~> Extracting {fpps4FileNameWIthExtension}");
                                        extractor.ExtractZipContent(fpps4Path, Path.Combine(customInstallDir, "Emu"));
                                        Console.WriteLine(label_processComplete);

                                        ///
                                        /// Creating Games and Creating Settings.json
                                        ///
                                        Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> Creating 'Games' folder");
                                        Directory.CreateDirectory(GamesFolderCustomInstallDir);
                                        writeTemmieConfig.parseAndWriteTemmieJsonConfig_sha(Path.Combine(customInstallDir, "Settings.json"), latestArtifactSha);
                                        File.Delete(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TL_Installer", "tempDownload", "Launcher.zip")));
                                        File.Delete(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TL_Installer", "tempDownload", "fpPS4-Temmie-s-Launcher-main.zip")));
                                        File.Delete(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TL_Installer", "tempDownload", "fpPS4.zip")));
                                        instSoF = true;
                                    }
                                    else if (Directory.Exists(customInstallDir))
                                    {
                                        if (Directory.GetFileSystemEntries(customInstallDir).Count() > 0)
                                        {
                                            string answer = "";
                                            Console.Write("\n~~~~~~~~~~~~~~~~~~~~> Do you want to overwrite folder 'Temmie Launcher' (Y/n): ");

                                            while (true)
                                            {
                                                answer = Console.ReadLine();
                                                if (answer == "y" || answer == "Y")
                                                {

                                                    deleter.Delete_Files_And_Folders(customInstallDir);
                                                    Directory.CreateDirectory(customInstallDir);

                                                    ///
                                                    /// Checking if nwjs.zip exists and if not it downloads
                                                    ///
                                                    if (File.Exists(Path.Combine(tempDownloadFolder, "nwjs.zip")))
                                                    {
                                                        Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> Nw.js already downloaded. Proceeding...");
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine(label_downloadingFiles);
                                                        nwjsDownloader.nwJsDownload();
                                                        Console.WriteLine(label_processComplete);
                                                    }

                                                    ///
                                                    /// Extracting nwjs
                                                    ///
                                                    Console.WriteLine($"\n~~~~~~~~~~~~~~~~~~~~> Extracting! {nwjs_FileNameWithExtension}");
                                                    extractor.ExtractZipContent(Path.Combine(tempDownloadFolder, "nwjs.zip"), Path.Combine(tempDownloadFolder, "nwjs"));
                                                    mover.Move_Files_And_Folders(Path.Combine(tempDownloadFolder, "nwjs", "nwjs-v0.70.1-win-x64"), customInstallDir);
                                                    Directory.Delete(Path.Combine(tempDownloadFolder, "nwjs", "nwjs-v0.70.1-win-x64"));
                                                    Directory.Delete(Path.Combine(tempDownloadFolder, "nwjs"));
                                                    Console.WriteLine(label_processComplete);

                                                    ///
                                                    /// Extracting Temmie Launcher
                                                    ///
                                                    Console.WriteLine($"\n~~~~~~~~~~~~~~~~~~~~> Extracting {tLFileNameWIthExtension}");
                                                    extractor.ExtractZipContent(temmiePath, customInstallDir);
                                                    Console.WriteLine(label_processComplete);

                                                    ///
                                                    /// Extracting Temmie Launcher Main
                                                    ///
                                                    Console.WriteLine($"\n~~~~~~~~~~~~~~~~~~~~> Extracting {tL_Main_FileNameWIthExtension}");
                                                    extractor.ExtractZipContent(temmie_mainPath, customInstallDir);
                                                    Console.WriteLine(label_processComplete);
                                                    Console.WriteLine($"\n~~~~~~~~~~~~~~~~~~~~> Moving Files...");
                                                    mover.Move_Files_And_Folders(Path.Combine(customInstallDir, "fpPS4-Temmie-s-Launcher-main"), customInstallDir);
                                                    Directory.Delete(Path.Combine(customInstallDir, "fpPS4-Temmie-s-Launcher-main"));

                                                    ///
                                                    /// Extracting fpPS4.zip
                                                    ///
                                                    Console.WriteLine($"\n~~~~~~~~~~~~~~~~~~~~> Extracting {fpps4FileNameWIthExtension}");
                                                    extractor.ExtractZipContent(fpps4Path, Path.Combine(customInstallDir, "Emu"));
                                                    Console.WriteLine(label_processComplete);

                                                    ///
                                                    /// Creating Games Folder and Creating Settings.json
                                                    ///
                                                    Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> Creating Games Folder");
                                                    writeTemmieConfig.parseAndWriteTemmieJsonConfig_sha(Path.Combine(customInstallDir, "Settings.json"), latestArtifactSha);
                                                    Directory.CreateDirectory(GamesFolderCustomInstallDir);
                                                    File.Delete(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TL_Installer", "tempDownload", "Launcher.zip")));
                                                    File.Delete(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TL_Installer", "tempDownload", "fpPS4-Temmie-s-Launcher-main.zip")));
                                                    File.Delete(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TL_Installer", "tempDownload", "fpPS4.zip")));
                                                    instSoF = true;
                                                    break;
                                                }
                                                else if (answer == "n" || answer == "N")
                                                {
                                                    instSoF = false;
                                                    break;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> Invalid expression!");
                                                    Console.Write("\n~~~~~~~~~~~~~~~~~~~~> Do you want to overwrite folder 'Temmie Launcher' (Y/n): ");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            ///
                                            /// Checking if nwjs.zip exists and if not it downloads
                                            ///
                                            if (File.Exists(Path.Combine(tempDownloadFolder, "nwjs.zip")))
                                            {
                                                Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> Nw.js already downloaded. Proceeding...");
                                            }
                                            else
                                            {
                                                Console.WriteLine(label_downloadingFiles);
                                                nwjsDownloader.nwJsDownload();
                                                Console.WriteLine(label_processComplete);
                                            }

                                            ///
                                            /// Extracting nwjs
                                            ///
                                            Console.WriteLine($"\n~~~~~~~~~~~~~~~~~~~~> Extracting! {nwjs_FileNameWithExtension}");
                                            extractor.ExtractZipContent(Path.Combine(tempDownloadFolder, "nwjs.zip"), Path.Combine(tempDownloadFolder, "nwjs"));
                                            mover.Move_Files_And_Folders(Path.Combine(tempDownloadFolder, "nwjs", "nwjs-v0.70.1-win-x64"), customInstallDir);
                                            Directory.Delete(Path.Combine(tempDownloadFolder, "nwjs", "nwjs-v0.70.1-win-x64"));
                                            Directory.Delete(Path.Combine(tempDownloadFolder, "nwjs"));
                                            Console.WriteLine(label_processComplete);

                                            ///
                                            /// Extracting Temmie Launcher
                                            ///
                                            Console.WriteLine($"\n~~~~~~~~~~~~~~~~~~~~> Extracting {tLFileNameWIthExtension}");
                                            extractor.ExtractZipContent(temmiePath, customInstallDir);
                                            Console.WriteLine(label_processComplete);

                                            ///
                                            /// Extracting Temmie Launcher Main
                                            ///

                                            Console.WriteLine($"\n~~~~~~~~~~~~~~~~~~~~> Extracting {tL_Main_FileNameWIthExtension}");
                                            extractor.ExtractZipContent(temmie_mainPath, customInstallDir);
                                            Console.WriteLine(label_processComplete);
                                            Console.WriteLine($"\n~~~~~~~~~~~~~~~~~~~~> Moving Files...");
                                            mover.Move_Files_And_Folders(Path.Combine(customInstallDir, "fpPS4-Temmie-s-Launcher-main"), customInstallDir);
                                            Directory.Delete(Path.Combine(customInstallDir, "fpPS4-Temmie-s-Launcher-main"));

                                            ///
                                            /// Extracting fpPS4.zip
                                            ///
                                            Console.WriteLine($"\n~~~~~~~~~~~~~~~~~~~~> Extracting {fpps4FileNameWIthExtension}");
                                            extractor.ExtractZipContent(fpps4Path, Path.Combine(customInstallDir, "Emu"));
                                            Console.WriteLine(label_processComplete);

                                            ///
                                            /// Creating Games Folder and Creating Settings.json
                                            ///
                                            Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> Creating 'Games' folder");
                                            writeTemmieConfig.parseAndWriteTemmieJsonConfig_sha(Path.Combine(customInstallDir, "Settings.json"), latestArtifactSha);
                                            Directory.CreateDirectory(GamesFolderCustomInstallDir);
                                            File.Delete(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TL_Installer", "tempDownload", "Launcher.zip")));
                                            File.Delete(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TL_Installer", "tempDownload", "fpPS4-Temmie-s-Launcher-main.zip")));
                                            File.Delete(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TL_Installer", "tempDownload", "fpPS4.zip")));
                                            instSoF = true;
                                        }
                                    }

                                }
                            }
                            else
                            {
                                throw new Exception("Paths or Files are missing");
                            }

                            if (instSoF == true)
                            {
                                Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> Installation finished successfully!");
                                Console.Write(label_pressAnyKey);
                                Console.ReadKey();
                                Console.Clear();
                                break;
                            }
                            else
                            {
                                Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> Installation Canceled!");
                                Console.Write(label_pressAnyKey);
                                Console.ReadKey();
                                Console.Clear();
                                break;
                            }
                            
                        }
                        else if (operationMenuInstall == "3")
                        {
                            Console.Clear();
                            break;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("\n\n\n                                                Error: Invalid expression!\n");
                            Console.Write("                                               (Press anything to go back)");
                            Console.ReadKey();
                            Console.Clear();
                        }

                    }
                    

                    
                }
                else if (operation == "2")
                {
                    Console.Clear();
                    Console.Write("Coming soon!");
                    Console.ReadKey();
                    Console.Clear();
                    continue;
                }
                else if (operation == "3")
                {
                    Environment.Exit(0);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\n\n\n                                                Error: Invalid expression!\n");
                    Console.Write("                                               (Press anything to go back)");
                    Console.ReadKey();
                    Console.Clear();
                }
            }

            
        }

    }
}