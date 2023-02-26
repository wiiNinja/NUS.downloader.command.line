using System;
using NUS_Downloader;

namespace nusd
{
    public class Program
    {
        const string NUSD_VERSION = "v0.6";

        static public void Main(string[] args)
        {
            bool TruchaBugEnable = false;
            bool ESIdentityPatchEnable = false;
            bool NandPermissionPatchEnable = false;
            Form1 nusForm = new Form1();
            bool successStatus = true;

            // Initialize the checkboxes and radio boxes
            nusForm.SetPackWad(false);
            nusForm.SetLocalUse(true);
            nusForm.SetKeepEncryptedContent(true);
            nusForm.SetCreateDecryptedContents(false);
            nusForm.SetPatchIOS(false);
            nusForm.SetServerButtonText("Wii");
            nusForm.SetTruchaBugEnable(false);

            Console.WriteLine("\n\n---------------------------------------------------------------------------------------------------------------");
            Console.WriteLine($"NUS Downloader Command Line {NUSD_VERSION} by wiiNinja. Based on GUI code {nusForm.GetVersion()} by WB3000");
            if (args.Length < 2)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("    nusd <titleID> <titleVersion | *> [option] [option] ... [option]");
                Console.WriteLine("\nWhere:");
                Console.WriteLine("    titleID = The ID of the title to be downloaded");
                Console.WriteLine("    titleVersion = The version of the title to be downloaded");
                Console.WriteLine("              Use \"*\" (without quotes) to get the latest version");
                Console.WriteLine("\n[option] can be any of the following:");
                Console.WriteLine("    packwad = A wad file will be generated when appropriate");
                Console.WriteLine("    localuse = All the downloaded files will be retained locally");
                Console.WriteLine("    createdecryptedcontents = Create decrypted (.app) contents");
                Console.WriteLine("    truchapatch = Apply Trucha patch to IOS");
                Console.WriteLine("    esidentitypatch = Apply ES Identity patch to IOS");
                Console.WriteLine("    nandpermissionpatch = Apply NAND Permission patch to IOS");
                Console.WriteLine("    <NUSUrl> = Define a custom NUS Url in the format: http://x.y.z.host.com/css/download/");
                Console.WriteLine("    <nusType> = Select NUS type. Can be either \"wii\" or \"dsi\". Default is \"wii\"");
            }
            else
            {
                string commandArgs = "";
                for (int i = 0; i < args.Length; i++)
                {
                    commandArgs += $" {args[i]}";
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
                    // If IOS, see if user wants to patch with one or more of the bugs
                    if ((TruchaBugEnable == true) || (ESIdentityPatchEnable == true) || (NandPermissionPatchEnable == true))
                    {
                        nusForm.SetPatchIOS(true);
                    }
                    nusForm.SetTruchaBugEnable(TruchaBugEnable);
                    nusForm.SetEsIdentityBugEnable(ESIdentityPatchEnable);
                    nusForm.SetNandPermissionBugEnable(NandPermissionPatchEnable);

                    // For debugging/verifying
                    //bool[] checkList = nusForm.GetPachesEnableList();

                    // Get the files from the server
                    try
                    {
                        Console.WriteLine($"Downloading using arguments: {commandArgs}");
                        nusForm.NUSDownloader_DoWork(null, null);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error encounted while downloading with args: {commandArgs}");
                        Console.WriteLine($"    Message: {ex.Message}");
                        successStatus = false;
                    }
                    
                }
                if (successStatus)
                {
                    Console.WriteLine($"Successfully downloaded the contents with args: {commandArgs}");
                }
                else
                {
                    Console.WriteLine($"Download failed using args: {commandArgs}");
                }
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------");
            }
        }
    }
}
