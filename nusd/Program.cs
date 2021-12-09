﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nusd
{
    class Program
    {
        const string NUSD_VERSION = "v0.3";

        static void Main(string[] args)
        {
            bool TruchaBugEnable = false;
            bool ESIdentityPatchEnable = false;
            bool NandPermissionPatchEnable = false;
            NUS_Downloader.Form1 nusForm = new NUS_Downloader.Form1();

            // Initialize the checkboxes and radio boxes
            nusForm.SetPackWad(false);
            nusForm.SetLocalUse(true);
            nusForm.SetKeepEncryptedContent(true);
            nusForm.SetCreateDecryptedContents(false);
            nusForm.SetPatchIOS(false);
            nusForm.SetServerButtonText("Wii");
            nusForm.SetTruchaBugEnable(true);

            Console.WriteLine($"NUS Downloader Command Line {NUSD_VERSION} by wiiNinja. Based on GUI code v19 by WB3000");
            if (args.Length < 2)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("    nusd <titleID> <titleVersion | *> [packwad] [localuse]");
                Console.WriteLine("\nWhere:");
                Console.WriteLine("    titleID = The ID of the title to be downloaded");
                Console.WriteLine("    titleVersion = The version of the title to be downloaded");
                Console.WriteLine("              Use \"*\" (no quotes) to get the latest version");
                Console.WriteLine("    packwad = Optional: A wad file will be generated");
                Console.WriteLine("    localuse = Optional: All the downloaded files will be retained locally");
                Console.WriteLine("    createdecryptedcontents = Optional: Create decrypted (.app) contents");
                Console.WriteLine("    truchapatch = Optional: Apply Trucha patch");
                Console.WriteLine("    esidentitypatch = Optional: Apply ES Identity patch");
                Console.WriteLine("    nandpermissionpatch = Optional: Apply NAND Permission patch");
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
                            break;
                    }
                }

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
                }
                Console.WriteLine($"\nSuccessfully downloaded the title {args[0]} version {args[1]}");
            }
        }
    }
}
