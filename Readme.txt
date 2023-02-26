I thought it would be useful to have a command line version of the NUS Download Tool, originally by 
WB3000 (http://wiibrew.org/wiki/NUS_Downloader), where it can be used in batch files, grabbing multiple titles. 
So I whipped out this version, based on the GUI v1 from WB3000. Nothing to write home about, just a few lines of
"glue logic" for my own use.

I've tested several titles by comparing the outputs from this tool, and the outputs from WB3000's tool. 
Both results are the same.

Usage: nusd <titleID> <titleVersion | *> [option] [option] ... [option]

Where: 
    titleID = The ID of the title to be downloaded
    titleVersion = The version of the title to be downloaded
                   To get the latest version, use a "*" (without quotes)
[option] can be any of the following:
    packwad = Optional: A wad file will be generated
    localuse = Optional: All the downloaded files will be retained locally
    createdecryptedcontents = Optional: Create decrypted (.app) content
        * I've only verified the creation of, but not the validity of the results. 
        * Content is same as the original GUI version.
    removeencryptedcontents = Remove the downloaded encrypted contents.
        ****** CAUTION ******* - Use this with care, because if this option is used by itself, no outputs will be produced.
        All the encrypted downloaded files will be removed after downloaded. To make any sense, this should be used 
        with "packwad" or "createdecryptedcontents"
    truchapatch = Optional: If IOS, then apply Trucha patch
        * I've only verified the creation of, but not the validity of the results. 
        * Content is same as the original GUI version.
    esidentitypatch = Optional: If IOS, then apply ES Identity patch
        * I've only verified the creation of, but not the validity of the results.
        * Content is same as the original GUI version.
    nandpermissionpatch = Optional: If IOS, then apply NAND Permission patch
        * I've only verified the creation of, but not the validity of the results.
        * Content is same as the original GUI version.
    wii = Default NUS server (automatically selects the first online server between official wii server and RC24)
    dsi = Use DSI server
    <Custom Server URL> = Optional - Can specify an alternate server URL to use instead of the built-in URLs
    help = Display this help text

*** NOTE ****: In the GUI <packwad> is defaulted to OFF, and <localuse> is defaulted to ON. As of v0.7 the CLI follows those defaults.

Credits: All credits go to WB3000 for the original software. 
         WiiPower for the hint on grabbing the latest version

Requirements:

    * You will need MS .NET Framework 4.8 or 3.5 to run this tool (branch dotnet3.5 is used for 3.5)
    * Internet connection

Included:

    * Binary NUSDownloader.exe
    * The original "NUSdownloader.exe" from WB3000 - Required to be in the same folder as nusd.exe
    * Modified source with Visual Studio soltuion and projects
    * Sample batch files - These are what I used to test in addition to the unit tests in the VS solution

Warnings:
    * I've only tested this latest version on a Windows11 machine

Disclaimer:
    * I'm not responsible for any bricks that may result from using this tool.

Revision History:
    v0.1b - 5/5/2009 - Initial version
    v0.2 - 5/6/2009 - Added option to retrieve the latest version of a title
    v0.3 - 12/08/2021 - Retrieved the latest version of NUS Downloader from Google Code that was already archived.
                        Provided additional command-line arguments per v1.9 of NUS Downloader.
    v0.4 - 2/22/2023 - Recompiled using .NET 3.5 instead of .NET 4.8 (Use branch dotnet3.5)
    v0.6 - 2/24/2023 - Additional wii/dsi options, custom URL override, help text, and use RC24 server as alternate.
                       Fixed up NUSDownloader.exe's CLI interface and removed the nusd.exe (No need to maintain two versions)
    v0.7 - 2/24/2023 - Added options "removeencryptedcontents" and "help". Option "packwad" now defaults to OFF to be consistent with GUI.

-------- 0 ---------
wiiNinja
