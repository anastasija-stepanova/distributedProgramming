start "Frontend" /d Frontend\ dotnet Frontend.dll

start "Backend" /d Backend\ dotnet Backend.dll

start "TextListener" /d TextListener\ dotnet TextListener.dll

start "TextRankCalc" /d TextRankCalc\ dotnet TextRankCalc.dll

start "TextProcessingLimiter" /d TextProcessingLimiter\ dotnet TextProcessingLimiter.dll

set file=%CD%\config\number.txt
for /f "tokens=1,2 delims=:" %%a in (%file%) do (
for /l %%i in (1, 1, %%b) do start "%%a" /d %%a dotnet %%a.dll
)
