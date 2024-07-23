@echo off
setlocal

REM Configura variables
set DB_USER=euroflor
set DB_PASS=motu1429
set DB_NAME=euroflor
set BACKUP_DIR=C:\Users\SebasGX\Documents\Personal\Projects\Euroflor\BackUpEuroFlor
set BACKUP_FILE=%BACKUP_DIR%\backup_%DATE:~-4,4%_%DATE:~-10,2%_%DATE:~-7,2%.sql

REM Realiza el backup
mysqldump -u %DB_USER% -p%DB_PASS% %DB_NAME% > %BACKUP_FILE%

REM Verifica si el backup se creó correctamente
if exist %BACKUP_FILE% (
    echo Backup creado exitosamente: %BACKUP_FILE%
) else (
    echo Error al crear el backup.
)

endlocal
