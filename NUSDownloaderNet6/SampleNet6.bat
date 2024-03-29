@echo off

rem set NUS_COMMAND=nusd
rem set NUS_COMMAND=NUSDownloader.exe
set NUS_COMMAND=NUSDownloaderNet6.exe

echo Get some titles with specific version
%NUS_COMMAND% 000000010000000b 10
%NUS_COMMAND% 000000010000000c 6
%NUS_COMMAND% 000000010000000d 10
%NUS_COMMAND% 000000010000000f 257
%NUS_COMMAND% 0000000100000011 512
%NUS_COMMAND% 0000000100000014 12
%NUS_COMMAND% 0000000100000015 514
%NUS_COMMAND% 0000000100000015 1038 createdecryptedcontents
%NUS_COMMAND% 000000010000001e 1040
%NUS_COMMAND% 000000010000001f 1040
%NUS_COMMAND% 0000000100000021 1040
%NUS_COMMAND% 0000000100000022 1039
%NUS_COMMAND% 0000000100000023 1040 createdecryptedcontents
%NUS_COMMAND% 0000000100000024 1042
%NUS_COMMAND% 0000000100000025 2070
%NUS_COMMAND% 0000000100000026 3610
%NUS_COMMAND% 0000000100000035 4113
%NUS_COMMAND% 0000000100000037 4633
%NUS_COMMAND% 0000000100000037 5149 createdecryptedcontents
%NUS_COMMAND% 0000000100000038 5661 truchapatch
%NUS_COMMAND% 000000010000003c 6174
%NUS_COMMAND% 000000010000003d 4890
%NUS_COMMAND% 0000000100000002 417

echo Get the latest versions of some titles
%NUS_COMMAND% 000000010000003d *
%NUS_COMMAND% 0000000100000021 * createdecryptedcontents
%NUS_COMMAND% 0000000100000022 *
%NUS_COMMAND% 0000000100000022 * esidentitypatch nandpermissionpatch
%NUS_COMMAND% 0000000100000023 *

echo Packing some wads
%NUS_COMMAND% 00000001000000FE 65280 packwad
rem %NUS_COMMAND% 0001000248414745 7 packwad
%NUS_COMMAND% 0001000157545245 * packwad
%NUS_COMMAND% 00000001000000FE 260 packwad
rem %NUS_COMMAND% 0000000100000002 518 packwad
%NUS_COMMAND% 000100014E414445 * packwad
%NUS_COMMAND% 0000000100000101 9 packwad
%NUS_COMMAND% 0000000100000100 * packwad
%NUS_COMMAND% 0000000100000035 4113 packwad truchapatch
%NUS_COMMAND% 0000000100000026 3610 packwad truchapatch esidentitypatch nandpermissionpatch

%NUS_COMMAND% 000000010000000b 10 wii
%NUS_COMMAND% 000000010000000b 10 rc24
%NUS_COMMAND% 000000010000000b 10 dsi

%NUS_COMMAND% 000000010000000b 10 http://nus.cdn.shop.wii.com/ccs/download/
%NUS_COMMAND% 000000010000000b 10 http://nus.cdn.t.shop.nintendowifi.net/ccs/download/ dsi
%NUS_COMMAND% 000000010000000b 10 http://ccs.cdn.sho.rc24.xyz/ccs/download/ wii

rem should fail
%NUS_COMMAND% 000000010000000b 10 http://ccs.cdn.sho.rc24.xyz/ccs/downloads/ wii

pause
