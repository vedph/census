@echo off
echo UPDATE LOCAL PACKAGES
set src=C:\Projects\_NuGet\

del .\local-packages\*.nupkg

xcopy %src%fusi.tools\1.1.15\*.nupkg .\local-packages\ /y
xcopy %src%fusi.text\1.1.12\*.nupkg .\local-packages\ /y

pause
