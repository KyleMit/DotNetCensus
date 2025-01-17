# DotNetCensus
[![CI/CD](https://github.com/samsmithnz/DotNetCensus/actions/workflows/workflow.yml/badge.svg)](https://github.com/samsmithnz/DotNetCensus/actions/workflows/workflow.yml)
[![Coverage Status](https://coveralls.io/repos/github/samsmithnz/AzurePipelinesToGitHubActionsConverter/badge.svg?branch=main)](https://coveralls.io/github/samsmithnz/AzurePipelinesToGitHubActionsConverter?branch=main)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=samsmithnz_DotNetCensus&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=samsmithnz_DotNetCensus)
[![Latest NuGet package](https://img.shields.io/nuget/v/dotnet-census?label=nuget%20dotnet%20tool)](https://www.nuget.org/packages/dotnet-census/)
[![Latest NuGet package](https://img.shields.io/nuget/v/dotnetcensus.core?label=nuget%20library)](https://www.nuget.org/packages/dotnetcensus.core/)
![Current Release](https://img.shields.io/github/release/samsmithnz/DotNetCensus/all.svg)

**A dotnet [tool](https://docs.microsoft.com/en-us/dotnet/core/tools/global-tools) to conduct a .NET census - count all of the different .NET versions - on a target directory or GitHub repo.** 

Ever wanted to understand what your .NET portfolio looks like? Do you know what projects are using .NET versions that expired and the amount of technical debt you are managing? Or maybe just looking to have more visibility into your .NET portfolio? DotNet Census is here to help.

DotNet Census currently supports these .NET versions:
- .NET 5, 6, 7, 8, etc
- .NET Core 1 to 3.1  (including older .NET Core 1 and 1.1 project.json formats)
- .NET Framework (including .NET Framework 3.5 website formats with no project files)
- .NET Standard 1 to 2.1

Additionally, this tool supports:
- The main .NET languages, C#, VB.NET, and F#
- [Directory.Build.props variables](https://github.com/samsmithnz/DotNetCensus/issues/44) in project files
- VB6 projects (as many are related to VB.NET projects)

## To use

1. First install:
`dotnet tool install -g dotnet-census`
2. Then run the command in the directory you need to count versions:
`dotnet census`
3. If you have an old version, you can update with `dotnet tool update -g dotnet-census`, and uninstall with `dotnet tool uninstall -g dotnet-census`

## Console arguments
Several options are supported:

`dotnet census [-d|--directory <DIRECTORY>] [-r|--repo <REPO>] [-u|--user <USERNAME>] [-p|--password <PASSWORD>] [-f|--file <FILE>] [-t|--total] [-i|--inventory]`

- `-d|--directory`: target directory to scan for .NET versions
- `-r|--repo`: target GitHub repo to scan for .NET versions
- `-u|--user`: target GitHub username to scan for .NET versions
- `-p|--password`: target GitHub PAT Token to scan for .NET versions
- `-f|--file`: file path to output and save CSV data. 
- `-t|--total`: Add totals to results. Ignored when used with `-i|--inventory`
- `-i|--inventory`: output inventory of all data (instead of the default aggregated summary)

### To target a specific directory from anywhere, use the `-d` argument to specify a target directory:
`dotnet census -d c:\users\me\desktop\repos`

### To add totals to the results, use the `-t` argument:
`dotnet census -t`

This is a sample of the results: 
```
Framework             FrameworkFamily  Count  Status          
--------------------------------------------------------------
.NET 5.0              .NET             1      deprecated      
.NET 6.0              .NET             3      supported       
.NET 6.0-android      .NET             1      supported       
.NET 6.0-ios          .NET             1      supported       
.NET 7.0              .NET             1      supported       
.NET 8.0              .NET             1      in preview      
.NET Core 1.0         .NET Core        1      deprecated      
.NET Core 1.1         .NET Core        1      deprecated      
.NET Core 2.0         .NET Core        1      deprecated      
.NET Core 2.1         .NET Core        1      deprecated      
.NET Core 2.2         .NET Core        1      deprecated      
.NET Core 3.0         .NET Core        2      deprecated      
.NET Core 3.1         .NET Core        3      EOL: 13-Dec-2022
.NET Framework 1.0    .NET Framework   1      deprecated      
.NET Framework 1.1    .NET Framework   1      deprecated      
.NET Framework 2.0    .NET Framework   1      deprecated      
.NET Framework 3.5    .NET Framework   2      EOL: 9-Jan-2029 
.NET Framework 4.0    .NET Framework   1      deprecated      
.NET Framework 4.5    .NET Framework   1      deprecated      
.NET Framework 4.6.1  .NET Framework   1      deprecated      
.NET Framework 4.6.2  .NET Framework   1      supported       
.NET Framework 4.7.1  .NET Framework   1      supported       
.NET Framework 4.7.2  .NET Framework   2      supported       
.NET Standard 2.0     .NET Standard    1      supported       
(Unknown)             (Unknown)        1      unknown         
Visual Basic 6        Visual Basic 6   1      deprecated      
total frameworks                       33                     
```

### To get an inventory of results, use the `-i` argument:
`dotnet census -i`

A sample of inventory results:
```
FileName                                    Path                                                                                     FrameworkCode   FrameworkName         Family          Language  Status
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
Assembly-CSharp.csproj                      \samples\Sample.Unity2020\Assembly-CSharp.csproj                                         v4.7.1          .NET Framework 4.7.1  .NET Framework  csharp    supported
DotNetCensus.Core.csproj                    \src\DotNetCensus.Core\DotNetCensus.Core.csproj                                          net6.0          .NET 6.0              .NET            csharp    supported
DotNetCensus.csproj                         \src\DotNetCensus\DotNetCensus.csproj                                                    net6.0          .NET 6.0              .NET            csharp    supported
DotNetCensus.Tests.csproj                   \src\DotNetCensus.Tests\DotNetCensus.Tests.csproj                                        net6.0          .NET 6.0              .NET            csharp    supported
project.json                                \samples\Sample.NetCore1.0.ConsoleApp\project.json                                       netcoreapp1.0   .NET Core 1.0         .NET Core       csharp    deprecated
project.json                                \samples\Sample.NetCore1.1.ConsoleApp\project.json                                       netcoreapp1.1   .NET Core 1.1         .NET Core       csharp    deprecated
Sample.MultipleTargets.ConsoleApp.csproj    \samples\Sample.MultipleTargets.ConsoleApp\Sample.MultipleTargets.ConsoleApp.csproj      netcoreapp3.1   .NET Core 3.1         .NET Core       csharp    EOL: 13-Dec-2022
Sample.MultipleTargets.ConsoleApp.csproj    \samples\Sample.MultipleTargets.ConsoleApp\Sample.MultipleTargets.ConsoleApp.csproj      net462          .NET Framework 4.6.2  .NET Framework  csharp    supported
Sample.Net5.ConsoleApp.csproj               \samples\Sample.Net5.ConsoleApp\Sample.Net5.ConsoleApp.csproj                            net5.0          .NET 5.0              .NET            csharp    deprecated
Sample.Net6.ConsoleApp.csproj               \samples\Sample.Net6.ConsoleApp\Sample.Net6.ConsoleApp.csproj                            net6.0          .NET 6.0              .NET            csharp    supported
Sample.Net7.ConsoleApp.csproj               \samples\Sample.Net7.ConsoleApp\Sample.Net7.ConsoleApp.csproj                            net7.0          .NET 7.0              .NET            csharp    supported
Sample.Net8.ConsoleApp.csproj               \samples\Sample.Net8.ConsoleApp\Sample.Net8.ConsoleApp.csproj                            net8.0          .NET 8.0              .NET            csharp    in preview
Sample.NetCore.ConsoleApp.csproj            \samples\Sample.NetCore3.1.ConsoleApp\Sample.NetCore.ConsoleApp.csproj                   netcoreapp3.1   .NET Core 3.1         .NET Core       csharp    EOL: 13-Dec-2022
Sample.NetCore2.0.ConsoleApp.csproj         \samples\Sample.NetCore2.0.ConsoleApp\Sample.NetCore2.0.ConsoleApp.csproj                netcoreapp2.0   .NET Core 2.0         .NET Core       csharp    deprecated
Sample.NetCore2.1.ConsoleApp.csproj         \samples\Sample.NetCore2.1.ConsoleApp\Sample.NetCore2.1.ConsoleApp.csproj                netcoreapp2.1   .NET Core 2.1         .NET Core       csharp    deprecated
Sample.NetCore3.0.ConsoleApp.csproj         \samples\Sample.NetCore3.0.ConsoleApp\Sample.NetCore3.0.ConsoleApp.csproj                netcoreapp3.0   .NET Core 3.0         .NET Core       csharp    deprecated
Sample.NetFramework.ConsoleApp.csproj       \samples\Sample.NetFramework.ConsoleApp\Sample.NetFramework.ConsoleApp.csproj            v4.7.2          .NET Framework 4.7.2  .NET Framework  csharp    supported
Sample.NetFrameworkVBNet.ConsoleApp.vbproj  \samples\Sample.NetFrameworkVBNet.ConsoleApp\Sample.NetFrameworkVBNet.ConsoleApp.vbproj  netcoreapp3.1   .NET Core 3.1         .NET Core       vb.net    EOL: 13-Dec-2022
Sample.NetStandard.Class.csproj             \samples\Sample.NetStandard.Class\Sample.NetStandard.Class.csproj                        netstandard2.0  .NET Standard 2.0     .NET Standard   csharp    supported
Sample.SSDT.Database.sqlproj                \samples\Sample.SSDT.Database\Sample.SSDT.Database.sqlproj                               v4.7.2          .NET Framework 4.7.2  .NET Framework  csharp    supported
Sample.VB6.WinApp.vbp                       \samples\Sample.VB6.Calculator\Sample.VB6.WinApp.vbp                                     vb6             Visual Basic 6        Visual Basic 6  vb6       deprecated
VBProj.vbproj                               \samples\Sample.NetFramework1.0.App\VBProj.vbproj                                        v1.0            .NET Framework 1.0    .NET Framework  vb.net    deprecated
VBProj.vbproj                               \samples\Sample.NetFramework1.1.App\VBProj.vbproj                                        v1.1            .NET Framework 1.1    .NET Framework  vb.net    deprecated
VBProj.vbproj                               \samples\Sample.NetFramework2.0.App\VBProj.vbproj                                        v2.0            .NET Framework 2.0    .NET Framework  vb.net    deprecated
VBProj.vbproj                               \samples\Sample.NetFrameworkInvalid.App\VBProj.vbproj                                                    (Unknown)             (Unknown)       vb.net    unknown
```

### To download results to a csv file, add the `-f` argument with a file name:
`dotnet census -f c:\temp\results.csv`

## To download results from a GitHub Repo, 
- Add the `-o` argument with the GitHub owner or organiation
- Add the `-r` argument with the GitHub repo name
- Add the `-u` argument with the GitHub user
- Add the `-p` argument with the GitHub PAT Token password
- Add the `-b` argument with the GitHub branch name (defaults to 'main')

For example, to download results from this repository:

`dotnet census -o samsmithnz -r dotnetcensus -u samsmithnz -p <PAT_TOKEN>`

## What's next?
- We are currently experimenting with scanning entire [organizations](https://github.com/samsmithnz/DotNetCensus/pull/48)
- Possibly considering adding the ability to scan target other types of Git Repos (e.g. Azure DevOps, BitBucket, etc)

## Contributions
If you have a sample that you think should have been picked up, please create an issue or a PR! I'm happy to consider anything, and I know this isn't perfect!  
