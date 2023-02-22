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
    truchapatch = Optional: If IOS, then apply Trucha patch
        * I've only verified the creation of, but not the validity of the results. 
        * Content is same as the original GUI version.
    esidentitypatch = Optional: If IOS, then apply ES Identity patch
        * I've only verified the creation of, but not the validity of the results.
        * Content is same as the original GUI version.
    nandpermissionpatch = Optional: If IOS, then apply NAND Permission patch
        * I've only verified the creation of, but not the validity of the results.
        * Content is same as the original GUI version.

<packwad> and <localuse> are defaulted to ON to make it easy for people.
If for some reason, you need these defaults to OFF, PM me in GBATemp.net, and I'll change it.

Credits: All credits go to WB3000 for the original software. 
         WiiPower for the hint on grabbing the latest version

Requirements:

    * You will need MS .NET Framework 4.8 to run this thing
    * Internet connection

Included:

    * Binary nusd.exe
    * The original "NUSdownloader.exe" from WB3000 - Required to be in the same folder as nusd.exe
    * Modified source with Visual Studio projects files and such
    * Sample batch file - This is what I used to test

Warnings:
    * I've only tested this on a Windows10 machine

Disclaimer:
    * I'm not responsible for any brick that may result from using this tool.

Revision History:
    v0.1b - 5/5/2009 - Initial version
    v0.2 - 5/6/2009 - Added option to retrieve the latest version of a title
    v0.3 - 12/08/2021 - Retrieved the latest version of NUS Downloader from Google Code that was already archived.
                        Provided additional command-line arguments per v1.9 of NUS Downloader.

-------- 0 ---------
wiiNinja
