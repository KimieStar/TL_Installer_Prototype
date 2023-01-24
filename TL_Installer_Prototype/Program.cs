using System.IO;
using System.Configuration;
using System.Runtime.InteropServices;
using TL_Installer_Prototype.Controllers.Github.fpPS4;
using TL_Installer_Prototype.Controllers;
using TL_Installer_Prototype.Controllers.Github.TemmieLauncher;

namespace TL_Installer_Prototype
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = ("TL Installer");
            ////////////////Declaring Variables//////////////////////////
            
            ///
            /// Declaring common string variables
            ///
            string label_processComplete = "\n~~~~~~~~~~~~~~~~~~~~> Done!";
            string label_pressAnyKey = "\n~~~~~~~~~~~~~~~~~~~~> Press any key to exit";
            string label_extractPleaseWait = "\n~~~~~~~~~~~~~~~~~~~~> Extracting files - please wait...";
            string label_downloadingFiles = "\n~~~~~~~~~~~~~~~~~~~~> Downloading required files! Please wait.";
            string label_pathNotFound = "\n~~~~~~~~~~~~~~~~~~~~> This is an invalid path (or provided path was not found) - Please try again";

            ///
            /// Declaring Menu Operation Variables
            ///
            string operation;
            string InstallDir;
            string customInstallDir;
            string operationMenuInstall;
            string updateAnswer;

            ///
            /// Declaring Manual Download File Paths
            ///
            string fpps4Path;
            string temmiePath;
            string temmie_mainPath;

            ///
            /// Declaring Bools
            ///
            bool instSoF = false;
            bool checkForTL;
            bool checkForFp;
            bool checkForTL_Main;

            ///
            /// Declaring Enviorment Paths
            ///
            string TLFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TL_Installer");
            string tempDownloadFolder = Path.Combine(TLFolder, "tempDownload");
            string downloadFolderInstallDir = Path.Combine(Path.Combine(System.Environment.GetEnvironmentVariable("USERPROFILE"), "Downloads"), "Temmie Launcher");

            ///
            ///  Declaring Files with Extensions(zip)
            ///
            string tLFileNameWIthExtension;
            string fpps4FileNameWIthExtension;
            string tL_Main_FileNameWIthExtension;

            ///
            /// Declaring References to classes
            ///

            Logos logos = new Logos();
            Extract_Files extractor = new Extract_Files();
            TL_Downloader tlDownloader = new TL_Downloader();
            File_Folder_Mover mover = new File_Folder_Mover();
            File_Folder_Deleter deleter = new File_Folder_Deleter();
            fpPS4_Artifact_Json artifactJson = new fpPS4_Artifact_Json();
            fpPS4_Action_Downloader actionDownloader = new fpPS4_Action_Downloader();
            Write_To_Temmie_Config_Json writeTemmieConfig = new Write_To_Temmie_Config_Json();
            TL_Commit_Json tL_Commit_Json = new TL_Commit_Json();
            TL_Json_Writer TljsonWriter = new TL_Json_Writer();
            

                         /////////Declaring fpPS4 Action Variables//////////
            string GToken = "";
            
            

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
                            string latestArtifactSha = artifactJson.getLatestArtifactSha(GToken);
                            string latestcommitSha = tL_Commit_Json.commitSha(GToken);
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
                            if (checkForFp == true && checkForTL == true && checkForTL_Main == true)
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
                                        if (!File.Exists(Path.Combine(TLFolder, "tl_config.json")))
                                        {
                                            TljsonWriter.createTlJson(Path.Combine(TLFolder, "tl_config.json"),downloadFolderInstallDir, latestcommitSha);
                                        }
                                        else
                                        {
                                            if (TljsonWriter.isTlJsonEmpty(Path.Combine(TLFolder, "tl_config.json")) == true)
                                            {
                                                File.Delete(Path.Combine(TLFolder, "tl_config.json"));
                                                TljsonWriter.createTlJson(Path.Combine(TLFolder, "tl_config.json"), downloadFolderInstallDir, latestcommitSha);
                                            }
                                            else
                                            {
                                                bool chk = TljsonWriter.checkIfInstallationExists(Path.Combine(TLFolder, "tl_config.json"), downloadFolderInstallDir);
                                                if (chk == false)
                                                {
                                                    TljsonWriter.addInstallationLog(Path.Combine(TLFolder, "tl_config.json"), downloadFolderInstallDir, latestcommitSha);
                                                }
                                                else
                                                {
                                                    string sha = TljsonWriter.getShaFromInstallation(Path.Combine(TLFolder, "tl_config.json"), downloadFolderInstallDir);
                                                    if (sha != latestcommitSha)
                                                    {
                                                        TljsonWriter.changeLatestSha(Path.Combine(TLFolder, "tl_config.json"), downloadFolderInstallDir, latestcommitSha);
                                                    }
                                                }
                                            }
                                        }
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
                                                    if (!File.Exists(Path.Combine(TLFolder, "tl_config.json")))
                                                    {
                                                        TljsonWriter.createTlJson(Path.Combine(TLFolder, "tl_config.json"), downloadFolderInstallDir, latestcommitSha);
                                                    }
                                                    else
                                                    {
                                                        if (TljsonWriter.isTlJsonEmpty(Path.Combine(TLFolder, "tl_config.json")) == true)
                                                        {
                                                            File.Delete(Path.Combine(TLFolder, "tl_config.json"));
                                                            TljsonWriter.createTlJson(Path.Combine(TLFolder, "tl_config.json"), downloadFolderInstallDir, latestcommitSha);
                                                        }
                                                        else
                                                        {
                                                            bool chk = TljsonWriter.checkIfInstallationExists(Path.Combine(TLFolder, "tl_config.json"), downloadFolderInstallDir);
                                                            if (chk == false)
                                                            {
                                                                TljsonWriter.addInstallationLog(Path.Combine(TLFolder, "tl_config.json"), downloadFolderInstallDir, latestcommitSha);
                                                            }
                                                            else
                                                            {
                                                                string sha = TljsonWriter.getShaFromInstallation(Path.Combine(TLFolder, "tl_config.json"), downloadFolderInstallDir);
                                                                if (sha != latestcommitSha)
                                                                {
                                                                    TljsonWriter.changeLatestSha(Path.Combine(TLFolder, "tl_config.json"), downloadFolderInstallDir, latestcommitSha);
                                                                }
                                                            }
                                                        }
                                                    }
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
                                            if (!File.Exists(Path.Combine(TLFolder, "tl_config.json")))
                                            {
                                                TljsonWriter.createTlJson(Path.Combine(TLFolder, "tl_config.json"), downloadFolderInstallDir, latestcommitSha);
                                            }
                                            else
                                            {
                                                if (TljsonWriter.isTlJsonEmpty(Path.Combine(TLFolder, "tl_config.json")) == true)
                                                {
                                                    File.Delete(Path.Combine(TLFolder, "tl_config.json"));
                                                    TljsonWriter.createTlJson(Path.Combine(TLFolder, "tl_config.json"), downloadFolderInstallDir, latestcommitSha);
                                                }
                                                else
                                                {
                                                    bool chk = TljsonWriter.checkIfInstallationExists(Path.Combine(TLFolder, "tl_config.json"), downloadFolderInstallDir);
                                                    if (chk == false)
                                                    {
                                                        TljsonWriter.addInstallationLog(Path.Combine(TLFolder, "tl_config.json"), downloadFolderInstallDir, latestcommitSha);
                                                    }
                                                    else
                                                    {
                                                        string sha = TljsonWriter.getShaFromInstallation(Path.Combine(TLFolder, "tl_config.json"), downloadFolderInstallDir);
                                                        if (sha != latestcommitSha)
                                                        {
                                                            TljsonWriter.changeLatestSha(Path.Combine(TLFolder, "tl_config.json"), downloadFolderInstallDir, latestcommitSha);
                                                        }
                                                    }
                                                }
                                            }
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
                                        if (!File.Exists(Path.Combine(TLFolder, "tl_config.json")))
                                        {
                                            TljsonWriter.createTlJson(Path.Combine(TLFolder, "tl_config.json"),customInstallDir, latestcommitSha);
                                        }
                                        else
                                        {
                                            if (TljsonWriter.isTlJsonEmpty(Path.Combine(TLFolder, "tl_config.json")) == true)
                                            {
                                                File.Delete(Path.Combine(TLFolder, "tl_config.json"));
                                                TljsonWriter.createTlJson(Path.Combine(TLFolder, "tl_config.json"), customInstallDir, latestcommitSha);
                                            }
                                            else
                                            {
                                                bool chk = TljsonWriter.checkIfInstallationExists(Path.Combine(TLFolder, "tl_config.json"), customInstallDir);
                                                if (chk == false)
                                                {
                                                    TljsonWriter.addInstallationLog(Path.Combine(TLFolder, "tl_config.json"), customInstallDir, latestcommitSha);
                                                }
                                                else
                                                {
                                                    string sha = TljsonWriter.getShaFromInstallation(Path.Combine(TLFolder, "tl_config.json"), customInstallDir);
                                                    if (sha != latestcommitSha)
                                                    {
                                                        TljsonWriter.changeLatestSha(Path.Combine(TLFolder, "tl_config.json"), customInstallDir, latestcommitSha);
                                                    }
                                                }
                                            }
                                        }
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
                                                    if (!File.Exists(Path.Combine(TLFolder, "tl_config.json")))
                                                    {
                                                        TljsonWriter.createTlJson(Path.Combine(TLFolder, "tl_config.json"), customInstallDir, latestcommitSha);
                                                    }
                                                    else
                                                    {
                                                        if (TljsonWriter.isTlJsonEmpty(Path.Combine(TLFolder, "tl_config.json")) == true)
                                                        {
                                                            File.Delete(Path.Combine(TLFolder, "tl_config.json"));
                                                            TljsonWriter.createTlJson(Path.Combine(TLFolder, "tl_config.json"), customInstallDir, latestcommitSha);
                                                        }
                                                        else
                                                        {
                                                            bool chk = TljsonWriter.checkIfInstallationExists(Path.Combine(TLFolder, "tl_config.json"), customInstallDir);
                                                            if (chk == false)
                                                            {
                                                                TljsonWriter.addInstallationLog(Path.Combine(TLFolder, "tl_config.json"), customInstallDir, latestcommitSha);
                                                            }
                                                            else
                                                            {
                                                                string sha = TljsonWriter.getShaFromInstallation(Path.Combine(TLFolder, "tl_config.json"), customInstallDir);
                                                                if (sha != latestcommitSha)
                                                                {
                                                                    TljsonWriter.changeLatestSha(Path.Combine(TLFolder, "tl_config.json"), customInstallDir, latestcommitSha);
                                                                }
                                                            }
                                                        }
                                                    }
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
                                            if (!File.Exists(Path.Combine(TLFolder, "tl_config.json")))
                                            {
                                                TljsonWriter.createTlJson(Path.Combine(TLFolder, "tl_config.json"), customInstallDir, latestcommitSha);
                                            }
                                            else
                                            {
                                                if (TljsonWriter.isTlJsonEmpty(Path.Combine(TLFolder, "tl_config.json")) == true)
                                                {
                                                    File.Delete(Path.Combine(TLFolder, "tl_config.json"));
                                                    TljsonWriter.createTlJson(Path.Combine(TLFolder, "tl_config.json"), customInstallDir, latestcommitSha);
                                                }
                                                else
                                                {
                                                    bool chk = TljsonWriter.checkIfInstallationExists(Path.Combine(TLFolder, "tl_config.json"), customInstallDir);
                                                    if (chk == false)
                                                    {
                                                        TljsonWriter.addInstallationLog(Path.Combine(TLFolder, "tl_config.json"), customInstallDir, latestcommitSha);
                                                    }
                                                    else
                                                    {
                                                        string sha = TljsonWriter.getShaFromInstallation(Path.Combine(TLFolder, "tl_config.json"), customInstallDir);
                                                        if (sha != latestcommitSha)
                                                        {
                                                            TljsonWriter.changeLatestSha(Path.Combine(TLFolder, "tl_config.json"), customInstallDir, latestcommitSha);
                                                        }
                                                    }
                                                }
                                            }
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
                            logos.Logo4();
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
                    string latestcommitSha = tL_Commit_Json.commitSha(GToken);
                    Console.Clear();
                    logos.Logo4();
                    while (true)
                    {
                        Console.Write("\n~~~~~~~~~~~~~~~~~~~~> Enter path to an installation: ");
                        InstallDir = Console.ReadLine();
                        if (InstallDir.StartsWith("\"") && InstallDir.EndsWith("\""))
                        {
                            InstallDir = InstallDir.Remove(0, 1);
                            InstallDir = InstallDir.Remove(InstallDir.Length - 1, 1);
                        }
                        customInstallDir = Path.Combine(InstallDir, "Temmie Launcher");
                        if (Directory.Exists(InstallDir) && TljsonWriter.checkIfInstallationExists(Path.Combine(TLFolder, "tl_config.json"), InstallDir) == true && !InstallDir.Contains("//"))
                        {
                            
                            break;
                        }
                        else
                        {
                            Console.WriteLine(label_pathNotFound);
                        }
                    }
                    Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> Installation found!");
                    Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> Checking for updates!");

                    string shaOfInstallation = TljsonWriter.getShaFromInstallation(Path.Combine(TLFolder, "tl_config.json"), Path.Combine(InstallDir, "Temmie Launcher")); 
                    if (shaOfInstallation != latestcommitSha)
                    {
                        Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> Update detected!");
                        while (true)
                        {
                            Console.Write("\n~~~~~~~~~~~~~~~~~~~~> Do you wish to update? (Y/n): ");
                            updateAnswer = Console.ReadLine();
                            if (updateAnswer == "Y" || updateAnswer == "y")
                            {
                                Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> Updating! Please wait...");
                                if (File.Exists(Path.Combine(tempDownloadFolder, "fpPS4-Temmie-s-Launcher-main.zip")))
                                {
                                    File.Delete(Path.Combine(tempDownloadFolder, "fpPS4-Temmie-s-Launcher-main.zip"));
                                }
                                Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> Downloading fpPS4-Temmie-s-Launcher-main! Please wait.");
                                tlDownloader.downloadFromMain();
                                Console.WriteLine(label_processComplete);
                                Console.WriteLine(label_extractPleaseWait);
                                extractor.ExtractZipContent(Path.Combine(tempDownloadFolder, "fpPS4-Temmie-s-Launcher-main.zip"), Path.Combine(tempDownloadFolder, "fpPS4-Temmie-s-Launcher-main"));
                                Console.WriteLine(label_processComplete);
                                Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> Moving files");
                                mover.Move_Files_And_Folders(Path.Combine(tempDownloadFolder, "fpPS4-Temmie-s-Launcher-main", "fpPS4-Temmie-s-Launcher-main"), Path.Combine(InstallDir));
                                Directory.Delete(Path.Combine(tempDownloadFolder, "fpPS4-Temmie-s-Launcher-main", "fpPS4-Temmie-s-Launcher-main"));
                                Directory.Delete(Path.Combine(tempDownloadFolder, "fpPS4-Temmie-s-Launcher-main"));
                                TljsonWriter.changeLatestSha(Path.Combine(TLFolder, "tl_config.json"), Path.Combine(InstallDir, "Temmie Launcher"), latestcommitSha);
                                break;
                            }
                            else if (updateAnswer == "N" || updateAnswer == "n")
                            {
                                break;
                            }
                            else
                            {
                                Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> Invalid expression!");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> No updates found!");
                        Console.Write(label_pressAnyKey);
                        Console.ReadKey();
                    }
                    Console.Clear();
                    //continue;
                }
                else if (operation == "3")
                {
                    Console.Clear();
                    logos.Logo4();
                    if (!File.Exists(Path.Combine(TLFolder, "tl_config.json")))
                    {
                        TljsonWriter.createTlJson(Path.Combine(TLFolder, "tl_config.json"), "", "");
                    }
                    while (true)
                    {
                        Console.Write("\n~~~~~~~~~~~~~~~~~~~~> Enter path to an installation: ");
                        InstallDir = Console.ReadLine();
                        if (InstallDir.StartsWith("\"") && InstallDir.EndsWith("\""))
                        {
                            InstallDir = InstallDir.Remove(0, 1);
                            InstallDir = InstallDir.Remove(InstallDir.Length - 1, 1);
                        }
                        customInstallDir = Path.Combine(InstallDir, "Temmie Launcher");
                        if (Directory.Exists(InstallDir) && !InstallDir.Contains("//"))
                        {

                            break;
                        }
                        else
                        {
                            Console.WriteLine(label_pathNotFound);
                        }
                    }
                    TljsonWriter.addInstallationLog(Path.Combine(TLFolder, "tl_config.json"), InstallDir,"");
                    Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~~> Successfully added!");
                    Console.Write(label_pressAnyKey);
                    Console.ReadKey();
                    Console.Clear();
                }
                else if (operation == "4")
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