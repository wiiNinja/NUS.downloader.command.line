using NUnit.Framework;
using NUS_Downloader;
using System;

namespace UnitTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CreateForm1Success()
        {
            Form1 nusForm = new Form1();
            Assert.NotNull(nusForm);
        }

        [Test]
        public void CreateWadSuccess()
        {
            String titleId = "000000010000000b";
            String titleVersion = "10";

            Form1 nusForm = new Form1();
            Assert.NotNull(nusForm);
            nusForm.SetTileId(titleId);
            nusForm.SetTileVersion(titleVersion);
            nusForm.SetNusType("wii");
            // nusForm.SetPackWad(true);

            /*
            nusForm.SetLocalUse(true);
            //TruchaBugEnable = true;
            //ESIdentityPatchEnable = true;
            //NandPermissionPatchEnable = true;
            nusForm.SetEsIdentityBugEnable(true);
            nusForm.SetNandPermissionBugEnable(true);
            nusForm.SetCreateDecryptedContents(true);
            //nusForm.SetCustomNusUrl(args[i]);
            nusForm.SetNusType("wii");
            //nusForm.SetNusType("dsi");
            */

            // Perform the download
            Assert.DoesNotThrow(() => nusForm.NUSDownloader_DoWork(null, null));
        }
    }
}