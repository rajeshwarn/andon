@echo off

setlocal
rem rdir=��������_���������_���������
set rdir=bin
rem ���������������� ���� �������� ��� ���������� �������� � ������ ����������:
rem set rdir=%1
set fpath=%~dps0
call :func %fpath:~0,-1%
goto end
:func
for /f "delims=" %%i in ('dir %1 /a:d /b') do IF /I %%i==%rdir% ( rmdir /s /q %1\%%i && echo deleted %1\%%i ) ELSE ( call :func %1\%%i )
exit /b
:end
rem ������� ������ ����:
rem del /Q /F %fpath%%~nx0
