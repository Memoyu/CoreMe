::需要提前配置nuget命令行环境
:: 命令默认排除开头为.的文件，需要增加 -NoDefaultExcludes参数
@echo off
cls

nuget pack coreme.nuspec -NoDefaultExcludes
ECHO pack successfully...
pause