# Introduction
Have you ever been curious to see information for all of the projects in a Visual Studio solution file at a glance? This command line application takes in the path to a .sln file, parses it to determine the top-level projects, and then recurses down through those projects to determine project information for itself and all projects it references.

# Usage
You can run the console application .exe and it will prompt you for the path to the .sln file.
Alternatively, you can run it from the command line and pass an argument of the solution path.
```powershell
.\ProjectReferencesBuilder.exe "path\to\solutionfile.sln"
```

# Information Displayed
## Name
The name of the project as per the solution file.

## TFM (Target Framework Moniker)
The target framework of the project.

## Absolute Path
The fully qualified path to the .csproj file.

## Project Type
Whether the .csproj file is the newer SDK style or the older pre-2017 style.

## Projects Referenced
All projects that the project references. Displays all of the same data listed here for all referenced projects.
