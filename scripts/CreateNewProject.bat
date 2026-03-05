color 3
@echo off

dotnet new -i .template\CoreMeTemplate.2.0.0.nupkg

set /p OP=Please set your project name(for example:Memo.Core):

md .project

cd .project

dotnet new coreme -n %OP%

cd ../

echo "----------------Create Successfully!! ^ please see the folder .NewProject----------------"

dotnet new uninstall CoreMeTemplate

echo "----------------Delete Template Successfully!----------------"

pause