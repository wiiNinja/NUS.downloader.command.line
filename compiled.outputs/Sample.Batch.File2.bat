@echo off
echo Get some titles with specific version
nusd 000000010000000b 10 wii
nusd 000000010000000b 10 rc24
nusd 000000010000000b 10 dsi

nusd 000000010000000b 10 http://nus.cdn.shop.wii.com/ccs/download/ wii
nusd 000000010000000b 10 http://nus.cdn.t.shop.nintendowifi.net/ccs/download/ dsi
nusd 000000010000000b 10 http://ccs.cdn.sho.rc24.xyz/ccs/download/ wii

rem should fail
rem nusd 000000010000000b 10 http://ccs.cdn.sho.rc24.xyz/ccs_bogus/download/ wii

pause
