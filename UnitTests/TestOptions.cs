using NUnit.Framework;
using NUS_Downloader;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace UnitTests
{
    public class Tests
    {
        private string TestRootFolder;
        private string AssemblyFolder;

        [SetUp]
        public void Setup()
        {
            AssemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Directory.SetCurrentDirectory($"{AssemblyFolder}/../../../../");
            TestRootFolder = Directory.GetCurrentDirectory();
        }

        private bool WipeOutputFolder (string rootFolder)
        {
            // Remove the "title" and "script" folders
            string scriptsPath = Path.Combine(rootFolder, "scripts");
            string titlesPath = Path.Combine(rootFolder, "titles");

            try
            {
                if (Directory.Exists(scriptsPath)) 
                {
                    Directory.Delete(scriptsPath, true);
                }
                if (Directory.Exists(titlesPath)) 
                {
                    Directory.Delete(titlesPath, true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: Could not remove output folders: {ex.Message}");
                return false;
            }

            return true; 
        }

        private static string CombinePaths(string path1, params string[] paths)
        {
            if (path1 == null)
            {
                throw new ArgumentNullException("path1");
            }
            if (paths == null)
            {
                throw new ArgumentNullException("paths");
            }
            return paths.Aggregate(path1, (acc, p) => Path.Combine(acc, p));
        }

        private void SetDefaultNusOptions (ref Form1 nusForm)
        {
            nusForm.SetLocalUse(true);
            //TruchaBugEnable = true;
            //ESIdentityPatchEnable = true;
            //NandPermissionPatchEnable = true;
            //nusForm.SetEsIdentityBugEnable(true);
            //nusForm.SetNandPermissionBugEnable(true);
            nusForm.SetCreateDecryptedContents(false);
            //nusForm.SetCustomNusUrl(args[i]);
            nusForm.SetNusType("wii");
            //nusForm.SetNusType("dsi");
        }

        private void SetNusOptions (ref Form1 nusForm, string[] options)
        {
            bool TruchaBugEnable = false;
            bool ESIdentityPatchEnable = false;
            bool NandPermissionPatchEnable = false;

            foreach (string nusOption in options)
            {
                switch (nusOption.ToLower())
                {
                    case "packwad":
                        nusForm.SetPackWad(true);
                        break;

                    case "wii":
                    case "dsi":
                        nusForm.SetNusType(nusOption);
                        break;

                    case "truchapatch":
                        TruchaBugEnable = true;
                        break;

                    case "esidentitypatch":
                        ESIdentityPatchEnable = true;
                        break;

                    case "nandpermissionpatch":
                        NandPermissionPatchEnable = true;
                        break;

                    case "createdecryptedcontents":
                        nusForm.SetCreateDecryptedContents(true);
                        break;

                    case "localuse":
                        nusForm.SetLocalUse(true);
                        break;

                    default:
                        if (nusOption.ToLower().Contains("http") )
                        {
                            nusForm.SetCustomNusUrl(nusOption);
                        }
                        break;
                }
            }
            if ((TruchaBugEnable == true) || (ESIdentityPatchEnable == true) || (NandPermissionPatchEnable == true))
            {
                nusForm.SetPatchIOS(true);
                if (TruchaBugEnable == true)
                {
                    nusForm.SetTruchaBugEnable(true);
                }
                if (ESIdentityPatchEnable == true)
                {
                    nusForm.SetEsIdentityBugEnable(true);
                }
                if (NandPermissionPatchEnable == true)
                {
                    nusForm.SetNandPermissionBugEnable(true);
                }
            }
        }

        private string RunNusCliCmd(string nusCmd)
        {
            ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd", $"/c {nusCmd}")
            {
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            string output;
            using (Process proc = new Process())
            {
                proc.StartInfo = procStartInfo;
                proc.Start();

                output = proc.StandardOutput.ReadToEnd();

                if (string.IsNullOrEmpty(output))
                {
                    output = proc.StandardError.ReadToEnd();
                }
            }
            return output;
        }

        private bool CheckOutput(bool buildWad, string titleId, string titleVersion)
        {
            if (buildWad)
            {
                if (titleVersion.Equals("*"))
                {
                    // Check just the folder
                    string[] paths = { "titles", titleId };
                    string folderPath = CombinePaths(TestRootFolder, paths);
                    if (!Directory.Exists(folderPath))
                    {
                        return false;
                    }
                    string[] fileList = Directory.GetFiles(folderPath, "*.wad", SearchOption.AllDirectories);
                    if (fileList.Length < 1)
                    {
                        return false;
                    }
                }
                else
                {
                    string[] paths = { "titles", titleId, titleVersion, $"{titleId}-NUS-v{titleVersion}.wad" };
                    string wadPath = CombinePaths(TestRootFolder, paths);
                    if (!File.Exists(wadPath))
                    {
                        return false;
                    }
                }
            }
            else
            {
                // Just check folder
                string[] paths = { "titles", titleId };
                string folderPath = CombinePaths(TestRootFolder, paths);
                if (!titleVersion.Equals("*"))
                {
                    folderPath = CombinePaths(folderPath, titleVersion);
                }

                if (!Directory.Exists(folderPath))
                {
                    return false;
                }
                string[] fileList = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories);
                if (fileList.Length < 1) // Check that folder contains at least 1 file of any type
                {
                    return false;
                }
            }
            return true;
        }

        // Check the output folder for any file with .app extension
        private bool CheckDecryptedOutput(string titleId, string titleVersion)
        {
            // Just check folder
            string[] paths = { "titles", titleId };
            string folderPath = CombinePaths(TestRootFolder, paths);
            if (!titleVersion.Equals("*"))
            {
                folderPath = CombinePaths(folderPath, titleVersion);
            }

            if (!Directory.Exists(folderPath))
            {
                return false;
            }
            string[] fileList = Directory.GetFiles(folderPath, "*.app", SearchOption.AllDirectories);
            if (fileList.Length < 1) // Check that folder contains at least 1 app file
            {
                return false;
            }
            return true;
        }

        [Test]
        public void CreateForm1Success()
        {
            WipeOutputFolder(Directory.GetCurrentDirectory());
            Form1 nusForm = new Form1();
            Assert.NotNull(nusForm);
        }

        [TestCase("000000010000000b", "*")]
        [TestCase("000000010000003d", "*")]
        [TestCase("0000000100000021", "*")]
        [TestCase("0000000100000022", "*", new string[] { "packwad" })]
        [TestCase("0000000100000022", "*")]
        [TestCase("0000000100000023", "*")]
        [TestCase("000000010000000b", "10" )]
        [TestCase("000000010000000c", "6" )]
        [TestCase("000000010000000d", "10" )]
        [TestCase("000000010000000f", "257" )]
        [TestCase("0000000100000011", "512", new string[] { "packwad" })]
        [TestCase("0000000100000014", "12" )]
        [TestCase("0000000100000015", "514" )]
        [TestCase("0000000100000015", "1038", new string[] { "packwad", "truchapatch" })]
        [TestCase("000000010000001e", "1040")]
        [TestCase("000000010000001f", "1040")]
        [TestCase("0000000100000021", "1040")]
        [TestCase("0000000100000022", "1039")]
        [TestCase("0000000100000023", "1040")]
        [TestCase("0000000100000024", "1042")]
        [TestCase("0000000100000025", "2070", new string[] { "packwad" })]
        [TestCase("0000000100000026", "3610")]
        [TestCase("0000000100000035", "4113")]
        [TestCase("0000000100000037", "4633")]
        [TestCase("0000000100000037", "5149")]
        [TestCase("0000000100000038", "5661")]
        [TestCase("000000010000003c", "6174")]
        [TestCase("000000010000003d", "4890")]
        [TestCase("0000000100000002", "417")]
        [TestCase("0000000100000002", "417", new string[] { "packwad", "truchapatch" })]
        [TestCase("000000010000000b", "10", new string[] { "http://nus.cdn.shop.wii.com/ccs/download/" })]
        [TestCase("000000010000000b", "10", new string[] { "http://nus.cdn.t.shop.nintendowifi.net/ccs/download/", "dsi" })]
        [TestCase("000000010000000b", "10", new string[] { "http://ccs.cdn.sho.rc24.xyz/ccs/download/", "wii" })]
        [TestCase("0000000100000026", "3610", new string[] { "packwad", "truchapatch", "esidentitypatch", "nandpermissionpatch" })]
        public void DownloadNusSuccess(string titleId, string titleVersion, string [] options = null)
        {
            bool buildWad = false;
            WipeOutputFolder(TestRootFolder);

            Form1 nusForm = new Form1();
            Assert.NotNull(nusForm);

            SetDefaultNusOptions(ref nusForm);

            nusForm.SetTileId(titleId);
            if (titleVersion.Equals("*")) // * means latest version, which equates to an empty string
            {
                nusForm.SetTileVersion("");
            }
            else
            {
                nusForm.SetTileVersion(titleVersion);
            }

            if (options != null)
            {
                SetNusOptions(ref nusForm, options);
                // See if one of the options is to packwad
                var match = options.FirstOrDefault(stringToCheck => stringToCheck.Contains("packwad"));
                if (match != null)
                {
                    buildWad = true;
                }
            }

            // Perform the download
            Assert.DoesNotThrow(() => nusForm.NUSDownloader_DoWork(null, null));

            // Check for existence of folder and wad. 000000010000000b-NUS-v10.wad
            // Note that can't really check for the latest version, ie "*", so only check for folder in that case
            if (!CheckOutput(buildWad, titleId, titleVersion))
            {
                Assert.Fail();
            }
        }

        [TestCase("0000000100000037", "4633", new string[] { "http://nus.bogus.shop.wii.com/ccs/download/" })]
        [TestCase("0000000100000037", "4633", new string[] { "http://nus.cdn.t2.shop.nintendowifi.net/ccs/download/" })]
        [TestCase("0000000100000037", "4633", new string[] { "http://ccs.cdn.sho.rc24.xyz/ccs/downloads/" })]
        public void TestInvalidUrl(string titleId, string titleVersion, string[] options = null)
        {
            WipeOutputFolder(TestRootFolder);

            Form1 nusForm = new Form1();
            Assert.NotNull(nusForm);

            SetDefaultNusOptions(ref nusForm);

            nusForm.SetTileId(titleId);
            if (titleVersion.Equals("*")) // The "*" means latest version, which equates to an empty string
            {
                nusForm.SetTileVersion("");
            }
            else
            {
                nusForm.SetTileVersion(titleVersion);
            }

            if (options != null)
            {
                SetNusOptions(ref nusForm, options);
            }

            // Perform the download - should fail because of the invalid args
            var ex = Assert.Throws<Exception>(() => nusForm.NUSDownloader_DoWork(null, null));
            Assert.True(ex.Message.Contains("Downloading TMD Failed"));
        }

        [TestCase("000000010000000b", "*")]
        [TestCase("000000010000003d", "*")]
        [TestCase("0000000100000021", "*")]
        [TestCase("0000000100000022", "*", new string[] { "packwad" })]
        [TestCase("0000000100000022", "*")]
        [TestCase("0000000100000023", "*")]
        [TestCase("000000010000000b", "10")]
        [TestCase("000000010000000c", "6")]
        [TestCase("000000010000000d", "10")]
        [TestCase("000000010000000f", "257")]
        [TestCase("0000000100000011", "512", new string[] { "packwad" })]
        [TestCase("0000000100000014", "12")]
        [TestCase("0000000100000015", "514")]
        [TestCase("0000000100000015", "1038", new string[] { "packwad", "truchapatch" })]
        [TestCase("000000010000001e", "1040")]
        [TestCase("000000010000001f", "1040")]
        [TestCase("0000000100000021", "1040")]
        [TestCase("0000000100000022", "1039")]
        [TestCase("0000000100000023", "1040")]
        [TestCase("0000000100000024", "1042")]
        [TestCase("0000000100000025", "2070", new string[] { "packwad" })]
        [TestCase("0000000100000026", "3610")]
        [TestCase("0000000100000035", "4113")]
        [TestCase("0000000100000037", "4633")]
        [TestCase("0000000100000037", "5149")]
        [TestCase("0000000100000038", "5661")]
        [TestCase("000000010000003c", "6174")]
        [TestCase("000000010000003d", "4890")]
        [TestCase("0000000100000002", "417")]
        [TestCase("0000000100000002", "417", new string[] { "packwad", "truchapatch" })]
        [TestCase("000000010000000b", "10", new string[] { "http://nus.cdn.shop.wii.com/ccs/download/" })]
        [TestCase("000000010000000b", "10", new string[] { "http://nus.cdn.t.shop.nintendowifi.net/ccs/download/", "dsi" })]
        [TestCase("000000010000000b", "10", new string[] { "http://ccs.cdn.sho.rc24.xyz/ccs/download/", "wii" })]
        [TestCase("0000000100000026", "3610", new string[] { "packwad", "truchapatch", "esidentitypatch", "nandpermissionpatch" })]
        [TestCase("000000010000001f", "1040", new string[] { "http://ccs.cdn.sho.rc24.xyz/ccs/download/" })]
        [TestCase("0000000100000021", "1040", new string[] { "http://nus.cdn.shop.wii.com/ccs/download/" })]
        [TestCase("0000000100000022", "1039", new string[] { "http://ccs.cdn.sho.rc24.xyz/ccs/download/" })]
        [TestCase("0000000100000023", "1040", new string[] { "http://nus.cdn.shop.wii.com/ccs/download/" })]
        [TestCase("000000010000000b", "*", new string[] { "packwad", "nandpermissionpatch" })]
        [TestCase("000000010000003d", "*", new string[] { "packwad", "esidentitypatch" })]
        [TestCase("0000000100000021", "*", new string[] { "packwad", "truchapatch", "http://ccs.cdn.sho.rc24.xyz/ccs/download/" })]
        [TestCase("000000010000000b", "*", new string[] { "packwad", "nandpermissionpatch", "esidentitypatch" })]
        [TestCase("000000010000003d", "*", new string[] { "packwad", "esidentitypatch" })]
        [TestCase("0000000100000021", "*", new string[] { "packwad", "truchapatch", "esidentitypatch", "http://nus.cdn.shop.wii.com/ccs/download/" })]

        // These should fail
        [TestCase("000000010000000b", "10", new string[] { "http://nus.cdn.shop99.wii.com/ccs/download/" }, false)]
        [TestCase("0000000100000002", "417", new string[] { "packwaddle", "truchapatch" }, false)]
        [TestCase("000000010000000b", "10", new string[] { "http://ccs.cdn.shogun.rc24.xyz/ccs/download/", "wii" }, false)]
        [TestCase("0000000100000037", "4633", new string[] { "wiiU" }, false)]
        [TestCase("000000010000001f1", "1040", null, false)]  // Too long
        [TestCase("000000010000001", "1040", null, false)]    // Too short
        [NonParallelizable]
        public void TestCliInterface(string titleId, string titleVersion, string[] options = null, bool resultExists = true)
        {
            bool buildWad = false;

            // Clean the folders of any results from previous test
            WipeOutputFolder(AssemblyFolder);
            WipeOutputFolder(TestRootFolder);

            // Append any options to command
            string nusCmd = $"{AssemblyFolder}/nusd.exe {titleId} {titleVersion}";
            if (options != null)
            {
                foreach (string arg in options)
                {
                    nusCmd += $" {arg}";
                    if (arg.Equals("packwad"))
                    {
                        buildWad = true;
                    }
                }
            }

            // Run the CLI util
            string output = RunNusCliCmd(nusCmd);

            // Check for existence of folder and/or wad.
            // Note that can't really check for the latest version, ie "*", so only check for folder in that case
            if (CheckOutput( buildWad,  titleId,  titleVersion) != resultExists)
            {
                Assert.Fail();
            }
        }

        [TestCase("0000000100000100", "5", new string[] { "createdecryptedcontents" }, true)]
        [TestCase("0000000100000100", "5", null, false)]
        [TestCase("0001000248414141", "2", new string[] { "createdecryptedcontents" }, true)]
        [TestCase("0001000248414141", "2", null, false)]
        // These have no tickets and CANNOT produce decrypted content, even with the option enabled
        [TestCase("0001000157545245", "*", new string[] { "createdecryptedcontents" }, false)]
        [TestCase("0001000146424845", "*", new string[] { "createdecryptedcontents" }, false)]
        public void CreateDecryptedContentSuccess(string titleId, string titleVersion, string[] options = null, bool resultExists = true)
        {
            bool buildDecryptApp = false;

            // Clean the folders of any results from previous test
            WipeOutputFolder(AssemblyFolder);
            WipeOutputFolder(TestRootFolder);

            // Append any options to command
            string nusCmd = $"{AssemblyFolder}/nusd.exe {titleId} {titleVersion}";
            if (options != null)
            {
                foreach (string arg in options)
                {
                    nusCmd += $" {arg}";
                    if (arg.Equals("createdecryptedcontents"))
                    {
                        buildDecryptApp = true;
                    }
                }
            }

            // Run the CLI util
            string output = RunNusCliCmd(nusCmd);

            // Check for existence of folder and/or app file.
            if (CheckDecryptedOutput(titleId, titleVersion) != resultExists)
            {
                Assert.Fail();
            }
        }

        [TestCase("000000010000000B", "256", new string[] { "packwad" }, true)]
        // Any pached IOS will produce wad even without specifying packwad
        [TestCase("000000010000000B", "256", new string[] { "esidentitypatch" }, true)]
        [TestCase("000000010000000B", "256", new string[] { "nandpermissionpatch" }, true)]
        [TestCase("000000010000000B", "256", new string[] { "truchapatch" }, true)]
        [TestCase("000000010000000B", "256", new string[] { "truchapatch", "nandpermissionpatch", "esidentitypatch" }, true)]
        [TestCase("000000010000000B", "256", new string[] { "nandpermissionpatch", "esidentitypatch" }, true)]
        [TestCase("000000010000000B", "256", new string[] { "truchapatch", "esidentitypatch" }, true)]

        // Non-IOS cannot be patched and will not produce .wad
        [TestCase("00030005484e4441", "256", new string[] { "truchapatch", "esidentitypatch" }, false)]
        [TestCase("00030005484e4441", "256", new string[] { "packwad", "esidentitypatch" }, false)]
        public void TestCreateWad(string titleId, string titleVersion, string[] options = null, bool resultExists = true)
        {
            //bool buildWad = false;

            // Clean the folders of any results from previous test
            WipeOutputFolder(AssemblyFolder);
            WipeOutputFolder(TestRootFolder);

            // Append any options to command
            string nusCmd = $"{AssemblyFolder}/nusd.exe {titleId} {titleVersion}";
            if (options != null)
            {
                foreach (string arg in options)
                {
                    nusCmd += $" {arg}";
                    //if (arg.Equals("packwad"))
                    //{
                    //    buildWad = true;
                    //}
                }
            }

            // Run the CLI util
            string output = RunNusCliCmd(nusCmd);

            // Check for existence of folder and/or wad.
            // Force buildWad to true in this call to always search for .wad files in the output folder
            if (CheckOutput(true, titleId, titleVersion) != resultExists)
            {
                Assert.Fail();
            }
        }
    }
}