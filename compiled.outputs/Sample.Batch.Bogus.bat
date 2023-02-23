@echo off
echo Get some titles with specific version
rem should fail
nusd 000000010000000b 10 http://ccs.cdn.sho.rc24.xyz/ccs_bogus/download/ wii

nusd 0000000100000015 1038 badarg
pause
