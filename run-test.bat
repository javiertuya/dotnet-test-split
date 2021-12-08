dotnet test --logger "trx;LogFileName=../../reports/mstest-report.trx" TestAssetsMstest/TestAssetsMstest.csproj
dotnet build
cd DotnetTestSplit\bin\Debug\netcoreapp3.1
DotnetTestSplit.exe ..\..\..\..\reports\mstest-report.trx
cd ..\..\..\..\
cmd /c ant report
pause