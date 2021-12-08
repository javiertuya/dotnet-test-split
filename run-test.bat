REM Prepare test assets
dotnet test TestAssetsMstest/TestAssetsMstest.csproj --logger "trx;LogFileName=../../reports/mstest-report.trx" 

REM Split tests assets
dotnet build
cd DotnetTestSplit\bin\Debug\netcoreapp3.1
DotnetTestSplit.exe ..\..\..\..\reports\mstest-report.trx
cd ..\..\..\..\

REM Split test assets when installed as local tool
REM https://docs.microsoft.com/es-es/dotnet/core/tools/global-tools-how-to-create
REM dotnet nuget locals all --list
rmdir /s /q .config
rmdir /s /q DotnetTestSplit\nupkg
dotnet new tool-manifest
dotnet pack DotnetTestSplit/DotnetTestSplit.csproj
dotnet tool install --no-cache --add-source DotnetTestSplit/nupkg DotnetTestSplit --version 1.1.0-3
dotnet tool run DotnetTestSplit reports/mstest-report.trx

REM Run test
dotnet test TestDotnetTestSplit/TestDotnetTestSplit.csproj --logger "trx;LogFileName=dotnet-test-split-report.trx" 

cmd /c ant report
pause