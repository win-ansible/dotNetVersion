# dotNetVersion
Simple command line tool to get latest version of installed .net, based on [this](http://stackoverflow.com/a/951915) stackoverflow discussion.

Can be compiled on mac with mono:
```
brew install mono
mcs -out:dotNetVersion.exe dotNetVersion.cs
```

# Usage

Run without arguments to get version printed and program paused, pass any argument to let it exit after printing dotnet version.
