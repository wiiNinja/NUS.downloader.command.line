@echo off
rem set NUS_COMMAND=nusd
set NUS_COMMAND=NUSDownloader.exe

echo Get some titles with invalid args - Should fail
%NUS_COMMAND% 000000010000000b 10 http://ccs.cdn.sho.rc24.xyz/ccs_bogus/download/ wii
%NUS_COMMAND% 0000000100000015 1038 badarg
%NUS_COMMAND% 0000000100000015 1038 packwaddle

rem Should produce warnings
%NUS_COMMAND% 000000010000003d 4890 removeencryptedcontents
%NUS_COMMAND% 0000000100000011 512 removeencryptedcontents
rem Should NOT produce warnings
%NUS_COMMAND% 0000000100000035 4113 removeencryptedcontents packwad
%NUS_COMMAND% 0000000100000021 * removeencryptedcontents createdecryptedcontents

pause
