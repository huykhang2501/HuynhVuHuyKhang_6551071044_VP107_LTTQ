@echo off
cd /d "%~dp0"
if exist "%~dp0bin\BanChay\Dulieu.exe" (
    start "" "%~dp0bin\BanChay\Dulieu.exe"
) else (
    dotnet run --project "%~dp0Dulieu.csproj" -c Release
)
