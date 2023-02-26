using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace NUSDownloaderNet6
{
    public class NusDownloader
    {
        // Flags to be used in this class for downloading files and producing output
        private bool UseLocalFileOption = true; // Default to true
        private bool PackWadOption = false;
        private bool KeepEncryptedContentsOption = true;
        private bool CreateDecryptedContentsOption = false;
        private bool PatchIosOption = false;
        private bool PatchEsIdentityBug = false;
        private bool PatchNandPermissionBug = false;
        private bool PatchTruchaBug = false;
        private string NusTypeOption = "wii";
        private string TitleIdOption = "";
        private string TitleVersionOption = "";
        private string CustomNusUrl = "";
        private string WadFileName = "";
        private ProxyInfo UserProxyInfo = new ProxyInfo();
        private readonly string CURRENT_DIR = Directory.GetCurrentDirectory();

        public void SetPackWad(bool packWadFlag)
        {
            PackWadOption = packWadFlag;
        }
        public void SetLocalUse(bool localUseFlag)
        {
            UseLocalFileOption = localUseFlag;
        }
        public void SetKeepEncryptedContents(bool keepContentFlag)
        {
            KeepEncryptedContentsOption = keepContentFlag;
        }
        public void SetCreateDecryptedContents(bool createFlag)
        {
            CreateDecryptedContentsOption = createFlag;
        }
        public void SetPatchIOS(bool patchFlag)
        {
            PatchIosOption = patchFlag;
        }

        public void SetTileId(string titleId)
        {
            // Check for length of 16 characters
            TitleIdOption = titleId;
        }
        public void SetTileVersion(string titleVersion)
        {
            // Wildcard "*" for latest version
            TitleVersionOption = titleVersion;
        }

        public void SetNusType(string nusType)
        {
            NusTypeOption = nusType;
        }
        public void SetCustomNusUrl(string nusUrl)
        {
            CustomNusUrl = nusUrl;
        }

        public void SetEsIdentityBugEnable(bool esidPatchFlag)
        {
            PatchEsIdentityBug = esidPatchFlag;
        }
        public void SetNandPermissionBugEnable(bool nandPermFlag)
        {
            PatchNandPermissionBug = nandPermFlag;
        }
        public void SetTruchaBugEnable(bool truchaFlag)
        {
            PatchTruchaBug = truchaFlag;
        }

        public void WriteStatus(string Update)
        {
            Console.WriteLine(Update);
        }

        // Expect user to have a file named proxy.json in the same folder as the executable
        private bool GetProxyInfoFromFile ()
        {
            bool success = true;
            UserProxyInfo.url = "";
            UserProxyInfo.userName = "";
            UserProxyInfo.password = "";

            // Read JSON directly from a file, CURRENT_DIR
            string filePath = System.IO.Path.Combine(CURRENT_DIR, "proxy.json");
            if (File.Exists(filePath))
            {
                try
                {
                    UserProxyInfo = JsonConvert.DeserializeObject<ProxyInfo>(File.ReadAllText(filePath));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Warning: Cannot read proxy information from {filePath}");
                    success = false;
                }
            }
            return success;
        }

        private WebClient ConfigureWithProxy(WebClient client)
        {
            GetProxyInfoFromFile();

            // MTP: Proxy is set and saved in proxy.txt
            if (!(String.IsNullOrEmpty(UserProxyInfo.url)))
            {
                WebProxy customproxy = new WebProxy();
                customproxy.Address = new Uri(UserProxyInfo.url);
                if (String.IsNullOrEmpty(UserProxyInfo.userName))
                    customproxy.UseDefaultCredentials = true;
                else
                {
                    NetworkCredential cred = new NetworkCredential();
                    cred.UserName = UserProxyInfo.userName;

                    if (!(String.IsNullOrEmpty(UserProxyInfo.password)))
                        cred.Password = UserProxyInfo.password;

                    customproxy.Credentials = cred;
                }
                client.Proxy = customproxy;
                WriteStatus(">>> Warning: The file proxy.json exists. Custom proxy settings applied!");
            }
            else
            {
                try
                {
                    client.Proxy = WebRequest.GetSystemWebProxy();
                    client.UseDefaultCredentials = true;
                }
                catch (NotImplementedException)
                {
                    // Linux support
                    WriteStatus("This operating system does not support automatic system proxy usage. Operating without a proxy...");
                }
            }
            return client;
        }

        private string NameFromDatabase(string titleid)
        {
            // DANGER: BAD h4x HERE!!
            // Fix MIOS/BC naming
            if (titleid == "0000000100000101")
                return "MIOS";
            else if (titleid == "0000000100000100")
                return "BC";

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("database.xml");

            // Variables
            string[] XMLNodeTypes = new string[4] { "SYS", "IOS", "VC", "WW" };

            // Loop through XMLNodeTypes
            for (int i = 0; i < XMLNodeTypes.Length; i++) // FOR THE FOUR TYPES OF NODES
            {
                XmlNodeList XMLSpecificNodeTypeList = xDoc.GetElementsByTagName(XMLNodeTypes[i]);

                for (int x = 0; x < XMLSpecificNodeTypeList.Count; x++) // FOR EACH ITEM IN THE LIST OF A NODE TYPE
                {
                    bool found_it = false;

                    // Lol.
                    XmlNodeList ChildrenOfTheNode = XMLSpecificNodeTypeList[x].ChildNodes;

                    for (int z = 0; z < ChildrenOfTheNode.Count; z++) // FOR EACH CHILD NODE
                    {
                        switch (ChildrenOfTheNode[z].Name)
                        {
                            case "titleID":
                                if (ChildrenOfTheNode[z].InnerText == titleid)
                                    found_it = true;
                                else if ((ChildrenOfTheNode[z].InnerText.Substring(0, 14) + "XX") ==
                                         (titleid.Substring(0, 14) + "XX") &&
                                         (titleid.Substring(0, 14) != "00000001000000"))
                                    found_it = true;
                                else
                                    found_it = false;
                                break;
                            default:
                                break;
                        }
                    }

                    if (found_it)
                    {
                        for (int z = 0; z < ChildrenOfTheNode.Count; z++) // FOR EACH CHILD NODE
                        {
                            switch (ChildrenOfTheNode[z].Name)
                            {
                                case "name":
                                    return ChildrenOfTheNode[z].InnerText;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            return null;
        }

        public string OfficialWADNaming(string titlename)
        {
            if (titlename == "MIOS")
                titlename = "RVL-mios-[v].wad";
            else if (titlename.Contains("IOS"))
                titlename = titlename + "-64-[v].wad";
            else if (titlename.Contains("System Menu"))
                titlename = "RVL-WiiSystemmenu-[v].wad";
            else if (titlename.Contains("System Menu"))
                titlename = "RVL-WiiSystemmenu-[v].wad";
            else if (titlename == "BC")
                titlename = "RVL-bc-[v].wad";
            else if (titlename.Contains("Mii Channel"))
                titlename = "RVL-NigaoeNR-[v].wad";
            else if (titlename.Contains("Shopping Channel"))
                titlename = "RVL-Shopping-[v].wad";
            else if (titlename.Contains("Weather Channel"))
                titlename = "RVL-Weather-[v].wad";
            else
                titlename = titlename + "-NUS-[v].wad";

            /* MTP - not needed - if (wadnamebox.InvokeRequired)
            {
                OfficialWADNamingCallback ownc = new OfficialWADNamingCallback(OfficialWADNaming);
                wadnamebox.Invoke(ownc, new object[] { titlename });
                return titlename;
            } */

            WadFileName = titlename;

            if (TitleVersionOption != "")
                WadFileName = WadFileName.Replace("[v]", "v" + TitleVersionOption);

            return titlename;
        }

        private static string RemoveIllegalCharacters(string databasestr)
        {
            // Database strings must contain filename-legal characters.
            foreach (char illegalchar in System.IO.Path.GetInvalidFileNameChars())
            {
                if (databasestr.Contains(illegalchar.ToString()))
                    databasestr = databasestr.Replace(illegalchar, '-');
            }
            return databasestr;
        }

        private void UpdatePackedName()
        {
            // Change WAD name if applicable
            string title_name = null;

            if (PackWadOption)
            {
                if (!TitleVersionOption.Equals (""))
                {
                    WadFileName = TitleIdOption + "-NUS-v" + TitleVersionOption + ".wad";
                }
                else
                {
                    WadFileName = TitleIdOption + "-NUS-[v]" + TitleVersionOption + ".wad";
                }

                if ((File.Exists("database.xml") == true) && (TitleIdOption.Length == 16))
                    title_name = NameFromDatabase(TitleIdOption);

                if (title_name != null)
                {
                    WadFileName = WadFileName.Replace(TitleIdOption, title_name);
                    OfficialWADNaming(title_name);
                }
            }
            WadFileName = RemoveIllegalCharacters(WadFileName);
        }

        void nusClient_Debug(object sender, libWiiSharp.MessageEventArgs e)
        {
            WriteStatus(e.Message);
        }

        private bool ValidateOptions()
        {
            if ((KeepEncryptedContentsOption == false) && (PackWadOption == false) && (CreateDecryptedContentsOption == false))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // This is the meat of the implementation. Name is kept for reference back to the original code
        public void NUSDownloader_DoWork()
        {
            // MTP Control.CheckForIllegalCrossThreadCalls = false; // this function would need major rewriting to get rid of this...

            // Some sanity checks to warn the user if some options don't make sense
            if (!ValidateOptions())
            {
                Console.WriteLine($"*** Warning: The option \"removeencryptedcontents\" if used by itself will result in no files in the output folder.");
            }

            WriteStatus("Starting NUS Download. Please be patient!");

            // WebClient configuration
            WebClient nusWC = new WebClient();
            nusWC = ConfigureWithProxy(nusWC);

            // Create\Configure NusClient
            libWiiSharp.NusClient nusClient = new libWiiSharp.NusClient();
            nusClient.ConfigureNusClient(nusWC);
            nusClient.UseLocalFiles = UseLocalFileOption; //  localuse.Checked;
            nusClient.ContinueWithoutTicket = true;

            // Server
            if ((CustomNusUrl == null) || (CustomNusUrl == ""))
            {
                if (NusTypeOption.ToLower() == "wii")
                {
                    // If user wants a wii server, select first one that has connectivity
                    nusClient.SetToWiiServer();
                }
                else // if (serverLbl.Text == "DSi")
                {
                    nusClient.SetToDSiServer();
                }
            }
            else
            {
                // This is a custom URL override. Just use it
                nusClient.SetCustomServer(CustomNusUrl, NusTypeOption.ToLower() == "wii");
            }

            // Events
            nusClient.Debug += new EventHandler<libWiiSharp.MessageEventArgs>(nusClient_Debug);
            // nusClient.Progress += new EventHandler<ProgressChangedEventArgs>(nusClient_Progress);

            libWiiSharp.StoreType[] storeTypes = new libWiiSharp.StoreType[3];
            if (PackWadOption)
            {
                storeTypes[0] = libWiiSharp.StoreType.WAD;
            }
            else
            {
                storeTypes[0] = libWiiSharp.StoreType.Empty;
            }
            if (CreateDecryptedContentsOption)
            {
                storeTypes[1] = libWiiSharp.StoreType.DecryptedContent;
            }
            else
            {
                storeTypes[1] = libWiiSharp.StoreType.Empty;
            }
            if (KeepEncryptedContentsOption)
            {
                storeTypes[2] = libWiiSharp.StoreType.EncryptedContent;
            }
            else
            {
                storeTypes[2] = libWiiSharp.StoreType.Empty;
            }

            UpdatePackedName();

            // ----------------------------------------------------
            // Perform the download
            // ----------------------------------------------------
            try
            {
                nusClient.DownloadTitle(TitleIdOption, TitleVersionOption, Path.Combine(CURRENT_DIR, "titles"), WadFileName, storeTypes);
            }
            catch (Exception ex)
            {
                WriteStatus("Download failed: \"" + ex.Message + " ):\"");
                throw;
            }

            // ----------------------------------------------------
            // Apply patches if requested
            // ----------------------------------------------------
            if (PatchIosOption == true)
            {
                // Apply patches then...
                bool didpatch = false;
                int noofpatches = 0;
                string appendpatch = "";
                // Okay, it's checked.
                libWiiSharp.IosPatcher iosp = new libWiiSharp.IosPatcher();
                libWiiSharp.WAD ioswad = new libWiiSharp.WAD();
                WadFileName = WadFileName.Replace("[v]", nusClient.TitleVersion.ToString());
                if (WadFileName.Contains(Path.DirectorySeparatorChar.ToString()) || WadFileName.Contains(Path.AltDirectorySeparatorChar.ToString()))
                    ioswad.LoadFile(WadFileName);
                else
                    ioswad.LoadFile(Path.Combine(Path.Combine(Path.Combine(Path.Combine(CURRENT_DIR, "titles"), TitleIdOption), nusClient.TitleVersion.ToString()), WadFileName));
                try
                {
                    iosp.LoadIOS(ref ioswad);
                }
                catch (Exception)
                {
                    WriteStatus("NUS Download Finished.");
                    return;
                }

                if (PatchTruchaBug)
                {
                    noofpatches = iosp.PatchFakeSigning();
                    if (noofpatches > 0)
                    {
                        WriteStatus(" - Patched in fake-signing:");
                        if (noofpatches > 1)
                            appendpatch = "es";
                        else
                            appendpatch = "";
                        WriteStatus(String.Format("     {0} patch{1} applied.", noofpatches, appendpatch));
                        didpatch = true;
                    }
                    else
                        WriteStatus(" - Could not patch fake-signing");
                }
                if (PatchNandPermissionBug)
                {
                    noofpatches = iosp.PatchNandPermissions();
                    if (noofpatches > 0)
                    {
                        WriteStatus(" - Patched in NAND permissions:");
                        if (noofpatches > 1)
                            appendpatch = "es";
                        else
                            appendpatch = "";
                        WriteStatus(String.Format("     {0} patch{1} applied.", noofpatches, appendpatch));
                        didpatch = true;
                    }
                    else
                        WriteStatus(" - Could not patch NAND permissions");
                }
                if (PatchEsIdentityBug)
                {
                    noofpatches = iosp.PatchEsIdentify();
                    if (noofpatches > 0)
                    {
                        WriteStatus(" - Patched in ES_Identify:");
                        if (noofpatches > 1)
                            appendpatch = "es";
                        else
                            appendpatch = "";
                        WriteStatus(String.Format("     {0} patch{1} applied.", noofpatches, appendpatch));
                        didpatch = true;
                    }
                    else
                        WriteStatus(" - Could not patch ES_Identify");
                }
                if (didpatch)
                {
                    WadFileName = WadFileName.Replace(".wad", ".patched.wad");
                    try
                    {
                        if (WadFileName.Contains(Path.DirectorySeparatorChar.ToString()) || WadFileName.Contains(Path.AltDirectorySeparatorChar.ToString()))
                            ioswad.Save(WadFileName);
                        else
                            ioswad.Save(Path.Combine(Path.Combine(Path.Combine(Path.Combine(CURRENT_DIR, "titles"), TitleIdOption), nusClient.TitleVersion.ToString()), WadFileName));
                        WriteStatus(String.Format("Patched WAD saved as: {0}", Path.GetFileName(WadFileName)));
                    }
                    catch (Exception ex)
                    {
                        WriteStatus(String.Format("Couldn't save patched WAD: \"{0}\" :(", ex.Message));
                    }
                }
            }
        } // Dowork
    } // Class

    internal class ProxyInfo
    {
        public string url;
        public string userName;
        public string password;
    }
} // namespace
