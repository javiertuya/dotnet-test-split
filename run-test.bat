REM Prepare test assets
dotnet test TestAssetsMstest/TestAssetsMstest.csproj --logger "trx;LogFileName=../../reports/mstest-report.trx" 

REM Split tests assets
dotnet build
cd DotnetTestSplit\bin\Debug\netcoreapp3.1
DotnetTestSplit.exe ..\..\..\..\reports\mstest-report.trx
cd ..\..\..\..\

REM Run test
dotnet test TestDotnetTestSplit/TestDotnetTestSplit.csproj --logger "trx;LogFileName=dotnet-test-split-report.trx" 

cmd /c ant report
pause