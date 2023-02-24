using System;
using NUS_Downloader;

namespace nusd
{
    class Program
    {
        const string NUSD_VERSION = "v0.6";

        static void Main(string[] args)
        {
            bool TruchaBugEnable = false;
            bool ESIdentityPatchEnable = false;
            bool NandPermissionPatchEnable = false;
            Form1 nusForm = new Form1();
            bool successStatus = true;

            // Initialize the checkboxes and radio boxes
            nusForm.SetPackWad(true);
            nusForm.SetLocalUse(true);
            nusForm.SetKeepEncryptedContent(true);
            nusForm.SetCreateDecryptedContents(false);
            nusForm.SetPatchIOS(false);
            nusForm.SetServerButtonText("Wii");
            nusForm.SetTruchaBugEnable(true);

            Console.WriteLine($"NUS Downloader Command Line {NUSD_VERSION} by wiiNinja. Based on GUI code v1.9 by WB3000");
            if (args.Length < 2)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("    nusd <titleID> <titleVersion | *> [option] [option] ... [option]");
                Console.WriteLine("\nWhere:");
                Console.WriteLine("    titleID = The ID of the title to be downloaded");
                Console.WriteLine("    titleVersion = The version of the title to be downloaded");
                Console.WriteLine("              Use \"*\" (without quotes) to get the latest version");
                Console.WriteLine("\n[option] can be any of the following:");
                Console.WriteLine("    packwad = Optional: A wad file will be generated");
                Console.WriteLine("    localuse = Optional: All the downloaded files will be retained locally");
                Console.WriteLine("    createdecryptedcontents = Optional: Create decrypted (.app) contents");
                Console.WriteLine("    truchapatch = Optional: Apply Trucha patch");
                Console.WriteLine("    esidentitypatch = Optional: Apply ES Identity patch");
                Console.WriteLine("    nandpermissionpatch = Optional: Apply NAND Permission patch");
                Console.WriteLine("    <NUSUrl> = Optional: Define a custom NUS Url in the format: http://x.y.z.host.com/css/download/");
                Console.WriteLine("    <nusType> = Optional: Select NUS type. Can be either \"wii\" or \"dsi\". Default is \"wii\"");
            }
            else
            {
                for (int i = 0; i < args.Length; i++)
                {
                    Console.WriteLine("{0}", args[i]);
                    switch (i)
                    {
                        case 0:
                            // First command line argument is ALWAYS the TitleID
                            nusForm.SetTileId(args[i]);
                            break;

                        case 1:
                            // Second command line argument is ALWAYS the TitleVersion. 
                            // User may specify a "*" to retrieve the latest version
                            if (args[i] == "*")
                            {
                                nusForm.SetTileVersion("");
                            }
                            else
                            {
                                nusForm.SetTileVersion(args[i]);
                            }
                            break;

                        default:
                            // Other cmd line args are handled: packwad, localuse, TruchaPatch, EsIdentityPatch, NandPermissionPatch
                            if (args[i].ToLower() == "packwad")
                            {
                                nusForm.SetPackWad(true);
                            }
                            else if (args[i].ToLower() == "localuse")
                            {
                                nusForm.SetLocalUse(true);
                            }
                            else if (args[i].ToLower() == "truchapatch")
                            {
                                TruchaBugEnable = true;
                            }
                            else if (args[i].ToLower() == "esidentitypatch")
                            {
                                ESIdentityPatchEnable = true;
                            }
                            else if (args[i].ToLower() == "nandpermissionpatch")
                            {
                                NandPermissionPatchEnable = true;
                            }
                            else if (args[i].ToLower() == "createdecryptedcontents")
                            {
                                nusForm.SetCreateDecryptedContents(true);
                            }
                            else if (args[i].Contains("http:"))
                            {
                                nusForm.SetCustomNusUrl(args[i]);
                            }
                            else if (args[i].ToLower() == "wii")
                            {
                                nusForm.SetNusType("wii");
                            }
                            else if (args[i].ToLower() == "dsi")
                            {
                                nusForm.SetNusType("dsi");
                            }
                            else
                            {
                                // Invalid option
                                // throw new InvalidProgramException($"Error: Invalid option \"{args[i]}\" was specified on the command line");
                                Console.WriteLine ($"Error: Invalid option \"{args[i]}\" was specified on the command line");
                                successStatus = false;
                            }
                            break;
                    }
                }

                if (successStatus)
                {
                    // If IOS, see if user wants to patch with one of the bugs
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

                    // Call to get the files from server
                    try
                    {
                        nusForm.NUSDownloader_DoWork(null, null);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"\nError encounted while downloading the title {args[0]} version {args[1]}");
                        Console.WriteLine($"Error message: {ex.Message}");
                        throw;
                    }
                    Console.WriteLine($"\nSuccessfully downloaded the title {args[0]} version {args[1]}");
                }
                else
                {
                    Console.WriteLine("Download failed.");
                }
            }
        }
    }
}
