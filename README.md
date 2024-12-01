# AdventOfCode2024

My Advent of Code 2024 workspace.

This project is **NOT** production-grade and should be treated as a playground. I normally participate
in the Advent of Code using [LINQPad snippets]( https://www.linqpad.net/ ) but wanted to share 
with a broader group.


### AdventTemplate

I created the 'dotnet new advent' template to quickly stand up a project for each day
of the event. Over the course of the event, the template may (will) be
updated to include any helpers or references. To make it easier to install/uninstall, I added these PowerShell
scripts.

#### Install the advent template
```powershell
.\Install-Template.ps1

The following template packages will be installed:
   S:\DanielCarey\AdventOfCode2024\src\AdventTemplate\Content

Success: S:\DanielCarey\AdventOfCode2024\src\AdventTemplate\Content installed the following templates:
Template Name                             Short Name  Language  Tags
----------------------------------------  ----------  --------  --------------
Daniel Carey Advent Application Template  advent      [C#]      Common/Console
```

#### Uninstall the advent template
```powershell
.\Uninstall-Template.ps1

Success: S:\DanielCarey\AdventOfCode2024\src\AdventTemplate\Content was uninstalled.
```

#### Using the template

The template is specific to this solution and project structure. It contains project
references based on this file layout. Once installed, it can be invoked like this from
the solution/root folder.

```powershell
dotnet new advent -o .\src\Day03
```

### Add-DayNumber.ps1

This helper script creates the project using the advent template and 
adds the project to the solution. 

```powershell
.\Add-DayNumber.ps1 03
```

**Happy Coding**

### Reference Material

[Advent of Code]( https://adventofcode.com/ )

[LINQPad]( https://www.linqpad.net/ )

