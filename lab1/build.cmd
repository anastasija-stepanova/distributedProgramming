@ECHO OFF

SET INVALID_ARGS_ERROR_CODE=1
SET BUILD_ERROR=2

SET VERSION=%~1
SET BUILD_DIR=v%VERSION%
SET CONFIG_DIR=%BUILD_DIR%/config

SET BACKEND_NAME=backend
SET BACKEND_SRC=src/%BACKEND_NAME%
SET BACKEND_OUT=%BUILD_DIR%/%BACKEND_NAME%
SET BACKEND_OUT_FROM_PROJ=../../%BACKEND_OUT%
SET BACKEND_WINDOW_NAME=%BACKEND_NAME% v%VERSION%

SET FRONTEND_NAME=frontend
SET FRONTEND_SRC=src/%FRONTEND_NAME%
SET FRONTEND_OUT=%BUILD_DIR%/%FRONTEND_NAME%
SET FRONTEND_OUT_FROM_PROJ=../../%FRONTEND_OUT%
SET FRONTEND_WINDOW_NAME=%FRONTEND_NAME% v%VERSION%

IF "%VERSION%"=="" (
  GOTO InvalidArgs
)

CALL :Build
CALL :CreateConfig
CALL :CreateRunScript
CALL :CreateStopScript

ECHO Build completed!
EXIT /B 0

:Build
  dotnet build %BACKEND_SRC% -o %BACKEND_OUT_FROM_PROJ%
  dotnet build %FRONTEND_SRC% -o %FRONTEND_OUT_FROM_PROJ%
  EXIT /B 0
  
:CreateConfig
  md "%CONFIG_DIR%"
  @ECHO > %CONFIG_DIR%/%FRONTEND_NAME%Config.cfg
  @ECHO > %CONFIG_DIR%/%BACKEND_NAME%Config.cfg
  EXIT /B 0

:CreateRunScript
  (
    @ECHO start "%FRONTEND_WINDOW_NAME%" dotnet %FRONTEND_NAME%/%FRONTEND_NAME%.dll
    @ECHO start "%BACKEND_WINDOW_NAME%" dotnet %BACKEND_NAME%/%BACKEND_NAME%.dll
  ) > %BUILD_DIR%/run.cmd
  EXIT /B 0

:CreateStopScript
  (
    @ECHO @ECHO OFF
    @ECHO
    @ECHO taskkill /IM dotnet.exe
  ) > %BUILD_DIR%/stop.cmd
  EXIT /B 0

:InvalidArgs
  ECHO Build version not found...
  EXIT /B %INVALID_ARGS_ERROR_CODE%

:BuildError
  ECHO Error during build project...
  EXIT /B %BUILD_ERROR%