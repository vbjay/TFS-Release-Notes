[![Stories in Ready](https://badge.waffle.io/vbjay/TFS-Release-Notes.png?label=ready&title=Work%20Approved%20Issues)](https://waffle.io/vbjay/TFS-Release-Notes) [![Stories in In Progress](https://badge.waffle.io/vbjay/TFS-Release-Notes.png?label=In%20Progress&title=Issues%20In%20Progress)](https://waffle.io/vbjay/TFS-Release-Notes) [![Github Releases](https://img.shields.io/github/downloads/vbjay/TFS-Release-Notes/latest/total.svg?maxAge=2592000?style=plastic)](https://github.com/vbjay/TFS-Release-Notes/releases/latest)
# TFS-Release-Notes
Generates a list of files changed from a work item.

# Use

```powershell
Import-Module '.\TFS Release Notes.dll'

$tfs=Get-Tfs <URI to TFS project collection>
$wi=$tfs.WIT.GetWorkItem(<work item id>)

Get-WorkItemFiles <URI to TFS project collection> <work item id1,work item id2...>
Get-WorkItemFiles $tfs <work item id1,work item id2...>
```
