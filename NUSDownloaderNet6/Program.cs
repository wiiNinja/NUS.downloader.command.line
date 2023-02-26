// See https://aka.ms/new-console-template for more information

using NUSDownloaderNet6;

const string NUSD_VERSION = "v0.8";

bool TruchaBugEnable = false;
bool ESIdentityPatchEnable = false;
bool NandPermissionPatchEnable = false;
NusDownloader nusDownloader = new NusDownloader();
bool successStatus = true;

// Initialize the checkboxes and radio boxes
nusDownloader.SetPackWad(false);
nusDownloader.SetLocalUse(true);
nusDownloader.SetKeepEncryptedContents(true);
nusDownloader.SetCreateDecryptedContents(false);
nusDownloader.SetPatchIOS(false);
nusDownloader.SetNusType("Wii");
nusDownloader.SetTruchaBugEnable(false);

Console.WriteLine("\n\n---------------------------------------------------------------------------------------------------------------");
Console.WriteLine($"NUS Downloader Command Line {NUSD_VERSION} by wiiNinja. Based on GUI code v1.9 by WB3000");
if (args.Length < 2)
{
    ShowHelpText();
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
                nusDownloader.SetTileId(args[i]);
                break;

            case 1:
                // Second command line argument is ALWAYS the TitleVersion. 
                // User may specify a "*" to retrieve the latest version
                if (args[i] == "*")
                {
                    nusDownloader.SetTileVersion("");
                }
                else
                {
                    nusDownloader.SetTileVersion(args[i]);
                }
                break;

            default:
                // Other cmd line args are handled: packwad, localuse, TruchaPatch, EsIdentityPatch, NandPermissionPatch
                if (args[i].ToLower() == "packwad")
                {
                    nusDownloader.SetPackWad(true);
                }
                else if (args[i].ToLower() == "localuse")
                {
                    nusDownloader.SetLocalUse(true);
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
                    nusDownloader.SetCreateDecryptedContents(true);
                }
                else if (args[i].Contains("http:"))
                {
                    nusDownloader.SetCustomNusUrl(args[i]);
                }
                else if (args[i].ToLower() == "wii")
                {
                    nusDownloader.SetNusType("wii");
                }
                else if (args[i].ToLower() == "dsi")
                {
                    nusDownloader.SetNusType("dsi");
                }
                else if (args[i].ToLower() == "help")
                {
                    ShowHelpText();
                    successStatus = false; // Just don't continue after a help arg
                }
                else if (args[i].ToLower() == "removeencryptedcontents")
                {
                    // Encrypted content is downloaded, decrypted (if specified), and
                    // create .app or .wad (if specified). If decrypt and .wad/.app are not specified, then nothing
                    // will be left in the output folder after the command is run.
                    //nusDownloader.RemoveDownloadedEncryptedContents();
                    nusDownloader.SetKeepEncryptedContents(false);
                }
                else
                {
                    // Invalid option
                    // throw new InvalidProgramException($"Error: Invalid option \"{args[i]}\" was specified on the command line");
                    Console.WriteLine($"Error: Invalid option \"{args[i]}\" was specified on the command line");
                    successStatus = false;
                }
                break;
        }
    }

    if (successStatus)
    {
        // If IOS, see if user wants to patch with one or more of the bugs
        if (TruchaBugEnable || ESIdentityPatchEnable || NandPermissionPatchEnable)
        {
            nusDownloader.SetPatchIOS(true);
            nusDownloader.SetPackWad(true); // Per GUI behavior any patch automatically creates wad
        }
        nusDownloader.SetTruchaBugEnable(TruchaBugEnable);
        nusDownloader.SetEsIdentityBugEnable(ESIdentityPatchEnable);
        nusDownloader.SetNandPermissionBugEnable(NandPermissionPatchEnable);

        // For debugging/verifying
        //bool[] checkList = nusForm.GetPachesEnableList();

        // Get the files from the server
        try
        {
            Console.WriteLine($"Downloading using arguments: {commandArgs}");
            nusDownloader.NUSDownloader_DoWork();
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

void ShowHelpText()
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
    Console.WriteLine("    removeencryptedcontent = *** CAUTION *** - Remove downloaded contents after other operations are done. Refer to Readme.txt");
    Console.WriteLine("    help = Show help text");
}