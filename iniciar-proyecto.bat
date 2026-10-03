@echo off
echo =====================================================
echo   Destino de Ensuenos - Iniciando proyecto completo
echo =====================================================
echo.
echo [1/2] Iniciando API Backend (http://localhost:5058)...
start "Destino API" cmd /k "cd /d "%~dp0DestinoEnsueno.API" && dotnet run"

echo [2/2] Iniciando Frontend (http://localhost:5500)...
timeout /t 3 /nobreak >nul
start "Destino Frontend" cmd /k "cd /d "%~dp0" && node .serve.js"

echo.
echo Abriendo el navegador en 5 segundos...
timeout /t 5 /nobreak >nul
start http://localhost:5500

echo.
echo Listo. Cierra las dos ventanas de cmd para detener el proyecto.
