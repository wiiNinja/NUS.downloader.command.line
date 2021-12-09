I thought it would be useful to have a command line version of the NUS Download Tool, originally by 
WB3000 (http://wiibrew.org/wiki/NUS_Downloader), where it can be used in batch files, grabbing multiple titles. 
So I whipped out this version, based on the GUI v1 from WB3000. Nothing to write home about, just a few lines of
"glue logic" for my own use.

I've tested several titles by comparing the outputs from this tool, and the outputs from WB3000's tool. 
Both results are the same.

Usage: nusd <titleID> <titleVersion | *> [packwad] [localuse]

Where: 
    titleID = The ID of the title to be downloaded
    titleVersion = The version of the title to be downloaded
                   To get the latest version, use a "*" (without quotes)
    packwad = Optional: A wad file will be generated
    localuse = Optional: All the downloaded files will be retained locally

At this time <packwad> and <localuse> are default to ON, so you don't even need to specify it. 
If for some reason, you need these defaults to OFF, PM me in GBATemp.net, and I'll change it.

Credits: All credits go to WB3000 for the original software. 
         WiiPower for the hint on grabbing the latest version

Requirements:

    * You will need MS .NET Framework 2.0 to run this thing
    * Internet connection

Included:

    * Binary nusd.exe
    * Modified source with Visual Studio projects files and such
    * The original "NUSdownloader_v1.exe" from WB3000
    * Sample batch file

Warnings:
    * I've only tested this on a 32 bit Windows XP machine, not Vista

Disclaimer:
    * I'm not responsible for any brick that may result from using this tool.

Revision History:
    v0.1b - 5/5/2009 - Initial version
    v0.2 - 5/6/2009 - Added option to retrieve the latest version of a title
    v0.3 - 12/21/2021 - Retrieved the latest version of NUS Downloader from Google Code that was already archived.

-------- 0 ---------
wiiNinja
