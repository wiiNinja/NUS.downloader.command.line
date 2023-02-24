using NUnit.Framework;
using NUS_Downloader;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace UnitTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        private bool WipeOutputFolder (string rootFolder)
        {
            // Remove the "title" and "script" folders
            //string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
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
            string testRootFolder = Directory.GetCurrentDirectory();
            WipeOutputFolder(testRootFolder);

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
            // Note that can't really check for the latest version, so only check for folder in that case
            if (buildWad)
            {
                if (titleVersion.Equals("*"))
                {
                    // Check just the folder
                    string[] paths = { "titles", titleId };
                    string folderPath = CombinePaths(testRootFolder, paths);
                    if (!Directory.Exists(folderPath))
                    {
                        Assert.Fail();
                    }
                    string[] fileList = Directory.GetFiles(folderPath, "*.wad", SearchOption.AllDirectories);
                    if (fileList.Length < 1)
                    {
                        Assert.Fail();
                    }
                }
                else
                {
                    string[] paths = { "titles", titleId, titleVersion, $"{titleId}-NUS-v{titleVersion}.wad" };
                    string wadPath = CombinePaths(testRootFolder, paths);
                    if (!File.Exists(wadPath))
                    {
                        Assert.Fail();
                    }
                }
            }
            else
            {
                // Just check folder
                string[] paths = { "titles", titleId }; ;
                string folderPath = CombinePaths(testRootFolder, paths);
                if (!titleVersion.Equals("*"))
                {
                    folderPath = CombinePaths(folderPath, titleVersion);
                }
                
                if (!Directory.Exists(folderPath))
                {
                    Assert.Fail();
                }
                string[] fileList = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories);
                if (fileList.Length < 1) // Check that folder at least contains 1 file
                {
                    Assert.Fail();
                }
            }
        }

        [TestCase("0000000100000037", "4633", new string[] { "http://nus.bogus.shop.wii.com/ccs/download/" })]
        [TestCase("0000000100000037", "4633", new string[] { "http://nus.cdn.t2.shop.nintendowifi.net/ccs/download/" })]
        [TestCase("0000000100000037", "4633", new string[] { "http://ccs.cdn.sho.rc24.xyz/ccs/downloads/" })]
        public void TestInvalidUrl(string titleId, string titleVersion, string[] options = null)
        {
            bool buildWad = false;
            string testRootFolder = Directory.GetCurrentDirectory();
            WipeOutputFolder(testRootFolder);

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
                // See if one of the options is to packwad
                var match = options.FirstOrDefault(stringToCheck => stringToCheck.Contains("packwad"));
                if (match != null)
                {
                    buildWad = true;
                }
            }

            // Perform the download - should fail because of the invalid args
            var ex = Assert.Throws<Exception>(() => nusForm.NUSDownloader_DoWork(null, null));
            Assert.True(ex.Message.Contains("Downloading TMD Failed"));
        }
    }
}