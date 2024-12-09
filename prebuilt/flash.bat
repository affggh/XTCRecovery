@echo off

set com_port=%1
set search_dir=%2
set local_dir=%~dp0

echo Starting Qualcomm Sahara Server

if exist "%search_dir%\prog_emmc_firehose_8937_ddr.mbn" (
    "%local_dir%\QSaharaServer.exe" -p \\.\%COMPORT% -s "13:%search_dir%\prog_emmc_firehose_8937_ddr.mbn" || exit /b 1
) else (
    "%local_dir%\QSaharaServer.exe" -p \\.\%COMPORT% -s "13:%search_dir%\prog_emmc_firehose_8909w_ddr.mbn" || exit /b 1
)

echo "%search_dir%\rawprogram1.xml"
if exist "%search_dir%\rawprogram1.xml" (
    "%local_dir%\xtcfh_loader.exe" --port=\\.\%COMPORT% --memoryname=EMMC --search_path="%search_dir%" --sendxml=rawprogram0.xml,rawprogram1.xml,rawprogram2.xml,patch0.xml --noprompt --reset || exit /b 1
) else (
    "%local_dir%\xtcfh_loader.exe" --port=\\.\%COMPORT% --memoryname=EMMC --search_path="%search_dir%" --sendxml=rawprogram0.xml,patch0.xml --noprompt --reset || exit /b 1
)
echo Done!
echo Your device will reboot!
