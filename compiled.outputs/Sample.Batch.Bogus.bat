@echo off
set NUS_COMMAND=nusd
rem set NUS_COMMAND=NUSDownloader.exe

echo Get some titles with invalid args - Should fail

%NUS_COMMAND% 000000010000000b 10 http://ccs.cdn.sho.rc24.xyz/ccs_bogus/download/ wii

%NUS_COMMAND% 0000000100000015 1038 badarg

%NUS_COMMAND% 0000000100000015 1038 packwaddle

pause
