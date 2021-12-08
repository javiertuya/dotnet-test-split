REM Prepare test assets
dotnet test --logger "trx;LogFileName=../../reports/mstest-report.trx" TestAssetsMstest/TestAssetsMstest.csproj

REM Split tests assets
dotnet build
cd DotnetTestSplit\bin\Debug\netcoreapp3.1
DotnetTestSplit.exe ..\..\..\..\reports\mstest-report.trx
cd ..\..\..\..\

REM Run test
dotnet test --logger "trx;LogFileName=dotnet-test-split-report.trx" TestDotnetTestSplit/TestDotnetTestSplit.csproj

cmd /c ant report
pause