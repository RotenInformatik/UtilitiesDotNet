# Deployment

## 1. Update

* Currently, the version information must be set manually in the following places:
  * `/sources/RI.Utilities/Properties/AssemblyInfo.cs` (3x)
  * `/sources/RI.Utilities.Documentation/RI.Utilities.Documentation.shfbproj` (1x)
  * `/docs/index.md` (1x)
  * `/docs/assemblies.md` (1x)
  * `/docs/versionhistory.md` (1x)
* Finish version history in `/docs/versionhistory.md`

## 2. Build

* Build `/sources/RI.Utilities/RI.Utilities.csproj`

* Build `/sources/RI.Utilities.Documentation/RI.Utilities.Documentation.shfbproj`

* Export to HTML:
  * `/docs/index.md`
  * `/docs/assemblies.md`
  * `/docs/versionhistory.md`

## 4. Upload

* Upload NuGet package to NuGet server
* Commit & Push to GitHub
* Create release tag in GitHub