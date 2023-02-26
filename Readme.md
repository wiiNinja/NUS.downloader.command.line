# Description

I thought it would be useful to have a command line version of the NUS Download Tool, originally by 
WB3000 (http://wiibrew.org/wiki/NUS_Downloader), where it can be used in batch files, grabbing multiple titles. 
So I whipped out this version, based on the GUI v1 from WB3000. Nothing to write home about, just a few lines of
"glue logic" for my own use.

I've tested several titles by comparing the outputs from this tool, and the outputs from WB3000's tool. 
Both results are the same.

2023-02 - At the request of XFlak, I rebuilt the CLI latest code, based on WB3000's v1.9, using .NET Framework 3.5. 
   Some enhancements were made to this build, but I also took this opportunity to port it to Microsoft's latest
   .NET6 (core) which supports both Linux and Windows. By doing this port the GUI is eliminated because .NET6 currently
   does not support GUI in Linux (perhaps a future thing when Microsoft decided to support that).

# Usage: 
    <executableName> <titleID> <titleVersion | *> [option] [option] ... [option]

## Where: 
    executableName = nusd.exe (legacy), NUSDownloader.exe (.net framework 3.5 and 4.8), NUSDownloaderNet6.exe (.NET6)
        For Linux, replace executableName with: "dotnet NUSDownloader.dll" - The rest of the options are the same.
    titleID = The ID of the title to be downloaded
    titleVersion = The version of the title to be downloaded
                   To get the latest version, use a "*" (without quotes)
### [option] can be any of the following:
    help = Display this help text
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

*** NOTE ****: In the GUI the option <packwad> is defaulted to OFF, and <localuse> is defaulted to ON. As of v0.7 
    the CLI follows those defaults.
## Proxy
    To use a proxy, provide your proxy information in the file named proxy.json, located in the same folder as the
    executable. If the fields in that file are left empty, the proxy will be ignored.

# Credits:
    All credits go to WB3000 for the original software: https://github.com/WB3000/nusdownloader/tree/master/NUS%20Downloader
    WiiPower for the hint on grabbing the latest version

# Linux:
    Install dotnet6 in your Linux machine.
    MS .NET6 supports different flavors of Linux. Build it yourself if you wish to build it for other flavors.

# Requirements:
    * You will need MS .NET Framework 4.8 or 3.5 to run this tool (branch dotnet3.5 is used for 3.5)
    * Need to install .NET6 to run in a Linux environment
    * Internet connection

# Included:
    * Binary NUSDownloader.exe
    * The original "NUSdownloader.exe" from WB3000 - Required to be in the same folder as nusd.exe
    * Modified source with Visual Studio solutions and projects
          NUSDownloaderNet6.sln - VS solution for the latest and greatest .NET6 - Can run natively in Linux
          NUSDownloaderCli.sln - VS solution for the legacy .NET Framework 3.5 and 4.8
    * Sample batch files - These are what I used to test in addition to the unit tests in the VS solution

# Warnings:
    * I've only tested this latest version on a Windows11 machine and my RaspberryPi4 running Linux.

# Disclaimer:
    * I'm not responsible for any bricks that may result from using this tool.

# Revision History:
    v0.1b - 5/5/2009 - Initial version
    v0.2 - 5/6/2009 - Added option to retrieve the latest version of a title
    v0.3 - 12/08/2021 - Retrieved the latest version of NUS Downloader from Google Code that was already archived.
                        Provided additional command-line arguments per v1.9 of NUS Downloader.
    v0.4 - 2/22/2023 - Recompiled using .NET 3.5 instead of .NET 4.8 (Use branch dotnet3.5)
    v0.6 - 2/24/2023 - Additional wii/dsi options, custom URL override, help text, and use RC24 server as alternate.
                       Fixed up NUSDownloader.exe's CLI interface and removed the nusd.exe (No need to maintain two versions)
    v0.7 - 2/24/2023 - Added options "removeencryptedcontents" and "help". Option "packwad" now defaults to OFF to be consistent with GUI.
    v0.8 - 2/24/2023 - Ported to Dotnet6, which allows CLI app to run in both Windows and Linux environments. 
                       Note that this port is completely CLI-based. No GUI because MS NET6 currently does not support GUI in Linux.

-------- 0 ---------
wiiNinja
