@echo off
title Meta Quest OVRServer Reset
echo ==========================================
echo   Meta Quest - OVRServer Reset
echo ==========================================
echo.

echo [1/3] OVRServer_x64.exe sonlandiriliyor...
taskkill /F /IM OVRServer_x64.exe >nul 2>&1

if %ERRORLEVEL%==0 (
    echo OVRServer_x64.exe sonlandirildi.
) else (
    echo OVRServer_x64.exe bulunamadi veya zaten kapali.
)

echo.
echo [2/3] 5 saniye bekleniyor...
timeout /t 5 /nobreak >nul

echo.
echo [3/3] OVRServer'in yeniden baslamasi bekleniyor...
timeout /t 10 /nobreak >nul

echo.
echo Kontrol:
tasklist /FI "IMAGENAME eq OVRServer_x64.exe" 2>nul | find /I "OVRServer_x64.exe"

echo.
echo ==========================================
echo Islem tamamlandi.
echo Meta Horizon Link'i manuel olarak kapatilmadi.
echo ==========================================
pause
