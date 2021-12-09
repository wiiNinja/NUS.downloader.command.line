using System;
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
            NUS_Downloader.Form1 nusForm = new NUS_Downloader.Form1();

            Console.WriteLine($"nusd version: {NUSD_VERSION}");
            // Initialize the checkboxes and radio boxes
            nusForm.SetPackWad(false);
            nusForm.SetLocalUse(true);
            nusForm.SetKeepEncryptedContent(true);
            nusForm.SetServerButtonText("Wii");

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
                            // nusForm.titleidbox.Text = args[i];
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
                            // Only two other cmd line args are handled: packwad and localuse
                            if (args[i] == "packwad")
                            {
                                nusForm.SetPackWad(true);
                            }
                            else if (args[i] == "localuse")
                            {
                                nusForm.SetLocalUse(true);
                            }
                            break;
                    }
                }

                // Just a test download of the System Menu 4.0U
                //myForm.titleidbox.Text = "0000000100000002";
                //myForm.titleversion.Text = "417";
                //myForm.packbox.Checked = true;
                //myForm.localuse.Checked = true;
                //myForm.radioButton1Wii.Checked = true;
                //myForm.radioButton2DS.Checked = false;

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
