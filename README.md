# Rebased on 12/14/2017

[![Stories in Ready](https://badge.waffle.io/vbjay/TFS-Release-Notes.png?label=ready&title=Work%20Approved%20Issues)](https://waffle.io/vbjay/TFS-Release-Notes) [![Stories in In Progress](https://badge.waffle.io/vbjay/TFS-Release-Notes.png?label=In%20Progress&title=Issues%20In%20Progress)](https://waffle.io/vbjay/TFS-Release-Notes) [![Github Releases](https://img.shields.io/github/downloads/vbjay/TFS-Release-Notes/latest/total.svg?maxAge=2592000?style=plastic)](https://github.com/vbjay/TFS-Release-Notes/releases/latest)
# TFS-Release-Notes
Generates a list of files changed from a work item.

# Use

See [Sample Script](https://github.com/vbjay/TFS-Release-Notes/blob/master/TFS%20Release%20Notes/test.ps1) to see how to use this module.
Authentication is now handled with [Personal Access Tokens](https://goo.gl/8vJTqY)

# Types of Info

- Get-Tfs: Allows you to get a TfsCollection object that you can use to gather nore information from TFS and pass to the other Cmdlets to prevent having to reauthenticate

## Changesets
- Get-UnlinkedChangsets: Allows you to find changesets that are not linked to any workitems
- Get-ChangesetFiles: Allows you to get details about the changed files in a changeset

## WorkItems

All workitem Cmdlets have an an option to process all children workitems too.  This allows you to specify a feature workitem and get all sub workitems.

- Get-WorkItemFiles: Allows you to get a list of files modifed by a workitem 
- Get-WorkItemChangesets: Allows you to get associated changesets of specified workitems
- Get-WorkItemAttachments: Allows you to get the files that are attached to workitems and optionally generate a zip of those files.  You can filter the names of the attachments using regex. 
