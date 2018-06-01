Imports System.Management.Automation
Imports Microsoft.TeamFoundation.WorkItemTracking.Client
<Cmdlet(VerbsCommon.Get, "WorkItemLinks")>
Public Class getGit
    Inherits WorkItemCmdlet

    Protected Overrides Sub ProcessRecord()
        MyBase.ProcessRecord()

        Dim links = currentWorkItems.SelectMany(Function(w) w.Links.Cast(Of Link).OfType(Of ExternalLink)())

        For Each lnk In links
            WriteObject(lnk)

        Next
    End Sub
End Class
