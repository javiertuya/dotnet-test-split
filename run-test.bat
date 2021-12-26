REM Prepare test assets
dotnet test TestAssetsMstest/TestAssetsMstest.csproj --logger "trx;LogFileName=../../reports/mstest-report.trx" 
dotnet test TestAssetsNunit/TestAssetsNunit.csproj --logger "trx;LogFileName=../../reports/nunit-report.trx" 
dotnet test TestAssetsXunit/TestAssetsXunit.csproj --logger "trx;LogFileName=../../reports/xunit-report.trx" 

REM Split tests assets
rem dotnet build
rem cd DotnetTestSplit\bin\Debug\netcoreapp3.1
rem DotnetTestSplit.exe ..\..\..\..\reports\mstest-report.trx
rem cd ..\..\..\..\

REM Split test assets when installed as local tool
REM https://docs.microsoft.com/es-es/dotnet/core/tools/global-tools-how-to-create
REM dotnet nuget locals all --list

REM limpieza de caches y otros restos de anteriores paquetes
dotnet nuget locals all --clear
rmdir /s /q .config
rmdir /s /q DotnetTestSplit\nupkg
REM crea paquete desde cero, si se usa una prerelease hay que especificar --version en install
dotnet new tool-manifest
dotnet pack DotnetTestSplit/DotnetTestSplit.csproj
dotnet tool install --no-cache --add-source DotnetTestSplit/nupkg DotnetTestSplit
dotnet tool run DotnetTestSplit reports/mstest-report.trx reports/mstest-report.trx.split
dotnet tool run DotnetTestSplit reports/nunit-report.trx reports/nunit-report.trx.split
dotnet tool run DotnetTestSplit reports/xunit-report.trx reports/xunit-report.trx.split

REM Run test
dotnet test TestDotnetTestSplit/TestDotnetTestSplit.csproj --logger "trx;LogFileName=dotnet-test-split-report.trx" 

cmd /c ant report
pause