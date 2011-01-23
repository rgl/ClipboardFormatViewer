@ REM Set command switch for building debug or retail (default is to build debug)
@ REM Type "build.bat -r" to build for retail
@ SET DEBUGSAMPLE=/debug+ /d:DEBUG
@ IF "%1"=="-r" SET DEBUGSAMPLE=/debug- /optimize+
@ IF "%1"=="-R" SET DEBUGSAMPLE=/debug- /optimize+

csc.exe /nologo /target:winexe %DEBUGSAMPLE% /R:System.DLL /out:bin/main.exe /res:i.gif Rgl.Components.ClipboardViewer.cs main.cs
